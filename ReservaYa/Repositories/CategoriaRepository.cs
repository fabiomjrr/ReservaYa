using ReservaYa.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace ReservaYa.Repositories
{
    public class CategoriaRepository
    {
        private readonly string _conexion;
        public CategoriaRepository()
        {
            ConnectionStringSettings cs = ConfigurationManager.ConnectionStrings["conex"];
            _conexion = (cs != null) ? cs.ConnectionString : "";
        }

        public List<Models.Categoria> ObtenerTodas() 
        {
            var lista = new List<Models.Categoria>();
            string sql = "SELECT * FROM Categorias";
            try
            {
                using (var conex = new SqlConnection(_conexion)) 
                {
                    conex.Open();
                    using (var command = new SqlCommand(sql, conex))
                    {
                        //Lectura
                        using (var reader = command.ExecuteReader())
                        {
                            if (!reader.HasRows)
                                throw new InvalidOperationException("No se encontraron registros de espacios.");


                            while (reader.Read())
                            {
                                int categoriaID = reader.GetInt32(0);
                                string Nombre = reader.GetString(1);

                                lista.Add(new Models.Categoria(categoriaID, Nombre));
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
            return lista;
        }

    }

}