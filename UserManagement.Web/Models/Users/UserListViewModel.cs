using UserManagement.Models;

namespace UserManagement.Web.Models.Users;

public class UserListViewModel
{
    public List<UserListItemViewModel> Items { get; set; } = new();
}

public class UserListItemViewModel
{
    public UserListItemViewModel(User user)
    {
        Id = user.Id;
        Forename = user.Forename;
        Surname = user.Surname;
        Email = user.Email;
        IsActive = user.IsActive;
    }
    public long Id { get; set; }
    public string? Forename { get; set; }
    public string? Surname { get; set; }
    public string? Email { get; set; }
    public bool IsActive { get; set; }
}
