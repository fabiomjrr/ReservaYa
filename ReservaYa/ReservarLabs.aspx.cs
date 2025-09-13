using ReservaYa.Models;
using ReservaYa.Services;
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
        //controller de espacios
        private readonly EspacioService _service;

        public ReservarLabs()
        {
            _service = new EspacioService();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarEspacios();
                CargarCategorias();
            }
        }

        private void CargarCategorias()
        {
            var categorias = _service.ObtenerCategorias();
            ddlCategoria.DataSource = categorias;
            ddlCategoria.DataTextField = "Nombre";
            ddlCategoria.DataValueField = "CategoriaID";
            ddlCategoria.DataBind();
        }

        private void CargarEspacios()
        {
            var espacios = EspaciosInicio();

            rptEspacios.DataSource = espacios;
            rptEspacios.DataBind();
        }

        private object EspaciosInicio()
        {
            return _service.ObtenerEspacios();
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
    }
}