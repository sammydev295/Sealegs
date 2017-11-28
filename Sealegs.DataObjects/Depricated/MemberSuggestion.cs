using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sealegs.DataObjects
{
    public class MemberSuggestion : BaseDataObject
    {
        [Display(Name = "Member Suggestion ID")]
        public System.Guid MemberSuggestionID { get; set; }

        [Display(Name = "Locker Member ID")]
        public System.Guid LockerMemberID { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        public virtual LockerMember LockerMember { get; set; }
    }
}