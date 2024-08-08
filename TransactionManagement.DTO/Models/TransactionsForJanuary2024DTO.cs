using System.ComponentModel.DataAnnotations;

namespace TransactionManagement.DTO.Models
{
    public class TransactionsForJanuary2024DTO
    {
        [Required]
        public string Identifier { get; set; }
        public List<string>? SelectedColumns { get; set; }
        [Required]
        public bool ExportToExcel { get; set; }
    }
}