using CsvHelper.Configuration;
using TransactionManagement.DTO.Models;

public class TransactionDTOMap : ClassMap<TransactionDTO>
{
    public TransactionDTOMap()
    {
        Map(m => m.TransactionId).Name("transaction_id");
        Map(m => m.Name).Name("name");
        Map(m => m.Email).Name("email");
        Map(m => m.Amount).Name("amount");
        Map(m => m.TransactionDate).Name("transaction_date");
        Map(m => m.Location.Coordinates).Name("client_location");
    }
}