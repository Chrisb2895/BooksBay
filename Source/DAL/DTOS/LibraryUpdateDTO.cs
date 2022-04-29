using System.ComponentModel.DataAnnotations;

namespace DAL.DTOS
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
