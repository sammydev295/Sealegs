using System.Collections.Generic;
using Sealegs.Clients.Portable.ViewModel;
using Xamarin.Forms;

namespace Sealegs.Clients.UI.Pages.Support
{
    public partial class AboutUs : ContentPage
    {
        private readonly AboutUsViewModel _viewModel = App.Locator.AboutUsViewModel;
        public AboutUs()
        {
            InitializeComponent();
            BindingContext = _viewModel;
          //  CFLip.ItemsSource=new List<int>(){0,1,2};
        }
    }
}