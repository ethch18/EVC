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
    public sealed partial class WidePushpin : UserControl
    {
        public new String Content
        {
            get
            {
                return this.content.Text;
            }
            set
            {
                this.content.Text = value;
            }
        }
        public WidePushpin()
        {
            this.InitializeComponent();
        }
        public WidePushpin(String text)
        {
            this.InitializeComponent();
            this.content.Text = text;
        }
    }
}
