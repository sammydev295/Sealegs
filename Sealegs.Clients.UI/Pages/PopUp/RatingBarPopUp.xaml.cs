using Xamarin.Forms.Xaml;
using AsNum.XFControls;

using Rg.Plugins.Popup.Pages;

using Sealegs.DataObjects;
using Sealegs.Clients.Portable.ViewModel;

namespace Sealegs.Clients.UI.Pages.PopUp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RatingBarPopUp : PopupPage
    {
        private readonly RatingBarPopUpViewModel _viewModel = App.Locator.RatingBarPopUpViewModel;

        public RatingBarPopUp()
        {
            InitializeComponent();
            BindingContext = _viewModel;
        }

        public RatingBarPopUp(Sealegs.DataObjects.Wine value)
        {
            InitializeComponent();
            BindingContext = _viewModel;
            _viewModel.SelectedWine = value;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.Initialize();
        }

        private void Repeater_OnItemTaped(object sender, RepeaterTapEventArgs e)
        {

            var item = e.SelectedItem as RatingBarBO;
            _viewModel.ItemSelected(item);
        }
    }
}