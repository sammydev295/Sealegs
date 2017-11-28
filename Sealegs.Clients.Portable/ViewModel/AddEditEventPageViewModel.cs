using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormsToolkit;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Sealegs.DataObjects;
using Xamarin.Forms;

namespace Sealegs.Clients.Portable.ViewModel
{
    public class AddEditEventPageViewModel : BaseViewModel
    {
        private INavigationService _navService;
        public AddEditEventPageViewModel(INavigationService navigation) 
        {
            _navService = navigation;
        }

        #region SelectedEvent

        private FeaturedEvent _event;
        public FeaturedEvent Event
        {
            get => _event;
            set
            {
                _event = value;
                RaisePropertyChanged();
            }
        }

        #endregion

       public RelayCommand AddCommand => new RelayCommand(Add);
        private void Add()
        {
            if (IsBusy)return;
            try
            {
                IsBusy = true;

                if (ButtonName == "Update")
                {
                    FeaturedEventsDb.UpdateFeaturedEvent(Event);
                }
                else
                {
                    FeaturedEventsDb.InsertFeaturedEvent(Event);
                }
                _navService.GoBack(); _navService.GoBack();
            }
            catch (Exception e)
            {
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
