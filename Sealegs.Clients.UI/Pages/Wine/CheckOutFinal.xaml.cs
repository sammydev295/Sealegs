using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using MvvmHelpers;

using Sealegs.Clients.Portable;
using Sealegs.Clients.UI.Pages.Wine;
using Sealegs.DataObjects;

namespace Sealegs.Clients.UI
{
    public partial class CheckOutFinal : ContentPage
    {
        #region Properties 

        private readonly CheckOutFinalViewModel _viewModel = App.Locator.CheckOutFinalViewModel;
       

     

        #endregion

        #region CTOR

        public CheckOutFinal(ObservableRangeCollection<Wine> wines)
        {
            InitializeComponent();
            BindingContext = _viewModel;
            _viewModel.SelectedWines.ReplaceRange(wines);
        }

        #endregion

        #region Confirm_OnClicked

        private void Confirm_OnClicked(object sender, EventArgs e)
        {
       //     Navigation.PushAsync(new Signature(Locker, vm.SelectedWines));
        }

        #endregion
    }
}
