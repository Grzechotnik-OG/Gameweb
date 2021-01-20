using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using GameWeb.Models;
using GameWeb.Models.DTO;
using GameWeb.Models.Entities;
using GameWeb.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace GameWeb.Services
{
    public class AuthService : IAuthService
    {
        private readonly JWTConfig _jwtConfig;
        private readonly IUsersRepository _usersRepository;
        private readonly Context _context;
        public AuthService(JWTConfig jwtConfig, IUsersRepository usersRepository, Context context)
        {
            _jwtConfig = jwtConfig;
            _usersRepository = usersRepository;
            _context = context;
        }
        public TokenDTO GenerateTokenDTO(User user)
        {
            TokenDTO tokens = new TokenDTO()
            {
                Token = GenerateToken(user),
                RefreshToken = GenerateRefreshTokenString(user)
            };
            return tokens;
        }

        public string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]{
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserId.ToString()),
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

        private string GenerateRefreshTokenString(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]{
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserId.ToString()),
                new Claim("role", user.Role),
                new Claim("refreshToken","RefreshToken"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
            var refreshToken = new JwtSecurityToken(
                issuer: _jwtConfig.Issuer,
                audience: _jwtConfig.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtConfig.RefreshTokenExpiration),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(refreshToken);
        }

        public string ExchangeToken(long userId, string role){
            User user = new User(){
                UserId = userId,
                Role = role
            };
            string newToken = GenerateToken(user);
            return newToken;
        }
    }
}