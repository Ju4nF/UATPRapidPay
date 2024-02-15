using UATP.RapidPay.Data.Models.DTOs;

namespace UATP.RapidPay.BusinessLogic.Interfaces
{
    public interface ICardsManagement
    {
        Task<CardDTO> GetCardAsync(string cardNumber);
        Task CreateCardAsync(CardCreateDTO cardCreateDTO);
    }
}
