using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Sealegs.DataObjects;

namespace Sealegs.Backend.Identity
{
    public sealed class LoginResult
    {
        [JsonProperty("authenticationToken")]
        public string RawToken { get; set; }

        [JsonProperty("user")]
        public SealegsUser User { get; set; }
    }
}
