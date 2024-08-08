using ClosedXML.Excel;
using TransactionManagement.BLL.Interfaces;
using TransactionManagement.DAL.Entities;

namespace TransactionManagement.BLL.Services
{
    public class ExportToExcelService: IExportToExcel
    {
        public async Task<byte[]> ExportTransactionsToExcelAsync(List<Transactions> transactions, List<string> selectedColumns)
        {
            return await Task.Run(() =>
            {
                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Transactions");

                int colIndex = 1;
                if (selectedColumns.Contains("TransactionId"))
                    worksheet.Cell(1, colIndex++).Value = "TransactionId";
                if (selectedColumns.Contains("Name"))
                    worksheet.Cell(1, colIndex++).Value = "Name";
                if (selectedColumns.Contains("Email"))
                    worksheet.Cell(1, colIndex++).Value = "Email";
                if (selectedColumns.Contains("Amount"))
                    worksheet.Cell(1, colIndex++).Value = "Amount";
                if (selectedColumns.Contains("TransactionDate"))
                    worksheet.Cell(1, colIndex++).Value = "TransactionDate";
                if (selectedColumns.Contains("IANAZone"))
                    worksheet.Cell(1, colIndex++).Value = "IANAZone";
                if (selectedColumns.Contains("UTC"))
                    worksheet.Cell(1, colIndex++).Value = "UTC";
                if (selectedColumns.Contains("Coordinates"))
                    worksheet.Cell(1, colIndex++).Value = "Coordinates";

                for (int i = 0; i < transactions.Count; i++)
                {
                    colIndex = 1;
                    var transaction = transactions[i];
                    if (selectedColumns.Contains("TransactionId"))
                        worksheet.Cell(i + 2, colIndex++).Value = transaction.TransactionId;
                    if (selectedColumns.Contains("Name"))
                        worksheet.Cell(i + 2, colIndex++).Value = transaction.Name;
                    if (selectedColumns.Contains("Email"))
                        worksheet.Cell(i + 2, colIndex++).Value = transaction.Email;
                    if (selectedColumns.Contains("Amount"))
                        worksheet.Cell(i + 2, colIndex++).Value = transaction.Amount;
                    if (selectedColumns.Contains("TransactionDate"))
                        worksheet.Cell(i + 2, colIndex++).Value = transaction.TransactionDate;
                    if (selectedColumns.Contains("IANAZone"))
                        worksheet.Cell(i + 2, colIndex++).Value = transaction.Location.IANAZone;
                    if (selectedColumns.Contains("UTC"))
                        worksheet.Cell(i + 2, colIndex++).Value = transaction.Location.UTC;
                    if (selectedColumns.Contains("Coordinates"))
                        worksheet.Cell(i + 2, colIndex++).Value = transaction.Location.Coordinates;
                }

                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                return stream.ToArray();
            });
        }
    }
}