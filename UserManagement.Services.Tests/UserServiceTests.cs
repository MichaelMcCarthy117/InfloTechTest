using System;
using System.Linq;
using UserManagement.Models;
using UserManagement.Services.Domain.Implementations;

namespace UserManagement.Data.Tests;

public class UserServiceTests
{
    private IQueryable<User> SetupUsers(DateTime dateOfBirth, string forename = "Johnny", string surname = "User", string email = "juser@example.com", bool isActive = true)
    {
        var users = new[]
        {
        new User
        {
            Forename = forename,
            Surname = surname,
            Email = email,
            IsActive = isActive,
            DateOfBirth = dateOfBirth
        }
    };

        // Mocking the behavior of IDataContext
        _dataContext
            .Setup(s => s.GetAll<User>())
            .Returns(users.AsQueryable());

        // Return users as IQueryable
        return users.AsQueryable();
    }

    private readonly Mock<IDataContext> _dataContext = new Mock<IDataContext>();
    private UserService CreateService() => new UserService(_dataContext.Object);

    [Fact]
    public void GetAll_WhenContextReturnsEntities_MustReturnSameEntities()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var dateOfBirth = new DateTime(1985, 01, 12);
        var service = CreateService();
        var users = SetupUsers(dateOfBirth);

        // Act: Invokes the method under test with the arranged parameters.
        var result = service.GetAll();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeEquivalentTo(users);
    }

    [Fact]
    public void GetUserById_WhenValidUserIdProvided_MustReturnCorrectUser()
    {
        // Arrange
        var dateOfBirth = new DateTime(1985, 01, 12);
        var service = CreateService();
        var users = SetupUsers(dateOfBirth);
        var userIdToRetrieve = users.First().Id;

        // Act
        var result = service.GetUserById(userIdToRetrieve);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(users.First());
        result.Should().NotBeNull();// Ensure updatedUser is not null

        if (result != null)
        {
            result.DateOfBirth.Should().Be(dateOfBirth);
        }
    }

    [Fact]
    public void UpdateUser_WhenExistingUserUpdated_MustPersistChangesInDataStore()
    {
        // Arrange
        var service = CreateService();
        var users = SetupUsers(DateTime.Now);
        var userToUpdate = users.First();
        var updatedEmail = "updated@example.com";
        userToUpdate.Email = updatedEmail;

        // Act
        service.UpdateUser(userToUpdate);
        var result = service.GetAll();

        // Assert
        result.Should().NotBeNull();
        var updatedUser = result.FirstOrDefault(u => u.Id == userToUpdate.Id);
        updatedUser.Should().NotBeNull();

        if (updatedUser != null)
        {
            updatedUser.Email.Should().Be(updatedEmail);
        }
    }
}
