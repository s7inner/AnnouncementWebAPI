using System.ComponentModel.DataAnnotations;

namespace AnnouncementWebAPI.Models
{
    public class Announcement
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(50, ErrorMessage = "Title must be between 4 and 50 characters.", MinimumLength = 4)]
        [RegularExpression("^[^0-9]+$", ErrorMessage = "Title cannot contain numbers.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(2000, ErrorMessage = "Description must be between 50 and 2000 characters.", MinimumLength = 50)]
        [RegularExpression("^[^0-9]+$", ErrorMessage = "Description cannot contain numbers.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "DateAdded is required.")]
        [Range(typeof(DateTime), "1/1/1900", "12/31/2099", ErrorMessage = "Invalid DateAdded value.")]
        public DateTime DateAdded { get; set; }
    }

}
