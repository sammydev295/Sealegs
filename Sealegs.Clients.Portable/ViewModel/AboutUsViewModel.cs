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
    public class AboutUsViewModel : BaseViewModel
    {
        private INavigationService _navService;
        public AboutUsViewModel(INavigationService navService)
        {
            this._navService = navService;
        }

        #region Relay Commands

        public RelayCommand<string> LinkCommand=>new RelayCommand<string>(Link_OnClick);
        
        #endregion

        #region Event Handlers

        private void Link_OnClick(string link)
        {
            Device.OpenUri(new Uri(link));
        }

        #endregion
    }
}
