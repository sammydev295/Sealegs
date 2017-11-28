using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Rg.Plugins.Popup.Extensions;

using Sealegs.Clients.Portable.Interfaces;
using Sealegs.Clients.Portable.Locator;

namespace Sealegs.Clients.Portable.NavigationService
{
    public class PopUpNavigationService
    {
        public static INavigation NavService;

        //public static void PushPopUp(Rg.Plugins.Popup.Pages.PopupPage page)
        //{
        //    NavigationExtension.PushPopupAsync(NavService, page);
        //}

        public static void PopPopUp()
        {
            NavigationExtension.PopPopupAsync(NavService);
        }

        public static void PopAllPopUp()
        {
            NavigationExtension.PopAllPopupAsync(NavService);
        }

        public static void PushPopUp(string pageName)
        {
            var page = DependencyService.Get<IPopUp>().Page(pageName);
            NavigationExtension.PushPopupAsync(NavService, page);
        }

        public static void PushPopUp(string pageName,object value)
        {
            var page = DependencyService.Get<IPopUp>().Page(pageName, value);
            NavService.PushPopupAsync(page);
        //    NavigationExtension.PushPopupAsync(NavService, page);
        }
    }
}
