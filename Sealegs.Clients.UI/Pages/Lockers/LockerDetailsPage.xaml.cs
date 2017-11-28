using System;
using System.Collections.Generic;

using Xamarin.Forms;
using FormsToolkit;

using Sealegs.DataObjects;
using Sealegs.Clients.Portable;

namespace Sealegs.Clients.UI
{
    public partial class LockerDetailsPage : ContentPage
    {
        #region ViewModel

        private LockerDetailsViewModel ViewModel => App.Locator.LockerDetailsViewModel;

        #endregion 

        #region CTOR

        public LockerDetailsPage(Tuple<LockerMember, List<LockerType>, List<WineVarietal>> parameters)
        {
            InitializeComponent();
            BindingContext = ViewModel;
            ViewModel.Locker = parameters.Item1;
            ViewModel.LockerTypes = parameters.Item2;
            ViewModel.WineVarietals = parameters.Item3;
        }

        #endregion
    }
}

