using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace vsfs.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "Home";
        }

        public ICommand OpenWebCommand { get; }
    }
}