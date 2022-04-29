﻿using System.ComponentModel.DataAnnotations;

namespace DAL.DTOS
{
    public class LibraryCreateDTO
    {

        [Required]
        [MaxLength(60)]
        public string Name { get; set; }

        [Required]
        [MaxLength(60)]
        public string City { get; set; }

        public DateTime? BuiltDate { get; set; }

        [Required]
        public string Library_CreatedBy { get; set; }


    }
}
