using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ReservaYa.Models;
using ReservaYa.ViewModels;

namespace ReservaYa.Controllers
{
    public class EspaciosController : Controller
    {
        private readonly DEVELOSERSEntities db = new DEVELOSERSEntities();
        public async Task<ActionResult> Index()
        {
            var espacios = await db.Espacios.Where(e => e.Disponible).ToListAsync();
            return View(espacios);
        }
        
        public async Task<ActionResult> Details(int? id)
        {
            var espacio = await db.Espacios.FindAsync(id);
            if (espacio == null) return HttpNotFound();

            var fechas = await db.ReservasFechasDisponibles
                .Include(rf => rf.FechasDisponibles)
                .Where(rf => rf.EspacioID == id && rf.FechasDisponibles.Disponible)
                .ToListAsync();

            var vm = new EspacioDetailsViewModel
            {
                Espacio = espacio,
                FechasDisponibles = fechas
            };

            return View(vm);
        }

        [Authorize(Roles = "Usuario")]
        public async Task<ActionResult> Reserve(int reservaFechaId)
        {
            var rf = await db.ReservasFechasDisponibles
                .Include(r => r.FechasDisponibles)
                .Include(r => r.Espacios)
                .FirstOrDefaultAsync(r => r.ReservaFechaID == reservaFechaId);

            if (rf == null) return HttpNotFound();

            var valor = db.EspaciosDetalles
                .Where(d => d.EspacioID == rf.EspacioID)
                .Select(d => d.ValorPorHora)
                .FirstOrDefault();

            var vm = new ReservaCreateViewModel
            {
                ReservaFechaID = rf.ReservaFechaID,
                EspacioID = rf.EspacioID,
                Fecha = rf.FechasDisponibles.Fecha,
                HoraInicio = rf.FechasDisponibles.HoraInicio,
                HoraFin = rf.FechasDisponibles.HoraFin,
                ValorPorHora = valor,
                EspacioNombre = rf.Espacios.Nombre
            };

            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Usuario")]
        public async Task<ActionResult> Reserve(ReservaCreateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            using (var tx = db.Database.BeginTransaction())
            {
                try
                {
                    var rf = await db.ReservasFechasDisponibles
                        .Include(r => r.FechasDisponibles)
                        .FirstOrDefaultAsync(r => r.ReservaFechaID == model.ReservaFechaID);

                    if (rf == null || !rf.FechasDisponibles.Disponible)
                    {
                        ModelState.AddModelError("", "La fecha ya no está disponible.");
                        return View(model);
                    }

                    var fechaO = new FechasOcupadas
                    {
                        Fecha = rf.FechasDisponibles.Fecha,
                        HoraInicio = rf.FechasDisponibles.HoraInicio,
                        HoraFin = rf.FechasDisponibles.HoraFin,
                        Activa = true
                    };
                    db.FechasOcupadas.Add(fechaO);
                    await db.SaveChangesAsync();

                    var horas = (rf.FechasDisponibles.HoraFin - rf.FechasDisponibles.HoraInicio).TotalHours;
                    decimal monto = model.ValorPorHora * (decimal)horas;

                    // Temporal: simulamos usuario logueado
                    int usuarioId = 1;

                    db.Reservas.Add(new Reservas
                    {
                        MontoTotal = monto,
                        UsuarioID = usuarioId,
                        FechaOcupadaID = fechaO.FechaOcupadaID,
                        ReservaFechaID = rf.ReservaFechaID
                    });

                    rf.FechasDisponibles.Disponible = false;
                    db.Entry(rf.FechasDisponibles).State = EntityState.Modified;

                    await db.SaveChangesAsync();
                    tx.Commit();

                    return RedirectToAction("Index");
                }
                catch
                {
                    tx.Rollback();
                    ModelState.AddModelError("", "Error al procesar la reserva.");
                    return View(model);
                }

            }

        }
    }
}
