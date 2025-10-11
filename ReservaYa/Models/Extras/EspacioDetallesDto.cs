using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReservaYa.Models.Extras
{
    public class EspacioDetallesDto
    {
        public int EspacioId { get; set; }
        public string NombreEspacio { get; set; }

        public int CategoriaId { get; set; }
        public string NombreCategoria { get; set; }

        public int? EspacioDetalleId { get; set; }
        public decimal? ValorPorHora { get; set; }

        public List<int> FechasDisponiblesIds { get; set; }
        public List<DateTime> FechasDisponibles { get; set; }

    }

}