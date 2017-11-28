using System;
using System.ComponentModel.DataAnnotations;

namespace Sealegs.DataObjects
{
    public class Notification : BaseDataObject
    {
        [Display(Name = "Text")]
        public string Text { get; set; }

        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        [Display(Name = "RoleId")]
        public String RoleId { get; set; }
    }
}

