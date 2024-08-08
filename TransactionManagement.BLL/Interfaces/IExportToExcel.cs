using TransactionManagement.DAL.Entities;

namespace TransactionManagement.BLL.Interfaces
{
    public interface IExportToExcel
    {
        public Task<byte[]> ExportTransactionsToExcelAsync(List<Transactions> transactions, List<string> selectedColumns);
    }
}