using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sealegs.Clients.Portable.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sealegs.Clients.UI.Pages.Support
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ContactUs : ContentPage
	{
	    private ContactUsViewModel _viewModel = App.Locator.ContactUsViewModel;
        public ContactUs ()
		{
			InitializeComponent ();
		    BindingContext = _viewModel;
		}
	}
}