using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using GameWeb.Models;
using GameWeb.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace GameWeb.Services
{
    public class AuthService : IAuthService
    {
        private readonly JWTConfig _jwtConfig;
        private readonly IUsersRepository _usersRepository;
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

        public RefreshToken GenerateRefreshToken(User user)
        {
            RefreshToken refreshToken = new RefreshToken()
            {
                Token = GenerateRefreshTokenString(),
                Expiration = DateTime.Now.AddMinutes(30)
            };
            user.RefreshTokens.Add(refreshToken);
            _usersRepository.UpdateUser(user);
            return refreshToken;
        }
        private static string GenerateRefreshTokenString()
        {
            var randomNumber = new byte[32];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public RefreshTokenDTO ExchangeRefreshToken(RefreshTokenDTO message)
        {
            var user = _usersRepository.GetUserByRefreshToken(message.RefreshToken);
            if (user.RefreshTokens.Any(rt => rt.Token == message.RefreshToken && rt.Expiration <= DateTime.Now))
            {
                var jwtToken = GenerateToken(user);
                var generatedRefreshToken = GenerateRefreshTokenString();
                user.RefreshTokens.Remove(user.RefreshTokens.First(t => t.Token == message.RefreshToken));
                RefreshToken refresh = new RefreshToken();
                refresh.Token = generatedRefreshToken;
                refresh.Expiration = DateTime.Now;
                refresh.UserId = user.UserId;
                user.RefreshTokens.Add(refresh);
                _usersRepository.UpdateUser(user);
                RefreshTokenDTO refreshTokenDTO = new RefreshTokenDTO();
                refreshTokenDTO.Token = jwtToken;
                refreshTokenDTO.RefreshToken = generatedRefreshToken;
                return refreshTokenDTO;
            }
            else{
                throw new SystemException("Invalid token");
            }
        }

        public void RemoveRefreshTokenById(long id)
        {
            throw new System.NotImplementedException("Not implemented");
        }
    }
}