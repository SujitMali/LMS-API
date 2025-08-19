using System;
using System.Configuration;
using System.Security.Claims;
using System.Text;
using Library_Mangament_System_API_ClassLibrary.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Library_Mangament_System_API_ClassLibrary.DAL;

public static class JwtTokenManager
{
    private static string secretKey = ConfigurationManager.AppSettings["JwtSecretKey"];

    public static string GenerateToken(UsersModel user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secretKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]{
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("UserTypeId", user.UserTypeId.ToString()),
                    new Claim(ClaimTypes.Role, user.UserTypeName),
                    new Claim("IdentityProvider", "JWT")
                }),
            Expires = DateTime.UtcNow.AddMinutes(20),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public static ClaimsPrincipal ValidateToken(string token)
    {
        try
        {
            if (string.IsNullOrEmpty(token))
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var parameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero
            };

            SecurityToken validatedToken;
            return tokenHandler.ValidateToken(token, parameters, out validatedToken);
        }
        catch (Exception ex)
        {
            Errors.LogErrorToDb(ex, 1);
            Errors.LogErrorToFile(ex);
            return null;
        }
    }

}
