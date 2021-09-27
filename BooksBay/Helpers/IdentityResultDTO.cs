using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksBay.Helpers
{
    public class IdentityResultDTO : IdentityResult
    {
        [JsonProperty]
        public new bool Succeeded { get; protected set; }

    }
}
