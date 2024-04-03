﻿using System;

namespace UserManagement.Web.Models.Users
{
    public class UserDetailViewModel
    {
        public long Id { get; set; }
        public string? Forename { get; set; }
        public string? Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Email { get; set; }
        public bool IsActive { get; set; }
    }
}
