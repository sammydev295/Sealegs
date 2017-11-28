using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sealegs.DataObjects
{
    public class WineVarietal : BaseDataObject
    {
        [Display(Name = "Wine Varietal ID")]
        public String WineVarietalId { get; set; }

        private string _varietalName;
        [Display(Name = "Varietal Name")]
        public string VarietalName
        {
            get { return _varietalName; }
            set
            {
                if (_varietalName == value)
                    return;

                _varietalName = value;
#if MOBILE
                OnPropertyChanged();
#endif
            }
        }
    }
}