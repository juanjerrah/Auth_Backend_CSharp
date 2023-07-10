using AuthApplication.Models;

namespace AuthApplication.Repository.Interfaces;

public interface IUserRepository
{
    User? GetUserByEmail(string email);
    void AddUser(User user);
}