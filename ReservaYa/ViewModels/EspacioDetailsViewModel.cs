using ReservaYa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReservaYa.ViewModels
{
	public class EspacioDetailsViewModel
	{
        public Espacios Espacio { get; set; }
        public IEnumerable<ReservasFechasDisponibles> FechasDisponibles { get; set; }
    }
}