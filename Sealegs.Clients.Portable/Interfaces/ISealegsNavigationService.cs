using System;
using System.Threading.Tasks;

namespace Sealegs.Clients.Portable
{
    public interface ISealegsNavigationService
    {
        void PopupNavigateTo(string pageKey);

        void PopupNavigateTo(string pageKey, params object[] parameter);

        void PopupGoBack();
    }
}

