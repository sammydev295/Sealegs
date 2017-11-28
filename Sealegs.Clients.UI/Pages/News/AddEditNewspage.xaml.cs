using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Sealegs.Clients.Portable.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sealegs.Clients.UI.Pages.News
{


    public partial class AddEditNewspage : ContentPage
    {
        private readonly AddEditNewsViewModel _viewModel = App.Locator.AddEditNewsViewModel;

        public AddEditNewspage(DataObjects.News news)
        {
            InitializeComponent();
            BindingContext = _viewModel;
            _viewModel.News = news;
            _viewModel.PageTitle = news.Name;
            _viewModel.ButtonName = "Update";
        }
        public AddEditNewspage()
        {
            InitializeComponent();
            BindingContext = _viewModel;
            _viewModel.News=new DataObjects.News();
            _viewModel.PageTitle = "Add News";
            _viewModel.ButtonName = "Save";
        }
    }
}
