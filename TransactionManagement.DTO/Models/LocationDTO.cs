using System.ComponentModel.DataAnnotations;

namespace TransactionManagement.DTO.Models
{
    public class LocationDTO
    {
        public int Id { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string UTC { get; set; }
        [Required]
        public string Coordinates { get; set; }
    }
}