using System;
using System.ComponentModel.DataAnnotations;

namespace Sealegs.DataObjects
{
    public class News : BaseDataObject
    {
        [Display(Name = "Name")]
        public string Name { get; set; }
       
        [Display(Name = "Description")]
        public string Description { get; set; }
      
        [Display(Name = "ImageUrl")]
        public string ImageUrl { get; set; }
      
        [Display(Name = "WebsiteUrl")]
        public string WebsiteUrl { get; set; }
      
        [Display(Name = "TwitterUrl")]
        public string TwitterUrl { get; set; }
    
        [Display(Name = "Rank")]
        public int Rank { get; set; }

        //[Display(Name = "CreatedAt")]
        //public DateTime CreatedAt { get; set; }
        
    }
}