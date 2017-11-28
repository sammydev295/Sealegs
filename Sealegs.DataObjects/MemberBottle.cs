using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sealegs.DataObjects
{
    public class MemberBottle : BaseDataObject
    {
        [Display(Name = "Member Bottle ID")]
        public System.Guid MemberBottleID { get; set; }

        [Display(Name = "Locker Member ID ")]
        public System.Guid LockerMemberID { get; set; }

        [Display(Name = "Wine Title")]
        public string WineTitle { get; set; }

        [Display(Name = "Vintage")]
        public string Vintage { get; set; }

        [Display(Name = "Wine Varietal ID")]
        public Nullable<System.Guid> WineVarietalID { get; set; }

        [Display(Name = "For Special Occassion")]
        public Nullable<bool> ForSpecialOccassion { get; set; }

        [Display(Name = "Special Occassion Description")]
        public string SpecialOccassionDescription { get; set; }

        [Display(Name = "Bottle Size")]
        public string BottleSize { get; set; }

        [Display(Name = "CheckedIn Date")]
        public System.DateTime CheckedInDate { get; set; }

        [Display(Name = "CheckedOut Date")]
        public Nullable<System.DateTime> CheckedOutDate { get; set; }

        [Display(Name = "CheckedOut Locker Member ID")]
        public Nullable<System.Guid> CheckedOutLockerMemberID { get; set; }

        [Display(Name = "CheckedOut Member Friend ID")]
        public Nullable<System.Guid> CheckedOutMemberFriendID { get; set; }

        [Display(Name = "Member Signature")]
        public string CheckedOutMemberSignature { get; set; }

        [Display(Name = "Employee Signature")]
        public string CheckedOutEmployeeSignature { get; set; }

        public virtual LockerMember LockerMember { get; set; }
    }
}