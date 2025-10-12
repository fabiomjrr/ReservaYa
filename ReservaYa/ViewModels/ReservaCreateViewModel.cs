using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System;
using System.ComponentModel.DataAnnotations;

namespace ReservaYa.ViewModels
{
	public class ReservaCreateViewModel
	{
        public int ReservaFechaID { get; set; }
        public int EspacioID { get; set; }

        public DateTime Fecha { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }

        [DataType(DataType.Currency)]
        public decimal ValorPorHora { get; set; }

        public string EspacioNombre { get; set; }
    }
}