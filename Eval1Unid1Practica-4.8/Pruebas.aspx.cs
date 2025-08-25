using Eval1Unid1Practica_4._8.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Eval1Unid1Practica_4._8
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        TareasController tareas = new TareasController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                IniciarListaDesplegable();
                CargarTareas();
                ValidarFecha();
            }

        }

        private void ValidarFecha()
        {
            RangeValidator1.MinimumValue = DateTime.Now.ToString("yyyy-MM-dd");
            RangeValidator1.MaximumValue = DateTime.Now.AddYears(1).ToString("yyyy-MM-dd"); //si no se establece el max no funciona
        }

        private void CargarTareas()
        {            
            gv_tareas.DataSource = tareas.Tareas();            
            gv_tareas.DataKeyNames = new string[] { "Id" };
            gv_tareas.DataBind();
        }

        private void IniciarListaDesplegable()
        {
            var Prioridades = CargarPrioridades();
            ddl_prioridades.DataSource = Prioridades; //fuenta
            // campo y valor
            ddl_prioridades.DataValueField = "id";
            ddl_prioridades.DataTextField = "Nombre";
            ddl_prioridades.DataBind(); //rederizar

        }

        private List<object> CargarPrioridades()
        {
            return new List<object>{
                new { id = 0, Nombre = "Baja" },
                new {id=1,Nombre="Media"},
                new {id=1,Nombre="Alta"}
            };
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                bool final = DateTime.TryParse(txb_fechaFinal.Text,out DateTime result);
                tareas.addTareas(0,txb_titulo.Text,txb_descripcion.Text,DateTime.Now,result);
                CargarTareas();
                LimpiarCampos();
            }
        }

        private void LimpiarCampos()
        {
            txb_titulo.Text = string.Empty;
            txb_descripcion.Text = string.Empty;
            txb_fechaFinal.Text = string.Empty;

        }
    }
}