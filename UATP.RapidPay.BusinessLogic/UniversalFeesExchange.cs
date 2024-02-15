namespace UATP.RapidPay.BusinessLogic
{
    public class UniversalFeesExchange
    {
        private static UniversalFeesExchange _instance;
        private static readonly object _lock = new object();
        private decimal _lastFee;
        private DateTime _lastUpdate;

        private UniversalFeesExchange()
        {
            _lastFee = 2;
            _lastUpdate = DateTime.UtcNow;
        }

        public static UniversalFeesExchange Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new UniversalFeesExchange();
                        }
                    }
                }
                return _instance;
            }
        }

        public decimal GetCurrentFee()
        {
            if ((DateTime.UtcNow - _lastUpdate).TotalHours >= 1)
            {
                CalculateFee();
            }

            return _lastFee;
        }

        private void CalculateFee()
        {
            Random random = new Random();
            decimal randomDecimal = (decimal)(random.NextDouble() * 2);

            _lastFee *= randomDecimal;

            _lastUpdate = DateTime.UtcNow;
        }
    }
}
