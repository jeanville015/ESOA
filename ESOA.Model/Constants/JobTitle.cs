using ESOA.Model;
using System.Collections.Generic;

namespace ESOA.Model.Constant
{
    public static class JobTitle
    {
        public const string FundingAnalyst = "Funding Analyst";
        public const string TreasuryAssociate = "Treasury Associate"; 


        public static List<NameValuePair> List()
        {
            List<NameValuePair> result = new List<NameValuePair>();

            result.Add(new NameValuePair() { Value = Translate(FundingAnalyst), Name = FundingAnalyst });
            result.Add(new NameValuePair() { Value = Translate(TreasuryAssociate), Name = TreasuryAssociate }); 

            return result;
        }

        public static string Translate(string key)
        {
            if (string.IsNullOrEmpty(key)) return string.Empty;

            string result = key;
            switch (key)
            {
                case FundingAnalyst:
                    result = "Funding Analyst";
                    break;
                case TreasuryAssociate:
                    result = "Treasury Associate";
                    break;
                default:
                    break;
            }

            return result;
        }
    }
}
