using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Library
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(60)]
        [Required]
        public string Name { get; set; }

        [MaxLength(60)]
        [Required]
        public string City { get; set; }

        public DateTime? BuiltDate { get; set; }

        [Required]
        public DateTime Library_CreationDate { get; set; }

        public DateTime Library_ModifiedDate { get; set; }

        [Required]
        public string Library_CreatedBy { get; set; }

        public string? Library_ModifiedBy { get; set; }

        public bool Enabled { get; set; }

    }
}
