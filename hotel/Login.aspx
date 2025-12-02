<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="hotel.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link href="Styles/Login.css" rel="stylesheet" />
    <title>Welcome</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Panel ID="navbar" runat="server" CssClass="navbar">
        <asp:Label ID="lblLogo" runat="server" Text="MaJu Hotel" CssClass="logo"></asp:Label>
            <ul class="nav-links">
                <li><asp:HyperLink runat="server" NavigateUrl="~/LandingPage.aspx" Text="Home" /></li>
                <li><a href="#rooms">Rooms</a></li>
                <li><a href="#about">About</a></li>
                <li><a href="#location">Location</a></li>
                <li><asp:HyperLink runat="server" CssClass="login-btn" NavigateUrl="~/Login.aspx" Text="Login" /></li>
            </ul>
        </asp:Panel>

        <div class="login-container">
            <h2>Login - Maju Hotel</h2>
            <asp:TextBox ID="username" runat="server" CssClass="input" placeholder="Username"></asp:TextBox>
            <asp:TextBox ID="password" runat="server" TextMode="Password" CssClass="input" placeholder="Password"></asp:TextBox>
            <asp:Button ID="Button1" runat="server" Text="Login" CssClass="btn-login" OnClick="Button1_Click" />
            <asp:Label ID="Label1" runat="server" CssClass="error-label"></asp:Label>
        </div>

        <footer class="footer">
            <p>© 2025 MaJu Hotel — All Rights Reserved</p>
            <p>Contact: info@majuhotel.com · +34 600 123 456</p>
        </footer>
    </form>
</body>
</html>
