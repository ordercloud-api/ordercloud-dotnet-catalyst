using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Integrations.Payment.CardConnect.Models
{
    /// <summary>
    /// https://developer.cardpointe.com/cardconnect-api#create-update-profile-request
    /// </summary>
    public class CardConnectCreateUpdateProfileRequest
    {
        /// <summary>
        /// 20-digit profile ID and (optional) 3-digit account ID string in the format &lt;profile id&gt;/&lt;account id&gt;, required to update an existing profile.
        /// </summary>
        public string profile { get; set; }
        /// <summary>
        /// "Y" to update profile data with non-empty request data only as opposed to full profile replacement including empty values
        /// </summary>
        public string profileupdate { get; set; }
        /// <summary>
        /// "Y" to assign as default account
        /// </summary>
        public string defaultacct { get; set; }
        /// <summary>
        /// Account US state, Mexican state, or Canadian province.
        /// </summary>
        public string region { get; set; }
        /// <summary>
        /// Account phone; optional for credit cards, but required for ACH(ECHK or ESAV) transactions.
        /// </summary>
        public string phone { get; set; }
        /// <summary>
        /// One of PPAL, PAID, GIFT, PDEBIT, otherwise not required
        /// </summary>
        public string accttype { get; set; }
        /// <summary>
        /// Account postal code. if country is "US", must be 5 or 9 digits otherwise any alphanumeric string is accepted
        /// </summary>
        public string postal { get; set; }
        /// <summary>
        /// Card expiration in one of the following formats:
        /// - MMYY
        /// - YYYYM(for single-digit months)
        /// - YYYYMM
        /// - YYYYMMDD
        /// Not required for eCheck(ACH) or digital wallet(for example, Apple Pay or Google Pay) payments.
        /// </summary>
        public string expiry { get; set; }
        /// <summary>
        /// Account city.
        /// </summary>
        public string city { get; set; }
        /// <summary>
        /// Account country (2 character country code).
        /// </summary>
        public string country { get; set; }
        /// <summary>
        /// Account street address.
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// Account name; optional for credit cards, but required for E-check/ACH authorizations.
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Merchant ID, required for all requests
        /// </summary>
        public string merchid { get; set; }
        /// <summary>
        /// One of the following:
        /// - CardSecure Token: See the CardSecure Developer Guide for more information.
        /// - Clear-text card number
        /// - Bank Account Number: Account(s) must be entitled with electronic check capability. When using this field, the bankaba field is also required.
        /// </summary>
        public string account { get; set; }
    }

    /// <summary>
    /// https://developer.cardpointe.com/cardconnect-api#create-update-profile-response
    /// </summary>
    public class CardConnectCreateUpdateProfileResponse : CardConnectProfile
    {
        ///<summary>
        /// Alpha-numeric response code that represents the description of the response
        /// </summary>
        public string respcode { get; set; }
        /// <summary>
        /// Abbreviation that represents the platform and the processor for the transaction
        /// </summary>
        public string respproc { get; set; }
        /// <summary>
        /// - A: Approved
        /// - B: Retry
        /// - C: Declined
        /// </summary>
        public string respstat { get; set; }
        /// <summary>
        /// Text description of the 
        /// </summary>
        public string resptext { get; set; }
    }

    public class CardConnectProfile
    {
        /// <summary>
        /// Account US state, Mexican state, or Canadian province.
        /// </summary>
        public string region { get; set; }

        /// <summary>
        /// Account postal code.
        /// if country is "US", must be 5 or 9 digits
        /// otherwise any alphanumeric string is accepted
        /// </summary>
        public string postal { get; set; }

        /// <summary>
        /// Account street address.
        /// </summary>
        public string address { get; set; }

        /// <summary>
        /// One of PPAL, PAID, GIFT, PDEBIT, otherwise not required
        /// </summary>
        public string accttype { get; set; }

        /// <summary>
        /// Y marks the default account referenced when a profile ID is used without an associated account ID.
        /// </summary>
        public string defaultacct { get; set; }

        /// <summary>
        /// The token included in the request. If a card number or bank account number was included in the request, returns a new token generated for that account. 
        /// </summary>
        public string token { get; set; }
        /// <summary>
        /// Account name; optional for credit cards, but required for E-check/ACH authorizations.
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Account country (2 character country code).
        /// </summary>
        public string country { get; set; }

        /// <summary>
        /// Account city.
        /// </summary>
        public string city { get; set; }

        /// <summary>
        /// Card expiration in one of the following formats:
        /// - MMYY
        /// - YYYYM(for single-digit months)
        /// - YYYYMM
        /// - YYYYMMDD
        /// Not required for eCheck(ACH) or digital wallet(for example, Apple Pay or Google Pay) payments.
        /// </summary>
        public string expiry { get; set; }

        /// <summary>
        /// Primary identifier to access profiles
        /// </summary>
        public string profileid { get; set; }

        /// <summary>
        /// Account identifier within a profile
        /// </summary>
        public string acctid { get; set; }

        /// <summary>
        /// Identifies whether or not the profile is set to opt out of the CardPointe Account Updater service.
        /// Y = Yes(updates are not retrieved for this profile)
        /// N = No(updates are retrieved for this profile)
        /// </summary>
        public string auoptout { get; set; }

        /// <summary>
        /// Account phone; optional for credit cards, but required for ACH(ECHK or ESAV) transactions.
        /// </summary>
        public string phone { get; set; }
    }

    /// <summary>
    /// https://developer.cardpointe.com/cardconnect-api#get-profile-response
    /// </summary>
    public class CardConnectGetProfileRequest
    {
        public string profileid { get; set; }
        public string merchid { get; set; }
        public string accountid { get; set; } = null;
    }

    /// <summary>
    /// https://developer.cardpointe.com/cardconnect-api#get-profile-response
    /// </summary>
    public class CardConnectGetProfileResponse
    {
        public List<CardConnectProfile> profiles { get; set; }
    }

    /// <summary>
    /// https://developer.cardpointe.com/cardconnect-api#delete-profile-response
    /// </summary>
    public class CardConnectDeleteProfileResponse : CardConnectResponseData {}
}
