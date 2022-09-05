namespace CurrencyExchangeService.Models
{
    public class ConversionLogModel
    {
        public int Id { get; set; }
        public string Currency1 { get; set; }
        public string Currency2 { get; set; }
        public double Amount { get; set; }
        public double Result { get; set; }
    }
}
