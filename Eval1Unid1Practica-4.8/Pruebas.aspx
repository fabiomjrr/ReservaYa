<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Pruebas.aspx.cs" Inherits="Eval1Unid1Practica_4._8.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- INTERFAZ RESERVA DE USUARIO. -->
    <div class="container-fluid mt-5">
        <div class="row">

            <div class="col-md-6">
                <h2 class="mb-4">Agregar Nueva Tarea</h2>
                <form id="form_nuevaTarea">
                    <div class="mb-3">
                        <asp:Label ID="Label1" runat="server" Text="Titulo" CssClass="form-label"></asp:Label>
                        <asp:TextBox ID="txb_titulo" runat="server" placeholder="Limpiar la casa" CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Titulo vacio!" ControlToValidate="txb_titulo" CssClass="text-danger"></asp:RequiredFieldValidator>                        
                    </div>

                    <div class="mb-3">
                        <asp:Label ID="Label2" runat="server" Text="Descripcion" CssClass="form-label"></asp:Label>
                        <asp:TextBox ID="txb_descripcion" runat="server" TextMode="MultiLine" CssClass="form-control" Rows="3"></asp:TextBox>
                    </div>

                    <div class="mb-3">
                        <asp:Label ID="Label3" runat="server" Text="Prioridad" CssClass="form-label"></asp:Label>
                        <asp:DropDownList ID="ddl_prioridades" runat="server" CssClass="form-select"></asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="RequiredFieldValidator" ControlToValidate="ddl_prioridades" CssClass="text-danger"></asp:RequiredFieldValidator>
                    </div>

                    <div class="mb-3">
                        <asp:Label ID="Label4" runat="server" Text="Fecha Limite" CssClass="form-label"></asp:Label>
                        <asp:TextBox ID="txb_fechaFinal" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                            ErrorMessage="La fecha es obligatoria" ControlToValidate="txb_fechaFinal" ForeColor="Red">
                        </asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="RangeValidator1" runat="server"
                            ErrorMessage="La fecha debe ser hoy o posterior" ControlToValidate="txb_fechaFinal"
                            Type="Date" ForeColor="Red">
                        </asp:RangeValidator>
                    </div>

                    <div>
                        <asp:Button ID="Button1" runat="server" Text="Agregar" CssClass="mt-4 btn btn-primary" OnClick="Button1_Click" />
                    </div>
                </form>
            </div>

            <div class="col-md-6">
                <h2 class="mb-4">Lista de Tareas</h2>
                <asp:GridView ID="gv_tareas" runat="server" CssClass="table table-bordered table-striped" AutoGenerateColumns="False">
                    <Columns>
                        <asp:TemplateField HeaderText="Seleccionar">
                            <ItemTemplate>
                                <asp:CheckBox ID="chk_finalizar" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Name" HeaderText="Nombre" />
                        <asp:BoundField DataField="Description" HeaderText="Descripcion" />
                        <asp:BoundField DataField="Created" HeaderText="Inicio" />
                        <asp:BoundField DataField="FinalDate" HeaderText="Fin" />
                    </Columns>
                </asp:GridView>

                <div class="d-flex flex-row">
                    <asp:Button ID="Button2" runat="server" Text="Agregar" CssClass="btn btn-primary" style="margin-right:10px"/>
                    <asp:Button ID="Button3" runat="server" Text="Eliminarf" CssClass="btn btn-secondary"/>
                </div>
            </div>

        </div>
    </div>


</asp:Content>
