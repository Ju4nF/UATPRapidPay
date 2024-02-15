using UATP.RapidPay.BusinessLogic.Interfaces;
using UATP.RapidPay.Data.Models.DTOs;
using UATP.RapidPay.Data.Models;
using UATP.RapidPay.Data.Repository.IRepository;

namespace UATP.RapidPay.BusinessLogic
{
    public class PaymentsManagement(IRapidPayRepository<Card> cardRepository, IRapidPayRepository<Payment> paymentRepository) : IPaymentsManagement
    {
        private readonly IRapidPayRepository<Card> _cardRepository = cardRepository;
        private readonly IRapidPayRepository<Payment> _paymentRepository = paymentRepository;

        public async Task MakePayment(PaymentDTO paymentDTO)
        {
            var card = await _cardRepository.GetAsync(x => x.CardNumber == paymentDTO.CardNumber);
            if (card == null) 
            {
                throw new Exception("Card not found");
            }
            var ufe = UniversalFeesExchange.Instance;
            var fee = ufe.GetCurrentFee();
            var payment = new Payment()
            {
                CardId = card.Id,
                Amount = paymentDTO.Amount,
                Fee = fee
            };

            card.Balance = card.Balance + payment.Amount + fee;

            await _paymentRepository.CreateAsync(payment);
            await _cardRepository.UpdateAsync(card);
        }
    }
}
