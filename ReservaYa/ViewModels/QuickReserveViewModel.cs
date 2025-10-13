using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReservaYa.ViewModels
{
	public class QuickReserveViewModel
	{
        public string Cliente { get; set; }
        public DateTime? Fecha { get; set; }
        public TimeSpan? Hora { get; set; }
        public int? Personas { get; set; }
    }
}