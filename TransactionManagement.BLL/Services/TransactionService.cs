using CsvHelper;
using System.Globalization;
using TransactionManagement.BLL.Interfaces;
using TransactionManagement.DAL.Context;
using TransactionManagement.DAL.Entities;
using TransactionManagement.DTO.Models;

namespace TransactionManagement.BLL.Services
{
    public class TransactionService : ITransaction
    {
        private readonly ApiDbContext _context;
        private readonly LocationService _locationService;

        public TransactionService(ApiDbContext context, LocationService locationService)
        {
            _context = context;
            _locationService = locationService;
        }

        public async Task UploadingCSVFile(Stream stream)
        {
            const int batchSize = 50;
            using (var reader = new StreamReader(stream))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<TransactionDTOMap>();
                var records = csv.GetRecords<TransactionDTO>().ToList();

                for (int i = 0; i < records.Count; i += batchSize)
                {
                    var batch = records.Skip(i).Take(batchSize).ToList();

                    try
                    {
                        foreach (var record in batch)
                        {
                            var locationInfo = await _locationService.GetLocationInfoAsync(record.Location.Coordinates);
                            if (locationInfo != null)
                            {
                                var location = new Location
                                {
                                    City = locationInfo.City,
                                    UTC = locationInfo.UTC,
                                    Coordinates = locationInfo.Coordinates
                                };

                                _context.Locations.Add(location);
                                await _context.SaveChangesAsync();

                                var transactionEntity = new Transactions
                                {
                                    TransactionId = record.TransactionId,
                                    Name = record.Name,
                                    Email = record.Email,
                                    Amount = float.Parse(record.Amount.TrimStart('$'), CultureInfo.InvariantCulture),
                                    TransactionDate = DateTime.Parse(record.TransactionDate, CultureInfo.InvariantCulture),
                                    LocationId = location.Id
                                };

                                _context.Transactions.Add(transactionEntity);
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message, ex);
                    }
                }
            }
        }
    }
}