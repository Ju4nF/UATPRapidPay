using UATP.RapidPay.Data.Models.DTOs;

namespace UATP.RapidPay.BusinessLogic.Interfaces
{
    public interface IUsersManagement
    {
        Task<bool> IsUniqueUserAsync(string userName);
        Task<LoginResponseDTO> LoginAsync(LoginRequestDTO loginRequestDTO);
        Task RegisterAsync(RegistrationRequestDTO registrationRequestDTO);
    }
}
