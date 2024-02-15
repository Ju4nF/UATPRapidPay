namespace UATP.RapidPay.Data.Models.DTOs
{
    public class LoginResponseDTO
    {
        public LocalUser User { get; set; }
        public string Token { get; set; }
    }
}
