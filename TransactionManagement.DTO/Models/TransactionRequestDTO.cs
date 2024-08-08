using System.ComponentModel.DataAnnotations;

namespace TransactionManagement.DTO.Models
{
    public class TransactionRequestDTO
    {
        [Required]
        public string Identifier { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public List<string>? SelectedColumns { get; set; }
        [Required]
        public bool ExportToExcel { get; set; }
    }
}