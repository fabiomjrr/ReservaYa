using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eval1Unid1Practica_4._8.Models
{
    public class Tareas //Lo ideal es usar propiedades públicas para que el GridView pueda leerlos bien:
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime FinalDate { get; set; }
        public bool Finished { get; set; }
    }

}