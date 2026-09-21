using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace PickMeUp.Api.Account.Authentication.Session
{
    // Generates JWT access tokens and random refresh tokens.

    public sealed class TokenGeneratorService(Jwt jwt)
    {
        private readonly Jwt _jwt = jwt;

        public string GenerateAccessToken(Guid accountId)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.AccessToken.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, accountId.ToString()),
                new Claim("accountId", accountId.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.AccessToken.ExpireInMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken(Guid accountId)
        {
            byte[] bytes = RandomNumberGenerator.GetBytes(64);
            return Convert.ToHexString(bytes);
        }
    }
}
