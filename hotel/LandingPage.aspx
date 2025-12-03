<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LandingPage.aspx.cs" Inherits="hotel.LandingPage" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>MaJu Family Hotel</title>
    <link rel="stylesheet" href="Styles/Landing.css" />
</head>

<body>

    <form id="form1" runat="server">

        <asp:Panel ID="navbar" runat="server" CssClass="navbar">
            <asp:Label ID="lblLogo" runat="server" Text="MaJu Hotel" CssClass="logo"></asp:Label>
            <div class="hamburger" onclick="toggleMenu()">☰</div>
            <div class="nav-links-container">
                <ul class="nav-links">
                    <li><asp:HyperLink runat="server" NavigateUrl="~/LandingPage.aspx" Text="Home" /></li>
                    <li><a href="#rooms">Rooms</a></li>
                    <li><a href="#about">About</a></li>
                    <li><a href="#location">Location</a></li>
                    <li><asp:HyperLink runat="server" CssClass="login-btn" NavigateUrl="~/Login.aspx" Text="Login" /></li>
                </ul>
            </div>
        </asp:Panel>

        <asp:Panel ID="hero" runat="server" CssClass="hero-panel">
            <asp:Image ID="HeroImg" runat="server" CssClass="hero-img" ImageUrl="~/Img/gato.jpg" />
            <div class="hero-text">
                <h1>Welcome to MaJu Hotel</h1>
                <p>Your cozy family escape surrounded by nature.</p>
                <asp:HyperLink runat="server" CssClass="cta-btn" NavigateUrl="~/Login.aspx" Text="Book Now" />
            </div>
        </asp:Panel>

        <section id="about" class="info-section">
            <h2>About Us</h2>
            <div class="info-cards">
                <asp:Panel runat="server" CssClass="info-card">
                    <asp:Image runat="server" CssClass="card-img"
                        ImageUrl="~/Img/cama.jpg" />
                    <h3>Comfortable Rooms</h3>
                    <p>Enjoy spacious family rooms with warm natural décor and cozy lighting.</p>
                </asp:Panel>

                <asp:Panel runat="server" CssClass="info-card">
                    <asp:Image runat="server" CssClass="card-img"
                        ImageUrl="~/Img/familia.jpg"/>
                    <h3>Family-Friendly Services</h3>
                    <p>We offer breakfast, kid-friendly activities, babysitting and lounge areas.</p>
                </asp:Panel>

                <asp:Panel runat="server" CssClass="info-card">
                    <asp:Image runat="server" CssClass="card-img"
                        ImageUrl="~/Img/nature.jpg"/>
                    <h3>Nature & Peace</h3>
                    <p>Surrounded by gardens and trees, enjoy calm and serenity every moment.</p>
                </asp:Panel>
            </div>
        </section>

        <section id="rooms" class="rooms-alt-section">
            <h2>Our Rooms</h2>
            <div class="room-block">
                <img src="Img/bed.jpg" class="room-img" />
                <div class="room-info">
                    <h3>Standard Room</h3>
                    <p>Cozy double room with private bathroom and garden views.</p>
                </div>
            </div>
            
            <div class="room-block reverse">
                <img src="Img/suite.jpg" class="room-img" />
                <div class="room-info">
                    <h3>Family Suite</h3>
                    <p>Perfect for families — two rooms, plenty of space and comfort.</p>
                </div>
            </div>
            
            <div class="room-block">
                <img src="Img/deluxe.jpg" class="room-img" />
                <div class="room-info">
                    <h3>Eco Deluxe</h3>
                    <p>Soft lighting, natural wood and a relaxing environment.</p>
                </div>
            </div>
        </section>

        <section id="location" class="location-section">
            <h2>Our Location</h2>
            <div class="map-container">
                <iframe 
                    width="100%" 
                    height="450" 
                    style="border:0; border-radius:12px;"
                    loading="lazy" 
                    allowfullscreen 
                    referrerpolicy="no-referrer-when-downgrade"
                    src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3111.775673585103!2d-0.16728582410516847!3d38.99533500016286!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0xd61a3c020e5cf0f%3A0x903f6f2f25b98e65!2sGrau%20i%20Platja%2C%20Gandia!5e0!3m2!1ses!2ses!4v1701539465140!5m2!1ses!2ses">
                </iframe>
            </div>

            <p class="map-text">
                Located in the beautiful Grau de Gandia, just steps away from the beach,
                restaurants, and family-friendly activities.
            </p>
        </section>

        <footer class="footer">
            <p>© 2025 MaJu Hotel — All Rights Reserved</p>
            <p>Contact: info@majuhotel.com · +34 600 123 456</p>
        </footer>

        <script>
            function toggleMenu() {
                document.querySelector(".nav-links-container").classList.toggle("open");
            }
        </script>

    </form>

</body>
</html>
