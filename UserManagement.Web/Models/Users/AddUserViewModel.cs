﻿using System;
using System.ComponentModel.DataAnnotations;

namespace UserManagement.Web.Models.Users
{
    public class AddUserViewModel
    {
        [Required(ErrorMessage = "Forename is required")]
        public required string Forename { get; set; }

        [Required(ErrorMessage = "Surname is required")]
        public required string Surname { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public required string Email { get; set; }
        public bool IsActive { get; set; }
    }
}
