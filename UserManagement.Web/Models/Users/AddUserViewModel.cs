using System;
using System.ComponentModel.DataAnnotations;

namespace UserManagement.Web.Models.Users
{
    public class AddUserViewModel
    {
        public AddUserViewModel()
        {
            // Initialize non-nullable properties here
            Forename = "";
            Surname = "";
            DateOfBirth = DateTime.Now;
            Email = "";
        }

        [Required(ErrorMessage = "Forename is required")]
        [MaxLength(50)]
        public string Forename { get; set; }

        [Required(ErrorMessage = "Surname is required")]
        [MaxLength(50)]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [MaxLength(254)]
        public string Email { get; set; }

        public bool IsActive { get; set; }
    }
}
