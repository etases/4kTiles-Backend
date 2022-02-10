using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using _4kTiles_Backend.Entities;
using Microsoft.IdentityModel.Tokens;

namespace _4kTiles_Backend.Services.Auth
{
    // JWTService interface
    public interface IJwtService
    {
        // Generate a JWT token
        string GenerateAccountToken(string secureKey, int accountId, ICollection<string> roleName);

        // Validate a JWT token
        JwtSecurityToken? VerifyToken(string secretKey, string token);
    }


    public class JwtService : IJwtService
    {
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

        public JwtService()
        {
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        }

        /// <summary>
        /// Generate a JWT token
        /// </summary>
        /// <param name="secureKey">Secure key</param>
        /// <param name="accountId">Account Id from database</param>
        /// <param name="roleName"></param>
        /// <returns>token string</returns>
        public string GenerateAccountToken(string secureKey, int accountId, ICollection<string> roleName)
        {
            var claims = new List<Claim> { new Claim("accountId", accountId.ToString()) };
            claims.AddRange(roleName.Select(role => new Claim(ClaimTypes.Role, role)));

            // Generate a new security key
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secureKey));

            // Generate a new signing credentials using the security key
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            // Generate a new JWT token
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );
            // Write token
            return _jwtSecurityTokenHandler.WriteToken(token);
        }

        public JwtSecurityToken? VerifyToken(string secureKey, string token)
        {
            var key = Encoding.ASCII.GetBytes(secureKey);
            _jwtSecurityTokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            }, out var validatedToken);
            return validatedToken as JwtSecurityToken;
        }
    }
}