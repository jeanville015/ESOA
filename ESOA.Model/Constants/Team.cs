using ESOA.Model;
using System.Collections.Generic;

namespace ESOA.Model.Constant
{
    public static class Team
    {
        public const string MoneyRemittanceAccounting = "Money Remittance Accounting";
        public const string MoneyTreasury = "Money Treasury";
        public const string Sales = "Sales";


        public static List<NameValuePair> List()
        {
            List<NameValuePair> result = new List<NameValuePair>();

            result.Add(new NameValuePair() { Value = Translate(MoneyRemittanceAccounting), Name = MoneyRemittanceAccounting });
            result.Add(new NameValuePair() { Value = Translate(MoneyTreasury), Name = MoneyTreasury }); 
            result.Add(new NameValuePair() { Value = Translate(Sales), Name = Sales });

            return result;
        }

        public static string Translate(string key)
        {
            if (string.IsNullOrEmpty(key)) return string.Empty;

            string result = key;
            switch (key)
            {
                case MoneyRemittanceAccounting:
                    result = "Money Remittance Accounting";
                    break;
                case MoneyTreasury:
                    result = "Money Treasury";
                    break;
                case Sales:
                    result = "Sales";
                    break;
                default:
                    break;
            }

            return result;
        }
    }
}
