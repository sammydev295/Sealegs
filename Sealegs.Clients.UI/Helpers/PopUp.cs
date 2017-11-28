using Rg.Plugins.Popup.Pages;

using Sealegs.Clients.UI.Helpers;
using Sealegs.Clients.Portable.Locator;
using Sealegs.Clients.Portable.Interfaces;

using Sealegs.Clients.UI.Pages.PopUp;
using Xamarin.Forms;

[assembly: Dependency(typeof(PopUp))]

namespace Sealegs.Clients.UI.Helpers
{
   public class PopUp : IPopUp
    {

        public PopupPage Page(string pageName)
        {
            switch (pageName)
            {
                case ViewModelLocator.RatingBarPopUp:
                    return new RatingBarPopUp();

                default:
                    return null;
            }
        }

        public PopupPage Page(string pageName, object value)
        {
            switch (pageName)
            {
                case ViewModelLocator.RatingBarPopUp:
                default:
                    return null;
            }
        }

    }
}
