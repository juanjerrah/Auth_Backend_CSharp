using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthApplication.Models;
using Microsoft.IdentityModel.Tokens;

namespace AuthApplication.Service;

public class TokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(User user)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Key:TokenKey"] ?? string.Empty));

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, "admin")
        };

        var token = new JwtSecurityToken(
            issuer: "eu_mesmo",
            audience: "auth_back",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256)
        );

        var tokenHandler = new JwtSecurityTokenHandler();

        var userToken = tokenHandler.WriteToken(token);

        return userToken;
    }
}