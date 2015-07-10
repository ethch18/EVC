using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Controls.Maps;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238
///phone
namespace EeVeeCee1._0
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private static bool firstTime = true;
        private const string head = "https://developer.nrel.gov/api/alt-fuel-stations/v1/nearest.json?api_key=";
        private const string key = "yz9FA6EpH8giJUkWlKzrX5IQ61YRruO9nZ91ZUIo";
        private string qString;
        private DataWrapper data;
        public MainPage()
        {
            if (firstTime == true)
            {
                firstTime = false;
                Frame rootFrame = Window.Current.Content as Frame;
                rootFrame.Navigate(new DataPage().GetType());

            }
        //    timeOutLabel.Visibility = Visibility.Collapsed;
            //noResultLabel.Visibility = Visibility.Collapsed;
            Run();
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
            DataWrapper d = e.Parameter as DataWrapper;
            this.data = d;
            
        }

        private void Run()
        {
            try
            {
                string query = head + key
                                + "&location=" + data.location + "&radius=" + data.radius + "&status=E&fuel_type=ELEC&ev_charging_levels="
                                + data.level + "&limit=" + data.limit;
                Execute(query);
                PaintMap(qString, data.radius);
            }
            catch (NullReferenceException) //entrypoint
            {
                Frame rootFrame = Window.Current.Content as Frame;
                rootFrame.Navigate(new DataPage().GetType());
            }
        }

        private void PaintMap(string qString, decimal radius)
        {
            try
            {
                Rootobject test = JsonConvert.DeserializeObject<Rootobject>(qString);
                foreach (Fuel_Stations f in test.fuel_stations)
                {
                    //Pushpin p = new Pushpin();
                    //p.Text = f.station_name;
                    //MapLayer.SetPosition(p, new Location(f.latitude, f.longitude));
                    //myMap.Children.Add(p);
                    MapIcon m = new MapIcon();
                    m.Title = f.station_name;
                    m.Location = new Windows.Devices.Geolocation.Geopoint(new Windows.Devices.Geolocation.BasicGeoposition() {Latitude = f.latitude, Longitude = f.longitude});
                    myMap1.MapElements.Add(m);


                }
                Windows.Devices.Geolocation.Geopoint focusCenter = new Windows.Devices.Geolocation.Geopoint(new Windows.Devices.Geolocation.BasicGeoposition() { Latitude = test.latitude, Longitude = test.longitude });
                MapControl.SetLocation(myMap1, focusCenter);
                if (radius < 50) { myMap1.ZoomLevel = 16.0; }
                else if (radius < 150) { myMap1.ZoomLevel = 12.0; }
                else { myMap1.ZoomLevel = 8.0; }
            }
            catch (JsonSerializationException)
            {
                int beginLat = qString.IndexOf("\"latitude\":") + 11;
                int beginLong = qString.IndexOf("\"longitude\":") + 12;
                int endLat = qString.IndexOf(',', beginLat);
                int endLong = qString.IndexOf(',', beginLong);

                string tempLat = qString.Substring(beginLat, endLat - beginLat);
                string tempLong = qString.Substring(beginLong, endLong - beginLong);

                double lat = double.Parse(tempLat);
                double longi = double.Parse(tempLong);
                Windows.Devices.Geolocation.Geopoint focusCenter = new Windows.Devices.Geolocation.Geopoint(new Windows.Devices.Geolocation.BasicGeoposition() { Latitude = lat, Longitude = longi});
                MapControl.SetLocation(myMap1, focusCenter);
                myMap1.ZoomLevel = 11.0;
                //none found;
                //noResultLabel.Visibility = Visibility.Visible;
            }
            catch (ArgumentNullException) //entrypoint
            {
                Frame rootFrame = Window.Current.Content as Frame;
                rootFrame.Navigate(new DataPage().GetType());
            }
        }

        private void Execute(string query)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://developer.nrel.gov");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    HttpResponseMessage response = client.GetAsync(query).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        Stream messageContent = response.Content.ReadAsStreamAsync().Result;
                        StreamReader sr = new StreamReader(messageContent);
                        string jString = sr.ReadToEnd();
                        this.qString = jString;
                        sr.Dispose();
                        //JArray arr = JArray.Parse(jString);
                        //this.queued = arr; //sketchy stuff here
                    }
                }
                catch (AggregateException)
                {
                    //time out
                //    timeOutLabel.Visibility = Visibility.Visible;
                }
            }

        }
    }
}
