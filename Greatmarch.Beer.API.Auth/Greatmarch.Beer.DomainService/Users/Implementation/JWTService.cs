using Greatmarch.Beer.DAL.Repositories.User.Interface;
using Greatmarch.Beer.DAL.Repositories.User.Realization;
using Greatmarch.Beer.DomainService.Users.Interface;
using Greatmarch.Beer.Model.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Greatmarch.Beer.DomainService.Users.Implementation
{
    public class JWTService : IJWTService
    {
        private readonly JWTSettings _options;
        private readonly IUserRepository _userRepository;
        public JWTService(IOptions<JWTSettings> optAccess, IUserRepository userRepository)
        {
            _options = optAccess.Value;
            _userRepository = userRepository;
        }
        public string GetJWT(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim("id", user.Id.ToString()),
                new Claim("firstName", user.FirstName),
                new Claim("lastName", user.LastName),
                new Claim("email", user.Email)
            };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));

            var jwt = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromHours(1)),
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
             );
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public async Task<string> AuthAsync(string email, string password)
        {
            var users = await _userRepository.GetAllAsync();
            var user = users.Where(u => u.Email == email).ToList().FirstOrDefault();
            var sha1 = SHA1.Create();
            var shaPass = sha1.ComputeHash(Encoding.Unicode.GetBytes(password));
            if (user is not null && user.Password == Encoding.Unicode.GetString(shaPass))
            {
                return GetJWT(user);
            }
            return "401";
        }

        public async Task<string> RegistrationAsync(User user)
        {
            var usersWithoutUser = await _userRepository.GetAllAsync();
            bool metkaEmail = false;
            foreach (var item in usersWithoutUser)
            {
                if (item.Email == user.Email)
                {
                    metkaEmail = true; break;
                }
            }
            if (!metkaEmail)
            {
                _userRepository.Create(user);
                var users = await _userRepository.GetAllAsync();
                user = users.Where(u => u.Email == user.Email).ToList().FirstOrDefault();
                return GetJWT(user);
            }
            return "Email занят!";
        }

        public async Task<string> UpdateTokenAsync(int userId)
        {
            var user = await _userRepository.GetAsync(userId);
            return GetJWT(user);
        }
    }
}
