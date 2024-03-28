using System.Linq;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;

namespace UserManagement.WebMS.Controllers;

[Route("users")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService) => _userService = userService;

    [HttpGet]
    public ViewResult List(bool? filterIsActive)
    {
        IEnumerable<UserListItemViewModel> items;
        if (!filterIsActive.HasValue)
        {
            items = _userService.GetAll().Select(user => new UserListItemViewModel(user));
        }
        else if (filterIsActive == true)
        {
            items = _userService.GetAll().Where(user => user.IsActive).Select(user => new UserListItemViewModel(user));
        }
        else
        {
            items = _userService.GetAll().Where(user => !user.IsActive).Select(user => new UserListItemViewModel(user));
        }

        var model = new UserListViewModel
        {
            Items = items.ToList()
        };

        return View(model);
    }
}
