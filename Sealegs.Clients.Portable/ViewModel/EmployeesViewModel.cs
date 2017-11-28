using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;
using Acr.UserDialogs;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using MvvmHelpers;

using Sealegs.Clients.Portable.Locator;
using Sealegs.DataObjects;

namespace Sealegs.Clients.Portable.ViewModel
{
    public class EmployeesViewModel : BaseViewModel
    {
        private INavigationService navService;

        public EmployeesViewModel(INavigationService navigation)
        {
            navService = navigation;
           
        }

        #region Observable properties

        private ObservableRangeCollection<LockerMember> _employeesGetListItems=new ObservableRangeCollection<LockerMember>();

        public ObservableRangeCollection<LockerMember> EmployeesList
        {
            get => _employeesGetListItems;
            set
            {
                _employeesGetListItems = value;
                RaisePropertyChanged();
            }
        }

        private LockerMember _selectedEmployee;

        public LockerMember SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                _selectedEmployee = value;
                RaisePropertyChanged();
            }
        }

        #region Filtering
        private bool _employeesFound;

        public bool EmployeesFound
        {
            get => _employeesFound;
            set
            {
                _employeesFound = value;
                RaisePropertyChanged();
            }

        }

        private bool _noEmployeesFound;
        public bool NoEmployeesFound
        {
            get => _noEmployeesFound;
            set
            {
                _noEmployeesFound = value;
                RaisePropertyChanged();
            }
        }

        private string _noEmployeesFoundMessage;
        public string NoEmployeesFoundMessage
        {
            get => _noEmployeesFoundMessage;
            set
            {
                _noEmployeesFoundMessage = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #endregion

        #region Relay Commands

        public RelayCommand AddCommand=>new RelayCommand(Add);

        #endregion

        #region Event Handlers

        private void Add()
        {
            navService.NavigateTo(ViewModelLocator.AddEmployee);

        }
        #endregion

        #region Initialize

        public async void Initialize()
        {
            IsBusy = true;
            EmployeesFound = true;

            IEnumerable<LockerMember> lockers = await LockerMemberDb.GetAllStaff();
            IEnumerable<LockerMember> employees = lockers as IList<LockerMember> ?? lockers.ToList();
           
            if (!employees.Any())
            {
                NoEmployeesFoundMessage = "No Employees Found";
                EmployeesFound = false;
                NoEmployeesFound = true;
            }
            else
            {
                NoEmployeesFound = false;
                EmployeesFound = true;
                EmployeesList=new ObservableRangeCollection<LockerMember>(employees);
            }

            IsBusy = false;
        }

        public async void ItemSelected(LockerMember locker)
        {
            var result = await UserDialogs.Instance.ActionSheetAsync("Select", "Cancel", null, null, "Edit", "Delete");
            if (result == "Edit")
            {
                navService.NavigateTo(ViewModelLocator.AddEmployee, locker);
                return;
            }
            if (result == "Delete")
            {
               var isDeleted= await LockerMemberDb.DeleteLocker(locker.Id);
                if (isDeleted)
                {
                    Initialize();
                }
            }
         
        }

        #endregion
    }
}
