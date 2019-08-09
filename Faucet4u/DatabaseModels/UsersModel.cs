using MongoDB.Entities;
using System;

namespace API.DatabaseModels
{
    public class Users : Entity
    {
        public Guid UserID { get; set; } = Guid.NewGuid();
        public string Username { get; set; } = "Unknown";
        public string Email { get; set; } = "unknown@unknown.com";
        public string Password { get; set; } = "unknown123";
        public bool IsConfirmed { get; set; } = false;
        public Guid ConfirmationCode { get; set; } = Guid.NewGuid();
        public DateTime ConfirmationCodeExpiryTime { get; set; } = DateTime.Now;
        public string IP { get; set; } = "0.0.0.0";
        public string Country { get; set; } = "Unknown";
        public Guid SessionId { get; set; } = Guid.NewGuid();
        public DateTime SessionExpiry { get; set; } = DateTime.Now;
        public string FaucetHubBitcoinAddress { get; set; } = null;
        public bool Locked { get; set; } = false;
        public int CheatCounter { get; set; } = 0;
        public string Referrer { get; set; } = null;
        public double Balance { get; set; } = 0;
        public double OfferwallBalance { get; set; } = 0;
        public double PurchaseBalance { get; set; } = 0;
        public double LinkClaimMarker { get; set; } = 0;
        public string LinkClaimMarkerLink { get; set; } = null;
        public DateTime LinkClaimInterval { get; set; } = DateTime.Now;
        public string LinkClaimMarkerIP { get; set; } = null;
        public Guid LinkClaimMarkerId { get; set; } = Guid.NewGuid();
        public DateTime LinkClaimFalseDateTime { get; set; } = DateTime.Now;
        public double TotalWithdrawn { get; set; } = 0;
        public double TotalOfferwallWithdrawn { get; set; } = 0;
        public double WithdrawalAmount { get; set; } = 0;
        public double OfferwallWithdrawalAmount { get; set; } = 0;
        public double LinkShortnerEarning { get; set; } = 0;
        public double BonusAdEarning { get; set; } = 0;
        public double OfferwallEarning { get; set; } = 0;
        public double PTPUniqueEarning { get; set; } = 0;
        public double PTPNonUnique1Earning { get; set; } = 0;
        public double PTPNonUnique2Earning { get; set; } = 0;
        public double PTPNonUnique3Earning { get; set; } = 0;
        public DateTime LastLogin { get; set; } = DateTime.Now;
        public DateTime LastClaim { get; set; } = DateTime.Now;
        public DateTime DateRegistered { get; set; } = DateTime.Now;
        public bool JustClaimed { get; set; } = false;
        public bool ChatBanned { get; set; } = false;
        public int PTPCredit { get; set; } = 0;
        public int PTPDayCredit { get; set; } = 0;
        public int BannerCredit { get; set; } = 0;
        public int BannerDayCredit { get; set; } = 0;
        public int BonusAdCredit { get; set; } = 0;
        public int BonusAdDayCredit { get; set; } = 0;
        public Guid BonusAdConfirmationCode { get; set; } = Guid.NewGuid();
        public string BonusAdMarkerIP { get; set; } = null;
        public Guid BonusAdMarkerId { get; set; } = Guid.NewGuid();
        public Guid BonusAdLinkMarkerId { get; set; } = Guid.NewGuid();
        public DateTime BonusAdLinkFalseDateTime { get; set; } = DateTime.Now;
        public double BonusAdMarkerAmount { get; set; } = 0;
        public string SocketSession { get; set; } = null;

        public CheatHistoryModel[] CheatHistory { get; set; } = new CheatHistoryModel[0];
        public LoginHistoryModel[] LoginHistory { get; set; } = new LoginHistoryModel[0];
        public DepositHistoryModel[] DepositHistory { get; set; } = new DepositHistoryModel[0];
        public WithdrawalHistoryModel[] WithdrawalHistory { get; set; } = new WithdrawalHistoryModel[0];
        public LinkShortnersHistoryModel[] LinkShortnersHistory { get; set; } = new LinkShortnersHistoryModel[0];
        public BonusAdsHistoryModel[] BonusAdsHistory { get; set; } = new BonusAdsHistoryModel[0];
    }
}