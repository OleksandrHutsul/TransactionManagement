﻿namespace TransactionManagement.DAL.Entities
{
    public class Location
    {
        public int Id { get; set; }
        public required string City { get; set; }
        public required string UTC { get; set; }
        public required string Coordinates { get; set; }
    }
}