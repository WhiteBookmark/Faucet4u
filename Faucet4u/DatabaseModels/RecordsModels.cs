using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Entities;

namespace API.DatabaseModels
{
    public class Records : Entity
    {
        //Add things here that change frequently
        public Guid RecordsID { get; set; } = Guid.NewGuid();
        public ChatHistoryModel[] ChatHistory { get; set; } = new ChatHistoryModel[0];
        public PTPSourceModel[] PTPSource { get; set; } = new PTPSourceModel[0];
        public PTPAbsoluteSourceModel[] PTPAbsoluteSource { get; set; } = new PTPAbsoluteSourceModel[0];
        public PTPIP[] PTPIP { get; set; } = new PTPIP[0];
        public LinkShortnersRecords[] LinkShortnersRecord { get; set; } = new LinkShortnersRecords[0];

        //Counters for rotators/networks
        //Counter Ids must be defaulted to 1 and not 0

        public int PTPCounter { get; set; } = 1;
        public int BannerRotatorCounter { get; set; } = 1;
        public int SquareBannerRotatorCounter { get; set; } = 1;
        public int StandardBannerNetworkCounter { get; set; } = 1;
        public int SquareBannerNetworkCounter { get; set; } = 1;
        public int SkyscraperBannerNetworkCounter { get; set; } = 1;
        public int PTPStandardBannerNetworkCounter { get; set; } = 1;
        public int PTPSquareBannerNetworkCounter { get; set; } = 1;
    }
}
