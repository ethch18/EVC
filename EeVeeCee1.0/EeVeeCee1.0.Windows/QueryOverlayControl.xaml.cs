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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace EeVeeCee1._0
{
    public sealed partial class QueryOverlayControl : UserControl
    {
        public QueryOverlayControl()
        {
            this.InitializeComponent();
        }
        public TextBox LocationBox
        {
            get
            {
                return this.locationBox;
            }
            set
            {
                this.locationBox = value;
            }
        }
        public ComboBox RadiusBox
        {
            get
            {
                return this.radiusBox;
            }
            set
            {
                this.radiusBox = value;
            }
        }
        public ComboBox ChargeLevelBox
        {
            get
            {
                return this.chargeLevelBox;
            }
            set
            {
                this.chargeLevelBox = value;
            }
        }
        public ComboBox NetworkBox
        {
            get
            {
                return this.networkBox;
            }
            set
            {
                this.networkBox = value;
            }
        }
        public Button GoButton
        {
            get
            {
                return this.goButton;
            }
            set
            {
                this.goButton = value;
            }
        }
        public TextBlock TimeOutLabel
        {
            get
            {
                return this.timeOutLabel;
            }
            set
            {
                this.timeOutLabel = value;
            }
        }

        public TextBlock NoResultLabel
        {
            get
            {
                return this.noResultLabel;
            }
            set
            {
                this.noResultLabel = value;
            }
        }
        public TextBlock FailLabel
        {
            get
            {
                return this.failLabel;
            }
            set
            {
                this.failLabel = value;
            }
        }
        public TextBlock BadInputLabel
        {
            get
            {
                return this.badInputLabel;
            }
            set
            {
                this.badInputLabel = value;
            }
        }
        public TextBlock StatusLabel
        {
            get
            {
                return this.statusLabel;
            }
            set
            {
                this.statusLabel = value;
            }
        }
        public TextBlock NoInternetLabel
        {
            get
            {
                return this.noInternetLabel;
            }
            set
            {
                this.noInternetLabel = value;
            }
        }

        public TextBlock WorkingLabel
        {
            get
            {
                return this.workingLabel;
            }
            set
            {
                this.workingLabel = value;
            }
        }
        public CheckBox AllNetworksCheck
        {
            get
            {
                return this.allNetworksCheck;
            }
            set
            {
                this.allNetworksCheck = value;
            }
        }

        public CheckBox BlinkNetworkCheck
        {
            get
            {
                return this.blinkNetworkCheck;
            }
            set
            {
                this.blinkNetworkCheck = value;
            }
        }

        public CheckBox ChargePointCheck
        {
            get { return this.chargePointCheck; }
            set { this.chargePointCheck = value; }
        }

        public CheckBox EVgoCheck
        {
            get { return this.eVgoCheck; }
            set { this.eVgoCheck= value; }
        }
        public CheckBox EvSECheck
        {
            get { return this.EVSECheck; }
            set { this.EVSECheck = value; }
        }
        public CheckBox RechargeAccessCheck
        {
            get { return this.rechargeAccessCheck; }
            set { this.rechargeAccessCheck = value; }
        }
        public CheckBox ShorepowerCheck
        {
            get { return this.shorepowerCheck; }
            set { this.shorepowerCheck = value; }
        }
    }
}
