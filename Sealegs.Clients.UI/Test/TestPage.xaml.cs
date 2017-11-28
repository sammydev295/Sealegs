using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sealegs.Clients.UI.Test
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TestPage : ContentPage
	{
		public TestPage ()
		{
		    try
		    {
		        InitializeComponent();
		        CstmList.ItemsSource = new List<string>()
		        {
		            "Abc","def"
		        };
            }
		    catch (Exception e)
		    {
		        // ignored
		    }
		}

	    private void CstmList_OnOnItemSelected(object sender, PhotoResultEventArgs e)
	    {
	        
	    }
	}
}