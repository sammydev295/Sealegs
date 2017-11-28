using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Xamarin.Forms;

namespace Sealegs.Clients.Portable.ViewModel
{
    public class ContactUsViewModel:BaseViewModel
    {
        private INavigationService _navService;
        public ContactUsViewModel(INavigationService navService)
        {
            this._navService = navService;
        }

        #region Relay Commands
        public RelayCommand FacebookCommand=>new  RelayCommand(Facebook_OnClick);
        public RelayCommand GoogleCommand => new RelayCommand(Google_OnClick);
        public RelayCommand TwitterCommand => new RelayCommand(Twitter_OnClick);
        public RelayCommand YoutubeCommand => new RelayCommand(Youtube_OnClick);
        #endregion

        #region Events

        private void Facebook_OnClick()
        {
            Device.OpenUri(new Uri("https://www.facebook.com/SeaLegsWineBar/"));
        }
        private void Google_OnClick()
        {
            Device.OpenUri(new Uri("https://www.facebook.com/SeaLegsWineBar/"));
        }
        private void Twitter_OnClick()
        {
            Device.OpenUri(new Uri("https://twitter.com/SeaLegsWineBar"));
        }
        private void Youtube_OnClick()
        {
            Device.OpenUri(new Uri("https://www.facebook.com/SeaLegsWineBar/"));
        }
        #endregion
    }
}
