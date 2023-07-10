using AuthApplication.Models;
using AuthApplication.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuthApplication.Controllers;

[Route("api/Auth")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _repository;
    private readonly IAuthRepository _authRepository;

    public AuthController(IUserRepository repository, IAuthRepository authRepository)
    {
        _repository = repository;
        _authRepository = authRepository;
    }

    [HttpPost]
    [Route("Register")]

    public ActionResult<Guid> RegisterUser(string email, string password)
    {
        var hasUser = _repository.GetUserByEmail(email);

        if (hasUser != null)
            return BadRequest($"User with email: {email} already exists");

        var user = new User(Guid.NewGuid(), email, password);

        var encryptedPassword = _authRepository.EncryptPassword(password, "pepper");
        
        user.SetPassword(encryptedPassword);
        
        _repository.AddUser(user);
        
        return Ok(user.Id);
    }

    [HttpPost]
    [Route("Login")]
    public ActionResult<bool> LoginUser(string email, string password)
    {
        var user = _repository.GetUserByEmail(email);

        if (user == null)
            return NotFound("User not found");

        var isCorrectPassword = _authRepository.ValidatePassword(user.Password, password);

        if (!isCorrectPassword)
            return BadRequest("Wrong password");

        return isCorrectPassword;
    }
    
    
}