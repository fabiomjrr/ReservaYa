using Microsoft.Ajax.Utilities;
using ReservaYa.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReservaYa.Services
{
    public class EspacioService
    {
        //Repos
        private readonly EspacioRepository _espacio;
        private readonly CategoriaRepository _reserva;
        public EspacioService()
        {
            _espacio = new EspacioRepository();
            _reserva = new CategoriaRepository();
        }

        internal object ObtenerEspacios(int opcion=0)
        {
            return _espacio.ObtenerTodosSegun(opcion);
        }

        internal object ObtenerCategorias()
        {
            return _reserva.ObtenerTodas();
        }


    }
}