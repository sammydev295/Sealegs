using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

using Sealegs.Clients.Portable;
using Sealegs.Clients.Portable.ViewModel;
using Sealegs.DataObjects;

namespace Sealegs.Clients.UI
{
    public partial class Employees : ContentPage
    {
        #region Data Members

        private readonly EmployeesViewModel _viewModel = App.Locator.EmployeesViewModel;

        #endregion

        #region Constructor
        public Employees()
        {

            BindingContext = _viewModel;
            InitializeComponent();
            _viewModel.Initialize();
        }
        #endregion

        private void ListViewEmployees_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var locker = e.SelectedItem as LockerMember;
            if (locker == null)return;
            _viewModel.ItemSelected(locker);
          
        }

        private void ListViewEmployees_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var list = sender as ListView;
            if (list == null)
                return;
            list.SelectedItem = null;
        }
    }
}
