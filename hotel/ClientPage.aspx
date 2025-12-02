<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClientPage.aspx.cs" Inherits="hotel.ClientPage" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Client Page</title>
    <link rel="stylesheet" href="Styles/ClientPage.css" />
</head>

<body>
    <form id="form1" runat="server">

        <asp:Panel ID="header" runat="server" CssClass="header">
            <span class="header-logo">MaJu Hotel</span>

            <asp:Button ID="btnLogout" runat="server"
                        Text="Logout"
                        CssClass="logout-btn"
                        OnClick="btnLogout_Click" />
        </asp:Panel>

        <div class="container">

            <h2>Your Information</h2>

            <div class="info-card">
                <h3>Personal Details</h3>
                <p><strong>Name:</strong> <asp:Label ID="lblName" runat="server" /></p>
                <p><strong>ID:</strong> <asp:Label ID="lblID" runat="server" /></p>
                <p><strong>Date of Birth:</strong> <asp:Label ID="lblDOB" runat="server" /></p>
                <p><strong>Address:</strong> <asp:Label ID="lblAddress" runat="server" /></p>
                <p><strong>Mobile:</strong> <asp:Label ID="lblMobile" runat="server" /></p>
            </div>

            <h2>Your Reservations</h2>

            <div class="info-card">
                <h3>Reservations</h3>
                <asp:GridView ID="gvReservations" runat="server" CssClass="reservations-table"></asp:GridView>
            </div>

        </div>

    </form>
</body>
</html>
