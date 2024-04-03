using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UserManagement.Models;
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

    [HttpGet]
    [Route("{userId}")]
    public IActionResult ViewUserDetails(long userId)
    {
        var user = _userService.GetUserById(userId);

        if (user == null)
        {
            return NotFound();
        }

        var viewModel = new UserDetailViewModel
        {
            Id = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            DateOfBirth = user.DateOfBirth,
            Email = user.Email,
            IsActive = user.IsActive
        };

        return View("UserDetails", viewModel);
    }

    [HttpGet]
    [Route("edit/{userId}")]
    public IActionResult Edit(long userId)
    {
        var user = _userService.GetUserById(userId);
        if (user == null)
        {
            return NotFound();
        }

        var viewModel = new EditUserViewModel
        {
            UserId = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            DateOfBirth = user.DateOfBirth,
            Email = user.Email,
            IsActive = user.IsActive,
        };

        return View("EditUser", viewModel);
    }

    [HttpPost]
    [Route("edit/{userId}")]
    public IActionResult Edit(EditUserViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        else
        {
            var user = _userService.GetUserById(model.UserId);
            if (user == null)
            {
                return NotFound();
            }

            user.DateOfBirth = model.DateOfBirth;
            user.Email = model.Email ?? user.Email;
            user.IsActive = model.IsActive;
            user.Forename = model.Forename ?? user.Forename;
            user.Surname = model.Surname ?? user.Surname;

            _userService.UpdateUser(user);

            return RedirectToAction("ViewUserDetails", new { userId = user.Id });
        }
    }

    [HttpGet]
    [Route("adduser")]
    public ViewResult AddUser()
    {
        var model = new AddUserViewModel
        {
            Forename = "",
            Surname = "",
            Email = "",
            IsActive = false
        };
        return View(model);
    }

    [HttpPost]
    [Route("adduser")]
    public IActionResult Add(AddUserViewModel model)
    {
        if (ModelState.IsValid)
        {
            var newUser = new User
            {
                Forename = model.Forename,
                Surname = model.Surname,
                DateOfBirth = model.DateOfBirth,
                Email = model.Email,
                IsActive = model.IsActive
            };

            _userService.AddUser(newUser);

            return RedirectToAction("List");
        }

        return View("AddUser", model);
    }

    [HttpPost]
    [Route("delete")]
    public IActionResult Delete(long userId)
    {
        _userService.DeleteUser(userId);
        return RedirectToAction("List");
    }
}
