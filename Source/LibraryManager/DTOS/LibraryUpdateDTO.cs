using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManager.DTOS
{
    public class LibraryUpdateDTO
    {
        [Required]
        [MaxLength(60)]
        public string Name { get; set; }

        [MaxLength(60)]
        public string City { get; set; }

        public DateTime BuiltDate { get; set; }
    }
}
