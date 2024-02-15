using System.ComponentModel.DataAnnotations;

namespace UATP.RapidPay.Data.Models.DTOs
{
    public class CardCreateDTO
    {
        [Required]
        [MinLength(15)]
        [MaxLength(15)]
        public string CardNumber { get; set; }
    }
}
