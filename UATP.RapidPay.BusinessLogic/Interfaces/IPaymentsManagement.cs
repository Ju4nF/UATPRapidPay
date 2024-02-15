using UATP.RapidPay.Data.Models.DTOs;

namespace UATP.RapidPay.BusinessLogic.Interfaces
{
    public interface IPaymentsManagement
    {
        Task MakePayment(PaymentDTO paymentDTO);
    }
}
