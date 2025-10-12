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
        // POST: Espacios/QuickReserve
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult QuickReserve(QuickReserveViewModel model,int idFechaDp)
        {
            // Validaciones básicas
            if (model == null || !model.Fecha.HasValue || !model.Hora.HasValue)
            {
                // Podrías retornar con ModelState a la vista Index y mostrar errores
                TempData["QuickReserveError"] = "Por favor completa Fecha y Hora.";
                return RedirectToAction("Index");
            }
            else
            {                
                return RedirectToAction("Reserve",idFechaDp);
            }

            // Lógica mínima: aquí decides cómo procesar.
            // Opciones comunes:
            //  - Crear una reserva "rápida" sin seleccionar espacio (no recomendable).
            //  - Redirigir a Index/Details para que el usuario seleccione el espacio (más lógico).
            //
            // Vamos a redirigir a Index con un mensaje y mantener los datos en TempData para UX,
            // o podrías redirigir a Details si conoces el EspacioID.

            //Donde el proceso de guardar? // aqui javier


            TempData["QuickReserveSuccess"] = $"Reserva provisional para {model.Cliente ?? "usuario"} en {model.Fecha:yyyy-MM-dd} {model.Hora}";

            return RedirectToAction("Index");
        }

        // GET: Espacios/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var espacio = await db.Espacios.FindAsync(id);
            if (espacio == null) return HttpNotFound();

            var fechas = await db.ReservasFechasDisponibles
                .Include(rf => rf.FechasDisponibles)
                .Where(rf => rf.EspacioID == id && rf.FechasDisponibles.Disponible)
                .ToListAsync();

            //si fechas.count == 0 falla

            var vm = new ReservaYa.ViewModels.EspacioDetailsViewModel
            {
                Espacio = espacio,
                FechasDisponibles = fechas
            };

            //join entre fechas dp on espcaio id return FechaDpId
            var reservaFechDP = db.ReservasFechasDisponibles
    .AsNoTracking()
    .Where(p => p.EspacioID == espacio.EspacioID)
    .Select(p => p.FechaDisponibleID)
    .ToList();

            ViewBag.IdFechaDp = 1;            

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

            //insertar fecha ocupada id 
            var fechaOcupada = new FechasOcupadas
            {
                Fecha = vm.Fecha,
                HoraInicio = vm.HoraInicio,
                HoraFin = vm.HoraFin,
                Activa = true
                

            };
            db.FechasOcupadas.Add(fechaOcupada);
            db.SaveChanges();
            //actualizar 

            //Proceso de guardado
            var reserva = new Reservas
            {
                MontoTotal = valor,
                UsuarioID = Convert.ToInt32(Session["UsuarioID"]),
                //funciona por el tracking de EF -- espero
                FechaOcupadaID = db.FechasOcupadas.AsNoTracking().Where(x => x.FechaOcupadaID == fechaOcupada.FechaOcupadaID).Select(x => x.FechaOcupadaID).FirstOrDefault(),
                ReservaFechaID = vm.ReservaFechaID
                
            };
            db.Reservas.Add(reserva);
            db.SaveChanges();

            //actualizar


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
