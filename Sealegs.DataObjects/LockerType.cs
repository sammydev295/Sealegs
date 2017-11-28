using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Sealegs.DataObjects
{
    public class LockerType : BaseDataObject
    {
        public const String Bottlesx48 = "4B5A72A4-F8B7-487B-909C-8DE27C3A1240";
        public const String Bottlesx24 = "FBEEED6F-ECE5-4F14-8815-32DB27F0D727";
        public const String Unlimited = "8F584C26-3AFE-4929-89A4-5FC3C670E9E0";

        [Display(Name = "Locker Type ID")]
        public System.Guid LockerTypeID { get; set; }

        [Display(Name = "Locker Type Name")]
        public string LockerTypeName { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public static ObservableCollection<LockerType> All => new ObservableCollection<LockerType>()
        {
            new LockerType() { LockerTypeID = new Guid(LockerType.Bottlesx24), LockerTypeName = "24 Bottles" },
            new LockerType() { LockerTypeID = new Guid(LockerType.Bottlesx48), LockerTypeName = "48 Bottles" },
            new LockerType() { LockerTypeID = new Guid(LockerType.Bottlesx48), LockerTypeName = "Unlimited Bottles" }
        };
    }
}