<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="ReceptionistPage.aspx.cs"
    Inherits="hotel.ReceptionistPage"
    MaintainScrollPositionOnPostBack="true" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Receptionist - Hotel Maju</title>
    <link href="Styles/Receptionist.css" rel="stylesheet" />
</head>

<body>
    <form id="form1" runat="server">

        <!-- ======================= -->
        <!--       NAVBAR/HEADER     -->
        <!-- ======================= -->
        <asp:Panel ID="navbar" runat="server" CssClass="navbar">
            <div class="navbar-content">
                <asp:Label ID="lblLogo" runat="server" Text="MaJu Hotel" CssClass="logo"></asp:Label>

                <asp:Button ID="btnLogout"
                            runat="server"
                            Text="Logout"
                            CssClass="logout-btn"
                            OnClick="btnLogout_Click" />
            </div>
        </asp:Panel>


        <div class="page-wrapper">

            <!-- ======================================================= -->
            <!--           CLIENT SEARCH PANEL (Search client)           -->
            <!-- ======================================================= -->
            <div class="panel">
                <h2>Search client</h2>
                <p class="panel-subtitle">
                    Search clients by name or ID, then select one to manage their data and reservations.
                </p>

                <div class="panel-fields">
                    <div class="form-field">
                        <label for="txtSearchClient">Search</label>
                        <asp:TextBox ID="txtSearchClient" runat="server" placeholder="Ana, 1001, etc." />
                    </div>

                    <div class="form-field">
                        <label>&nbsp;</label>
                        <asp:Button ID="btnSearchClient" runat="server"
                                    Text="Search"
                                    CssClass="btn btn-primary"
                                    OnClick="btnSearchClient_Click" />
                    </div>
                </div>

                <!-- Tabla de resultados de búsqueda -->
                <asp:GridView ID="gvClients" runat="server"
                              CssClass="reservations-table"
                              AutoGenerateColumns="False"
                              GridLines="None"
                              BorderStyle="None"
                              HeaderStyle-CssClass="table-header"
                              RowStyle-CssClass="table-row"
                              AlternatingRowStyle-CssClass="table-row-alt"
                              SelectedRowStyle-CssClass="table-selected"
                              OnSelectedIndexChanged="gvClients_SelectedIndexChanged">

                    <Columns>
                        <asp:BoundField DataField="UserID" HeaderText="Internal ID" />
                        <asp:BoundField DataField="ClientID" HeaderText="Client ID" />
                        <asp:BoundField DataField="Name" HeaderText="Name" />
                        <asp:BoundField DataField="Mobile" HeaderText="Phone" />
                        <asp:CommandField ShowSelectButton="True" SelectText="Select" />
                    </Columns>
                </asp:GridView>

                <asp:Label ID="lblSearchMsg" runat="server" CssClass="status-label" />
            </div>



            <!-- ======================================================= -->
            <!--                   CLIENT INFORMATION PANEL              -->
            <!-- ======================================================= -->
            <div class="panel">
                <h2>Client information</h2>

                <!-- quién está seleccionado ahora mismo -->
                <asp:Label ID="lblSelectedClient" runat="server"
                           CssClass="status-label"
                           Text="No client selected yet." />

                <div class="panel-fields">

                    <div class="form-field">
                        <label for="txtClientID">Client ID</label>
                        <asp:TextBox ID="txtClientID" runat="server" />
                    </div>

                    <div class="form-field">
                        <label for="txtName">Full Name</label>
                        <asp:TextBox ID="txtName" runat="server" />
                    </div>

                    <div class="form-field">
                        <label for="txtDOB">Birth date</label>
                        <asp:TextBox ID="txtDOB" runat="server" TextMode="Date" />
                    </div>

                    <div class="form-field">
                        <label for="txtAddress">Address</label>
                        <asp:TextBox ID="txtAddress" runat="server" />
                    </div>

                    <div class="form-field">
                        <label for="txtMobile">Phone</label>
                        <asp:TextBox ID="txtMobile" runat="server" />
                    </div>

                </div>

                <div class="button-row">
                    <asp:Button ID="btnAddClient" runat="server"
                                Text="Create"
                                CssClass="btn btn-primary"
                                OnClick="btnAddClient_Click" />

                    <asp:Button ID="btnUpdateClient" runat="server"
                                Text="Update"
                                CssClass="btn btn-primary"
                                OnClick="btnUpdateClient_Click" />

                    <asp:Button ID="btnDeleteClient" runat="server"
                                Text="Delete"
                                CssClass="btn btn-primary"
                                OnClick="btnDeleteClient_Click" />
                </div>

                <asp:Label ID="lblClientMsg" runat="server" CssClass="status-label" />
            </div>



            <!-- ======================================================= -->
            <!--                    RESERVATIONS PANEL                   -->
            <!-- ======================================================= -->
            <div class="panel">
                <h2>Reservations</h2>
                <p class="panel-subtitle">
                    Add, edit or remove reservations for the selected client.
                </p>

                <!-- Campos de reserva -->
                <div class="panel-fields">

                    <div class="form-field">
                        <label for="txtArrival">Arrival date</label>
                        <asp:TextBox ID="txtArrival" runat="server" TextMode="Date" />
                    </div>

                    <div class="form-field">
                        <label for="txtDeparture">Departure date</label>
                        <asp:TextBox ID="txtDeparture" runat="server" TextMode="Date" />
                    </div>

                    <div class="form-field">
                        <label for="ddlRoomType">Room type</label>
                        <asp:DropDownList ID="ddlRoomType" runat="server">
                            <asp:ListItem>Individual</asp:ListItem>
                            <asp:ListItem>Doble</asp:ListItem>
                            <asp:ListItem>Suite</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                </div>

                <!-- Tabla de reservas del cliente seleccionado -->
                <asp:GridView ID="gvReservations" runat="server"
                              CssClass="reservations-table"
                              AutoGenerateColumns="False"
                              GridLines="None"
                              BorderStyle="None"
                              HeaderStyle-CssClass="table-header"
                              RowStyle-CssClass="table-row"
                              AlternatingRowStyle-CssClass="table-row-alt"
                              SelectedRowStyle-CssClass="table-selected"
                              OnSelectedIndexChanged="gvReservations_SelectedIndexChanged">

                    <Columns>
                        <asp:BoundField DataField="ReservationID" HeaderText="ID" />
                        <asp:BoundField DataField="ArrivalDate" HeaderText="Arrival" />
                        <asp:BoundField DataField="DepartureDate" HeaderText="Departure" />
                        <asp:BoundField DataField="RoomType" HeaderText="Room Type" />
                        <asp:CommandField ShowSelectButton="True" SelectText="Select" />
                    </Columns>
                </asp:GridView>

                <!-- Botones de CRUD de reserva -->
                <div class="button-row">
                    <asp:Button ID="btnAddReservation" runat="server"
                                Text="Create"
                                CssClass="btn btn-primary"
                                OnClick="btnAddReservation_Click" />

                    <asp:Button ID="btnUpdateReservation" runat="server"
                                Text="Update"
                                CssClass="btn btn-primary"
                                OnClick="btnUpdateReservation_Click" />

                    <asp:Button ID="btnDeleteReservation" runat="server"
                                Text="Delete"
                                CssClass="btn btn-primary"
                                OnClick="btnDeleteReservation_Click" />
                </div>

                <asp:Label ID="lblReservationMsg" runat="server" CssClass="status-label" />
            </div>

        </div><!-- page-wrapper -->

    </form>
</body>
</html>
