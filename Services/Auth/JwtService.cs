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
        string GenerateAccountToken(string secureKey, int accountId, string roleName);

        // Validate a JWT token
        JwtSecurityToken VerifyToken(string secretKey, string token);
    }


    public class JwtService : IJwtService
    {
        private JwtSecurityTokenHandler _jwtSecurityTokenHandler;

        /// <summary>
        /// Generate a JWT token
        /// </summary>
        /// <param name="secureKey">Secure key</param>
        /// <param name="accountId">Account Id from database</param>
        /// <returns>token string</returns>
        public string GenerateAccountToken(string secureKey, int accountId, string roleName)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, roleName),
                new Claim("accountId", accountId.ToString()),
            };

            // Generate a new security key
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secureKey));

            // Generate a new signing credentials using the security key
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            // Generate a new JWT token
            JwtSecurityToken token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            // Generate a new JwtSecurityTokenHandler
            return _jwtSecurityTokenHandler.WriteToken(token);
        }

        public JwtSecurityToken VerifyToken(string secureKey, string token)
        {
            Byte[] key = Encoding.ASCII.GetBytes(secureKey);
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            _jwtSecurityTokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            }, out SecurityToken validatedToken);

            return validatedToken as JwtSecurityToken;
        }
    }
}