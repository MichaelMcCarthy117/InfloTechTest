using Microsoft.Extensions.Logging;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;
using UserManagement.WebMS.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;

public class UserControllerTests
{
    [Fact]
    public void List_WhenServiceReturnsUsers_ModelMustContainUsers()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<UsersController>>();
        var controller = CreateController(loggerMock.Object);
        var users = SetupUsers();

        // Act
        var result = controller.List(filterIsActive: null);

        // Assert
        result.Model
            .Should().BeOfType<UserListViewModel>()
            .Which.Items.Should().BeEquivalentTo(users);
    }

    [Fact]
    public void Add_WhenModelStateIsValid_ShouldAddUserAndRedirectToList()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<UsersController>>();
        var controller = CreateController(loggerMock.Object);
        var model = new AddUserViewModel
        {
            Forename = "John",
            Surname = "Doe",
            Email = "johndoe@example.com",
            DateOfBirth = new DateTime(1990, 1, 1),
            IsActive = true
        };

        // Act
        var result = controller.Add(model);

        // Assert
        _userService.Verify(s => s.AddUser(It.IsAny<User>()), Times.Once);
        result.Should().BeOfType<RedirectToActionResult>()
            .Which.ActionName.Should().Be("List");
    }

    private User[] SetupUsers(string forename = "Johnny", string surname = "User", string email = "juser@example.com", bool isActive = true)
    {
        var users = new[]
        {
            new User
            {
                Forename = forename,
                Surname = surname,
                Email = email,
                IsActive = isActive
            }
        };

        _userService
            .Setup(s => s.GetAll())
            .Returns(users);

        return users;
    }

    private readonly Mock<IUserService> _userService = new();
    private UsersController CreateController(ILogger<UsersController> logger) => new UsersController(_userService.Object, logger);
}
