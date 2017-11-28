using System.Windows.Input;
using DLToolkit.Forms.Controls;
using Xamarin.Forms;

using Sealegs.DataObjects;
using Sealegs.Clients.Portable;

namespace Sealegs.Clients.UI
{
    public class WineCell: FlowListView
    {
        readonly INavigation navigation;
        public WineCell (INavigation navigation = null)
        {
            FlowColumnTemplate = new WineCellView ();
            this.navigation = navigation;

        }
    }

    public partial class WineCellView : DataTemplate
    {
        public WineCellView()
        {
            InitializeComponent();
        }
    }
}

