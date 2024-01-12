using Microsoft.IdentityModel.Tokens;
using Notes.Domain;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Notes.DataTransfer.Output.Token;

<<<<<<< HEAD

=======
>>>>>>> 2537bb1d14a80078e99313883c95dd51bb0392b9
namespace Notes.Controllers;

public partial class AuthController
{
    private TokenOutput GenerateToken(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Email),
            new Claim("<<_Notes_>>", "<<_{Notes@2023}_>>"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expirationToken = DateTime.UtcNow.AddHours(double.Parse(_configuration["TokenConfiguration:ExpireHour"]));

        JwtSecurityToken token = new(issuer: _configuration["TokenConfiguration:Issuer"], audience: _configuration["TokenConfiguration:Audience"], claims: claims, expires: expirationToken, signingCredentials: credentials);

        return new TokenOutput()
        {
            Authenticated = true,
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = expirationToken,
            Message = "Generate token sucessfull.",
            authorId = user.Id,
            UserName = user.UserName
        };
    }
}


