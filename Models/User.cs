namespace AuthApplication.Models;

public class User
{
    public Guid Id { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }

    public User(Guid id, string email, string password)
    {
        Id = id;
        Email = email;
        Password = password;
    }

    public void SetPassword(string password) => Password = password;
}