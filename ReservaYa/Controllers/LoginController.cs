using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using ReservaYa.Models;
using System.Collections.Generic;

namespace ReservaYa.Controllers
{
    public class LoginController : Controller
    {
        private DEVELOSERSEntities db = new DEVELOSERSEntities();

        // GET: Login/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Login/Login
        [HttpPost]
        public ActionResult Login(string Correo, string Contrasena)
        {
            if (string.IsNullOrEmpty(Correo) || string.IsNullOrEmpty(Contrasena))
            {
                ViewBag.Mensaje = "Por favor, complete todos los campos.";
                return View();
            }

            byte[] correoBytes = Encoding.UTF8.GetBytes(Correo.Trim());
            byte[] contraBytes = Encoding.UTF8.GetBytes(Contrasena.Trim());

            // Traemos todos los usuarios activos y filtramos en memoria (AsEnumerable()).
            // Esto es necesario porque SequenceEqual en byte[] no es soportado por LINQ to Entities.
            var usuario = db.Usuarios
                .Where(u => u.Activo == true)
                .AsEnumerable() // Fuerza la ejecución de la consulta hasta aquí y continua en memoria
                .FirstOrDefault(u =>
                    ((IEnumerable<byte>)u.Correo).SequenceEqual(correoBytes) &&
                    ((IEnumerable<byte>)u.Contrasena).SequenceEqual(contraBytes));
            

            if (usuario != null)
            {
                Session["UsuarioID"] = usuario.UsuarioID;
                Session["NombreUsuario"] = $"{usuario.Nombres} {usuario.Apellidos}";

                // *** REDIRECCIÓN***
                // Acción: Homepage, Controlador: GestionEspacios
                return RedirectToAction("Homepage", "GestionEspacios");
            }

            ViewBag.Mensaje = "Correo o contraseña incorrectos.";
            return View();
        }

        // GET: Login/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: Login/Register
        [HttpPost]
        public ActionResult Register(string Nombres, string Apellidos, DateTime FechaNacimiento, string Correo, string Contrasena)
        {
            if (string.IsNullOrEmpty(Correo) || string.IsNullOrEmpty(Contrasena))
            {
                ViewBag.Mensaje = "Todos los campos son obligatorios.";
                return View();
            }

            byte[] correoBytes = Encoding.UTF8.GetBytes(Correo.Trim());
            byte[] contraBytes = Encoding.UTF8.GetBytes(Contrasena.Trim());

            // Traemos todos los correos activos y filtramos en memoria.
            bool correoExiste = db.Usuarios
                .AsEnumerable() // Ejecuta la consulta antes de filtrar
                .Any(u => ((IEnumerable<byte>)u.Correo).SequenceEqual(correoBytes));

            if (correoExiste)
            {
                ViewBag.Mensaje = "Ya existe un usuario con ese correo.";
                return View();
            }
            

            Usuarios nuevo = new Usuarios
            {
                Nombres = Nombres,
                Apellidos = Apellidos,
                FechaNacimiento = FechaNacimiento,
                Correo = correoBytes,
                Contrasena = contraBytes,
                RolID = 2, // Usuario común
                Activo = true
            };

            db.Usuarios.Add(nuevo);
            db.SaveChanges();

            ViewBag.Mensaje = "Registro exitoso. Ahora puede iniciar sesión.";
            return RedirectToAction("Login");
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}