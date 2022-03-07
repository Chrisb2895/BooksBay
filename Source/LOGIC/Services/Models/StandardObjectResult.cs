using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOGIC.Services.Models
{
    public abstract class StandardObjectResult
    {
        public bool Success { get; set; }

        public string UserMessage { get; set; }

        internal string InternalMessage { get; set; }

        internal Exception Exception { get; set; }

        public StandardObjectResult ()
        {
            Success = false;

            UserMessage = string.Empty;

            InternalMessage = string.Empty;

            Exception = null;
        }
    }
}
