using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Sealegs.DataObjects
{
    public class FeaturedEvent : BaseDataObject
    {
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "StartTime")]
        public DateTime? StartTime { get; set; }

        [Display(Name = "EndTime")]
        public DateTime? EndTime { get; set; }

        [Display(Name = "IsAllDay")]
        public bool IsAllDay { get; set; }

        [Display(Name = "LocationName")]
        public string LocationName { get; set; }

  

       

#if MOBILE

        [JsonIgnore]
        public DateTime StartTimeOrderBy { get { return StartTime.HasValue ? StartTime.Value : DateTime.MinValue; } }
#endif
    }
}

