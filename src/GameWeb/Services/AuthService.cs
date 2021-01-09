using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using GameWeb.Models;
using Microsoft.IdentityModel.Tokens;

namespace GameWeb.Services
{
    public class AuthService : IAuthService
    {
        private readonly JWTConfig _jwtConfig;
        public AuthService(JWTConfig jwtConfig)
        {
            _jwtConfig = jwtConfig;
        }
        public TokenDTO GenerateTokenDTO(User user)
        {
            TokenDTO tokens = new TokenDTO()
            {
                Token = GenerateToken(user)
            };
            return tokens;
        }

        public string GenerateToken(User user)
        {

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]{
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new Claim("role", user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
            var token = new JwtSecurityToken(
                issuer: _jwtConfig.Issuer,
                audience: _jwtConfig.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtConfig.AccessTokenExpiration),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static string GenerateRefreshTokenString()
        {
            var randomNumber = new byte[32];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public void RemoveRefreshTokenById(long id)
        {
            throw new System.NotImplementedException("Not implemented");
        }
    }
}