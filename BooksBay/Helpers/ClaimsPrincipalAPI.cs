using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace BooksBay.Helpers
{

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "9.13.15.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class ClaimsPrincipal
    {
        [Newtonsoft.Json.JsonProperty("Claims", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.ICollection<Claim> Claims { get; set; }

        [Newtonsoft.Json.JsonProperty("Identities", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.ICollection<ClaimsIdentity> Identities { get; set; }

        [Newtonsoft.Json.JsonProperty("Identity", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public IIdentity Identity { get; set; }

        public string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }

        public static ClaimsPrincipal FromJson(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<ClaimsPrincipal>(data);
        }

    }

}
