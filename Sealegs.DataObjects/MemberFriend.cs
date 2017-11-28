using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sealegs.DataObjects
{
    public class MemberFriend : BaseDataObject
    {
        [Display(Name = "Member Friend ID")]
        public String MemberFriendID { get; set; }

        [Display(Name = "Locker Member ID")]
        public String LockerMemberID { get; set; }

        [Display(Name = "Friend Name")]
        public string FriendName { get; set; }

        [Display(Name = "Street")]
        public string Street { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "State")]
        public string State { get; set; }

        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }

        [Display(Name = "Home Phone")]
        public string HomePhone { get; set; }

        [Display(Name = "Work Phone")]
        public string WorkPhone { get; set; }

        [Display(Name = "Mobile")]
        public string Mobile { get; set; }

        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        //public virtual LockerMember LockerMember { get; set; }
    }
}