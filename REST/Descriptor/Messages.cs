namespace AcumaticaMetalsAPI
{
    using System;
    using System.IO;
    using System.Web;
    using System.Reflection;

    [Obfuscation(Feature = "encryptmethod", Exclude = false)]
    internal static class ConfigKeys
    {
        internal static readonly string Asterisk = "*";
        internal static readonly string HTParser = "#";
        internal static readonly string HeaderApiVersion = "x-api-version";



    }

    public static class SharedConfig
    {
        public const string TaxProviderID = "ASCUseUpstreamSalesTax";
        public const string ActiveSettingID = "Active";
        public const string SandboxSettingID = "Sandbox";
        public const string ByPassLocalSettingID = "ByPassLocal";
        public const string MarketPlaceSettingID = "MarketPlace";
        public const string MarketPlaceExemptSettingID = "ExemptTaxCode";
        public const string MarketPlaceTaxableSettingID = "TaxableTaxCode";
        public const string UseTaxJarSettingID = "UseTaxJar";
        public const string UseSubstitutionTableID = "AddSubstitution";
        //public const string TaxJarDefaultAPIVersion = "2020-08-07";
        public const string TaxJarDefaultAPIVersion = "2022-01-24";
        
        public const string DestinationCountry = "Destination Country";
        public const string DestinationPostalCode = "Destination Postal Code";
        public const string DestinationRegion = "Destination Region/State";
        public const string DestinationCity = "Destination City";
        public const string DestinationAddress = "Destination Address Line 1";


        public const string OriginCountry = "Origin Country";
        public const string OriginPostalCode = "Origin Postal Code";
        public const string OriginRegion = "Origin Region/State";
        public const string OriginCity = "Origin City";
        public const string OriginAddress = "Origin Address Line 1";

    }
    #region ProviderCodes
    [Obfuscation(Feature = "encryptmethod", Exclude = false)]
    internal static class ProviderCode
    {
        public const string Wholesale = "wholesale";
        public const string Government = "government";
        public const string Other = "other";
        public const string NonExempt = "non_exempt";
    }

    #endregion
    #region Validation and Processing Messages
    [Obfuscation(Feature = "encryptmethod", Exclude = false)]
    internal static class Validations
    {
        internal static string CountryCannotBeEmpty = "Country cannot be empty.";
        internal static string StateCannotBeEmpty = "State cannot be empty.";
        internal static string ExemptTypeCannotBeEmpty = "Exemption Type cannot be empty.";
        internal static string CouldNotValidateLicense = "AnyTax could not validate license";
        internal static string UnableToLocate = "License did not contain {0}";
        internal static string UrlNotCoveredByLicense = "{0} is not included in license - {1}";
        internal static string UrlNotCoveredByAllowedDomains = "{0} is not included in allowed domains - {1}";
    }
    #endregion

    [Obfuscation(Feature = "encryptmethod", Exclude = true)]
    public static class Messages
    {
        [Obfuscation(Feature = "encryptmethod", Exclude = true)]
        public const string Prefix = "AnyTax Error";
        public const string Approval = "Approval";

        public const string TPNone = "No External Tax Provider";
        public const string TPTaxJar = "TaxJar";

        #region DAC Names

        internal static class DACNames
        {
            public const string ATExempt = "Exemptions";
            public const string ATSetup = "AnyTax Setup";
        }
        #endregion

        #region Customer Usage
        public const string NonExempt = "Non-Exempt";
        public const string FederalGovt = "Federal Government";
        public const string StateLocalGovt = "State/Local Govt.";
        public const string TribalGovt = "Tribal Government";
        public const string ForeignDiplomat = "Foreign Diplomat";
        public const string CharitableOrg = "Charitable Organization";
        public const string Religious = "Religious";
        public const string Resale = "Resale";
        public const string AgriculturalProd = "Agricultural Production";
        public const string IndustrialProd = "Industrial Prod/Mfg.";
        public const string DirectPayPermit = "Direct Pay Permit";
        public const string DirectMail = "Direct Mail";
        public const string Other = "Other";
        public const string Education = "Education";
        public const string LocalGovt = "Local Government";
        public const string ComAquaculture = "Commercial Aquaculture";
        public const string ComFishery = "Commercial Fishery";
        public const string NonResident = "Non-resident";
        #endregion

        #region Translatable Strings used in the code
        [Obfuscation(Feature = "encryptmethod", Exclude = true)]
        public static class TranslatableStrings
        {
            public const string ATAddExemption = "Add Exemption";
            public const string ATRemoveExemption = "Remove Exemption";
            public const string BAccountID = "Acct CD";
            public const string BIZACCT = "BIZACCT";
            public const string Country = "Country";
            public const string State = "State";
            public const string AcumaticaExemptionType = "Acumatica Exemption Type";
            public const string AnyTaxProviderExemptionType = "Provider Exemption Type";
            public const string ExemptionID = "Exemption ID";


            public const string Tstamp = "Time Stamp";
            public const string CreatedByID = "Created By";
            public const string CreatedByScreenID = "Created by Screen";
            public const string CreatedDateTime = "Create Date";
            public const string LastModifiedByID = "Last Modified By";
            public const string LastModifiedByScreenID = "Last Modified by Screen";
            public const string LastModifiedDateTime = "Last Modified Date";
            public const string DeletedRecord = "Deleted Record";
            public const string ExpirationDate = "Expiration Date";
            public const string AnyTaxSyncDateTime = "Provider Sync Date";
            public const string AnyTaxProviderMessage = "Provider Message";
            public const string LICENSE = "LICENSE";
            public const string LicenseNotFound = "Unable to locate license in AnyTax Table";

            public const string Create = "Create"; //Create
            public const string Update = "Update"; //Update
            public const string Delete = "Delete"; //Delete


        }
        #endregion

        #region Custom Actions
        [Obfuscation(Feature = "encryptmethod", Exclude = true)]
        public static class CustomActions
        {
            public const string Process = "Process";
            public const string ProcessAll = "Process All";
        }
        #endregion
    }
}
