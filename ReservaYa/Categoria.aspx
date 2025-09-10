<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Categoria.aspx.cs" Inherits="ReservaYa.Categoria" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- CONTENIDO PRINCIPAL -->
        <div class="contenedor">
            <h3>Búsqueda avanzada de Espacios</h3>

            <!-- Capacidad -->
            <div class="form-group">
                <label>Capacidad</label>
                <asp:TextBox ID="txtCapacidad" runat="server" CssClass="slider" TextMode="Number" Text="52"></asp:TextBox>
                <span>52 espacios</span>
            </div>

            <!-- Categoría -->
            <div class="form-group">
                <label>Categoría</label>
                <asp:DropDownList ID="ddlCategoria" runat="server">
                    <asp:ListItem Text="-- Selecciona --" />
                    <asp:ListItem>Salón</asp:ListItem>
                    <asp:ListItem>Laboratorio</asp:ListItem>
                </asp:DropDownList>
            </div>

            <!-- Departamento -->
            <div class="form-group">
                <label>Departamento</label>
                <asp:DropDownList ID="ddlDepartamento" runat="server">
                    <asp:ListItem Text="-- Selecciona --" />
                    <asp:ListItem>San Salvador</asp:ListItem>
                    <asp:ListItem>San Miguel</asp:ListItem>
                </asp:DropDownList>
            </div>

            <!-- Fecha -->
            <div class="form-group">
                <label>Fecha</label>
                <asp:TextBox ID="txtFecha" runat="server" TextMode="Date"></asp:TextBox>
            </div>

            <!-- Hora -->
            <div class="form-group">
                <label>Hora</label>
                <asp:TextBox ID="txtHoraInicio" runat="server" Width="120px" TextMode="Time"></asp:TextBox>
                <span>de</span>
                <asp:TextBox ID="txtHoraFin" runat="server" Width="120px" TextMode="Time"></asp:TextBox>
            </div>

            <!-- Checkbox y Radio -->
            <div class="opciones">
                <div>
                    <asp:CheckBox ID="chkEstacionamiento" runat="server" Text=" Estacionamiento disponible" /><br />
                    <asp:CheckBox ID="chkBanos" runat="server" Text=" Baños disponibles" />
                </div>
                <div>
                    <asp:RadioButton ID="rbMobiliarioSi" runat="server" GroupName="mobiliario" Text=" Con Mobiliario" /><br />
                    <asp:RadioButton ID="rbMobiliarioNo" runat="server" GroupName="mobiliario" Text=" Sin Mobiliario" />
                </div>
            </div>

            <!-- Botón + -->
            &nbsp;<!-- Botones --><div class="acciones">
                <asp:Button ID="btnCerrar" runat="server" Text="Close" CssClass="btn-cerrar" />
                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn-buscar" /> <!-- OnClick="btnBuscar_Click" -->
            </div>
        </div>

    <!-- CSS PERSONALIZADO-->
    <style>
        body {
            font-family: Arial, sans-serif;
            margin: 0;
            background: #fafafa;
        }
        .menu {
            background: #f2f2f2;
            padding: 10px;
            display: flex;
            justify-content: space-between;
            border-bottom: 2px solid #c00;
        }
        .menu span {
            font-size: 20px;
            color: #c00;
            font-weight: bold;
        }
        .menu button {
            margin: 0 5px;
            padding: 10px 15px;
            border: none;
            background: #428bca;
            color: white;
            border-radius: 5px;
        }
        .contenedor {
            padding: 20px;
        }
        h3 {
            margin-bottom: 20px;
        }
        .form-group {
            margin: 15px 0;
        }
        label {
            display: inline-block;
            width: 150px;
            font-weight: bold;
        }
        input[type=text], select {
            padding: 6px;
            width: 220px;
        }
        .slider {
            width: 200px;
        }
        .opciones {
            display: flex;
            gap: 40px;
            margin: 20px 0;
        }
        .acciones {
            margin-top: 30px;
            text-align: right;
        }
        .acciones button {
            padding: 8px 15px;
            border-radius: 5px;
            border: none;
            margin-left: 10px;
        }
        .btn-buscar {
            background: #007bff;
            color: white;
        }
        .btn-cerrar {
            background: #666;
            color: white;
        }
        .plus {
            display: block;
            text-align: center;
            margin: 20px 0;
            font-size: 25px;
            color: #ff3366;
            cursor: pointer;
        }
    </style>
</asp:Content>
