using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sealegs.DataObjects;

namespace Sealegs.Backend.Models {
    public class Photo : BaseDataObject 
    {
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}