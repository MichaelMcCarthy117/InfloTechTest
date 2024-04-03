using System;
using System.Collections.Generic;
using System.Linq;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Services.Domain.Implementations;

public class UserService : IUserService
{
    private readonly IDataContext _dataAccess;
    public UserService(IDataContext dataAccess) => _dataAccess = dataAccess;

    /// <summary>
    /// Return users by active state
    /// </summary>
    /// <param name="isActive"></param>
    /// <returns></returns>
    public IEnumerable<User> FilterByActive(bool isActive)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<User> GetAll() => _dataAccess.GetAll<User>();

    public User? GetUserById(long userId)
    {
        return _dataAccess.GetAll<User>().FirstOrDefault(user => user.Id == userId);
    }

    public void AddUser(User user)
    {
        _dataAccess.Create(user);
        _dataAccess.SaveChanges();
    }

    public void UpdateUser(User user)
    {
        _dataAccess.Update(user);
        _dataAccess.SaveChanges();
    }

    public void DeleteUser(long userId)
    {
        var userToDelete = _dataAccess.GetAll<User>().FirstOrDefault(user => user.Id == userId);
        if (userToDelete != null)
        {
            _dataAccess.Delete(userToDelete);
            _dataAccess.SaveChanges();
        }
    }
}
