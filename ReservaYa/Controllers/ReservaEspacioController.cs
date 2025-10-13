using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using ReservaYa.Models;

namespace ReservaYa.Controllers
{
    public class ReservaEspacioController : Controller
    {
        // 1. Uso del mismo contexto que tu LoginController
        private DEVELOSERSEntities db = new DEVELOSERSEntities();

        // Función auxiliar para cargar los datos de las tarjetas (versión SÍNCRONA)
        private List<ReservaEspaciosModelo.EspacioCard> CargarEspaciosDisponibles()
        {
            // Se usa .ToList() en lugar de .ToListAsync()
            return db.Espacios // Asumiendo que Espacios es un DbSet en DEVELOSERSEntities
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

        // Acción GET: Carga la página (Devuelve ActionResult para ser SÍNCRONO)
        public ActionResult ReservaEspacioVista()
        {
            // Verificación para asegurar que el usuario esté logueado
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
            // 1. Obtener ID de usuario de la sesión
            int? usuarioId = Session["UsuarioID"] as int?;
            if (!usuarioId.HasValue)
            {
                // Redirigir si la sesión expiró
                return RedirectToAction("Login", "Login");
            }

            // 2. Validación y recarga del modelo si hay errores
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
                // --- INICIO DE LA LÓGICA DE NEGOCIO (SÍNCRONA) ---

                // Asumiremos 2 horas de duración fija para el cálculo.
                decimal duracionHoras = 2.0m;

                // 3. Obtener Valor por Hora desde EspaciosDetalles
                var detalle = db.EspaciosDetalles
                    .FirstOrDefault(d => d.EspacioID == modelo.EspacioIDSeleccionado);

                if (detalle == null)
                {
                    throw new Exception("Error de configuración: No se encontró el valor por hora.");
                }

                // Cálculo del Monto Total
                decimal montoTotal = detalle.ValorPorHora * duracionHoras;

                // 4. CREAR ENTRADAS EN CASCADA (Transacción implícita de SaveChanges)

                // 4.1. Crear registro en FechasOcupadas
                var nuevaFechaOcupada = new FechasOcupadas // (Asumiendo que esta es la Entidad)
                {
                    Fecha = modelo.Fecha,
                    HoraInicio = modelo.Hora,
                    // Calcula HoraFin
                    HoraFin = modelo.Hora.Add(TimeSpan.FromHours((double)duracionHoras)),
                    Activa = true
                };
                db.FechasOcupadas.Add(nuevaFechaOcupada);
                db.SaveChanges(); // Guarda para obtener FechaOcupadaID

                // 4.2. Crear registro en ReservasFechasDisponibles
                // NOTA: Se requiere FechaDisponibleID. Se asume un valor 1 por simplicidad.
                var reservaFechaDisponible = new ReservasFechasDisponibles // (Asumiendo que esta es la Entidad)
                {
                    EspacioID = modelo.EspacioIDSeleccionado.Value,
                    FechaDisponibleID = 1 // Reemplazar con la lógica de obtención real
                };
                db.ReservasFechasDisponibles.Add(reservaFechaDisponible);
                db.SaveChanges(); // Guarda para obtener ReservaFechaID

                // 4.3. Crear la Reserva final
                var nuevaReserva = new Reservas // (Asumiendo que esta es la Entidad)
                {
                    UsuarioID = usuarioId.Value,
                    MontoTotal = montoTotal,
                    FechaOcupadaID = nuevaFechaOcupada.FechaOcupadaID,
                    ReservaFechaID = reservaFechaDisponible.ReservaFechaID
                };
                db.Reservas.Add(nuevaReserva);
                db.SaveChanges();

                // --- FIN DE LA LÓGICA DE NEGOCIO ---

                return RedirectToAction("Confirmacion", new { id = nuevaReserva.ReservaID });
            }
            catch (Exception ex)
            {
                // Manejo de errores
                ModelState.AddModelError("", "Error al reservar: " + ex.Message);
                modelo.EspaciosDisponibles = CargarEspaciosDisponibles();
                return View("ReservaEspacioVista", modelo);
            }
        }

        // El método Dispose es esencial en ASP.NET Framework para liberar la conexión de la BD
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