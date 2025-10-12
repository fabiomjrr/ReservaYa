using ReservaYa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReservaYa.Controllers
{
    public class RegistrarseController : Controller
    {
        // GET: Registrarse
        private readonly DEVELOSERSEntities db = new DEVELOSERSEntities();

        public ActionResult Registro()
        {
            var user = db.Usuarios.Find(1);
            return View(user);
        }
    }
}