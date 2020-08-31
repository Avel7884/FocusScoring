using System;
using System.Collections.Generic;
using FocusAccess.Methods;
using FocusAccess.Response;
using FocusAccess.ResponseClasses;

namespace FocusAccess
{
    public class Api3
    {
        internal Api3(IJsonAccess access, IJsonAccess forcedAccess)
        {
            Req = new ApiMultiRequestMethod<ReqValue, InnUrlArg>(access,forcedAccess,"/req");
            Contacts = new ApiMultiRequestMethod<ContactsValue, InnUrlArg>(access,forcedAccess,"/contacts");
            Analytics = new ApiMultiRequestMethod<AnalyticsValue,InnUrlArg>(access,forcedAccess,"/analytics"); 
            EgrDetails = new ApiMultiRequestMethod<EgrDetailsValue, InnUrlArg>(access,forcedAccess, "/egrDetails");
            Licences = new ApiMultiRequestMethod<LicencesValue, InnUrlArg>(access,forcedAccess, "/licences");
            Buh = new ApiMultiRequestMethod<BuhValue, InnUrlArg>(access,forcedAccess,"/buh");
            Taxes = new ApiMultiRequestMethod<TaxesValue, InnUrlArg>(access,forcedAccess,"/taxes");
            FnsBlockedBankAccounts = new ApiSingleValueMethod<FnsBlockedBankAccountsValue,InnUrlArg>(access,forcedAccess,"/fnsBlockedBankAccounts");
            MonList = new ApiSingleValueMethod<MonListValue,InnListUrlArg>(access,forcedAccess,"/monList",false);
            EgrDetailsMon = new ApiSingleValueMethod<MonValue,DateUrlArg>(access,forcedAccess,"egrDetails/mon",false);
            ReqMon = new ApiSingleValueMethod<MonValue,DateUrlArg>(access,forcedAccess,"req/mon",false);
            FizBankr = new ApiSingleValueMethod<FizBankrValue,DatedQueryUrlArg>(access,forcedAccess,"/fizBankr",false);
            PersonBankruptcy = new ApiMultiRequestMethod<PersonBankruptcyValue,DatedPersonUrlArg>(access,forcedAccess,"/personBankruptcy",false);
            CheckPassport = new ApiMultiRequestMethod<CheckPassportValue,PassportsUrlArg>(access,forcedAccess,"/checkPassport");
            ReqBase = new ApiMultiRequestMethod<ReqBaseValue,InnUrlArg>(access,forcedAccess,"/reqBase"); 
            Fssp  = new ApiSingleValueMethod<FsspValue,SkipableInnUrlArg > (access,forcedAccess, "/fssp");
            Sites = new ApiMultiRequestMethod<SitesValue,InnUrlArg>(access,forcedAccess,"/sites");
            Fsa = new ApiSingleValueMethod<FsaValue,SkipableInnUrlArg>(access,forcedAccess,"/fsa");
            GovPurchasesOfCustomer = new ApiSingleValueMethod<GovPurchasesOfCustomerValue,SkipableInnUrlArg>(access,forcedAccess,"/govPurchasesOfCustomer");
            GovPurchasesOfParticipant = new ApiSingleValueMethod<GovPurchasesOfParticipantValue,SkipableInnUrlArg>(access,forcedAccess,"/govPurchasesOfParticipant");
            BeneficialOwners =new ApiMultiRequestMethod<BeneficialOwnersValue,InnUrlArg>(access,forcedAccess,"/beneficialOwners");
            Stat = new ApiMultiValueMethod<StatValue,EmptyUrlArg>(access,forcedAccess,"/stat",false);
            CompanyAffiliatesReq = new ApiMultiValueMethod<ReqValue,InnUrlArg>(access,forcedAccess,"/companyAffiliates/req");
            CompanyAffiliatesEgrDetails = new ApiMultiValueMethod<EgrDetailsValue, InnUrlArg>(access,forcedAccess,"/companyAffiliates/egrDetails");
            CompanyAffiliatesAnalytics = new ApiMultiValueMethod<AnalyticsValue, InnUrlArg>(access,forcedAccess,"/companyAffiliates/analytics");
            PersonAffiliatesReq = new ApiMultiValueMethod<ReqValue,InnflUrlArg>(access,forcedAccess,"/personAffiliates/req");
            PersonAffiliatesEgrDetails = new ApiMultiValueMethod<EgrDetailsValue, InnflUrlArg>(access,forcedAccess,"/personAffiliates/egrDetails");
            PersonAffiliatesAnalytics = new ApiMultiValueMethod<AnalyticsValue, InnflUrlArg>(access,forcedAccess,"/personAffiliates/analytics");
            PepSearch = new ApiMultiValueMethod<PepSearchValue,QueryUrlArg>(access,forcedAccess,"/pepSearch",false); 
            SanctionedPersons = new ApiMultiRequestMethod<SanctionedPersonsValue,QueryUrlArg>(access,forcedAccess,"/sanctionedPersons");
            BankGuarantees = new ApiSingleValueMethod<BankGuaranteesValue,SkipableInnUrlArg>(access,forcedAccess,"/bankGuarantees");
            ForeignRepresentatives = new ApiMultiRequestMethod<ForeignRepresentativesValue,NzaUrlArg>(access,forcedAccess,"/foreignRepresentatives");
            Suggest = new ApiSingleValueMethod<SuggestValue,QUrlArg>(access,forcedAccess, "/suggest",false);
            Trademarks = new ApiSingleValueMethod<TrademarksValue,SkipableInnUrlArg>(access,forcedAccess,"/trademarks");
            KzCompanyDetails = new ApiMultiRequestMethod<KzCompanyDetailsValue,KzUrlArg>(access,forcedAccess,"kz/companyDetails");
            ByCompanyDetails = new ApiMultiRequestMethod<ByCompanyDetailsValue,ByUrlArg>(access,forcedAccess,"by/companyDetails");
        }

