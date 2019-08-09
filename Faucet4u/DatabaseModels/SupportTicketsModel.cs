using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Entities;

namespace API.DatabaseModels
{
    public class SupportTickets : Entity
    {
        public Guid Reference { get; set; } = Guid.NewGuid();
        public string Subject { get; set; } = "Unknown subject";
        public string Message { get; set; } = "Unknown message";
        public bool Locked { get; set; } = false;
        public string Username { get; set; } = "Unknown";
        public string Email { get; set; } = "unknown@unknown.com";
        public bool AdminRead { get; set; } = false;
        public bool UserRead { get; set; } = false;
        public DateTime DateTime { get; set; } = DateTime.Now;
        public string LastReplier { get; set; } = "Admin";
        public SupportTicketsReplyModel[] Replies { get; set; } = new SupportTicketsReplyModel[0];

    }
}
