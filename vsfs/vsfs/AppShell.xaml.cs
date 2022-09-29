using System;
using System.Collections.Generic;
using vsfs.ViewModels;
using vsfs.Views;
using Xamarin.Forms;

namespace vsfs
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            Routing.RegisterRoute(nameof(Timetable), typeof(Timetable));
            Routing.RegisterRoute(nameof(DebugOutput), typeof(DebugOutput));
            Routing.RegisterRoute(nameof(InfoPage), typeof(InfoPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
