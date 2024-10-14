
using System.ComponentModel.DataAnnotations;

namespace Banktransactions.Models
{ 
    public class Transaction
{
        public int Id { get; set; }
        public int BranchId { get; set; }
        public Branch Branch { get; set; }
        public decimal Amount  { get; set; }
        public bool IsCredit { get; set; }
        public DateTime  TransactionDate { get; set; }

    }
}
