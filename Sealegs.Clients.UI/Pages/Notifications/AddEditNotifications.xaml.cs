using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Pages;
using AsNum.XFControls;

using Sealegs.Clients.Portable.ViewModel;
using Sealegs.DataObjects;

namespace Sealegs.Clients.UI
{
    public partial class AddEditNotificationPage : PopupPage
    {
        #region Fields

        private readonly AddEditNotificationsViewModel _viewModel = App.Locator.AddEditNotificationsViewModel;

        #endregion

        #region CTOR (2 overloads)

        public AddEditNotificationPage()
        {

            InitializeComponent();
            BindingContext = _viewModel;
            _viewModel.PageTitle = "Add Notification";
            _viewModel.ButtonName = "Add";
            _viewModel.Notification = new Notification(){Date = DateTime.Now.Date};
            _viewModel.Initialize();
        }

        public AddEditNotificationPage(Notification notification)
        {
            InitializeComponent();
            BindingContext = _viewModel;
            _viewModel.ButtonName = "Update";

            _viewModel.PageTitle = notification.Text;
            _viewModel.Notification = notification;
            _viewModel.Initialize();
        }

        #endregion

        #region Event Handlers

        private void ListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var list = sender as ListView;
            if (list == null)
                return;

            list.SelectedItem = null;
        }

        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as SealegsUserRole;
            if (item == null) return;

            _viewModel.ItemSelected(item);
        }

        private void Repeater_OnItemTapped(object sender, RepeaterTapEventArgs e)
        {
            var item = e.SelectedItem as SealegsUserRole;
            if (item == null) return;

            _viewModel.ItemSelected(item);
        }

        #endregion
    }
}
