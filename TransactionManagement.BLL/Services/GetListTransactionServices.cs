using TransactionManagement.DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace TransactionManagement.BLL.Services
{
    public class GetListTransactionService
    {
        private readonly ApiDbContext _context;
        private readonly ExportToExcelService _exportToExcel;

        public GetListTransactionService(ApiDbContext context, ExportToExcelService exportToExcel)
        {
            _context = context;
            _exportToExcel = exportToExcel;
        }

        public async Task<object> GetTransactionsByUserAndDateRangeAsync(string identifier, DateTime startDate, DateTime endDate, List<string> selectedColumns, bool exportToExcel)
        {
            var userTransaction = await _context.Transactions
                .Include(x => x.Location)
                .FirstOrDefaultAsync(t => t.TransactionId == identifier || t.Name == identifier || t.Email == identifier);

            if (userTransaction == null)
            {
                throw new Exception("User not found");
            }

            var userTimeZone = userTransaction.Location.UTC;

            var transactions = await _context.Transactions
                .Include(x => x.Location)
                .Where(t => t.Location.UTC == userTimeZone &&
                            t.TransactionDate >= startDate &&
                            t.TransactionDate <= endDate)
                .ToListAsync();

            if (exportToExcel)
            {
                var excelData = await _exportToExcel.ExportTransactionsToExcelAsync(transactions, selectedColumns);
                return excelData;
            }
            else
            {
                return transactions;
            }
        }

        public async Task<object> GetTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate, List<string> selectedColumns, bool exportToExcel)
        {
            var transactions = await _context.Transactions
                .Include(x => x.Location)
                .Where(t => t.TransactionDate >= startDate &&
                            t.TransactionDate <= endDate)
                .ToListAsync();

            if (exportToExcel)
            {
                var excelData = await _exportToExcel.ExportTransactionsToExcelAsync(transactions, selectedColumns);
                return excelData;
            }
            else
            {
                return transactions;
            }
        }

        public async Task<object> GetTransactionsForJanuary2024Async(string userIdentifier, List<string> selectedColumns, bool exportToExcel)
        {
            var userTransaction = await _context.Transactions
                .Include(x => x.Location)
                .FirstOrDefaultAsync(t => t.TransactionId == userIdentifier || t.Name == userIdentifier || t.Email == userIdentifier);

            if (userTransaction == null)
            {
                throw new Exception("User not found");
            }

            var userUtcOffset = TimeSpan.Parse(userTransaction.Location.UTC);

            var startDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc).Add(-userUtcOffset);
            var endDate = new DateTime(2024, 1, 31, 23, 59, 59, DateTimeKind.Utc).Add(-userUtcOffset);

            var transactions = await _context.Transactions
                .Include(x => x.Location)
                .Where(t =>
                    t.TransactionDate >= startDate &&
                    t.TransactionDate <= endDate)
                .ToListAsync();

            if (exportToExcel)
            {
                var excelData = await _exportToExcel.ExportTransactionsToExcelAsync(transactions, selectedColumns);
                return excelData;
            }
            else
            {
                return transactions;
            }
        }
    }
}
