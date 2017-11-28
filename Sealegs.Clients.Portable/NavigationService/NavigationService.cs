using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using MoreLinq;
using Xamarin.Forms;

using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using PopupNavigation = Rg.Plugins.Popup.Services.PopupNavigation;
using GalaSoft.MvvmLight.Views;

using Sealegs.Clients.Portable;

namespace Sealegs.Clients.Portable.NavigationService
{
    public class NavigationService : INavigationService, ISealegsNavigationService
    {
        #region Fields

        private readonly Dictionary<string, Type> _pagesByKey = new Dictionary<string, Type>();
        private NavigationPage _navigation;

        #endregion

        #region CurrentPageKey

        public string CurrentPageKey
        {
            get
            {
                lock (_pagesByKey)
                {
                    if (_navigation.CurrentPage == null)
                    {
                        return null;
                    }

                    var pageType = _navigation.CurrentPage.GetType();

                    return _pagesByKey.ContainsValue(pageType)
                        ? _pagesByKey.First(p => p.Value == pageType).Key
                        : null;
                }
            }
        }

        #endregion

        #region GoBack

        public async void GoBack()
        {
            await _navigation.PopAsync(false);
        }

        #endregion

        #region NavigateTo (2 overloads)

        public void NavigateTo(string pageKey)
        {
            NavigateTo(pageKey, null);
        }

        public void NavigateTo(string pageKey, object parameter)
        {
            lock (_pagesByKey)
            {
                if (_pagesByKey.ContainsKey(pageKey))
                {
                    var type = _pagesByKey[pageKey];
                    ConstructorInfo constructor;
                    object[] parameters;

                    if (parameter == null)
                    {
                        constructor = type.GetTypeInfo()
                            .DeclaredConstructors
                            .FirstOrDefault(c => !c.GetParameters().Any());

                        parameters = new object[]
                        {
                        };
                    }
                    else
                    {
                        constructor = type.GetTypeInfo()
                            .DeclaredConstructors
                            .FirstOrDefault(
                                c =>
                                {
                                    var p = c.GetParameters();
                                    return p.Count() == 1
                                           && p[0].ParameterType == parameter.GetType();
                                });

                        parameters = new[]
                        {
                            parameter
                        };
                    }

                    if (constructor == null)
                    {
                        throw new InvalidOperationException(
                            "No suitable constructor found for page " + pageKey);
                    }

                    var page = constructor.Invoke(parameters) as Page;
                    _navigation.PushAsync(page);
                }
                else
                {
                    throw new ArgumentException(
                        string.Format(
                            "No such page: {0}. Did you forget to call NavigationService.Configure?",
                            pageKey),
                        "pageKey");
                }
            }
        }

        #endregion

        #region PopupGoBack

        public async void PopupGoBack()
        {
            await PopupNavigation.PopAsync();
        }

        #endregion

        #region PopupNavigateTo (2 overloads)

        public void PopupNavigateTo(string pageKey)
        {
            PopupNavigateTo(pageKey, null);
        }

        public void PopupNavigateTo(string pageKey, params object[] pageParameters)
        {
            lock (_pagesByKey)
            {
                if (_pagesByKey.ContainsKey(pageKey))
                {
                    var type = _pagesByKey[pageKey];
                    ConstructorInfo constructor;
                    object[] parameters;
                    if (pageParameters == null)
                    {
                        constructor = type.GetTypeInfo()
                            .DeclaredConstructors
                            .FirstOrDefault(c => !c.GetParameters().Any());
                        parameters = new object[] { };
                    }
                    else
                    {
                        var constructors = type.GetTypeInfo().DeclaredConstructors;

                        constructor = constructors.FirstOrDefault(
                                c =>
                                {
                                    var p = c.GetParameters();
                                    return p.Count() == pageParameters.Count()
                                                            && p.ToList().TrueForAll(t => pageParameters.Any(pp => t.ParameterType== pp.GetType()));
                                });
                    }

                    if (constructor == null)
                    {
                        throw new InvalidOperationException(
                            "No suitable constructor found for page " + pageKey);
                    }

                    var page = constructor.Invoke(pageParameters) as PopupPage;
                    PopupNavigation.PushAsync(page);
                }
                else
                {
                    throw new ArgumentException(
                        string.Format(
                            "No such page: {0}. Did you forget to call NavigationService.Configure?",
                            pageKey),
                        "pageKey");
                }
            }
        }

        #endregion

        #region Configure

        public void Configure(string pageKey, Type pageType)
        {
            lock (_pagesByKey)
            {
                if (_pagesByKey.ContainsKey(pageKey))
                {
                    _pagesByKey[pageKey] = pageType;
                }
                else
                {
                    _pagesByKey.Add(pageKey, pageType);
                }
            }
        }

        #endregion

        #region Initialize

        public void Initialize(NavigationPage navigation)
        {
            navigation.BarBackgroundColor = Color.FromHex("#7635EB");
            navigation.BarTextColor = Color.White;
            _navigation = navigation;
        }

        #endregion
    }
}
