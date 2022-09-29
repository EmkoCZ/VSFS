using System.ComponentModel;
using vsfs.ViewModels;
using Xamarin.Forms;

namespace vsfs.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}