<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Eval1Unid1Practica_4._8._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <section class="row" aria-labelledby="aspnetTitle">
            <h1 id="aspnetTitle">Gestion de Tareas Pendientes</h1>
            <p class="lead">Gestion de tareas simple</p>
            <p><a href="Pruebas.aspx" class="btn btn-primary btn-md">Crear &raquo;</a></p>
        </section>

        <section class="border border-primary row">
            <h1>Calendario</h1>
            <table class ="table">
            <asp:Calendar ID="Calendar1" runat="server"></asp:Calendar>
            </table>

        </section>

         <div>
            <div> <!--Pendientes-->
                <asp:GridView ID="gv_TareasP" runat="server" CssClass="table" AutoGenerateColumns="false">
                </asp:GridView>
            </div>

            <div> <!--Terminadas-->
                 <asp:GridView ID="gv_TareasT" runat="server" CssClass="table" AutoGenerateColumns="false">

                 </asp:GridView>

            </div>
        </div>
    </main>

</asp:Content>
