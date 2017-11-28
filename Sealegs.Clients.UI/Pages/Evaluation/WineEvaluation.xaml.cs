using Sealegs.Clients.Portable.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sealegs.Clients.UI.Pages.Evaluation
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WineEvaluation : ContentPage
    {
        private readonly WineEvaluationViewModel _viewModel = App.Locator.WineEvaluationViewModel;
        public WineEvaluation()
        {
            try
            {
                InitializeComponent();
                BindingContext = _viewModel;
            }
            catch (Exception e)
            {
              
            }

           
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.Intilize();

        }

        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //var item = e.SelectedItem as RemoteChillRequest;
            //if (item == null) return;

            //_viewModel.ItemSelected();
        }

        private void ListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            //var list = sender as ListView;
            //if (list == null)
            //    return;
            //list.SelectedItem = null;
        }
    }
}