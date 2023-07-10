namespace AuthApplication.Repository.Interfaces;

public interface IAuthRepository
{
    string EncryptPassword(string password, string salt);
    bool ValidatePassword(string currentPassword, string password);
}