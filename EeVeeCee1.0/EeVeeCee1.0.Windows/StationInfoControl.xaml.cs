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
    public sealed partial class StationInfoControl : UserControl
    {
        public Button CloseButton
        {
            get
            {
                return this.closeButton;
            }
            set
            {
                this.closeButton = value;
            }
        }
        public string StationName
        {
            get
            {
                return this.stationName.Text;
            }
            set
            {
                this.stationName.Text = value;
            }
        }
        public string Distance
        {
            get
            {
                return this.distance.Text;
            }
            set
            {
                this.distance.Text = value;
            }
        }
        public string AddressLine1
        {
            get
            {
                return this.addressLine1.Text;
            }
            set
            {
                this.addressLine1.Text = value;
            }
        }
        public string AddressLine2
        {
            get
            {
                return this.addressLine2.Text;
            }
            set
            {
                this.addressLine2.Text = value;
            }
        }
        public string Level1Count
        {
            get
            {
                return this.l1Count.Text;
            }
            set
            {
                this.l1Count.Text = value;
            }
        }
        public string Level2Count
        {
            get
            {
                return this.l2Count.Text;
            }
            set
            {
                this.l2Count.Text = value;
            }
        }
        public string DCFastCount
        {
            get
            {
                return this.dcCount.Text;
            }
            set
            {
                this.dcCount.Text = value;
            }
        }

        public string Network
        {
            get
            {
                return this.network.Text;
            }
            set
            {
                this.network.Text = value;
            }
        }

        public string Notes
        {
            get
            {
                return this.notes.Text;
            }
            set
            {
                this.notes.Text = value;
            }
        }

        public StationInfoControl()
        {
            this.InitializeComponent();
        }





    }
}
