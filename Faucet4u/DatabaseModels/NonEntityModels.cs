using System;
using System.Numerics;

namespace API.DatabaseModels
{
    public class AdvertisingPackagesModel
    {
        public Guid Reference { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = "Unknown";
        public bool IsTimeBased { get; set; } = false;
        public int Credit { get; set; } = 0;
        public double Price { get; set; } = 0;
    }
    public class CheatHistoryModel
    {
        public Guid Reference { get; set; } = Guid.NewGuid();
        public DateTime DateTime { get; set; } = DateTime.Now;
        public int CheatLevel { get; set; } = 1;
        public string Reason { get; set; } = "Unknown";
    }
    public class LoginHistoryModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime DateTime { get; set; } = DateTime.Now;
        public bool Success { get; set; } = false;
    }
    public class DepositHistoryModel
    {
        public Guid Reference { get; set; } = Guid.NewGuid();
        public string Username { get; set; } = "Unknown";
        public DateTime DepositDate { get; set; } = DateTime.Now;
        public double Amount { get; set; } = 0;
        public string WalletType { get; set; } = "Unknown";
        public string WalletAddress { get; set; } = "Unknown";
        public string Remarks { get; set; } = "None";
        public int Confirmations { get; set; } = 0;

        //Skipped adding TxID and InvoiceId, perhaps they are not need
    }
    public class WithdrawalHistoryModel
    {
        public Guid Reference { get; set; } = Guid.NewGuid();
        public DateTime RequestedDate { get; set; } = DateTime.Now;
        public string WithdrawalType { get; set; } = "Standard";
        public double RequestedAmount { get; set; } = 0;
        public DateTime PaymentDate { get; set; } = DateTime.Now;
        public double PaymentAmount { get; set; } = 0;
        public bool Paid { get; set; } = false;
        public string WalletType { get; set; } = "Not specified";
        public string WalletAddress { get; set; } = "Not specified";
        public string Remarks { get; set; } = "None";
    }
    public class OfferwallHistoryModel
    {
        public Guid Reference { get; set; } = Guid.NewGuid();
        public DateTime DateTime { get; set; } = DateTime.Now;
        public string Username { get; set; } = "Unknown";
        public string Offerwall { get; set; } = "Unknown";
        public double Amount { get; set; } = 0;
        public string Status { get; set; } = "Pending";
        public int CampaignId { get; set; } = 0;
        public string CampaignName { get; set; } = "Not found";

    }
    public class ChatHistoryModel
    {
        public Guid Reference { get; set; } = Guid.NewGuid();
        public DateTime DateTime { get; set; } = DateTime.Now;
        public string Username { get; set; } = "Unknown";
        public string Message { get; set; } = "Empty message";
    }
    public class SupportTicketsReplyModel
    {
        public Guid ReplyId { get; set; } = Guid.NewGuid();
        public Guid Reference { get; set; } = Guid.NewGuid();
        public DateTime DateTime { get; set; } = DateTime.Now;
        public string Reply { get; set; } = "User sent an empty reply";
        public string Username { get; set; } = "Unknown";
        public bool AdminRead { get; set; } = false;
        public bool UserRead { get; set; } = false;
    }

    //Deposit method class is no longer need

    //PTP blacklist needs a design pattern before implentation
    public class PTPSourceModel
    {
        public string Site { get; set; } = "Not Specified";
        public int Counter { get; set; } = 0;
    }
    public class PTPAbsoluteSourceModel
    {
        public string Site { get; set; } = "Not Specified";
        public int Counter { get; set; } = 0;
    }
    public class PTPModel
    {
        public int Id { get; set; } = 1;
        public Guid Reference { get; set; } = Guid.NewGuid();
        public DateTime Creation { get; set; } = DateTime.Now;

        public string Username { get; set; } = "Admin";

        public string Link { get; set; } = "http://faucet4u.com/IRotator3/Rotator.php";

        public int HitsReceived { get; set; } = 0;

        public bool IsTimeBased { get; set; } = false;

        public int Credit { get; set; } = 0;
    }
    public class BannerRotatorModel
    {
        public int Id { get; set; } = 1;
        public Guid Reference { get; set; } = Guid.NewGuid();
        public DateTime Creation { get; set; } = DateTime.Now;
        public string Type { get; set; } = "Unknown";

