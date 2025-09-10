<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="RegistroReservaYa.aspx.cs" Inherits="ReservaYa.Login" %>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <!-- CONTENIDO PRINCIPAL -->
      <div class="form-box">
             <h2>
                <img src="Content/Img/ReservaYaLogo.png" alt="ReservaYaLogo" style="height:277px; vertical-align:middle; margin-right:10px; margin-left: 0px; width: 342px;" />
                Registrarse
                </h2>

            <asp:Label ID="lblNombre" runat="server" Text="Nombre:" AssociatedControlID="txtNombre"></asp:Label><br />
            <asp:TextBox ID="txtNombre" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                ControlToValidate="txtNombre" ErrorMessage="El nombre es obligatorio" ForeColor="Red" />
            <br /><br />

            <asp:Label ID="lblApellido" runat="server" Text="Apellido: " AssociatedControlID="txtNombre"></asp:Label><br />
            <asp:TextBox ID="txtApellido" runat="server"></asp:TextBox>
            <br />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtApellido" ErrorMessage="El apellido es obligatorio" ForeColor="Red"></asp:RequiredFieldValidator>
            <br />
            <asp:Label ID="lblEmail" runat="server" Text="Correo Electrónico:" AssociatedControlID="txtEmail"></asp:Label><br />
            <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                ControlToValidate="txtEmail" ErrorMessage="El correo es obligatorio" ForeColor="Red" />
            <asp:RegularExpressionValidator ID="RegexEmail" runat="server"
                ControlToValidate="txtEmail"
                ErrorMessage="Formato de correo inválido"
                ForeColor="Red"
                ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" />
            <br /><br />

            <asp:Label ID="lblContrasenia" runat="server" Text="Contraseña:" AssociatedControlID="txtContrasenia"></asp:Label><br />
            <asp:TextBox ID="txtContrasenia" runat="server" TextMode="Password"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                ControlToValidate="txtContrasenia" ErrorMessage="La contraseña es obligatoria" ForeColor="Red" />
            <br /><br />

            <asp:Button ID="btnRegistrar" runat="server" Text="Registrarte" CssClass="btn-custom"/><br />
            <asp:HyperLink ID="hlRegistro" runat="server" NavigateUrl="~/LoginReservaYa.aspx" Text="¿Ya tienes una cuenta?" ForeColor="#0066FF" />
             <br />

            <asp:Label ID="lblMensaje" runat="server"></asp:Label>
        </div>
        <style>
        body {
    font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
    background: url("Content/Img/Fondo.jpeg") no-repeat center center fixed;
    background-size: cover;
    margin: 0;
    padding: 0;
}

.form-box {
    width: 380px;
    margin: 80px auto;
    background: #fff;
    padding: 25px 30px;
    border-radius: 15px;
    box-shadow: 0px 6px 20px rgba(0,0,0,0.25);
    animation: fadeIn 1s ease-in-out;
}

    .form-box h2 {
        text-align: center;
        color: #4A00E0;
        margin-bottom: 20px;
    }

    .form-box label {
        font-weight: bold;
        color: #333;
        display: block;
        margin-bottom: 5px;
    }

    .form-box input[type="text"],
    .form-box input[type="email"],
    .form-box input[type="password"] {
        width: 100%;
        padding: 10px;
        margin-bottom: 15px;
        border: 2px solid #ddd;
        border-radius: 8px;
        transition: 0.3s;
    }

    .form-box input:focus {
        border-color: #8E2DE2;
        outline: none;
        box-shadow: 0px 0px 8px rgba(142, 45, 226, 0.4);
    }

.btn-custom {
    width: 100%;
    padding: 12px;
    background: linear-gradient(135deg, #4A00E0, #8E2DE2);
    color: #fff;
    border: none;
    border-radius: 8px;
    font-weight: bold;
    cursor: pointer;
    transition: 0.3s;
}

    .btn-custom:hover {
        background: linear-gradient(135deg, #8E2DE2, #4A00E0);
        transform: translateY(-2px);
        box-shadow: 0px 5px 12px rgba(0,0,0,0.2);
    }

#hlRegistro {
    text-align: center;
    display: block;
    margin-top: 15px;
    font-weight: bold;
}

#lblMensaje {
    text-align: center;
    display: block;
    margin-top: 15px;
    font-weight: bold;
}

@keyframes fadeIn {
    from {
        opacity: 0;
        transform: translateY(-20px);
    }

    to {
        opacity: 1;
        transform: translateY(0);
    }
}


    </style>
</asp:Content>
