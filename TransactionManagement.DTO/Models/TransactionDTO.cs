using System.ComponentModel.DataAnnotations;

namespace TransactionManagement.DTO.Models
{
    public class TransactionDTO
    {
        [Required]
        public string TransactionId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Amount { get; set; }
        [Required]
        public string TransactionDate { get; set; }
        [Required]
        public LocationDTO Location { get; set; }
    }
}