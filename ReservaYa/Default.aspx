<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ReservaYa._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    
    <!-- Area de HTML -->
    <main>
        <section class="banner">
    <h1>BIENVENIDOS</h1>
    <p>Gestiona tus reservas de manera eficiente</p>
     <!-- <a href="Pruebas.aspx" class="btn-crear">Ver más &raquo;</a>
    <a href="Pruebas.aspx" class="btn-contactanos">Contactanos &raquo;</a> -->
</section>

    <h2>Galería</h2>
     <div class ="container">
        <div class="row">
            <div class ="rounded float-center" style="width:600px; height:400px;">
                <asp:Image ID="Image1" runat="server" class="rounded float-end" ImageUrl="Content/Img/estadio.jpg" />
            </div>
            <div class ="rounded float-center" style="width:600px; height:400px;">
                <asp:Image ID="Image2" runat="server" class="rounded float-end" ImageUrl="Content/Img/Lab.jpg" />
            </div>

            <div class ="rounded float-right" style="width:600px; height:400px;">
                <asp:Image ID="Image3" runat="server" class="rounded float-end" ImageUrl="Content/Img/auditorio.jpg" />
            </div>
            <div class ="rounded float-right" style="width:600px; height:400px;">
                <asp:Image ID="Image4" runat="server" class="rounded float-end" ImageUrl="Content/Img/classroom.jpg" />
            </div>
        </div>
    </div>

<div class="row">
    <div class="col tarea-card">
        <h3>Tareas Pendientes</h3>
        <asp:GridView ID="gv_TareasP" runat="server" CssClass="table" AutoGenerateColumns="false"></asp:GridView>
    </div>
    <div class="col tarea-card">
        <h3>Tareas Terminadas</h3>
        <asp:GridView ID="gv_TareasT" runat="server" CssClass="table" AutoGenerateColumns="false"></asp:GridView>
    </div>

</div>
    </main>

    <!-- Estilo CSS Embebido -->
    <style>
        * {
          box-sizing: border-box;
        }

        /* Wrapping element */
        /* Hoja de estilos para Default, homepage.*/
        .body-content {
            margin-top: 15px;
            padding-left: 15px;
            padding-right: 15px;
        }

        input,
        select,
        textarea {
            max-width: 280px;
        }

        .banner {
            background-color: #007bff;
            color: white;
            padding: 30px;
            text-align: center;
            border-radius: 10px;
            margin-bottom: 20px;
        }

        .btn-crear {
            background-color: #ffffff;
            color: #007bff;
            border: 2px solid #007bff;
            padding: 10px 20px;
            text-decoration: none;
            border-radius: 5px;
            font-weight: bold;
        }

            .btn-crear:hover {
                background-color: #007bff;
                color: white;
            }

        .calendario-box h2 {
            color: #007bff;
            margin-bottom: 15px;
        }

        .tarea-card {
            border: 2px solid #ccc;
            border-radius: 10px;
            padding: 15px;
            margin-bottom: 20px;
        }

            .tarea-card h3 {
                margin-bottom: 10px;
                color: #333;
            }

        .table {
            width: 100%;
            border-collapse: collapse;
        }

            .table th,
            .table td {
                border: 1px solid #ddd;
                padding: 8px;
            }

            .table th {
                background-color: #f2f2f2;
                color: #333;
            }

        @media screen and (min-width: 768px) {
            .body-content {
                padding: 0;
            }

            .row {
                display: flex;
                gap: 20px;
            }

            .col {
                flex: 1;
            }
        }
    </style>


</asp:Content>
