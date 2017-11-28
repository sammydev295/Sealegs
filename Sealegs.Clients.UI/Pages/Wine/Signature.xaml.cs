using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

using MvvmHelpers;
using Sealegs.Clients.Portable;
using Sealegs.DataObjects;

namespace Sealegs.Clients.UI.Pages.Wine
{
    public partial class Signature : ContentPage
    {
        #region SignatureViewModel

        public readonly SignatureViewModel ViewModel = App.Locator.SignatureViewModel;

        #endregion

        #region CTOR

        public Signature(ObservableRangeCollection<Sealegs.DataObjects.Wine> selectedWines)
        {
            InitializeComponent();
            BindingContext = ViewModel;
            ViewModel.SelectedWines.ReplaceRange(selectedWines);
            ViewModel.Wine = null;
        }

        public Signature(Sealegs.DataObjects.Wine selectedWine)
        {
            InitializeComponent();
            BindingContext = ViewModel;
            ViewModel.Wine=selectedWine;
        }

        #endregion

        #region Submit_OnClicked

        private async void Submit_OnClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Success", "Checkout request processed successfully", "OK");

            int count = Navigation.NavigationStack.Count;
            
            // Not a Final Solution
            //App.Current.MainPage = new NavigationPage(new RootPageiOS());
        }

        #endregion

       

      

        private void ClientSignature_OnUnfocused(object sender, FocusEventArgs e)
        {
           
        }

       

        private void ManagerSignature_OnUnfocused(object sender, FocusEventArgs e)
        {
          

        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            ViewModel.ClientSignature = ClientSignature.GetImage(Acr.XamForms.SignaturePad.ImageFormatType.Png);
            ViewModel.ManagerSignature = ManagerSignature.GetImage(Acr.XamForms.SignaturePad.ImageFormatType.Png); 

            ViewModel.UpdateWineCheckoutCommand.Execute(null);
        }
    }

    public class LockerEvent
    {

    }
}
