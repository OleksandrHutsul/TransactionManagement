using System.ComponentModel.DataAnnotations;

namespace TransactionManagement.DAL.Entities
{
    public class Transactions
    {
        public int Id { get; set; }
        public required string TransactionId { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required float Amount { get; set; }
        [DataType(DataType.Date)]
        public required DateTime TransactionDate { get; set; }
        public required int LocationId { get; set; }
        public virtual Location Location { get; set;}
    }
}