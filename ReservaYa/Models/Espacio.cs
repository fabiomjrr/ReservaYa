using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReservaYa.Models
{
    public class Espacio
    {
        public int EspacioID { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public int CategoriaID { get; set; }

        public int Capacidad { get; set; }

        public string Direccion { get; set; }

        public string UbicacionEnlace { get; set; } // enlace de Google Maps

        public bool Estacionamiento { get; set; } = false;

        public bool Sanitarios { get; set; } = false;

        public bool AccesoSillaRuedas { get; set; } = false;

        public string ImagenPrev { get; set; }

        public bool Disponible { get; set; } = true;

        // Relación con Categorias (FK)
        public Categoria Categoria { get; set; }

        public Espacio(int espacioID, string nombre, int categoriaID, int capacidad, string direccion, string ubicacionEnlace, bool estacionamiento, bool sanitarios, bool accesoSillaRuedas, string imagenPrev, bool disponible)
        {
            EspacioID = espacioID;
            Nombre = nombre;
            CategoriaID = categoriaID;
            Capacidad = capacidad;
            Direccion = direccion;
            UbicacionEnlace = ubicacionEnlace;
            Estacionamiento = estacionamiento;
            Sanitarios = sanitarios;
            AccesoSillaRuedas = accesoSillaRuedas;
            ImagenPrev = imagenPrev;
            Disponible = disponible;
        }

    }

}