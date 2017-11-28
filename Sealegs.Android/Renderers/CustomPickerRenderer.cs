using System;
using Sealegs.Clients.UI.Controls;
using Sealegs.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;


[assembly: ExportRenderer(typeof(CustomPicker), typeof(CustomPickerRenderer))]

namespace Sealegs.Droid.Renderers
{
    public class CustomPickerRenderer : PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            var picker = e.NewElement as CustomPicker;

            if (Control == null || e.NewElement == null || picker == null) return;

         //   Control.SetBackgroundColor(picker.UnderlineColor.ToAndroid()); //Setting underline color/background color
            Control.TextSize = Convert.ToSingle(picker.TitleFontSize);                 //Setting text size
         //   Control.SetHintTextColor(picker.TitleColor.ToAndroid());  //Setting title color

        }
    }
}