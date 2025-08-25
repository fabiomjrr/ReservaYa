using Eval1Unid1Practica_4._8.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eval1Unid1Practica_4._8.Controller
{
    public class TareasController
    {
        private List<Tareas> _tareas;

        public TareasController()
        {
            _tareas = new List<Tareas>();
            _tareas.Add(new Tareas { Id = 1, Name = "Hacer la cama", Description = "Hice un desastre al buscar las llaves", Created = DateTime.Today, FinalDate = new DateTime(2025, 08, 15), Finished = false });
            _tareas.Add(new Tareas { Id = 1, Name = "Buscar mi celular", Description = "Puedo vivir sin el por ahora", Created = DateTime.Today, FinalDate = new DateTime(2025, 08, 15), Finished = false });
        }
        public List<Tareas> Tareas()=> _tareas;
        
        public void addTareas(int id,string name,string descripcion, DateTime inicio,DateTime final)
        {
            _tareas.Add(new Tareas { Id = id, Name = name, Description = descripcion, Created = inicio, FinalDate = final, Finished= false });
        }
    }
}