using Microsoft.AspNetCore.Mvc;
using TransactionManagement.BLL.Services;
using TransactionManagement.DTO.Models;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly TransactionService _transactionService;
        private readonly GetListTransactionService _getListTransactionService;

        public TransactionsController(TransactionService transactionService, GetListTransactionService getListTransactionService)
        {
            _transactionService = transactionService;
            _getListTransactionService = getListTransactionService;
        }

        [HttpPost("upload-csv")]
        public async Task<IActionResult> UploadCsv(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File is not provided or empty");
            }

            if (Path.GetExtension(file.FileName).ToLower() != ".csv")
            {
                return BadRequest("Only files with .csv extension are allowed.");
            }

            using (var stream = file.OpenReadStream())
            {
                await _transactionService.UploadingCSVFile(stream);
            }

            return Ok("File uploaded and processed.");
        }

        [HttpPost("get-transactions")]
        public async Task<IActionResult> GetTransactions([FromBody] TransactionRequestDTO request)
        {
            try
            {
                var result = await _getListTransactionService.GetTransactionsByUserAndDateRangeAsync(
                    request.Identifier,
                    request.StartDate,
                    request.EndDate,
                    request.SelectedColumns,
                    request.ExportToExcel);

                if (request.ExportToExcel)
                {
                    return File((byte[])result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "transactions.xlsx");
                }
                else
                {
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("get-transactions-by-date")]
        public async Task<IActionResult> GetTransactionsByDate([FromBody] DateRangeRequestDTO request)
        {
            try
            {
                var result = await _getListTransactionService.GetTransactionsByDateRangeAsync(
                    request.StartDate,
                    request.EndDate,
                    request.SelectedColumns,
                    request.ExportToExcel);

                if (request.ExportToExcel)
                {
                    return File((byte[])result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "transactions.xlsx");
                }
                else
                {
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("get-transactions-for-january-2024")]
        public async Task<IActionResult> GetTransactionsForJanuary2024([FromBody] TransactionsForJanuary2024DTO request)
        {
            try
            {
                var result = await _getListTransactionService.GetTransactionsForJanuary2024Async(
                    request.Identifier,
                    request.SelectedColumns,
                    request.ExportToExcel);

                if (request.ExportToExcel)
                {
                    return File((byte[])result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "transactions.xlsx");
                }
                else
                {
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}