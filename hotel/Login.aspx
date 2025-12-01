<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="hotel.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Login</title>
</head>
<body style="height: 178px">
    <form id="form1" runat="server">
        <div style="height: 109px">
            <h1>Login</h1>
            <asp:Label ID="Label1" runat="server" Text="Username: "></asp:Label>
            <asp:TextBox ID="name" runat="server" OnTextChanged="username_TextChanged"></asp:TextBox>
            <asp:Label ID="Label2" runat="server" Text="Password: "></asp:Label>
            <asp:TextBox ID="password" runat="server" TextMode="Password" OnTextChanged="password_TextChanged"></asp:TextBox>
            <asp:Button ID="Button1" runat="server" Text="Enter" OnClick="Button1_Click" />
        </div>
    </form>
</body>
</html>
