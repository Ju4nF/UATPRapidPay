using AutoMapper;
using UATP.RapidPay.BusinessLogic.Interfaces;
using UATP.RapidPay.Data.Models.DTOs;
using UATP.RapidPay.Data.Models;
using UATP.RapidPay.Data.Repository.IRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace UATP.RapidPay.BusinessLogic
{
    public class UsersManagement(IRapidPayRepository<LocalUser> localUserRepository, IMapper mapper, IConfiguration configuration) : IUsersManagement
    {
        private readonly IRapidPayRepository<LocalUser> _localUserRepository = localUserRepository;
        private readonly IMapper _mapper = mapper;
        private string secretKey = configuration.GetValue<string>("ApiSettings:Secret");

        public async Task<bool> IsUniqueUserAsync(string userName)
        {
            var user = await _localUserRepository.GetAsync(x => x.UserName.ToLower() == userName.ToLower());
            return user != null;
        }

        public async Task<LoginResponseDTO> LoginAsync(LoginRequestDTO loginRequestDTO)
        {
            var user = await _localUserRepository.GetAsync(x => x.UserName.ToLower() == loginRequestDTO.UserName.ToLower() && x.Password == loginRequestDTO.Password);

            if (user == null)
            {
                return new LoginResponseDTO()
                {
                    User = null,
                    Token = ""
                };
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var loginResponseDTO = new LoginResponseDTO()
            {
                User = user,
                Token = tokenHandler.WriteToken(token)
            };
            return loginResponseDTO;
        }

        public async Task RegisterAsync(RegistrationRequestDTO registrationRequestDTO)
        {
            await _localUserRepository.CreateAsync(_mapper.Map<LocalUser>(registrationRequestDTO));
        }
    }
}
