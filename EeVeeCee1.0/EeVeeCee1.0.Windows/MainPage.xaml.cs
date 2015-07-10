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
using Windows.UI.Popups;
//using Windows.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Bing.Maps;
using System.Windows.Input;
#if DEBUG
using System.Diagnostics;
#endif
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238
///pc
namespace EeVeeCee1._0
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const int QUERY_RESULT_LIMIT = 200;

        //public delegate void OnClickHandler(EventArgs e);
        private const string head = "https://developer.nrel.gov/api/alt-fuel-stations/v1/nearest.json?api_key=";
        //private const string key = "yz9FA6EpH8giJUkWlKzrX5IQ61YRruO9nZ91ZUIo";
        private const string key = "Bg12QdXiH6svyYY1yyYFH41iHqP3DMsPTHjk50d4";
        private string qString;
        private List<Fuel_Stations> populated;
        private int stationsFound;
        private bool dontTrip;
        private bool noInternet;
        private bool canContinue;

        /// <summary>
        /// Creates a new instance of MainPage, which refreshes the association dictionary,
        /// clears all data, and sets the map view to 2.5x zoom on the world.
        /// </summary>
        public MainPage()
        {
            //initialize variables
            
            

            populated = new List<Fuel_Stations>();
            this.stationsFound = 0;

            this.InitializeComponent();
            this.allNetworksCheck.IsChecked = true;

            this.myMap.SetView(new Location(39.833, -98.533), 5);
            this.dontTrip = false;
            this.noInternet = false; // TODO: make this more accurate
            this.canContinue = true;
        }



        /// <summary>
        /// On button click, creates a query based on the data passed in through the screen fields.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void goButton_Click(object sender, RoutedEventArgs e)
        {
            //clear event fields
            myMap.Children.Clear();
            statusLabel.Text = "";
            failLabel.Visibility = Visibility.Collapsed;
            noResultLabel.Visibility = Visibility.Collapsed;
            timeOutLabel.Visibility = Visibility.Collapsed;
            badInputLabel.Visibility = Visibility.Collapsed;
            noInternetLabel.Visibility = Visibility.Collapsed;
            populated.Clear();
            stationsFound = 0;
            this.canContinue = true;

            //populate variables, or route to ShowFailMsg() or ShowBadInputMsg() if improper input
            string location = this.locationBox.Text;
            
            string tempRadius = this.radiusBox.Text;
            decimal radius;
            try
            {
                 string testForCharge = (string)((ListBoxItem)this.chargeLevelBox.SelectedValue).Content;
            }
            catch (NullReferenceException)
            {
                ShowFailMsg();
                return;
            }            

            string ev_charging_level = (string)((ListBoxItem)this.chargeLevelBox.SelectedValue).Content;
            if (ev_charging_level.Equals("DC Fast"))
            {
                ev_charging_level = "dc_fast";
            }
            
            string ev_network = "";
            if (allNetworksCheck.IsChecked == true)
            {
                ev_network = "all";
            }
            else
            {
                if (blinkNetworkCheck.IsChecked == true)
                {
                    ev_network += "Blink Network,";
                }
                if (chargePointCheck.IsChecked == true)
                {
                    ev_network += "ChargePoint Network,";
                }
                if (eVgoCheck.IsChecked == true)
                {
                    ev_network += "eVgo Network,";
                }
                if (EVSECheck.IsChecked == true)
                {
                    ev_network += "EVSE LLC WebNet,";
                }
                if (rechargeAccessCheck.IsChecked == true)
                {
                    ev_network += "RechargeAccess,";
                }
                if (shorepowerCheck.IsChecked == true)
                {
                    ev_network += "Shorepower,";
                }

                //remove extra comma, if applicable
                try
                {
                    ev_network = ev_network.Substring(0, ev_network.Length - 1);
                }
                catch (ArgumentOutOfRangeException)
                {
                    ev_network = "FAIL";
                }
            }

            if (String.IsNullOrEmpty(location)) 
            {
                ShowBadInputMsg();
                return;                
            }
            else if (!decimal.TryParse(tempRadius, out radius))
            {
                ShowBadInputMsg();
                return;
            }
            else if (String.IsNullOrEmpty(ev_charging_level))
            {
                ShowFailMsg();
                return;             
            }
            else if (ev_network.Equals("FAIL"))
            {
                ShowFailMsg();
                return;
            }


            //all safe now
            else
            {
                QueryAndPopulate(location, radius, ev_network, ev_charging_level);
            }
        }

        /// <summary>
        /// Execute connection to JSON API, queries data, and appends that data to local field qString.
        /// </summary>
        /// <param name="query"></param>
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

                    }
                }
                catch (AggregateException e)
                {
                    if (e.InnerException.Message.Equals("An error occurred while sending the request."))
                    {
                        //no internet
                        noInternetLabel.Visibility = Visibility.Visible;
                        noInternet = true;
                        canContinue = false;
                    }
                }
            }
        }

        /// <summary>
        /// Displays all stations from qString on map as pushpins
        /// </summary>
        /// <param name="qString"></param>
        /// <param name="radius"></param>
        private void PlacePushpins(string qString, decimal radius, out int resultCount)
        {
            resultCount = 0;
            try
            {
                Rootobject test = JsonConvert.DeserializeObject<Rootobject>(qString);
                foreach (Fuel_Stations f in test.fuel_stations)
                {
                    Pushpin p = new Pushpin();
                    p.Width *= 1.5;
                    p.Height *= 1.5;
                    p.Text = f.station_name;
                    Location pin = new Location(f.latitude, f.longitude);

                    MapLayer.SetPosition(p, pin);
                    ToolTipService.SetToolTip(p, f.station_name);
                    populated.Add(f);
                    myMap.Children.Add(p);
                    resultCount++;
                }
            }
            //catch (JsonSerializationException)
            //{
            //    double lat, longi;
            //    if (GetBadGPSLocation(qString, out longi, out lat))
            //    {
            //        this.myMap.SetView(new Location(lat, longi), 11.0);
            //    }
            //    //none found;
            //    noResultLabel.Visibility = Visibility.Visible;
            //}
            catch (ArgumentNullException)
            {
                if (!noInternet)
                {
                    //time out
                    timeOutLabel.Visibility = Visibility.Visible;
                }
                noInternet = false;
                canContinue = false;
                
            }
        }
        /// <summary>
        /// Generates the query, pings the server, parses the response, and displays all stations on map.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="radius"></param>
        /// <param name="ev_network"></param>
        /// <param name="ev_charging_level"></param>
        private void QueryAndPopulate(string location, decimal radius, string ev_network, string ev_charging_level)
        {
            string query = ConstructQuery(location, radius, ev_network, ev_charging_level);


            ////debug query
            //string query = head + key + "&location=" + location
            //    + "&status=E&access=public&fuel_type=ELEC";

            int offset = 0;
            int perQueryCount = 0;
            do
            {
                offset += perQueryCount;

                //Query the data
                string tempQuery = query + "&offset=" + offset;

                Execute(tempQuery);

#if (DEBUG)
                Debug.WriteLine(qString);
#endif
                perQueryCount = 0;
                //Plot stations onto map
                PlacePushpins(qString, radius, out perQueryCount);

                this.stationsFound += perQueryCount;

            } while (canContinue && perQueryCount >= QUERY_RESULT_LIMIT);

            //Set location
            if (canContinue)
            {
                Rootobject test = JsonConvert.DeserializeObject<Rootobject>(qString);
                this.statusLabel.Text = stationsFound + (stationsFound == 1 ? " station found." : " stations found.");
                Location focusCenter = new Location(test.latitude, test.longitude);
                //if (radius < 50) { this.myMap.SetView(focusCenter, 15.0); }
                //else if (radius < 150) { this.myMap.SetView(focusCenter, 12.0); }
                //else { this.myMap.SetView(focusCenter, 8.0); }
                this.myMap.SetView(focusCenter, 15.0);
            }

        }

        /// <summary>
        /// Constructs a query from the provided parameters
        /// </summary>
        /// <param name="location"></param>
        /// <param name="radius"></param>
        /// <param name="ev_network"></param>
        /// <param name="ev_charging_level"></param>
        /// <returns></returns>
        private string ConstructQuery(string location, decimal radius, string ev_network, string ev_charging_level)
        {
            return head + key 
                        + "&location=" + location
                        + "&radius=" + radius
                        + "&status=E&access=public&fuel_type=ELEC"
                        + "&ev_network=" + ev_network
                        + "&ev_charging_level=" + ev_charging_level
                        + "&limit=" + QUERY_RESULT_LIMIT;
        }

        /// <summary>
        /// Extracts GPS location from an unserializable JSON Response
        /// </summary>
        /// <param name="jasonQueryResult"></param>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        /// <returns></returns>
        private bool GetBadGPSLocation(string jsonQueryResult, out double longitude, out double latitude)
        {
            try
            {
                int beginLat = qString.IndexOf("\"latitude\":") + 11;
                int beginLong = qString.IndexOf("\"longitude\":") + 12;
                int endLat = qString.IndexOf(',', beginLat);
                int endLong = qString.IndexOf(',', beginLong);

                string tempLat = qString.Substring(beginLat, endLat - beginLat);
                string tempLong = qString.Substring(beginLong, endLong - beginLong);

                latitude = double.Parse(tempLat);
                longitude = double.Parse(tempLong);
                return true;
            }
            catch 
            {
                latitude = -1;
                longitude = -1;
                return false;
            }
        }

        /// <summary>
        /// Sets the error message for incomplete input to visible
        /// </summary>
        private void ShowFailMsg()
        {
            failLabel.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Sets the error message for bad location/radius input to visible
        /// </summary>
        private void ShowBadInputMsg()
        {
            badInputLabel.Visibility = Visibility.Visible;
        }
        
        /// <summary>
        /// Routes tapped event to clicked event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void goButton_Click(object sender, TappedRoutedEventArgs e)
        {
            goButton_Click(sender, e as RoutedEventArgs);
        }

        /// <summary>
        /// Checks if keyDown yields the enter key when engaged with focus of any of the entry fields.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckForEnter(object sender, KeyRoutedEventArgs e)
       {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                goButton_Click(sender, e as RoutedEventArgs);
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

            if (blinkNetworkCheck.IsChecked == true
                && chargePointCheck.IsChecked == true
                && eVgoCheck.IsChecked == true
                && EVSECheck.IsChecked == true
                && rechargeAccessCheck.IsChecked == true
                && shorepowerCheck.IsChecked == true)
            {
                allNetworksCheck.IsChecked = true;
            }
            networkBox.SelectedIndex = -1;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            this.dontTrip = true;
            this.allNetworksCheck.IsChecked = false;
            networkBox.SelectedIndex = -1;
        }

        private void allNetworksCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            if (this.dontTrip) return;

            blinkNetworkCheck.IsChecked = false;
            chargePointCheck.IsChecked = false;
            eVgoCheck.IsChecked = false;
            EVSECheck.IsChecked = false;
            rechargeAccessCheck.IsChecked = false;
            shorepowerCheck.IsChecked = false;

            this.dontTrip = false;
            networkBox.SelectedIndex = -1;
        }

        private void allNetworksCheck_Checked(object sender, RoutedEventArgs e)
        {
            blinkNetworkCheck.IsChecked = true;
            chargePointCheck.IsChecked = true;
            eVgoCheck.IsChecked = true;
            EVSECheck.IsChecked = true;
            rechargeAccessCheck.IsChecked = true;
            shorepowerCheck.IsChecked = true;


            networkBox.SelectedIndex = -1;
        }

        private void CheckBox_Toggle(object sender, RoutedEventArgs e)
        {

            if (((CheckBox) sender).IsChecked == true)
            {
                CheckBox_Unchecked(sender, e);
                ((CheckBox)sender).IsChecked = false;
            }
            else
            {
                CheckBox_Checked(sender, e);
                ((CheckBox)sender).IsChecked = true;
            }
            
        }

        private void allNetworksCheck_Toggle(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == true)
            {
                allNetworksCheck_Unchecked(sender, e);
                ((CheckBox)sender).IsChecked = false;
            }
            else
            {
                allNetworksCheck_Checked(sender, e);
                ((CheckBox)sender).IsChecked = true;
            }
            networkBox.SelectedIndex = -1;
        }

        

        
    }
}
