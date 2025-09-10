<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ReservarLabs.aspx.cs" Inherits="ReservaYa.ReservarLabs" %>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
        <div class="container my-5">
            <section class="banner">
                <h1>Haz tu reserva con nosotros</h1>
                
                 <!-- <a href="Pruebas.aspx" class="btn-crear">Ver más &raquo;</a>
                <a href="Pruebas.aspx" class="btn-contactanos">Contactanos &raquo;</a> -->
            </section>
            <!-- Formulario -->
            <div class="card p-4 mb-5 shadow-sm">
                <div class="row g-3">
                    <div class="col-md-6">
                        <asp:Label runat="server" AssociatedControlID="txtCliente" Text="Cliente" CssClass="form-label"></asp:Label>
                        <asp:TextBox runat="server" ID="txtCliente" CssClass="form-control" />
                    </div>
                    <div class="col-md-6">
                        <asp:Label runat="server" AssociatedControlID="txtFecha" Text="Fecha" CssClass="form-label"></asp:Label>
                        <asp:TextBox runat="server" ID="txtFecha" TextMode="Date" CssClass="form-control" />
                    </div>
                    <div class="col-md-6">
                        <asp:Label runat="server" AssociatedControlID="txtHora" Text="Hora" CssClass="form-label"></asp:Label>
                        <asp:TextBox runat="server" ID="txtHora" TextMode="Time" CssClass="form-control" />
                    </div>
                    <div class="col-md-6">
                        <asp:Label runat="server" AssociatedControlID="txtPersonas" Text="Personas" CssClass="form-label"></asp:Label>
                        <asp:TextBox runat="server" ID="txtPersonas" TextMode="Number" CssClass="form-control" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPersonas" 
                            ErrorMessage="Número de personas requerido" CssClass="text-danger" Display="Dynamic" />
                        <asp:RegularExpressionValidator runat="server" ControlToValidate="txtPersonas" 
                            ValidationExpression="^\d+$" ErrorMessage="Solo números enteros" CssClass="text-danger" />
                    </div>
                </div>
                <div class="text-end mt-3">
                    <asp:Button runat="server" ID="btnGuardar" Text="Guardar Reserva" CssClass="btn btn-primary px-4" OnClick="btnGuardar_Click" />
                </div>
            </div>

            <!-- Grid de Espacios -->
            <div class="row">
                <asp:Repeater ID="rptEspacios" runat="server" OnItemCommand="rptEspacios_ItemCommand">
                    <ItemTemplate>
                        <div class="col-md-4 mb-4">
                            <div class="card card-custom h-100">
                                <img src='<%# Eval("Imagen") %>' class="card-img-top" alt="Espacio" />
                                <div class="card-body">
                                    <h5 class="card-title"><%# Eval("Nombre") %></h5>
                                    <p class="card-text">Capacidad: <%# Eval("Capacidad") %> personas</p>
                                    <asp:Button runat="server" Text="Seleccionar" CssClass="btn btn-primary" 
                                        CommandName="Seleccionar" CommandArgument='<%# Eval("Id") %>' />
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>

        </div>
    <style>
    .banner {
    background-color: #007bff;
    color: white;
    padding: 30px;
    text-align: center;
    border-radius: 10px;
    margin-bottom: 20px;
}
    </style>
</asp:Content>

