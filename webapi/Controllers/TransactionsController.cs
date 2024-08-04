using Microsoft.AspNetCore.Mvc;
using TransactionManagement.BLL.Services;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly TransactionService _transactionService;

        public TransactionsController(TransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost("upload-csv")]
        public async Task<IActionResult> UploadCsv(IFormFile file)
        {
            if(file == null || file.Length == 0) 
            {
                return BadRequest("File is not provided or empty");
            }

            if(Path.GetExtension(file.FileName).ToLower() != ".csv")
            {
                return BadRequest("Only files with .csv extension are allowed.");
            }

            using(var stream = file.OpenReadStream())
            {
                await _transactionService.UploadingCSVFile(stream);
            }

            return Ok("File uploaded and processed.");
        }
    }
}