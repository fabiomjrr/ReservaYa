using ReservaYa.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ReservaYa.Controllers
{
    public class GestionEspaciosController : Controller
    {
        // GET: GestionEspacios
        private readonly DEVELOSERSEntities db = new DEVELOSERSEntities();
        public ActionResult Index()
        {
            List<Espacios> todos = db.Espacios.AsNoTracking().ToList();
            return View(todos);
        }

        public ActionResult Create() 
        {

            // Cargar categorías para el dropdown
            ViewBag.CategoriaID = new SelectList(db.Categorias.AsNoTracking(), "CategoriaID", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Espacios espacio)
        {
            if (ModelState.IsValid)
            {
                espacio.ImagenPrev=string.Empty;
                espacio.Disponible=false;
                db.Espacios.Add(espacio);
                db.SaveChanges();
                /*
                 * Después de SaveChanges(), Entity Framework actualiza el objeto espacio en memoria,
                 * incluyendo la propiedad EspacioID recién generada.
                 */

                return RedirectToAction("Index");
            }


            // Si falla la validación, volver a cargar el dropdown
            ViewBag.CategoriaID = new SelectList(db.Categorias.AsNoTracking(), "CategoriaID", "Nombre", espacio.CategoriaID);
            return View(espacio);
        }
        public ActionResult Update(int? id)
        {
            if (id == null) { return new HttpNotFoundResult();}
            // Cargar categorías para el dropdown
            var espacio = db.Espacios.Find(id);
            //cargar datos adropdownList
            ViewBag.CategoriaID = new SelectList(db.Categorias, "CategoriaID", "Nombre",espacio.CategoriaID);
            return View(espacio); //busca y regresa
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Espacios espacio)
        {
            if (ModelState.IsValid)
            {
                var original = db.Espacios.Find(espacio.EspacioID);
                if (original == null){ return HttpNotFound(); }

                // Copia los valores del modelo recibido al original rastreado por EF
                db.Entry(original).CurrentValues.SetValues(espacio);
                //guardar cambios
                db.SaveChanges();

                return RedirectToAction("Index");

            }

            // Si hay errores, recargamos el dropdown
            ViewBag.CategoriaID = new SelectList(db.Categorias.AsNoTracking(), "CategoriaID", "Nombre", espacio.CategoriaID);
            return View(espacio);                
        }
        // GET: Espacios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var espacio = db.Espacios.Find(id);
            if (espacio == null) return HttpNotFound();

            // modelo directamente a la vista para mostrar la info
            return View(espacio);
        }

        // POST: Espacios/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var espacio = db.Espacios.Find(id);
            if (espacio == null) return HttpNotFound();

            db.Espacios.Remove(espacio);
            db.SaveChanges();

            return RedirectToAction("Index"); // Redirige a la lista después de eliminar
        }       

    }
}