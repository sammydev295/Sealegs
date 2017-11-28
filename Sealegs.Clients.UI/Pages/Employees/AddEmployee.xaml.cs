using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sealegs.Clients.Portable;
using Sealegs.Clients.Portable.ViewModel;
using Sealegs.DataObjects;
using Xamarin.Forms;

namespace Sealegs.Clients.UI
{
    public partial class AddEmployee : ContentPage
    {
        #region Data Members

        private readonly AddEmployeeViewModel _viewModel = App.Locator.AddEmployeeViewModel;
       ToolbarItem _filterItem;
        #endregion

        #region Constructor
        public AddEmployee()
        {
            InitializeComponent();

            BindingContext = _viewModel;
            _viewModel.ButtonName = "Add";
            _viewModel.PageTitle = "Add Employee";
            //EditorNotes.Text = "Notes";
            //EditorNotes.TextColor=Color.FromHex("#7C7C7C");
        }
        public AddEmployee(LockerMember lockerMember)
        {
            InitializeComponent();

            BindingContext = _viewModel;
            _viewModel.Locker = lockerMember;
            _viewModel.ButtonName = "Update";
            _viewModel.PageTitle = lockerMember.DisplayName;
            //EditorNotes.Text = "Notes";
            //EditorNotes.TextColor=Color.FromHex("#7C7C7C");
        }

        #endregion
    }
}
