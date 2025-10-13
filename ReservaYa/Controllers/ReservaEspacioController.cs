using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using ReservaYa.Models;

namespace ReservaYa.Controllers
{
    public class ReservaEspacioController : Controller
    {
        // 1. Instancia del contexto de Entity Framework
        private DEVELOSERSEntities db = new DEVELOSERSEntities();

        // Función auxiliar para cargar las tarjetas de espacios (SÍNCRONA)
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
            // Verificación de sesión para redirigir al Login
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

                // 1. OBTENER VALOR POR HORA
                var detalle = db.EspaciosDetalles
                    .FirstOrDefault(d => d.EspacioID == modelo.EspacioIDSeleccionado);

                if (detalle == null)
                {
                    throw new Exception($"Error de configuración: No se encontró el valor por hora para el EspacioID {modelo.EspacioIDSeleccionado.Value}.");
                }

                decimal montoTotal = detalle.ValorPorHora * duracionHoras;

                // 2. CREAR REGISTRO DE FECHA OCUPADA
                var nuevaFechaOcupada = new FechasOcupadas
                {
                    Fecha = modelo.Fecha,
                    HoraInicio = modelo.Hora,
                    HoraFin = modelo.Hora.Add(TimeSpan.FromHours((double)duracionHoras)),
                    Activa = true
                };
                db.FechasOcupadas.Add(nuevaFechaOcupada);
                db.SaveChanges();

                // 3. CREAR LA RESERVA FINAL
                var nuevaReserva = new Reservas
                {
                    UsuarioID = usuarioId.Value,
                    MontoTotal = montoTotal,
                    FechaOcupadaID = nuevaFechaOcupada.FechaOcupadaID,
                    ReservaFechaID = null // Ignoramos la FK problemática
                };
                db.Reservas.Add(nuevaReserva);
                db.SaveChanges(); // Guarda la reserva final

                // ----------------------------------------------------
                // *** REDIRECCIÓN A GESTIÓN DE RESERVAS 
                // ----------------------------------------------------
                
                return RedirectToAction("MisReservas", "GestionReservas");
                // Si quieres que vaya al índice: 
                // return RedirectToAction("Index", "GestionReservas");
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