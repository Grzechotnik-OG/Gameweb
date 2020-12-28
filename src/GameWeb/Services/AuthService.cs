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
        public TokenDTO GenerateToken(User user)
        {
            TokenDTO tokens = new TokenDTO()
            {
                Token = GenerateToken(user.UserId,_jwtConfig.AccessTokenExpiration)
            };
            return tokens;
        }

        public string GenerateToken(long userId, int expireMinutes = 20)
        {

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]{
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                //new Claim(“fullName”, userInfo.FullName.ToString()),
                //new Claim(“role”,userInfo.UserRole), //To mozna wykorzystac później
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
            var token = new JwtSecurityToken(
                issuer: _jwtConfig.Issuer,
                audience: _jwtConfig.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(1),
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