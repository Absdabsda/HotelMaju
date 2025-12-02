<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReceptionistPage.aspx.cs" Inherits="hotel.ReceptionistPage" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Receptionist - Hotel Maju</title>
    <link href="Styles/login.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="panel">
            <h2>Gestión de clientes</h2>

            <div class="field">
                <label>UserID:</label>
                <asp:TextBox ID="txtUserID" runat="server" ReadOnly="true"></asp:TextBox>
                <span style="font-size:12px;color:gray;">(se rellena al seleccionar un cliente)</span>
            </div>

            <div class="field">
                <label>Username:</label>
                <asp:TextBox ID="txtUsername" runat="server"></asp:TextBox>
            </div>

            <div class="field">
                <label>Password:</label>
                <asp:TextBox ID="txtPassword" runat="server"></asp:TextBox>
            </div>

            <div class="field">
                <label>DOB:</label>
                <asp:TextBox ID="txtDOB" runat="server" Placeholder="YYYY-MM-DD"></asp:TextBox>
            </div>

            <div class="field">
                <label>Address:</label>
                <asp:TextBox ID="txtAddress" runat="server"></asp:TextBox>
            </div>

            <div class="field">
                <label>Mobile:</label>
                <asp:TextBox ID="txtMobile" runat="server"></asp:TextBox>
            </div>

            <div class="field">
                <label>Buscar:</label>
                <asp:TextBox ID="txtSearch" runat="server" Placeholder="nombre o móvil"></asp:TextBox>
                <asp:Button ID="btnSearchClient" runat="server" Text="Buscar" CssClass="btn" OnClick="btnSearchClient_Click" />
            </div>

            <asp:Button ID="btnCreateClient" runat="server" Text="Crear cliente" CssClass="btn" OnClick="btnCreateClient_Click" />
            <asp:Button ID="btnUpdateClient" runat="server" Text="Actualizar cliente" CssClass="btn" OnClick="btnUpdateClient_Click" />
            <asp:Button ID="btnDeleteClient" runat="server" Text="Eliminar cliente" CssClass="btn" OnClick="btnDeleteClient_Click" />

            <br /><br />
            <asp:Label ID="lblClientMsg" runat="server" ForeColor="Red"></asp:Label>

            <a
