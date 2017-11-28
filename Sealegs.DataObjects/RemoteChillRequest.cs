using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sealegs.DataObjects
{
    public class RemoteChillRequest : BaseDataObject
    {
        [Display(Name = "Remote Chill Request ID")]
        public String RemoteChillRequestID { get; set; }

        [Display(Name = "Locker Member ID")]
        public String LockerMemberID { get; set; }

        [Display(Name = "Member Bottle ID")]
        public String MemberBottleID { get; set; }

        [Display(Name = "Request Date ID")]
        public DateTime? RequestDate { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Is Completed")]
        public bool? IsCompleted { get; set; }

        [Display(Name = "Completed By ID")]
        public String CompletedByID { get; set; }

        // public virtual MemberBottle MemberBottle { get; set; }
    }

    public class RemoteChillRequestExteneded: BaseDataObject
    {
        public RemoteChillRequest ChillRequest { get; set; }
        public Wine Wine { get; set; }
        public LockerMember Locker { get; set; }
    }
}