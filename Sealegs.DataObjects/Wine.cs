using System;
using System.ComponentModel.DataAnnotations;

namespace Sealegs.DataObjects
{
    public class Wine : BaseDataObject
    {
        [Display(Name = "MemberBottleID")]
        public String MemberBottleID { get; set; }

        [Display(Name = "LockerID")]
        public string LockerID { get; set; }

        [Display(Name = "WineTitle")]
        public string WineTitle { get; set; }

        string _vintage = String.Empty;
        [Display(Name = "Vintage")]
        public string Vintage
        {
            get { return _vintage; }
            set
            {
                if (_vintage == value)
                    return;

                _vintage = value;
#if MOBILE
                OnPropertyChanged();
#endif

            }
        }

        [Display(Name = "WineVarietalID")]
        public String WineVarietalId { get; set; }

        private WineVarietal _wineVarietal;
        public WineVarietal WineVarietal
        {
            get { return _wineVarietal; }
            set
            {
                if (_wineVarietal == value)
                    return;

                _wineVarietal = value;
#if MOBILE
                OnPropertyChanged();
#endif
            }
        }

        private bool _specialOccassion = false;
        [Display(Name = "SpecialOccassion")]
        public bool SpecialOccassion
        {
            get { return _specialOccassion; }
            set
            {
                if (_specialOccassion == value)
                    return;

                _specialOccassion = value;
#if MOBILE
                OnPropertyChanged();
#endif
            }
        }

        string _specialOccassionDescription = String.Empty;
        [Display(Name = "SpecialOccassionDescription")]
        public string SpecialOccassionDescription
        {
            get { return _specialOccassionDescription; }
            set
            {
                if (_specialOccassionDescription == value)
                    return;

                _specialOccassionDescription = value;
#if MOBILE
                OnPropertyChanged();
#endif
            }
        }

        string _bottleSize = String.Empty;
        [Display(Name = "BottleSize")]
        public string BottleSize
        {
            get { return _bottleSize; }
            set
            {
                if (_bottleSize == value)
                    return;

                _bottleSize = value;
#if MOBILE
                OnPropertyChanged();
#endif
            }
        }


        string _notes = String.Empty;
        [Display(Name = "Notes")]
        public string Notes
        {
            get { return _notes; }
            set
            {
                if (_notes == value)
                    return;

                _notes = value;
#if MOBILE
                OnPropertyChanged();
#endif
            }
        }

        int _quantity;
        [Display(Name = "Quantity")]
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                if (_quantity == value)
                    return;

                _quantity = value;
#if MOBILE
                OnPropertyChanged();
#endif
            }
        }

        string _imagePath = String.Empty;
        [Display(Name = "ImagePath")]
        public string ImagePath
        {
            get { return _imagePath; }
            set
            {
                if (_imagePath == value)
                    return;

                _imagePath = value;
#if MOBILE
                OnPropertyChanged();
#endif
            }
        }

        [Display(Name = "CheckedInDate")]
        public System.DateTime CheckedInDate { get; set; }

        [Display(Name = "CheckedOutDate")]
        public DateTime? CheckedOutDate { get; set; }

        [Display(Name = "CheckedOutLockerMemberID")]
        public String CheckedOutLockerMemberID { get; set; }

        [Display(Name = "CheckedOutMemberFriendID")]
        public String CheckedOutMemberFriendID { get; set; }

        [Display(Name = "CheckedOutMemberSignature")]
        public string CheckedOutMemberSignature { get; set; }

        [Display(Name = "CheckedOutEmployeeSignature")]
        public string CheckedOutEmployeeSignature { get; set; }

#region IsChecked

        private bool? isChecked = false;
        [Display(Name = "IsChecked")]
        public bool? IsChecked
        {
            get { return isChecked; }
            set
            {
                isChecked = value;
#if !BACKEND
                OnPropertyChanged();
#endif
            }
        }
        [Display(Name = "CheckoutQuantity")]
        public int? CheckoutQuantity { get; set; }

#endregion

#region BottleImageUri

        [Newtonsoft.Json.JsonIgnore]
        public const string DefaultBottleImage = "profile_generic";

        #endregion

        [Newtonsoft.Json.JsonIgnore]
        public static Wine Defaults => new Wine() { Id = new Guid().ToString(), ImagePath = DefaultBottleImage, CheckedInDate = DateTime.Now, Quantity = 1 };

#if MOBILE
        //[SQLite.Net.Attributes.Ignore]
        [Newtonsoft.Json.JsonIgnore]
        public bool IsChilledRequestSent { get; set; }

#endif
    }
}
