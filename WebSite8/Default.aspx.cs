using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json.Linq;
using System.Xml;

public partial class _Default : System.Web.UI.Page
{
    protected List<MapData> mapItems;
    protected void Page_Load(object sender, EventArgs e)
    {
        mapItems = new List<MapData>();
    }
    protected void flightInfo(object sender, EventArgs e)
    {
        HttpWebRequest serviceRequest = (HttpWebRequest)WebRequest.Create("https://developer.goibibo.com/api/search/?app_id=68631801&app_key=a551de3cd320a8197e0dc9fc0ad31a66&format=json&source=" + convert_Source.Text.Trim().ToUpper() + "&destination=" + convert_Destination.Text.Trim().ToUpper() + "&dateofdeparture=" + convert_Date.Text.Trim() + "&seatingclass=E&adults=" + seats.Text.Trim() +"&children=0&infants=0&counter=100");
        serviceRequest.Method = "GET";
        serviceRequest.Accept = "text/plain";
        HttpWebResponse serviceResponse = (HttpWebResponse)serviceRequest.GetResponse();
        //Debug.WriteLine("Error Code: {0}", (int)serviceGETResponse.StatusCode);
        var departureTime = new object();
        var durationOfTravel = new object();
        var destination1 = new object();
        var totalFare = new object();
        var aline = new object();
        var departureTime2 = new object();
        var durationOfTravel2 = new object();
        var destination2 = new object();
        var finalMessage = new object();
        int finalFare = 0;
        bool hasConnectingFlight = false;
        int minFare = 0;
        int array_size = 10;
        finalMessage = "";

        if ((int)serviceResponse.StatusCode == 200)
        {
            Stream receiveGETStream = serviceResponse.GetResponseStream();
            Encoding GETencode = System.Text.Encoding.GetEncoding("utf-8");
            // Pipes the stream to a higher level stream reader with the required encoding format.
            StreamReader readGETStream = new StreamReader(receiveGETStream, GETencode, true);
            // output on the Console ...
            string serviceGETResult = readGETStream.ReadToEnd();
            JObject myOutput = JObject.Parse(serviceGETResult);
            var myData = myOutput["data"];
            var errorExists = myData["Error"];
            if (errorExists != null)
            {
                Debug.WriteLine(errorExists.ToString());
                finalMessage = errorExists;
            }
            else
            {
                try
                {
                    for (int i = 0; i < array_size; i++)
                    {
                        var myFlightData = myData["onwardflights"][i];
                        departureTime = myFlightData["deptime"];
                        durationOfTravel = myFlightData["duration"];
                        destination1 = myFlightData["destination"];
                        aline = myFlightData["airline"];
                        totalFare = myFlightData["fare"]["adulttotalfare"];
                        finalFare = int.Parse(totalFare.ToString()) / 65;
                        try
                        {
                            var myFlightData2 = myFlightData["onwardflights"][0];
                            if (myFlightData2 != null)
                            {
                                departureTime2 = myFlightData2["deptime"];
                                durationOfTravel2 = myFlightData2["duration"];
                                destination2 = myFlightData2["destination"];
                                hasConnectingFlight = true;
                            }
                        }
                        catch (Exception ex)
                        {

                        }

                        if (hasConnectingFlight)

                        {
                            finalMessage += "<br />" + "<br />" + "Airlines: " + aline.ToString() + "<br />" + "From: " + convert_Source.Text.Trim().ToUpper() + "-->" + " To: " + destination1.ToString().ToUpper() + " Departure Time:" + departureTime.ToString() + "<br />" + "From: " + destination1.ToString().ToUpper() + " -->" + " To: " + convert_Destination.Text.Trim().ToUpper() + " Departure Time: " + departureTime2.ToString() + "<br />" + "Total Fare:  $" + finalFare.ToString();
                        }
                        else
                        {
                            finalMessage += "<br />" + "<br />" + "Airlines: " + aline.ToString() + "<br />" + "From: " + convert_Source.Text.Trim().ToUpper() + "-->" + " To: " + destination1.ToString().ToUpper() + " Departure Time:" + departureTime.ToString() + "<br />" + "Total Fare:  $" + finalFare.ToString();
                        }
                    }
                } catch (Exception ex)
                {

                }
                
                
            }


        }
        else
        {
            //Debug.WriteLine("Error Code: {0}", (int)serviceGETResponse.StatusCode);
            finalMessage = "Error Code:" + serviceResponse.StatusCode.ToString();
        }

        Your_Information.Text = finalMessage.ToString();

        //Minimum Fare on previous day

        DateTime date = DateTime.ParseExact(convert_Date.Text, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);

        DateTime yesterday = date.AddDays(-1);

        var finalMessage_prev = new object();
        string yesterdayStr = yesterday.Year.ToString() + yesterday.Month.ToString().PadLeft(2, '0') + yesterday.Day.ToString().PadLeft(2, '0');


        if (yesterday <= DateTime.Now)
        {
            finalMessage_prev = "";
        }
        else
        {
            HttpWebRequest serviceRequestPrev = (HttpWebRequest)WebRequest.Create("https://developer.goibibo.com/api/search/?app_id=31d7e5ed&app_key=f5b3db741f9221097dadad17cd4cd98f&format=json&source=" + convert_Source.Text.Trim().ToUpper() + "&destination=" + convert_Destination.Text.Trim().ToUpper() + "&dateofdeparture=" + yesterdayStr.Trim() + "&seatingclass=E&adults=" + seats.Text.Trim() + "&children=0&infants=0&counter=100");
            serviceRequestPrev.Method = "GET";
            serviceRequestPrev.Accept = "text/plain";
            HttpWebResponse serviceResponse_prev = (HttpWebResponse)serviceRequestPrev.GetResponse();

            if ((int)serviceResponse_prev.StatusCode == 200)
            {
                Stream receiveGETStream = serviceResponse_prev.GetResponseStream();
                Encoding GETencode = System.Text.Encoding.GetEncoding("utf-8");
                // Pipes the stream to a higher level stream reader with the required encoding format.
                StreamReader readGETStream = new StreamReader(receiveGETStream, GETencode, true);
                // output on the Console ...
                string serviceGETResult = readGETStream.ReadToEnd();
                JObject myOutput = JObject.Parse(serviceGETResult);
                var myData = myOutput["data"];
                var errorExists = myData["Error"];
                if (errorExists != null)
                {
                    Debug.WriteLine(errorExists.ToString());
                    finalMessage_prev = errorExists;
                }
                {
                    try
                    {
                        //var myFlightData = myData["onwardflights"][0];
                        //totalFare = myFlightData["fare"]["adulttotalfare"];
                        var myFlightData = myData["onwardflights"][0];
                        totalFare = myFlightData["fare"]["adulttotalfare"];
                        minFare = int.Parse(totalFare.ToString()) / 65;

                        for (int i = 1; i < array_size; i++)
                        {
                            myFlightData = myData["onwardflights"][i];
                            totalFare = myFlightData["fare"]["adulttotalfare"];
                            finalFare = int.Parse(totalFare.ToString()) / 65;
                            if (minFare > finalFare)
                            {
                                minFare = finalFare;
                            }
                        }
                        finalMessage_prev = "Minimum Fare on previous day:  $" + minFare.ToString();
                    }
                    catch (Exception ex)
                    {

                    }
                }

            }
            else
            {
                finalMessage_prev = "Error Code for previous day:" + serviceResponse.StatusCode.ToString();
            }
        }

        Prev_Information.Text = finalMessage_prev.ToString();

        //Next day minimum fare

        DateTime tomorrow = date.AddDays(1);

        var finalMessage_next = new object();
        string tomorrowStr = tomorrow.Year.ToString() + tomorrow.Month.ToString().PadLeft(2, '0') + tomorrow.Day.ToString().PadLeft(2, '0');

        if (yesterday <= DateTime.Now)
        {
            finalMessage_prev = "";
        }
        else
        {
            HttpWebRequest serviceRequestPrev = (HttpWebRequest)WebRequest.Create("https://developer.goibibo.com/api/search/?app_id=31d7e5ed&app_key=f5b3db741f9221097dadad17cd4cd98f&format=json&source=" + convert_Source.Text.Trim().ToUpper() + "&destination=" + convert_Destination.Text.Trim().ToUpper() + "&dateofdeparture=" + tomorrowStr.Trim() + "&seatingclass=E&adults=" + seats.Text.Trim() + "&children=0&infants=0&counter=100");
            serviceRequestPrev.Method = "GET";
            serviceRequestPrev.Accept = "text/plain";
            HttpWebResponse serviceResponse_prev = (HttpWebResponse)serviceRequestPrev.GetResponse();
            if ((int)serviceResponse_prev.StatusCode == 200)
            {
                Stream receiveGETStream = serviceResponse_prev.GetResponseStream();
                Encoding GETencode = System.Text.Encoding.GetEncoding("utf-8");
                // Pipes the stream to a higher level stream reader with the required encoding format.
                StreamReader readGETStream = new StreamReader(receiveGETStream, GETencode, true);
                // output on the Console ...
                string serviceGETResult = readGETStream.ReadToEnd();
                JObject myOutput = JObject.Parse(serviceGETResult);
                var myData = myOutput["data"];
                var errorExists = myData["Error"];
                if (errorExists != null)
                {
                    Debug.WriteLine(errorExists.ToString());
                    finalMessage_next = errorExists;
                }
                {
                    try
                    {
                        var myFlightData = myData["onwardflights"][0];
                        totalFare = myFlightData["fare"]["adulttotalfare"];
                        minFare = int.Parse(totalFare.ToString()) / 65;

                        for (int i = 1; i <= array_size; i++)
                        {

                            myFlightData = myData["onwardflights"][i];
                            totalFare = myFlightData["fare"]["adulttotalfare"];
                            finalFare = int.Parse(totalFare.ToString()) / 65;
                            if (minFare > finalFare)
                            {
                                minFare = finalFare;
                            }

                        }
                        finalMessage_next = "Minimum Fare on next day:  $" + minFare.ToString();

                    }
                    catch (Exception ex)
                    {

                    }
                }

            }
            else
            {
                finalMessage_next = "Error Code for next day:" + serviceResponse.StatusCode.ToString();
            }
        }

        Next_Information.Text = finalMessage_next.ToString();


    }