        public static Type GetType(ApiMethodEnum eValue)
        {   // TODO implement switch
            return typeof(Api3).GetProperty(eValue.ToString())?.PropertyType.GenericTypeArguments[0];
        }

        public object GetMethodResult(ApiMethodEnum method, object inn)
        {
            switch (method)
            {
                case ApiMethodEnum.analytics: 
                    return Analytics.MakeRequest(new InnUrlArg(inn as INN));
                case ApiMethodEnum.req:
                    return Req.MakeRequest(new InnUrlArg(inn as INN));
                default: 
                    throw new NotSupportedException("You just wait.");
            }
        }
        
        public IApiMultiRequestMethod<ByCompanyDetailsValue, ByUrlArg> ByCompanyDetails { get; }
        public IApiMultiRequestMethod<KzCompanyDetailsValue, KzUrlArg> KzCompanyDetails { get; }
        public IApiSingleValueMethod<TrademarksValue, SkipableInnUrlArg> Trademarks { get; }
        public IApiSingleValueMethod<SuggestValue, QUrlArg> Suggest { get; }
        public IApiMultiRequestMethod<ForeignRepresentativesValue, NzaUrlArg> ForeignRepresentatives { get; }
        public IApiSingleValueMethod<BankGuaranteesValue, SkipableInnUrlArg> BankGuarantees { get; }
        public IApiMultiRequestMethod<SanctionedPersonsValue, QueryUrlArg> SanctionedPersons { get; }
        public IApiMultiValueMethod<PepSearchValue, QueryUrlArg> PepSearch { get; }
        public IApiMultiValueMethod<AnalyticsValue, InnflUrlArg> PersonAffiliatesAnalytics { get; }
        public IApiMultiValueMethod<EgrDetailsValue, InnflUrlArg> PersonAffiliatesEgrDetails { get; }
        public IApiMultiValueMethod<ReqValue, InnflUrlArg> PersonAffiliatesReq { get; }
        public IApiMultiValueMethod<AnalyticsValue, InnUrlArg> CompanyAffiliatesAnalytics { get; }
        public IApiMultiValueMethod<EgrDetailsValue, InnUrlArg> CompanyAffiliatesEgrDetails { get;}
        public IApiMultiValueMethod<ReqValue, InnUrlArg> CompanyAffiliatesReq { get;}
        internal IApiMultiValueMethod<StatValue, EmptyUrlArg> Stat { get; }
        public IApiMultiRequestMethod<BeneficialOwnersValue, InnUrlArg> BeneficialOwners { get; }
        public IApiSingleValueMethod<GovPurchasesOfParticipantValue, SkipableInnUrlArg> GovPurchasesOfParticipant { get;}
        public IApiSingleValueMethod<GovPurchasesOfCustomerValue, SkipableInnUrlArg> GovPurchasesOfCustomer { get;}
        public IApiSingleValueMethod<FsaValue,SkipableInnUrlArg> Fsa { get; }
        public IApiMultiRequestMethod<SitesValue, InnUrlArg> Sites { get; }
        public IApiSingleValueMethod<FsspValue,SkipableInnUrlArg > Fssp { get; }
        public IApiMultiRequestMethod<ReqBaseValue, InnUrlArg> ReqBase { get;  }
        public IApiMultiRequestMethod<CheckPassportValue, PassportsUrlArg> CheckPassport { get; }
        public IApiMultiRequestMethod<PersonBankruptcyValue, DatedPersonUrlArg> PersonBankruptcy { get; }
        public IApiSingleValueMethod<FizBankrValue, DatedQueryUrlArg> FizBankr { get; }
        public IApiSingleValueMethod<MonValue, DateUrlArg> ReqMon { get; }
        public IApiSingleValueMethod<MonValue, DateUrlArg> EgrDetailsMon { get; }
        public IApiSingleValueMethod<MonListValue, InnListUrlArg> MonList { get; }
        public IApiSingleValueMethod<FnsBlockedBankAccountsValue, InnUrlArg> FnsBlockedBankAccounts { get;}
        public IApiMultiRequestMethod<ReqValue,InnUrlArg> Req { get; }
        public IApiMultiRequestMethod<ContactsValue,InnUrlArg> Contacts { get; }
        public IApiMultiRequestMethod<AnalyticsValue,InnUrlArg> Analytics { get; }
        public IApiMultiRequestMethod<EgrDetailsValue, InnUrlArg> EgrDetails { get; }
        public IApiMultiRequestMethod<LicencesValue, InnUrlArg> Licences { get; }
        public IApiMultiRequestMethod<BuhValue,InnUrlArg> Buh { get; }
        public IApiMultiRequestMethod<TaxesValue,InnUrlArg> Taxes { get; }
    }
}