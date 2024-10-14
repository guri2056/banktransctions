using System.ComponentModel.DataAnnotations;

namespace Banktransactions.Models
{
    public class Branch
    {
        public int Id { get; set; }
        [Required]
        public string name { get; set; }
        
        public decimal creditlimit { get; set; }
        public decimal debitlimit { get; set; }
        public ICollection<Transaction> Transactions { get; set; }

    }
}
