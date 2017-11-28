using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sealegs.DataObjects;

namespace Sealegs.Backend.Identity
{
    public class SealegsAuthResponse
    {
        public bool Success { get; set; }

        public string Error { get; set; }

        public SealegsUser User { get; set; }

        public string Token { get; set; }
    }
}
