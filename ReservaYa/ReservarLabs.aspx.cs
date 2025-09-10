using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ReservaYa
{
    public partial class ReservarLabs : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarEspacios();
            }
        }

        private void CargarEspacios()
        {
            var espacios = new List<Espacio>()
            {
                new Espacio { Id = 1, Nombre = "Aula Común", Capacidad = 30, Imagen = "Content/img/classroom.jpg" },
                new Espacio { Id = 2, Nombre = "Laboratorio", Capacidad = 20, Imagen = "Content/img/Lab.jpg" },
                new Espacio { Id = 3, Nombre = "Estadio", Capacidad = 100, Imagen = "Content/img/estadio.jpg" }
            };

            rptEspacios.DataSource = espacios;
            rptEspacios.DataBind();
        }

        protected void rptEspacios_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Seleccionar")
            {
                string espacioId = e.CommandArgument.ToString();
                // Aquí podrías guardar en ViewState, Session o mostrar un mensaje
                Response.Write($"<script>alert('Has seleccionado el espacio con ID: {espacioId}');</script>");
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            string cliente = txtCliente.Text;
            string fecha = txtFecha.Text;
            string hora = txtHora.Text;
            string personas = txtPersonas.Text;

            // Aquí va la lógica para guardar la reserva (BD o Session)
            Response.Write($"<script>alert('Reserva guardada para {cliente} el {fecha} a las {hora} para {personas} personas');</script>");
        }

        public class Espacio
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
            public int Capacidad { get; set; }
            public string Imagen { get; set; }
        }
    }
}