namespace TransactionManagement.BLL.Interfaces
{
    public interface IGetListTransaction
    {
        public Task<object> GetTransactionsByUserAndDateRangeAsync(string identifier, DateTime startDate, DateTime endDate, List<string> selectedColumns, bool exportToExcel);
        public Task<object> GetTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate, List<string> selectedColumns, bool exportToExcel);
        public Task<object> GetTransactionsForJanuary2024Async(string userIdentifier, List<string> selectedColumns, bool exportToExcel);
    }
}
