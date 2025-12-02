<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClientPage.aspx.cs" Inherits="hotel.ClientPage" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Client Page</title>
    <link rel="stylesheet" href="Styles/ClientPage.css" />
</head>

<body>
    <form id="form1" runat="server">
        <div style="text-align:right; margin: 10px;">
    <asp:Button ID="btnLogout" runat="server" Text="Logout" CssClass="logout-btn" OnClick="btnLogout_Click" />
</div>


        <div class="container">

            <h2>Welcome!</h2>

            <div class="info-box">
                <asp:Label ID="lblName" runat="server"></asp:Label><br />
                <asp:Label ID="lblID" runat="server"></asp:Label><br />
                <asp:Label ID="lblDOB" runat="server"></asp:Label><br />
                <asp:Label ID="lblAddress" runat="server"></asp:Label><br />
                <asp:Label ID="lblMobile" runat="server"></asp:Label><br />
            </div>

            <h2>Your Reservations</h2>
            <asp:GridView ID="gvReservations" runat="server" CssClass="reservations-table"></asp:GridView>

        </div>

    </form>
</body>
</html>
