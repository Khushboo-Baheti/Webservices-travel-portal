<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            font-size: large;
            color: #0033CC;
            background-color: #FFFFFF;
        }
        #map {
        height: 400px;
        width: 500px;
       }
        .auto-style2 {
            width: 833px;
        }
    </style>
</head>
<body style="background-color:#FAEBD7;">

    <table>
    <tr>
    <td class="auto-style2">
    <form id="form1" runat="server">
        <p style="width: 440px">
            <strong><span class="auto-style1">Flight Information ...</span></strong></p>
        <p>
            Source(IATA code):
            <asp:TextBox ID="convert_Source" runat="server"></asp:TextBox>    
 
        </p>
        <p>
 
            Destination(IATA code):
            <asp:TextBox ID="convert_Destination" runat="server"></asp:TextBox>   
            
        </p>
        <p>
            
            Number of Seats:
            <asp:TextBox ID="seats" runat="server"></asp:TextBox>
        
        </p>
        <p>
        
            Journey Date(YYYYMMDD):
            <asp:TextBox ID="convert_Date" runat="server"></asp:TextBox>
        </p>
        <p>
            <asp:Button ID="convert_Submit" runat="server" OnClick="flightInfo" Text="Search Flights" />
        </p>
        <p>
                <asp:Button ID="hotel" runat="server" Text="Search for hotels" OnClick="hotel_Click" />
        </p>
    </form>
    </td>
    <td>
    <div id="map">
        <script type="text/javascript">
        var locations = <%=new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(mapItems) %>;

        function initMap() {
            var infowindow = new google.maps.InfoWindow();

            var map = new google.maps.Map(document.getElementById('map'), {
                zoom: 11,
                center: new google.maps.LatLng(41.9741625, -87.9073214)
            });

            setMarkers(map, locations, infowindow);
        }

        function setMarkers(map, locations, infowindow) {

            var marker, i;

            for (i = 0; i < locations.length; i++) {

                latlngset = new google.maps.LatLng(locations[i].latitude, locations[i].longitude);

                var marker = new google.maps.Marker({
                    map: map, title: locations[i].title, position: latlngset
                });
                map.setCenter(marker.getPosition())


                var content = "<h3>" + locations[i].title + "</h3>" + locations[i].description;

                google.maps.event.addListener(marker, 'click', (function (marker, content) {
                    return function () {
                        infowindow.close();
                        infowindow.setContent(content);
                        infowindow.open(map, marker);
                    };
                })(marker, content));

            }
        }
    </script>
    <script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyBB-20a_UTPAuR23u8Qq1H9eNMK6lnjlT8&callback=initMap">
    </script>
    </div>
    </td>
    </tr>
    <tr>
    <td class="auto-style2">
            <asp:Label ID="Your_Information" runat="server"></asp:Label>
    </td>
    <td>
            <asp:Label ID="Label1" runat="server" ForeColor="Black" Text="Hotel List with Ratings"></asp:Label>
    </td>
    </tr>
    </table>
    <p>
            <asp:Label ID="Prev_Information" runat="server"></asp:Label>
        </p>
    <p>
            <asp:Label ID="Next_Information" runat="server"></asp:Label>
        </p>
