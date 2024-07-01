// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
using System.Collections.Generic;

namespace FBOLinx.ServiceLayer.DTO.UseCaseModels.JetNet
{
    public class AircraftResult
    {
        public int aircraftid { get; set; }
        public int modelid { get; set; }
        public string airframetype { get; set; }
        public string maketype { get; set; }
        public string make { get; set; }
        public string model { get; set; }
        public string icaotype { get; set; }
        public string serialnbr { get; set; }
        public string regnbr { get; set; }
        public int yearmfr { get; set; }
        public int yeardlv { get; set; }
        public string weightclass { get; set; }
        public string categorysize { get; set; }
        public string baseicao { get; set; }
        public string baseairport { get; set; }
        public string ownership { get; set; }
        public string usage { get; set; }
        public string maintained { get; set; }
        public List<Company> companies { get; set; }
        public List<CompanyRelationship> companyrelationships { get; set; }
    }

    public class Company
    {
        public string company { get; set; }
        public List<CompanyRelationship> companyrelationships { get; set; }
    }

    public class CompanyRelationship
    {
        public int companyid { get; set; }
        public int parentcompanyid { get; set; }
        public string companyrelation { get; set; }
        public string companyname { get; set; }
        public string companyisoperator { get; set; }
        public string companyagencytype { get; set; }
        public string companybusinesstype { get; set; }
        public string companyaddress1 { get; set; }
        public object companyaddress2 { get; set; }
        public string companycity { get; set; }
        public string companystate { get; set; }
        public string companystateabbr { get; set; }
        public string companypostcode { get; set; }
        public string companycountry { get; set; }
        public string companyemail { get; set; }
        public string companyofficephone { get; set; }
        public int contactid { get; set; }
        public string contactsirname { get; set; }
        public string contactfirstname { get; set; }
        public object contactmiddleinitial { get; set; }
        public string contactlastname { get; set; }
        public object contactsuffix { get; set; }
        public string contacttitle { get; set; }
        public string contactemail { get; set; }
        public string contactbestphone { get; set; }
        public string contactofficephone { get; set; }
        public string contactmobilephone { get; set; }
    }

    public class JetNetDto
    {
        public string responseid { get; set; }
        public string responsestatus { get; set; }
        public AircraftResult aircraftresult { get; set; }
    }

    public class JetNetUser
    {
        public string emailaddress { get; set; }
        public string password { get; set; }
    }

    public class JetNetApiToken
    {
        public string bearerToken { get; set; }
        public string apiToken { get; set; }
    }
}