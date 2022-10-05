using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using vsfs.Models;
using System.Globalization;
using System.IO;

namespace vsfs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DebugOutput : ContentPage
    {
        public DebugOutput()
        {
            InitializeComponent();
            //setUp();
        }

        async void setUp()
        {
            //string code = "";
            //foreach (var item in ResponseParser.response)
            //{
            //    code += item + "\n";
            //}
            //view.Text = code;
        }
    }
}