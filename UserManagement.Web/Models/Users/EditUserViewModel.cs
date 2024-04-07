using System;
using System.ComponentModel.DataAnnotations;

namespace UserManagement.Web.Models.Users;

public class EditUserViewModel
{
    public long UserId { get; set; }

    [Required(ErrorMessage = "Forename is required")]
    [MaxLength(50)]
    public string? Forename { get; set; }

    [Required(ErrorMessage = "Surname is required")]
    [MaxLength(50)]
    public string? Surname { get; set; }

    [Required(ErrorMessage = "Date of Birth is required")]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [MaxLength(254)]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string? Email { get; set; }

    public bool IsActive { get; set; }
}
