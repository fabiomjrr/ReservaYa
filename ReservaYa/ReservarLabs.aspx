<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ReservarLabs.aspx.cs" Inherits="ReservaYa.ReservarLabs" %>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container my-5">
        <section class="banner">
            <h1>Publica tu espacio con nosotros</h1>
        </section>

        <!-- Formulario Espacios -->
        <div class="card p-4 mb-5 shadow-sm">
            <div class="row g-3">
                <!-- Nombre -->
                <div class="col-md-6">
                    <asp:Label runat="server" AssociatedControlID="txtNombre" Text="Nombre" CssClass="form-label"></asp:Label>
                    <asp:TextBox runat="server" ID="txtNombre" CssClass="form-control" placeholder="Nombre del establecimiento" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNombre"
                        ErrorMessage="El nombre es requerido" CssClass="text-danger" Display="Dynamic" />
                </div>

                <!-- Categoría -->
                <div class="col-md-6">
                    <asp:Label runat="server" AssociatedControlID="ddlCategoria" Text="Categoría" CssClass="form-label"></asp:Label>
                    <asp:DropDownList runat="server" ID="ddlCategoria" CssClass="form-select">
                        <asp:ListItem Text="-- Seleccione --" Value="" />                       
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlCategoria"
                        InitialValue="" ErrorMessage="Seleccione una categoría" CssClass="text-danger" Display="Dynamic" />
                </div>

                <!-- Capacidad -->
                <div class="col-md-6">
                    <asp:Label runat="server" AssociatedControlID="txtCapacidad" Text="Capacidad" CssClass="form-label"></asp:Label>
                    <asp:TextBox runat="server" ID="txtCapacidad" TextMode="Number" CssClass="form-control" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCapacidad"
                        ErrorMessage="La capacidad es requerida" CssClass="text-danger" Display="Dynamic" />
                    <asp:RangeValidator runat="server" ControlToValidate="txtCapacidad"
                        MinimumValue="1" MaximumValue="10000" Type="Integer"
                        ErrorMessage="Ingrese un número válido" CssClass="text-danger" />
                </div>

                <!-- Dirección -->
                <div class="col-md-6">
                    <asp:Label runat="server" AssociatedControlID="txtDireccion" Text="Dirección" CssClass="form-label"></asp:Label>
                    <asp:TextBox runat="server" ID="txtDireccion" CssClass="form-control" />
                </div>

                <!-- Ubicación Enlace -->
                <div class="col-md-12">
                    <asp:Label runat="server" AssociatedControlID="txtUbicacion" Text="Ubicación (Google Maps)" CssClass="form-label"></asp:Label>
                    <asp:TextBox runat="server" ID="txtUbicacion" CssClass="form-control" TextMode="Url" />
                </div>

                <!-- Imagen -->
                <div class="col-md-12">
                    <asp:Label runat="server" AssociatedControlID="fuImagen" Text="Imagen" CssClass="form-label"></asp:Label>
                    <asp:FileUpload runat="server" ID="fuImagen" CssClass="form-control" />
                </div>

                <!-- Disponible -->
                <div class="col-md-12 mt-3">
                    <asp:CheckBox runat="server" ID="chkDisponible" CssClass="form-check-input" />
                    <label for="chkDisponible" class="form-check-label ms-2">Disponible</label>
                </div>

                <!-- Comodidades -->
                <div class="col-md-12 mt-3">
                    <h5 class="mb-2">Comodidades</h5>
                    <div class="form-check">
                        <asp:CheckBox runat="server" ID="chkEstacionamiento" CssClass="form-check-input" />
                        <label for="chkEstacionamiento" class="form-check-label ms-2">Estacionamiento</label>
                    </div>
                    <div class="form-check">
                        <asp:CheckBox runat="server" ID="chkSanitarios" CssClass="form-check-input" />
                        <label for="chkSanitarios" class="form-check-label ms-2">Sanitarios</label>
                    </div>
                    <div class="form-check">
                        <asp:CheckBox runat="server" ID="chkAcceso" CssClass="form-check-input" />
                        <label for="chkAcceso" class="form-check-label ms-2">Acceso Silla de Ruedas</label>
                    </div>
                </div>
            </div>

            <!-- Botón -->
            <div class="text-end mt-3">
                <asp:Button runat="server" ID="btnGuardar" Text="Guardar Espacio" CssClass="btn btn-primary px-4" />
            </div>
        </div>




        <!-- Grid de Espacios -->
        <div class="row">
            <header>
                <h2>Espacios Activos-Disponibles</h2>
            </header>
            <asp:Repeater ID="rptEspacios" runat="server" OnItemCommand="rptEspacios_ItemCommand">
                <ItemTemplate>
                    <div class="col-md-4 mb-4">
                        <div class="card card-custom h-100">
                            <img src='<%# "Content\\Img\\"+Eval("ImagenPrev") %>' class="card-img-top" alt="Espacio" />
                            <div class="card-body">
                                <h5 class="card-title"><%# Eval("Nombre") %></h5>
                                <p class="card-text">Capacidad: <%# Eval("Capacidad") %> personas</p>
                                <asp:Button runat="server" Text="Seleccionar" CssClass="btn btn-primary"
                                    CommandName="Seleccionar" CommandArgument='<%# Eval("EspacioID") %>' />
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

