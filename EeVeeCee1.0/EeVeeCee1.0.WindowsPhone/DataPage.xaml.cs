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
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace EeVeeCee1._0
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DataPage : Page
    {

        private Dictionary<string, string> limitAssociations;

        private string location;
        private decimal radius;
        private string level;
        private int limit;

        public DataWrapper data { get; set; }
        public DataPage()
        {
            limitAssociations = new Dictionary<string, string>();
            limitAssociations.Add("All Charge Levels", "all");
            limitAssociations.Add("1", "1");
            limitAssociations.Add("2", "2");
            limitAssociations.Add("DC Fast", "dc_fast");
            limitAssociations.Add("Legacy", "legacy");
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void goButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            failBlock.Visibility = Visibility.Collapsed;
            bool nav = LoadToFields();
            if (nav)
            {
                Frame rootFrame = Window.Current.Content as Frame;
                rootFrame.Navigate(new MainPage().GetType(),data);
            }
        }

        private bool LoadToFields()
        {
            try
            {
                this.location = locationBox.Text;
                if (String.IsNullOrEmpty(location)) throw new InvalidDataException("Bad Location.");

                string tempRadius = radiusBox.Text;
                if (!decimal.TryParse(tempRadius, out this.radius)) throw new InvalidDataException("Bad Radius.");

                string testForCharge = (string)((ListBoxItem)this.chargeLevelBox.SelectedValue).Content;
                if (String.IsNullOrEmpty(testForCharge)) throw new InvalidDataException("Bad Level.");
                this.level = limitAssociations[testForCharge];

                if (!int.TryParse((string)((ListBoxItem)this.limitBox.SelectedValue).Content, out this.limit))
                {
                    throw new InvalidDataException("Bad Limit.");
                }
                data = new DataWrapper(location, radius, level, limit);

            }
            catch (Exception)
            {
                failBlock.Visibility = Visibility.Visible;
                return false;
            }
            return true;
        }
    }
}