        public string Username { get; set; } = "Admin";

        public string ImageLink { get; set; } = "http://faucet4u.com/IRotator3/Rotator.php";
        public string TargetLink { get; set; } = "http://faucet4u.com/IRotator3/Rotator.php";

        public int Impressions { get; set; } = 0;
        public int Clicks { get; set; } = 0;

        public bool IsTimeBased { get; set; } = false;

        public int Credit { get; set; } = 0;
    }
    public class BannerNetworkModel
    {
        public int Id { get; set; } = 1;
        public string Type { get; set; } = "Unknown";
        public Guid Reference { get; set; } = Guid.NewGuid();
        public string HTMLCode { get; set; } = "<p>Advertise here for as low as 1$</p>";
        public DateTime Creation { get; set; } = DateTime.Now;
    }
    public class LinkShortnersModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Link { get; set; } = "http://faucet4all.com";
        public DateTime Creation { get; set; } = DateTime.Now;
        public int Limit { get; set; } = 1;
        public string Remarks { get; set; } = "Testing, this is a new site";

    }
    public class BonusAdsModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Link { get; set; } = "http://faucet4all.com";
        public DateTime Creation { get; set; } = DateTime.Now;
        public int Limit { get; set; } = 1;
        public string Username { get; set; } = "Admin";
        public int HitsReceived { get; set; } = 0;
        public bool IsTimeBased { get; set; } = false;
        public int Credit { get; set; } = 0;
        public bool MaxOut { get; set; } = false;
        public string Title { get; set; } = "Bonus Ad";
        public string Description { get; set; } = "Click here to view the Bonus Ad";

    }
    public class BonusAdsHistoryModel
    {
        public Guid Reference { get; set; } = Guid.NewGuid();
        public string Link { get; set; } = "http://faucet4all.com";
        public DateTime DateTime { get; set; } = DateTime.Now;
        public string Username { get; set; } = "Admin";
        public double Amount { get; set; } = 0;

    }
    public class LinkShortnersHistoryModel
    {
        public Guid Reference { get; set; } = Guid.NewGuid();
        public DateTime DateTime { get; set; } = DateTime.Now;
        public string Username { get; set; } = "Admin";
        public double Amount { get; set; } = 0;
        public string Link { get; set; } = "http://faucet4all.com";

    }
    public class PTPIP
    {
        public string Username { get; set; } = "Unknown";
        public string UniqueIP { get; set; } = "0.0.0.0";
        public string NonUniqueIP1 { get; set; } = null;
        public string NonUniqueIP2 { get; set; } = null;
        public string NonUniqueIP3 { get; set; } = null;
    }
    public class LinkShortnersRecords
    {
        public string IP { get; set; } = "0.0.0.0";
    }

    //Models meant to be used for Admin panel
    public class FaucetHubHistoryModel
    {
        public Guid Reference { get; set; } = Guid.NewGuid();
        public DateTime DateTime { get; set; } = DateTime.Now;
        public long PayoutId { get; set; } = 0;
        public string Hash { get; set; } = "Unspecified";
        public string Address { get; set; } = "Unspecified";
        public double Amount { get; set; } = 0;

    }
    public class OrderHistoryModel
    {
        public Guid Reference { get; set; } = Guid.NewGuid();
        public DateTime DateTime { get; set; } = DateTime.Now;
        public string Username { get; set; } = "Unknown";
        public string Name { get; set; } = "Unknown";
        public bool IsTimeBased { get; set; } = false;
        public long Credit { get; set; } = 0;
        public double Price { get; set; } = 0;

    }
    public class EmailHistoryModel
    {
        public Guid Reference { get; set; } = Guid.NewGuid();
        public DateTime DateTime { get; set; } = DateTime.Now;
        public string Recipient { get; set; } = "Unknown";
        public string Subject { get; set; } = "Unknown";
        public string Message { get; set; } = "Unknown";

    }
    public class AdminLoginHistoryModel
    {
        public Guid SessionId { get; set; } = Guid.NewGuid();
        public DateTime LoggedIn { get; set; } = DateTime.Now;
        public DateTime SessionExpiry { get; set; } = DateTime.Now;
        public string Name { get; set; } = "Unknown";

    }
}