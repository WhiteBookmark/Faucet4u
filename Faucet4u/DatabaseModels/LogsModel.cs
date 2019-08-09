using MongoDB.Entities;
using System;

namespace API.DatabaseModels
{
    public class Logs : Entity
    {
        public Guid LogID { get; set; } = Guid.NewGuid();
        public DateTime DateTime { get; set; } = DateTime.Now;
        public string IP { get; set; } = "0.0.0.0";
        public string Username { get; set; } = "Unknown";
        public string Type { get; set; } = "Unknown";
        public string Message { get; set; } = "Message not specified";
        public string Exception { get; set; } = "Exception not specified";
    }
}