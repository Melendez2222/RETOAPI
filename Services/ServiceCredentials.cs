using Microsoft.IdentityModel.Tokens;
using RETOAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace RETOAPI.Services
{

    public class ServiceCredentials
    {
        public class TokenResult
        {
            public string Token { get; set; }
            public DateTime Expires { get; set; }
        }
        private readonly IConfiguration _configuration;
        public ServiceCredentials(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string HashString(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                var builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
        public TokenResult GetToken(string username, int Rolid)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, Rolid.ToString())
            };

            var keyString = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(keyString))
            {
                throw new Exception("JWT key is not configured properly.");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])),
                signingCredentials: creds);

            //return new JwtSecurityTokenHandler().WriteToken(token);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            var expires = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"]));

            return new TokenResult
            {
                Token = tokenString,
                Expires = expires,
            };
        }
    }

}
