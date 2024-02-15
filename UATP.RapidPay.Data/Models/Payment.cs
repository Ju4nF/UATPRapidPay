using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UATP.RapidPay.Data.Models
{
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Card")]
        public int CardId { get; set; }
        public Card Card { get; set; }
        public decimal Amount { get; set; }
        public decimal Fee { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
