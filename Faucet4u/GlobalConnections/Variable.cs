using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Faucet4u.GlobalConnections.Variable
{
    //Some structs are named as "{structName}Variable" because those particular structs were conflicting with the name of important classes
    public struct Other
    {
        public const string SQLConnectionString = "Server=localhost;Database=Faucet4u;Trusted_Connection=True;MultipleActiveResultSets=true";
        public const string secondaryLogsPath = @"..\Files\SecondaryLogs.txt";
        public const double minutesToAddInConfirmationCodeExpiryTime = 15;
        public const double sessionExpiryHoursToAdd = 6;
        public const string ipStackAPIKey = "0e11b2adc029d85b7d72621f39a8eaa6";
        public const string ipStackAPI = "http://api.ipstack.com";
        public const string blockchainRateAPI = "https://blockchain.info/tobtc";
        public const string sendEmailKey = "Ae^SolKz75H9";
        public const string exchangeAPIKey = "c7ffd9a4-e34c-4371-a68a-7d8f80a7f02b";
        public const string exchangeAPISecure = "c9e88960ccdc1b06990290a3f423ec3b63519b674a90185dd863ec31c48c3477";
    }

    public struct Email
    {
        public const int minimumLength = 3;
        public const int maximumLength = 350;
        public const string rangeErrorMessage = "Email address length should be between 3 and 350";
        public const string formatErrorMessage = "Invalid email address";
        public const string requiredErrorMessage = "Email address is required";
        public const string alreadyExistsMessage = "An account is registered with this email address already";
        public const string doesNotExistsMessage = "This email does not exist in database";
        public const string alreadyConfirmedMessage = "Your email is already confirmed";
        public const string notConfirmedMessaeg = "Your email is not confirmed";
    }

    public struct Password
    {
        public const int minimumLength = 10;
        public const int maximumLength = 128;
        public const string rangeErrorMessage = "Password length should be between 10 and 128";
        public const string requiredErrorMessage = "Password is required";
        public const string confirmRequiredErrorMessage = "Confirmation password is required";
        public const string confirmMismatchErrorMessage = "Passwords do not match";
        public const string resetSuccessfulMessage = "Your password has been successfully reset";
    }

    public struct Username
    {
        public const int minimumLength = 3;
        public const int maximumLength = 10;
        public const string rangeErrorMessage = "Username length should be between 3 and 10";
        public const string formatErrorMessage = "Username can contain only letters and digits";
        public const string requiredErrorMessage = "Username is required";
        public const string alreadyExistsMessage = "This username is already taken, please try different one";
        public const string regularExpression = "^[a-zA-Z][a-zA-Z0-9]*$";
        public const string doesNotExist = "This username does not exist in database";
    }

    public struct Recaptchav2Variable
    {
        public const string secretKey = "6LdlcHIUAAAAABeY1f5_WKRCUf_ncPoupzMwqdnZ";
        public const string requiredErrorMessage = "Recaptchav2 is required";
        public const string verificationLink = "https://www.google.com/recaptcha/api/siteverify";
        public const string invalidMessage = "Recaptchav2 is invalid";
    }

    public struct Recaptchav3Variable
    {
        public const string secretKey = "6LcJe3IUAAAAAPC0mVWjHfsKAmpOxRNtSw5v5zdx";
        public const string requiredErrorMessage = "Recaptchav3 is required";
        public const string verificationLink = "https://www.google.com/recaptcha/api/siteverify";
        public const string invalidMessage = "Recaptchav3 is invalid";
        //For the time being recaptcha v3 is disabled
        public const double minimumScore = 0;
    }

    public struct UserVariable //UserVariable should contain messages that are meant to be read by User
    {
        public const string accountRegisterSuccessfulMessage = "Your account has been successfully registered. A confirmation email has been sent, , it should reach you within 15 minutes. Please check your inbox or spam folder and click the verification link or copy the code and manually paste it in email confirmation page";
        public const string unknownErrorMessage = "An unknown error occured, please contact the support team and provide error reference {0}";
        public const string accountRegisterFailedSendingConfirmationCodeMessage = "Your account has been registered but some error occured due to which confirmation email was not sent. Please visit email confirmation page and request for a new confirmation email";
        public const string emailConfirmSuccessfulMessage = "Your email is now confirmed. You can now login and avail all the features of our faucet";
        public const string emailConfirmResendMessage = "A new confirmation email has been sent to your email address, it should reach you within 15 minutes. Please check your inbox or spam folder and click the verification link or copy the code and manually paste it in email confirmation page";
        public const string passwordResetCodeMessage = "A new confirmation code has been sent to your email address, it should reach you within 15 minutes. Please check your inbox or spam folder and click the verification link or copy the code and manually paste it in password reset page";
        public const string countryDifferentMessage = "Unfortunately you do not reside in the same country as the account is registered with, therefore your request is cancelled";
        public const string loginFailedMessage = "Username or password is incorrect";
        public const string settingsUpdationSuccessfulMessage = "Your settings have been updated";
        public const string accountLocked = "Your account is locked as you tried to cheat or attempt hacks";
    }

    public struct Logs //Logs messages are meant for server use only, especially for debugging/development mode
    {
        //Those logs who cannot have details like bodyValue, provide whatever information it can be provided, otherwise provide the whole bodyValue

        //Info
        public const string newUserRegisteredMessage = "New user registered, username {0} and email {1}";
        public const string userConfirmedEmailMessage = "A user associated with email {0} has  confirmed his email address";
        public const string emailConfirmResendMessage = "A user associated with email {0} had requested to resend confirmation email and it has been sent";
        public const string passwordResetCodeMessage = "A user associated with email {0} had requested to send confirmation code for password reset and it has been sent";
        public const string passwordResetMessage = "A user associated with username {0} has successfully reset his password";
        public const string loginUserLoggedIn = "A user associated with username {0} has successfully logged in";
        public const string infoMessage = "A user has accessed following route successfully {0} and with following details {1}";

        //Error
        public const string accountRegisterErrorMessage = "An unhandled error occured while registering new user with following details: {0}";
        public const string getUserCountryUnknownErrorMessage = "An unhandled error occured while using getting  country name for IP address {0}";
        public const string confirmUserEmailSendCodeUnknownErrorMessage = "An unhandled error occured while sending confirmation code for username {0}";
        public const string emailConfirmUnknownErrorMessage = "An unhandled error occured while confirming email with following details: {0}";
        public const string emailConfirmResendEmailUnknownErrorMessage = "An unhandled error occured while resending confirmation email with following details: {0}";
        public const string passwordResetCodeUnknownErrorMessage = "An unhandled error occured while sending confirmation code for password reset with following details: {0}";
        public const string passwordResetUnknownErrorMessage = "An unhandled error occured while resetting password for a user using following details: {0}";
        public const string getUserCountryCompareUnknownErrorMessage = "An unhandled error occured while comparing country name for IP address {0} and username/email {1}";
        public const string unknownErrorMessage = "An unhandled error occured for following route {0} and following details: {1}";

        //Hack
        public const string emailAvailableReuseEmailMessage = "An attempt was made to register an account with existing email address {0}";
        public const string usernameAvailableReuseUsernameMessage = "An attempt was made to register an account with existing username {0}";
        public const string emailConfirmInvalidConfirmationCodeMessage = "An attempt was made to confirm an email using invalid confirmation code using following details {0}";
        public const string emailExistsUseOfNonExistingEmailMessage = "An attempt was made to use an email address which does not exist {0}";
        public const string isEmaliConfirmedUseOfNonConfirmedEmailMessage = "An attempt was made to use an email address which is not yet confirmed {0}";
        public const string isEmailNotConfirmedReconfirmingMessaege = "An attempt was made to reconfirm an email address which is already confirmed {0}";
        public const string usernameExistsUseOfNonExistingUsernameMessage = "An attempt was made to use a username which does not exist in database {0}";
        public const string passwordResetInvalidCodeMessage = "An attempt was made to reset password using invalid confirmation code with following details: {0}";
        public const string countryDifferentMessage = "An attemtp was made to use server API for a user who is registered in different country compared to the requester for the following path {0} and following details {1}";
        public const string loginFailedMessage = "Login failed for following route {0} and following details: {1}";
        public const string hackAttemptMessage = "A hack attempt was made for route {0} with following details: {1}";

        //Cheat
    }

    public struct AnnotationsVariable //These messages are strictly related to annotations class
    {
        public const string emailAvailableUnknownErrorMessage = "Error occured during the use of EmailAvailable annotation with following details: {0}";
        public const string recaptchav2UnknownErrorMessage = "Error occured during the use of Recaptchav2 annotation with following details: {0}";
        public const string recaptchav3UnknownErrorMessage = "Error occured during the use of Recaptchav3 annotation with following details: {0}";
        public const string usernameAvailableUnknownErrorMessage = "Error occured during the use of UsernameAvailable annotation with following details: {0}";
        public const string emailExistsUnknownErrorMessage = "Error occured during the use of EmailExists annotation with following details: {0}";
        public const string isEmailNotConfirmedUnknownErrorMessage = "Error occured during the use of IsEmailNotConfirmed annotation with following details: {0}";
        public const string emailConfirmationCodeUnknownErrorMessage = "Error occured during the use of EmailConfirmationCode annotation with following details: {0}";
        public const string isCodeNotExpiredUnknownErrorMessage = "Error occured during the use of IsCodeNotExpired annotation with following details:  {0}";
        public const string isCodeExpiredUnknownErrorMessage = "Error occured during the use of IsCodeExpired annotation with following details: {0}";
        public const string isEmailConfirmedUnknownErrorMessage = "Error occured during the use of IsEmailConfirmed annotation with following details: {0}";
        public const string usernameExistsUnknownErrorMessage = "Error occured during the use of UsernameExists annotation with following details: {0}";

    }

    public struct EmailClient
    {
        //public const string host = "smtp.yandex.com";
        //public const int port = 587;
        //public const string username = "suleman.mehmood@yandex.com";
        //public const string password = "gmdieylkvgziultu";
        //public const string fromMailAddress = "suleman.mehmood@yandex.com";
        //public const string bodyMessage = "Confirmation code: {0}";
        //public const string subjectMessage = "Faucet4u - Confirm your email";
        //public const bool useDefaultCredentials = false;
        //public const bool useSSL = true;
        //public const int timeout = 10000;
        //public const string displayName = "Myself";

        public const string host = "66.206.39.103";
        public const int port = 25;
        public const string username = "faucet4all";
        public const string password = "U6%6!db9&Rdw";
        public const string fromMailAddress = "donotreply@faucet4all.com";
        public const string bodyMessage = @"
                                            Confirmation code: {0}
                                            Either click the below link or input the code manually by visiting http://www.faucet4all.com/#/EmailConfirm
                                            http://www.faucet4all.com/#/EmailConfirm?emailConfirmCode={0}";
        public const string subjectMessage = "Faucet4all - Confirm your email";
        public const bool useDefaultCredentials = false;
        public const bool useSSL = false;
        public const int timeout = 10000;
        public const string displayName = "Faucet4all";

    }

    public struct ConfirmationCode
    {
        public const string requiredErrorMessage = "Confirmation code is required to confirm your email";
        public const string regularExpression = "(^([0-9A-Fa-f]{8}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{12})$)";
        public const string formatErrorMessage = "Confirmation code format is invalid";
        public const string invalidCode = "Confirmation code is invalid";
        public const string expiredErrorMessage = "Your confirmation code is expired. Please request a new one";
        public const string notExpiredErrorMessage = "Your confirmation code has not been expired and was already sent. Please allow up to 15 minutes before requesting confirmation code again";
    }

    public struct Session
    {
        public const string requiredErrorMessage = "Session Id is required";
        public const string regularExpression = "(^([0-9A-Fa-f]{8}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{12})$)";
        public const string formatErrorMessage = "Session Id format is invalid";
        public const string invalidSessionId = "Session Id is invalid";
        public const string expiredErrorMessage = "Session is expired";
        public const string overrideValidation = "456a9477-e685-4ce9-b036-5819e8e2c468";
    }

    public struct SupportTickets
    {
        public const string createdSuccessMessage = "Your support ticket is successfully created and will be replied within 48 hours, please note down your reference number {0}";
    }

}
