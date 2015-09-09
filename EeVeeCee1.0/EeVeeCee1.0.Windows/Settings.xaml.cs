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

// The Settings Flyout item template is documented at http://go.microsoft.com/fwlink/?LinkId=273769

namespace EeVeeCee1._0
{
    public sealed partial class Settings : SettingsFlyout
    {
        public Settings()
        {
            this.InitializeComponent();
        }

        public Settings(bool locationAllowed)
        {
            this.InitializeComponent();
            if (locationAllowed)
            {
                this.locationToggle.IsOn = true;
            }
            else
            {
                this.locationToggle.IsOn = false;
            }
        }

        private void locationToggle_Toggled(object sender, RoutedEventArgs e)
        {
            Windows.Storage.ApplicationDataContainer localsettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            if (this.locationToggle.IsOn)
            {
                localsettings.Values["locationAllowed"] = true;
            }
            else
            {
                localsettings.Values["locationAllowed"] = false;
            }
        }

        public bool LocationAllowed
        {
            get
            {
                return locationToggle.IsOn;
            }
        }


    }
}
