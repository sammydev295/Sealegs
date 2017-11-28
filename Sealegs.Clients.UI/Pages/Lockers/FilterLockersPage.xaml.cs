using System;

using FormsToolkit;
using Xamarin.Forms;

using Sealegs.Clients.Portable;
using Sealegs.DataObjects;

namespace Sealegs.Clients.UI
{
    public partial class FilterLockersPage : ContentPage
    {
        FilterLockersViewModel vm;
        Category showFavorites, showPast;

        public FilterLockersPage()
        {
            InitializeComponent();

            if (Device.OS != TargetPlatform.iOS)
                ToolbarDone.Icon = "toolbar_close.png";

            ToolbarDone.Command = new Command(async () =>
           {
               Settings.Current.FavoritesOnly = showFavorites.IsFiltered;
               Settings.Current.ShowPastLockers = showPast.IsFiltered;
               vm.Save();
               await Navigation.PopModalAsync();
               if (Device.OS == TargetPlatform.Android)
                   MessagingService.Current.SendMessage("filter_changed");
           });

            BindingContext = vm = new FilterLockersViewModel(Navigation);
            LoadCategories();
        }

        void LoadCategories()
        {
            vm.LoadCategoriesAsync().ContinueWith((result) =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                        {
                            var allCell = new CategoryCell
                            {
                                BindingContext = vm.AllCategory
                            };

                            TableSectionCategories.Add(allCell);

                            foreach (var item in vm.Categories)
                            {
                                TableSectionCategories.Add(new CategoryCell
                                {
                                    BindingContext = item
                                });
                            }

                            var color = Device.OS == TargetPlatform.Windows || Device.OS == TargetPlatform.WinPhone ? "#7635EB" : string.Empty;

                            showPast = new Category
                            {
                                Name = "Show Past Lockers",
                                IsEnabled = true,
                                ShortName = "Show Past Lockers",
                                Color = color

                            };

                            showFavorites = new Category
                            {
                                Name = "Show Favorites Only",
                                IsEnabled = true,
                                ShortName = "Show Favorites Only",
                                Color = color
                            };

                            TableSectionFilters.Add(new CategoryCell
                            {
                                BindingContext = showPast
                            });

                            TableSectionFilters.Add(new CategoryCell
                            {
                                BindingContext = showFavorites
                            });

                            //if end of evolve
                            if (DateTime.UtcNow > Settings.EndOfEvolve)
                                showPast.IsEnabled = false;

                            showPast.IsFiltered = Settings.Current.ShowPastLockers;
                            showFavorites.IsFiltered = Settings.Current.FavoritesOnly;
                        });
                });
        }
    }
}

