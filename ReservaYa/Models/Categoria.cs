using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReservaYa.Models
{
    public class Categoria
    {
        public int CategoriaID { get; set; }

        public string Nombre { get; set; } = string.Empty;

        // Relación inversa: una categoría puede tener varios espacios
        public List<Espacio> Espacios { get; set; } = new List<Espacio>();

        public Categoria(int categoriaID, string nombre)
        {
            CategoriaID = categoriaID;
            Nombre = nombre;
        }
    }
}