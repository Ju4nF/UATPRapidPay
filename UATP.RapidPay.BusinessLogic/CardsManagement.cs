using AutoMapper;
using UATP.RapidPay.BusinessLogic.Interfaces;
using UATP.RapidPay.Data.Models.DTOs;
using UATP.RapidPay.Data.Models;
using UATP.RapidPay.Data.Repository.IRepository;

namespace UATP.RapidPay.BusinessLogic
{
    public class CardsManagement(IRapidPayRepository<Card> cardRepository, IMapper mapper) : ICardsManagement
    {
        private readonly IRapidPayRepository<Card> _cardRepository = cardRepository;
        private readonly IMapper _mapper = mapper;

        public async Task CreateCardAsync(CardCreateDTO cardCreateDTO)
        {
            await _cardRepository.CreateAsync(_mapper.Map<Card>(cardCreateDTO));
        }

        public async Task<CardDTO> GetCardAsync(string cardNumber)
        {
            var card = await _cardRepository.GetAsync(x => x.CardNumber == cardNumber);
            return _mapper.Map<CardDTO>(card);
        }
    }
}
