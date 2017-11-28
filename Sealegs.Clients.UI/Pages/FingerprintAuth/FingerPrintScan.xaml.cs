using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Sealegs.Clients.Portable;
using Sealegs.Clients.Portable.ViewModel;
using Sealegs.Utils;

namespace Sealegs.Clients.UI.Pages.FingerprintAuth
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FingerPrintScan : ContentPage
	{
	    private readonly FingerPrintScanViewModel _viewModel = App.Locator.FingerPrintScanViewModel;
        public FingerPrintScan ()
		{
			InitializeComponent ();
		    BindingContext = _viewModel;
        }
	    public FingerPrintScan(User user)
	    {
	        InitializeComponent();
	        BindingContext = _viewModel;
	        _viewModel.CurrentUser = user;
	        Addresses.Token = user.Token;
            _viewModel.InitlizeUser(user);
	    }

        protected  override async void OnAppearing()
	    {
	        base.OnAppearing();
	     _viewModel.FingerScanCommand.Execute(null);
	    }
	}
}