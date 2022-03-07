using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOGIC.Services.Models
{
    public class GenericResultSet<T>:StandardObjectResult
    {
        public T ResultSet { get; set; }
    }
}
