using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;

namespace Sealegs.Clients.Portable.Interfaces
{
    public interface IPopUp
    {

        PopupPage Page(string pageName);

        PopupPage Page(string pageName, object value);

    }
}
