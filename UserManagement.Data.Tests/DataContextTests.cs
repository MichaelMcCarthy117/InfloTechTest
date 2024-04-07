using System;
using System.Linq;
using UserManagement.Models;

namespace UserManagement.Data.Tests;

public class DataContextTests
{
    [Fact]
    public void GetAll_WhenNewEntityAdded_MustIncludeNewEntity()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var context = CreateContext();

        var entity = new User
        {
            Forename = "Brand New",
            Surname = "User",
            Email = "brandnewuser@example.com"
        };
        context.Create(entity);

        // Act: Invokes the method under test with the arranged parameters.
        var result = context.GetAll<User>();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result
            .Should().Contain(s => s.Email == entity.Email)
            .Which.Should().BeEquivalentTo(entity);
    }

    [Fact]
    public void GetAll_WhenDeleted_MustNotIncludeDeletedEntity()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var context = CreateContext();
        var entity = context.GetAll<User>().First();
        context.Delete(entity);

        // Act: Invokes the method under test with the arranged parameters.
        var result = context.GetAll<User>();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().NotContain(s => s.Email == entity.Email);
    }

    [Fact]
    public void Update_WhenEntityExists_MustUpdateEntity()
    {
        // Arrange
        var context = CreateContext();
        var entity = context.GetAll<User>().First();
        var originalForename = entity.Forename;
        var updatedForename = "Updated Forename";

        // Act
        entity.Forename = updatedForename;
        context.Update(entity);

        // Assert
        var updatedEntity = context.GetAll<User>().First(u => u.Id == entity.Id);
        updatedEntity.Forename.Should().Be(updatedForename);
    }

    [Fact]
    public void Delete_WhenEntityDoesNotExist_MustNotThrowException() //Test for Delete method when entity does not exist
    {
        // Arrange
        var context = CreateContext();
        var nonExistingEntity = new User { Id = 999 };

        // Act
        Action act = () => context.Delete(nonExistingEntity);

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void SaveChanges_AfterModifyingEntities_MustPersistChangesToDataStore() //Test for SaveChanges method
    {
        // Arrange
        var context = CreateContext();
        var entity = context.GetAll<User>().First();
        var originalForename = entity.Forename;
        var updatedForename = "Updated Forename";

        // Act
        entity.Forename = updatedForename;
        context.Update(entity);
        context.SaveChanges();

        // Assert
        var updatedEntity = context.GetAll<User>().First(u => u.Id == entity.Id);
        updatedEntity.Forename.Should().Be(updatedForename);
    }

    [Fact]
    public void GetAll_WhenCalled_MustReturnAllEntitiesOfType() //Test for GetAll method
    {
        // Arrange
        var context = CreateContext();

        // Act
        var users = context.GetAll<User>().ToList();

        // Assert
        users.Should().HaveCount(11); // Only work if I have 11 predefined users
    }

    private DataContext CreateContext() => new();
}
