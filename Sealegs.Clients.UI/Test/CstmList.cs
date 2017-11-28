using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Sealegs.Clients.UI.Test
{
    public class CstmList : StackLayout
    {
        #region CTOR

        public CstmList()
        {
            var settingsGestureRecognizer = new TapGestureRecognizer {NumberOfTapsRequired = 1};
            settingsGestureRecognizer.Tapped +=  (s, e) =>
            {
                SendCustomClicked();
            };
           
        }
        #endregion

        #region Events
        public delegate void PhotoResultEventHandler(PhotoResultEventArgs result);

        public event PhotoResultEventHandler OnItemSelected;

        public void SendCustomClicked()
        {
            OnItemSelected?.Invoke(new PhotoResultEventArgs(true));
        } 
        #endregion

        #region Properties
        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }
        public DataTemplate Template
        {
            get => (DataTemplate)GetValue(TemplateProperty);
            set => SetValue(TemplateProperty, value);
        }


        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create("ItemsSource", typeof(IEnumerable), typeof(CstmList),
                null);


        static readonly BindableProperty TemplateProperty =
            BindableProperty.CreateAttached("Template", typeof(DataTemplate), typeof(CstmList), default(DataTemplate));


     

        #endregion

        #region Methods
        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);


            var template = this.GetValue(TemplateProperty) as DataTemplate;
            var collection = (IEnumerable)GetValue(ItemsSourceProperty);

            if (template == null || collection == null)
                return;

            this.Children.Clear();

            foreach (var child in collection)
            {
                {
                    var content = (View)template.CreateContent();
                    content.BindingContext = child;

                    this.Children.Add(content);
                }
            }

        } 
        #endregion



    }

    public class PhotoResultEventArgs:EventArgs
    {
        public PhotoResultEventArgs()
        {
            Success = false;
        }

        public PhotoResultEventArgs(bool value)
        {
            Success = value;
           
        }


        public bool Success { get; private set; }
    }
}
