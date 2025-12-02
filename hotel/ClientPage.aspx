<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClientPage.aspx.cs" Inherits="hotel.ClientPage" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Client Page</title>
</head>
<body>
<form id="form1" runat="server">

    <h2>Your Information</h2>
    <asp:Label ID="lblName" runat="server" /><br />
    <asp:Label ID="lblID" runat="server" /><br />
    <asp:Label ID="lblDOB" runat="server" /><br />
    <asp:Label ID="lblAddress" runat="server" /><br />
    <asp:Label ID="lblMobile" runat="server" /><br />

    <h2>Your Reservations</h2>
    <asp:GridView ID="gvReservations" runat="server" AutoGenerateColumns="true"></asp:GridView>

</form>
</body>
</html>
