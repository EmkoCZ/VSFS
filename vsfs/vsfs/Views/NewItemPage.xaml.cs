using System;
using System.Collections.Generic;
using System.ComponentModel;
using vsfs.Models;
using vsfs.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace vsfs.Views
{
    public partial class NewItemPage : ContentPage
    {
        public Item Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();
            BindingContext = new NewItemViewModel();
        }
    }
}