    protected void hotel_Click(object sender, EventArgs e)
    {
        string lat = null;
        string lg = null;
        HttpWebRequest servicerequest = (HttpWebRequest)WebRequest.Create("https://maps.googleapis.com/maps/api/place/textsearch/xml?query=" + convert_Destination.Text + "+airport&sensor=false&key=AIzaSyBB-20a_UTPAuR23u8Qq1H9eNMK6lnjlT8");
        servicerequest.Method = "GET";
        servicerequest.ContentLength = 0;
        HttpWebResponse serviceresponse = (HttpWebResponse)servicerequest.GetResponse();
        if (serviceresponse.StatusCode == HttpStatusCode.OK)
        {
            System.Xml.XmlDocument xmlDoc = new XmlDocument();
            using (HttpWebResponse resp = servicerequest.GetResponse() as HttpWebResponse)
            {
                xmlDoc.Load(resp.GetResponseStream());
            }
            var latLong = xmlDoc["PlaceSearchResponse"].GetElementsByTagName("location")[0].ChildNodes;
            lat = latLong[0].InnerText;
            lg = latLong[1].InnerText;
        }
        HttpWebRequest service2request = (HttpWebRequest)WebRequest.Create("https://maps.googleapis.com/maps/api/place/nearbysearch/xml?key=AIzaSyBB-20a_UTPAuR23u8Qq1H9eNMK6lnjlT8&location=" + lat + "," + lg + "&radius=10000&type=lodging&keyword=hotel");
        service2request.Timeout = 2000;
        service2request.Method = "GET";
        service2request.ContentLength = 0;
        HttpWebResponse service2response = (HttpWebResponse)service2request.GetResponse();
        if (service2response.StatusCode == HttpStatusCode.OK)
        {
            XmlDocument Doc = new XmlDocument();
            using (HttpWebResponse resp = service2request.GetResponse() as HttpWebResponse)
            {
                Doc.Load(resp.GetResponseStream());
            }

            for (int i = 1; i < Doc["PlaceSearchResponse"].ChildNodes.Count - 1; i++)
            {
                var curResult = (XmlElement)Doc["PlaceSearchResponse"].ChildNodes[i];
                Label1.Text = Label1.Text + "<br />" + i.ToString() + ".&nbsp" + curResult.GetElementsByTagName("name")[0].InnerText;
                Label1.Text = Label1.Text + "&nbsp&nbsp&nbsp&nbsp&nbsp<b>Rating:</b>&nbsp " + curResult.GetElementsByTagName("rating")[0].InnerText;
                var latLong = (XmlElement)curResult.GetElementsByTagName("location")[0];
                mapItems.Add(new MapData()
                {
                    title = curResult.GetElementsByTagName("name")[0].InnerText,
                    description = curResult.GetElementsByTagName("vicinity")[0].InnerText,
                    latitude = latLong.GetElementsByTagName("lat")[0].InnerText,
                    longitude = latLong.GetElementsByTagName("lng")[0].InnerText
                });
            }
        }
    }
}