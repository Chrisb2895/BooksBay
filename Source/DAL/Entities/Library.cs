using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Library
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(60)]
        public string Name { get; set; }

        [Required]
        [MaxLength(60)]
        public string City { get; set; }

        [Required]
        public DateTime BuiltDate { get; set; }

        public DateTime Library_CreationDate { get; set; }

        public DateTime Library_ModifiedDate { get; set; }

        [Required]
        public string Library_CreatedBy { get; set; }
        [Required]
        public string Library_ModifiedBy { get; set; }

        public bool Enabled { get; set; }

    }
}
