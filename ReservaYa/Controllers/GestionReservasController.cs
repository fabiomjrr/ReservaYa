using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using ReservaYa.Models;

namespace ReservaYa.Controllers
{
    public class GestionReservasController : Controller
    {
        // Usa el nombre real de tu DbContext (Ejemplo: DEVELOSERSEntities)
        private readonly DEVELOSERSEntities _context = new DEVELOSERSEntities();

        // ---------------------------------------------------------------------
        // ACCIÓN 1: MisReservas() - Muestra las reservas del usuario actual
        // ---------------------------------------------------------------------
        public async Task<ActionResult> MisReservas()
        {
            // ... VALIDACIÓN Y OBTENCIÓN DE usuarioIdActual (Se mantiene igual) ...
            if (Session["UsuarioID"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            int usuarioIdActual = (int)Session["UsuarioID"];

            // MODIFICACIÓN CLAVE EN LA CONSULTA:
            var reservasDelUsuario = await (
                from r in _context.Reservas

                    // 1. Unir Reserva con la tabla intermedia (ReservasFechasDisponibles)
                join rfd in _context.ReservasFechasDisponibles
                // Asegúrate de que los nombres de las propiedades ID sean correctos
                on r.ReservaFechaID equals rfd.ReservaFechaID

                // 2. Unir la tabla intermedia con Espacios
                join e in _context.Espacios
                on rfd.EspacioID equals e.EspacioID

                // 3. Unir la tabla intermedia con FechasDisponibles
                join fd in _context.FechasDisponibles
                on rfd.FechaDisponibleID equals fd.FechaDisponibleID

                where r.UsuarioID == usuarioIdActual // Filtro principal

                orderby fd.Fecha descending, fd.HoraInicio descending
                select new ReservaUsuarioViewModel
                {
                    ReservaID = r.ReservaID,
                    NombreEspacio = e.Nombre, // Asegúrate que la columna se llame 'Nombre' en tu modelo Espacios
                    MontoTotal = r.MontoTotal,
                    FechaReserva = fd.Fecha,
                    HoraInicio = fd.HoraInicio,
                    HoraFin = fd.HoraFin
                }
            ).ToListAsync();

            // 4. Lógica para marcar si la reserva ya pasó
            reservasDelUsuario.ForEach(res =>
            {
                var fechaCompleta = res.FechaReserva.Date + res.HoraInicio;
                res.EsPasada = fechaCompleta < DateTime.Now;
            });

            // 5. Retorna la vista MisReservas.cshtml
            return View(reservasDelUsuario);
        }

       // ---------------------------------------------------------------------
// ACCIÓN 2: Cancelar una reserva (Lógica de eliminación en DB)
// ---------------------------------------------------------------------
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<ActionResult> Cancelar(int id)
{
    // 1. VALIDACIÓN DE SESIÓN Y SEGURIDAD
    if (Session["UsuarioID"] == null)
    {
         TempData["Error"] = "Sesión expirada. Por favor, inicia sesión de nuevo.";
         return RedirectToAction("Login", "Account"); 
    }
    int usuarioIdActual = (int)Session["UsuarioID"];

    try
    {
        // 2. ENCONTRAR Y VALIDAR PROPIEDAD DE LA RESERVA
        var reserva = await _context.Reservas
                                    .Where(r => r.ReservaID == id && r.UsuarioID == usuarioIdActual)
                                    .FirstOrDefaultAsync();

        if (reserva == null)
        {
            TempData["Error"] = "Error: La reserva no fue encontrada o no te pertenece.";
            return RedirectToAction("MisReservas");
        }
     
        int reservaFechaId = (int)reserva.ReservaFechaID; 

        // 3. ELIMINAR REGISTROS DEPENDIENTES
        
        var reservaFechaDisponible = await _context.ReservasFechasDisponibles
            .Where(rfd => rfd.ReservaFechaID == reservaFechaId)
            .FirstOrDefaultAsync();

        if (reservaFechaDisponible != null)
        {
            _context.ReservasFechasDisponibles.Remove(reservaFechaDisponible);
        }
        
        // 6. ELIMINAR LA RESERVA PRINCIPAL
        _context.Reservas.Remove(reserva);

        // 7. GUARDAR CAMBIOS EN LA BASE DE DATOS
        await _context.SaveChangesAsync();

        TempData["Mensaje"] = $"La reserva #{id} fue cancelada y el espacio liberado.";
    }
    catch (Exception)
    {
        TempData["Error"] = "Ocurrió un error en el servidor al intentar cancelar la reserva. Intenta de nuevo.";
    }

    return RedirectToAction("MisReservas");
}
    }
}