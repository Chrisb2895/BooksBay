using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Library
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(60)]
        public string Name { get; set; }

        [MaxLength(60)]
        public string City { get; set; }

        public DateTime BuiltDate { get; set; }

        public DateTime Library_CreationDate { get; set; }

        public DateTime Library_ModifiedDate { get; set; }

        public string Library_CreatedBy { get; set; }

        public string Library_ModifiedBy { get; set; }

    }
}
