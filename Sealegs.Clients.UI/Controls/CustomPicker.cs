using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Sealegs.Clients.UI.Controls
{
    public class CustomPicker:Picker
    {
        #region Constructor
        public CustomPicker()
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                BackgroundColor = Color.Transparent;
            }
        }
        #endregion

        //Picker Under Line Color
        #region Picker Underline property
        public static readonly BindableProperty UnderlineColorProperty = BindableProperty.Create(nameof(UnderlineColor),
            typeof(Color), typeof(CustomPicker), Color.Transparent, BindingMode.TwoWay);
        public Color UnderlineColor
        {
            get { return (Color)GetValue(UnderlineColorProperty); }
            set { SetValue(UnderlineColorProperty, value); }
        }
        #endregion

        //Picker Title Text Color
        #region Title Text Color Property
        public static readonly BindableProperty TitleColorProperty = BindableProperty.Create(nameof(TitleColor),
            typeof(Color), typeof(CustomPicker), Color.White, BindingMode.TwoWay);
        public Color TitleColor
        {
            get { return (Color)GetValue(TitleColorProperty); }
            set { SetValue(TitleColorProperty, value); }
        }
        #endregion

        //Picker Title Font Size
        #region Title Font Size Property
        public static readonly BindableProperty TitleFontSizeProperty = BindableProperty.Create(nameof(TitleFontSize),
            typeof(double), typeof(CustomPicker), 15d, BindingMode.TwoWay);
        public double TitleFontSize
        {
            get { return (double)GetValue(TitleFontSizeProperty); }
            set { SetValue(TitleFontSizeProperty, value); }
        }
        #endregion
    }
}
