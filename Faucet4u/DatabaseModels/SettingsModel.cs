using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Entities;
using System;

namespace API.DatabaseModels
{

    public class Settings : Entity
    {
        //Most of the stuff here has constant values, avoid adding things here whose values change frequently
        //Settings model should be used for GET queries 90% of the time

        public Guid SettingsID { get; set; } = Guid.NewGuid();
        public double BonusAdCredit { get; set; } = 0.00000010;
        public int CheatCounterForDirectClaims { get; set; } = 1;
        public int CheatCounterForDuplicateAccount { get; set; } = 1;
        public int CheatCounterForMultipleAccounts { get; set; } = 5;
        public int CheatCounterForProxy { get; set; } = 5;
        public double DirectUniqueIPCredit { get; set; } = 0.000000015;
        public double DirectNonUniqueIP1Credit { get; set; } = 0.00000001;
        public double DirectNonUniqueIP2Credit { get; set; } = 0.000000005;
        public double DirectNonUniqueIP3Credit { get; set; } = 0.000000005;
        public string ForumLink { get; set; } = "https://www.emoneyspace.com/forum/index.php/topic,467473.0.html";
        public int HitsFor1BonusAdDayCredit { get; set; } = 10000;
        public int HitsFor1PTPDayCredit { get; set; } = 10000;
        public int ImpressionsFor1BannerDayCredit { get; set; } = 10000;
        public int ImpressionsFor1SquareBannerDayCredit { get; set; } = 10000;
        public int Level1 { get; set; } = 25;
        public int Level2 { get; set; } = 15;
        public int Level3 { get; set; } = 10;
        public double LinkClaimCredit { get; set; } = 0.00000015;
        public int LinkClaimMinutes { get; set; } = 3;
        public double MinimumDeposit { get; set; } = 0.00010000;
        public double MinimumExchangeWithdrawal { get; set; } = 0.00150000;
        public double MinimumOfferwallWithdrawal { get; set; } = 0.00030000;
        public double MinimumWithdrawal { get; set; } = 0.00020000;
        public int OfferwallLevel1 { get; set; } = 12;
        public int OfferwallLevel2 { get; set; } = 5;
        public int OfferwallLevel3 { get; set; } = 3;
        public int PTPDirectRatio { get; set; } = 0;
        public int PTPLevel1 { get; set; } = 12;
        public int PTPLevel2 { get; set; } = 5;
        public int PTPLevel3 { get; set; } = 3;
        public double PTPUniqueIPCredit { get; set; } = 0.000000015;
        public double PTPNonUniqueIP1Credit { get; set; } = 0.00000001;
        public double PTPNonUniqueIP2Credit { get; set; } = 0.00000001;
        public double PTPNonUniqueIP3Credit { get; set; } = 0.000000005;
        public int PTPTotalRatio { get; set; } = 5;

        //Other stuff that don't need a new collection class as they're mostly constant

        public AdvertisingPackagesModel[] AdvertisingPackages { get; set; } = new AdvertisingPackagesModel[0];
        public BonusAdsModel[] BonusAds { get; set; } = new BonusAdsModel[0];
        public LinkShortnersModel[] LinkShortners { get; set; } = new LinkShortnersModel[0];

        //Rotators
        public PTPModel[] PTP { get; set; } = new PTPModel[0];
        public BannerRotatorModel[] BannerRotator { get; set; } = new BannerRotatorModel[0];

        //Networks
        public BannerNetworkModel[] BannerNetwork { get; set; } = new BannerNetworkModel[0];

    }
}