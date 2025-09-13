using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using ReservaYa.Models;

namespace ReservaYa.Repositories
{
    public class EspacioRepository
    {
        private readonly string _conexion;
        public EspacioRepository()
        {
            ConnectionStringSettings cs = ConfigurationManager.ConnectionStrings["conex"];
            _conexion = (cs != null) ? cs.ConnectionString : "";
        }

        public List<Espacio> ObtenerTodosSegun(int opcion = 0)
        {
            //Gestiona las consultas y reciclar codigo
            // 0 = disponibles y no disponibles
            // 1 = solo disponibles

            string sql;
            sql = @"SELECT EspacioID, Nombre, CategoriaID, Capacidad, Direccion, UbicacionEnlace, 
                          Estacionamiento, Sanitarios, AccesoSillaRuedas, ImagenPrev, Disponible 
                   FROM Espacios" + $"{((opcion==1)?"WHERE Disponible =1":string.Empty)}";

            List<Espacio> espacios = new List<Espacio>();

            try
            {
                using (SqlConnection conexion = new SqlConnection(_conexion))
                {
                    conexion.Open();
                    using (SqlCommand command = new SqlCommand(sql, conexion))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (!reader.HasRows)
                                throw new InvalidOperationException("No se encontraron registros de espacios.");

                            while (reader.Read())
                            {
                                // Índices correctos: comienzan en 0
                                int id = reader.GetInt32(0);
                                string nombre = reader.GetString(1);
                                int categoriaId = reader.GetInt32(2);
                                int capacidad = reader.GetInt32(3);

                                string direccion = reader.IsDBNull(4) ? null : reader.GetString(4);
                                string link = reader.IsDBNull(5) ? null : reader.GetString(5);
                                bool parqueo = reader.GetBoolean(6);
                                bool sanitarios = reader.GetBoolean(7);
                                bool sillaRd = reader.GetBoolean(8);
                                string rutaImgPrev = reader.IsDBNull(9) ? null : reader.GetString(9);
                                bool disponible = reader.GetBoolean(10);

                                espacios.Add(new Espacio(
                                    id, nombre, categoriaId, capacidad, direccion,
                                    link, parqueo, sanitarios, sillaRd, rutaImgPrev, disponible
                                ));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Puedes loguearlo o relanzarlo
                throw new ApplicationException("Error al obtener los espacios.", ex);
            }

            return espacios; // Siempre devuelve lista, aunque es
        }

        public void CrearEspacio()
        {

        }

        public void EliminarEspacio()
        {

        }

        public void ActulizarEspacio()
        {

        }
    }
}