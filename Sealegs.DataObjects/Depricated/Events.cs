using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sealegs.DataObjects
{
    public class Events : BaseDataObject
    {
        [Display(Name = "Event ID")]
        public System.Guid EventId { get; set; }

        [Display(Name = "Event Title")]
        public string EventTitle { get; set; }

        [Display(Name = "Description")]
        public string EventDescription { get; set; }

        [Display(Name = "Start Time")]
        public Nullable<System.TimeSpan> EventStartTime { get; set; }

        [Display(Name = "End Time")]
        public Nullable<System.TimeSpan> EventEndTime { get; set; }

        [Display(Name = "Event Date")]
        public Nullable<System.DateTime> EventDate { get; set; }
    }
}