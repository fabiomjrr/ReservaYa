using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using ReservaYa.Models;
using System.Data.Entity; 
using System.Threading.Tasks;

namespace ReservaYa.Controllers
{
    public class ReservaEspacioController : Controller
    {
        private DEVELOSERSEntities db = new DEVELOSERSEntities();

        // Función auxiliar para cargar las tarjetas de espacios
        private List<ReservaEspaciosModelo.EspacioCard> CargarEspaciosDisponibles()
        {
            return db.Espacios
                .Where(e => e.Disponible == true)
                .Select(e => new ReservaEspaciosModelo.EspacioCard
                {
                    EspacioID = e.EspacioID,
                    Nombre = e.Nombre,
                    Capacidad = e.Capacidad,
                    ImagenPrevUrl = e.ImagenPrev
                })
                .ToList();
        }

        // Acción GET: Muestra el formulario de reserva
        public ActionResult ReservaEspacioVista()
        {
            if (Session["UsuarioID"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            var espacios = CargarEspaciosDisponibles();
            var viewModel = new ReservaEspaciosModelo
            {
                EspaciosDisponibles = espacios
            };

            return View(viewModel);
        }

        // Acción POST: Procesa la solicitud de reserva
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearReserva(ReservaEspaciosModelo modelo)
        {
            int? usuarioId = Session["UsuarioID"] as int?;
            if (!usuarioId.HasValue)
            {
                return RedirectToAction("Login", "Login");
            }

            if (!modelo.EspacioIDSeleccionado.HasValue || !ModelState.IsValid)
            {
                modelo.EspaciosDisponibles = CargarEspaciosDisponibles();
                if (!modelo.EspacioIDSeleccionado.HasValue)
                {
                    ModelState.AddModelError("", "Debe seleccionar un espacio de la lista inferior.");
                }
                return View("ReservaEspacioVista", modelo);
            }

            try
            {
                decimal duracionHoras = 2.0m; // Duración fija

                // 1. OBTENER VALOR POR HORA (Se mantiene)
                var detalle = db.EspaciosDetalles
                    .FirstOrDefault(d => d.EspacioID == modelo.EspacioIDSeleccionado);

                if (detalle == null)
                {
                    throw new Exception($"Error de configuración: No se encontró el valor por hora para el EspacioID {modelo.EspacioIDSeleccionado.Value}.");
                }

                decimal montoTotal = detalle.ValorPorHora * duracionHoras;

                // A) CREAR Y GUARDAR EL REGISTRO DE FECHA DISPONIBLE (El slot de tiempo)
                var nuevaFecha = new FechasDisponibles 
                {
                    Fecha = modelo.Fecha,
                    HoraInicio = modelo.Hora,
                    HoraFin = modelo.Hora.Add(TimeSpan.FromHours((double)duracionHoras)),
                    
                };
                db.FechasDisponibles.Add(nuevaFecha);
                db.SaveChanges(); // CLAVE 1: Asigna nuevaFecha.FechaDisponibleID

                int fechaDisponibleId = nuevaFecha.FechaDisponibleID;


                // B) CREAR Y GUARDAR EL REGISTRO INTERMEDIO (ReservasFechasDisponibles)
                var nuevaReservaFecha = new ReservasFechasDisponibles
                {
                    EspacioID = modelo.EspacioIDSeleccionado.Value,
                    FechaDisponibleID = fechaDisponibleId
                };
                db.ReservasFechasDisponibles.Add(nuevaReservaFecha);
                db.SaveChanges(); // CLAVE 2: Asigna nuevaReservaFecha.ReservaFechaID

                int reservaFechaIdGenerado = nuevaReservaFecha.ReservaFechaID;


                // C) CREAR LA RESERVA FINAL
                var nuevaReserva = new Reservas
                {
                    UsuarioID = usuarioId.Value,
                    MontoTotal = montoTotal,
                 
                    ReservaFechaID = reservaFechaIdGenerado
                };
                db.Reservas.Add(nuevaReserva);
                db.SaveChanges(); // Guarda la reserva final

                // ----------------------------------------------------
                // REDIRECCIÓN FINAL
                // ----------------------------------------------------
                return RedirectToAction("MisReservas", "GestionReservas");
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += " Detalle interno: " + ex.InnerException.Message;
                }

                ModelState.AddModelError("", "Error al intentar guardar la reserva. " + errorMessage);

                modelo.EspaciosDisponibles = CargarEspaciosDisponibles();
                return View("ReservaEspacioVista", modelo);
            }
        }

        // Liberación de recursos de la BD
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}