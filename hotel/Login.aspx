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
        <div class="login-container">
            <h2>Login - Maju Hotel</h2>
            <asp:TextBox ID="username" runat="server" CssClass="input" placeholder="Username"></asp:TextBox>
            <asp:TextBox ID="password" runat="server" TextMode="Password" CssClass="input" placeholder="Password"></asp:TextBox>
            <asp:Button ID="Button1" runat="server" Text="Login" CssClass="btn-login" OnClick="Button1_Click" />
            <asp:Label ID="Label1" runat="server" CssClass="error-label"></asp:Label>
        </div>
    </form>
</body>
</html>
