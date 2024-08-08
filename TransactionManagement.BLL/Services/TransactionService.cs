using CsvHelper;
using Microsoft.EntityFrameworkCore;
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
        private readonly ILocation _locationService;

        public TransactionService(ApiDbContext context, ILocation locationService)
        {
            _context = context;
            _locationService = locationService;
        }

        public async Task UploadingCSVFile(Stream stream)
        {
            int requestCount = 0;

            using (var reader = new StreamReader(stream))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<TransactionDTOMap>();
                var records = csv.GetRecords<TransactionDTO>().ToList();

                try
                {
                    foreach (var record in records)
                    {
                        var existingTransaction = await _context.Transactions
                            .Include(t => t.Location)
                            .FirstOrDefaultAsync(t => t.TransactionId == record.TransactionId);

                        Location location = await _context.Locations
                            .FirstOrDefaultAsync(l => l.Coordinates == record.Location.Coordinates);

                        if (location == null || location.UTC == "Not found time")
                        {
                            var locationInfo = await _locationService.GetLocationInfoAsync(record.Location.Coordinates);
                            requestCount++;

                            if (locationInfo != null)
                            {
                                if (location == null)
                                {
                                    location = new Location
                                    {
                                        IANAZone = locationInfo.IANAZone,
                                        UTC = locationInfo.UTC,
                                        Coordinates = locationInfo.Coordinates
                                    };
                                    _context.Locations.Add(location);
                                }
                                else
                                {
                                    location.IANAZone = locationInfo.IANAZone;
                                    location.UTC = locationInfo.UTC;
                                    location.Coordinates = locationInfo.Coordinates;
                                }

                                await _context.SaveChangesAsync();
                            }
                        }

                        if (existingTransaction != null)
                        {
                            existingTransaction.Name = record.Name;
                            existingTransaction.Email = record.Email;
                            existingTransaction.Amount = float.Parse(record.Amount.TrimStart('$'), CultureInfo.InvariantCulture);
                            existingTransaction.TransactionDate = DateTime.Parse(record.TransactionDate, CultureInfo.InvariantCulture);
                            existingTransaction.LocationId = location.Id;

                            _context.Transactions.Update(existingTransaction);
                        }
                        else
                        {
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
                        }

                        await _context.SaveChangesAsync();
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