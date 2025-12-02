<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReceptionistPage.aspx.cs" Inherits="hotel.ReceptionistPage" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Receptionist - Hotel Maju</title>
    <link href="Styles/Receptionist.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="page-wrapper">

        <asp:Button ID="btnLogout" runat="server"  Text="Logout" CssClass="btn btn-primary" OnClick="btnLogout_Click" />

        <h1 class="page-title">Panel de Recepción</h1>

        <!-- CLIENTES -->
        <div class="panel">
            <h2>Clientes</h2>

            <div class="panel-fields">
                <div class="form-field">
                    <label>Número identificativo</label>
                    <asp:TextBox ID="txtClientID" runat="server" />
                </div>

                <div class="form-field">
                    <label>Nombre</label>
                    <asp:TextBox ID="txtName" runat="server" />
                </div>

                <div class="form-field">
                    <label>Fecha de nacimiento</label>
                    <asp:TextBox ID="txtDOB" runat="server" TextMode="Date" />
                </div>

                <div class="form-field">
                    <label>Dirección</label>
                    <asp:TextBox ID="txtAddress" runat="server" />
                </div>

                <div class="form-field">
                    <label>Teléfono</label>
                    <asp:TextBox ID="txtMobile" runat="server" />
                </div>
            </div>

            <asp:Button ID="btnAddClient" runat="server"
                Text="Crear cliente" CssClass="btn btn-primary"
                OnClick="btnAddClient_Click" />

            <asp:Button ID="btnUpdateClient" runat="server"
                Text="Actualizar cliente" CssClass="btn btn-primary"
                OnClick="btnUpdateClient_Click" />

            <asp:Button ID="btnDeleteClient" runat="server"
                Text="Eliminar cliente" CssClass="btn btn-primary"
                OnClick="btnDeleteClient_Click" />

            <asp:Button ID="btnFindClient" runat="server"
                Text="Buscar cliente" CssClass="btn btn-primary"
                OnClick="btnFindClient_Click" />

            <asp:Label ID="lblClientMsg" runat="server" CssClass="status-label" />
        </div>

        <hr class="section-divider" />

        <!-- RESERVAS -->
        <div class="panel">
            <h2>Reservas</h2>

            <div class="panel-fields">
                <div class="form-field">
                    <label>Cliente</label>
                    <asp:DropDownList ID="ddlUsers" runat="server" />
                </div>

                <div class="form-field">
                    <label>Fecha de llegada</label>
                    <asp:TextBox ID="txtArrival" runat="server" TextMode="Date" />
                </div>

                <div class="form-field">
                    <label>Fecha de ida</label>
                    <asp:TextBox ID="txtDeparture" runat="server" TextMode="Date" />
                </div>

                <div class="form-field">
                    <label>Tipo de habitación</label>
                    <asp:DropDownList ID="ddlRoomType" runat="server">
                        <asp:ListItem>Individual</asp:ListItem>
                        <asp:ListItem>Doble</asp:ListItem>
                        <asp:ListItem>Suite</asp:ListItem>
                    </asp:DropDownList>
                </div>

                <div class="form-field">
                    <label>Número de habitación (opcional)</label>
                    <asp:TextBox ID="txtRoomID" runat="server" />
                </div>
            </div>

            <asp:Button ID="btnAddReservation" runat="server"
                Text="Crear reserva" CssClass="btn btn-primary"
                OnClick="btnAddReservation_Click" />

            <asp:Button ID="btnUpdateReservation" runat="server"
                Text="Actualizar reserva" CssClass="btn btn-primary"
                OnClick="btnUpdateReservation_Click" />

            <asp:Button ID="btnDeleteReservation" runat="server"
                Text="Eliminar reserva" CssClass="btn btn-primary"
                OnClick="btnDeleteReservation_Click" />

            <asp:Button ID="btnFindReservation" runat="server"
                Text="Buscar reserva" CssClass="btn btn-primary"
                OnClick="btnFindReservation_Click" />

            <asp:Label ID="lblReservationMsg" runat="server" CssClass="status-label" />
        </div>
    </div>
</form>
</body>
</html>