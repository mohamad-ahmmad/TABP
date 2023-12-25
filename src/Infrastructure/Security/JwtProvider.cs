using Application.Abstractions;
using Domain.Entities;
using Infrastructure.Exceptions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Security;
public class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _jwtOptions;

    public JwtProvider(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }
    public string GenerateToken(User obj)
    {
        if (_jwtOptions.Key == null ||
            _jwtOptions.Issuer == null ||
            _jwtOptions.Audience == null)
        {
            throw new InvalidJwtOptionsException("Null JWT options.");
        }

        var claims = new Claim[]
        {
            new Claim("user_role", obj.UserLevel.ToString()),
            new Claim("user_id", obj.Id.ToString())
        };
        
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key)),
            SecurityAlgorithms.HmacSha256
            );

        var token = new JwtSecurityToken(
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            claims,
            null,
            DateTime.UtcNow.AddHours(2),
            signingCredentials
            );
        
        string tokenValue = new JwtSecurityTokenHandler()
            .WriteToken(token);

        return tokenValue;
    }
}

