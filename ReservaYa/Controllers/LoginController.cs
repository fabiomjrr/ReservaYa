using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using ReservaYa.Models;

namespace ReservaYa.Controllers
{
    // *** CLASE RENOMBRADA A LoginController ***
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

            // Nota: Se asume que Correo y Contrasena son campos byte[] en la base de datos,
            // y se manejan como UTF8 bytes para la comparación.
            byte[] correoBytes = Encoding.UTF8.GetBytes(Correo.Trim());
            byte[] contraBytes = Encoding.UTF8.GetBytes(Contrasena.Trim());

            var usuario = db.Usuarios.FirstOrDefault(u =>
                u.Correo.SequenceEqual(correoBytes) &&
                u.Contrasena.SequenceEqual(contraBytes) &&
                u.Activo == true);

            if (usuario != null)
            {
                // Iniciar sesión
                Session["UsuarioID"] = usuario.UsuarioID;
                Session["NombreUsuario"] = $"{usuario.Nombres} {usuario.Apellidos}";
                // Redirigir a Home/Index
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Mensaje = "Correo o contraseña incorrectos.";
            return View();
        }

        // GET: Login/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: Login/Register (Lógica para registrar un nuevo usuario)
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

            if (db.Usuarios.Any(u => u.Correo.SequenceEqual(correoBytes)))
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
                RolID = 2, 
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
