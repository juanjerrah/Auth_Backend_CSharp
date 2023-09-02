using AuthApplication.Models;
using AuthApplication.Repository.Interfaces;
using AuthApplication.Service;
using Microsoft.AspNetCore.Mvc;

namespace AuthApplication.Controllers;

[Route("api/Auth")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _repository;
    private readonly IAuthRepository _authRepository;
    private readonly TokenService _tokenService;

    public AuthController(IUserRepository repository, IAuthRepository authRepository, TokenService tokenService)
    {
        _repository = repository;
        _authRepository = authRepository;
        _tokenService = tokenService;
    }

    [HttpPost]
    [Route("Register")]

    public ActionResult<string> RegisterUser(string email, string password)
    {
        var hasUser = _repository.GetUserByEmail(email);

        if (hasUser != null)
            return BadRequest($"User with email: {email} already exists");

        var user = new User(Guid.NewGuid(), email, password);

        var encryptedPassword = _authRepository.EncryptPassword(password, "pepper");
        
        user.SetPassword(encryptedPassword);
        
        _repository.AddUser(user);

        var token = _tokenService.GenerateToken(user);
        
        return Ok(token);
    }

    [HttpPost]
    [Route("Login")]
    public ActionResult<string> LoginUser(string email, string password)
    {
        var user = _repository.GetUserByEmail(email);

        if (user == null)
            return NotFound("User not found");

        var isCorrectPassword = _authRepository.ValidatePassword(user.Password, password);

        if (!isCorrectPassword)
            return BadRequest("Wrong password");
        
        var token = _tokenService.GenerateToken(user);

        return Ok(token);
    }
    
    
}