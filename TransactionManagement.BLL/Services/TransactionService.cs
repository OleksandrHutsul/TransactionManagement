﻿using CsvHelper;
using System.Globalization;
using TransactionManagement.BLL.Interfaces;
using TransactionManagement.DAL.Context;
using TransactionManagement.DAL.Entities;
using TransactionManagement.DTO.Models;

namespace TransactionManagement.BLL.Services
{
    public class TransactionService: ITransaction
    {
        private readonly ApiDbContext _context;
        private readonly LocationService _locationService;

        public TransactionService(ApiDbContext context, LocationService locationService)
        {
            _context = context;
            _locationService = locationService;
        }

        public async Task DownloadingCSVFile(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<TransactionDTOMap>();
                var records = csv.GetRecords<TransactionDTO>().ToList();
                foreach (var record in records)
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

                        var transaction = new Transactions
                        {
                            TransactionId = record.TransactionId,
                            Name = record.Name,
                            Email = record.Email,
                            Amount = float.Parse(record.Amount.TrimStart('$'), CultureInfo.InvariantCulture),
                            TransactionDate = DateTime.Parse(record.TransactionDate, CultureInfo.InvariantCulture),
                            LocationId = location.Id
                        };

                        _context.Transactions.Add(transaction);
                    }
                }

                await _context.SaveChangesAsync();
            }
        }
    }
}