using ESOA.Model;
using System.Collections.Generic;

namespace ESOA.Model.Constant
{
    public static class RateType
    {
        public const string Amount = "AMOUNT";
        public const string Percentage = "PERCENTAGE"; 


        public static List<NameValuePair> List()
        {
            List<NameValuePair> result = new List<NameValuePair>();

            result.Add(new NameValuePair() { Value = Translate(Amount), Name = Amount });
            result.Add(new NameValuePair() { Value = Translate(Percentage), Name = Percentage }); 

            return result;
        }

        public static string Translate(string key)
        {
            if (string.IsNullOrEmpty(key)) return string.Empty;

            string result = key;
            switch (key)
            {
                case Amount:
                    result = "AMOUNT";
                    break;
                case Percentage:
                    result = "PERCENTAGE";
                    break;
                default:
                    break;
            }

            return result;
        }
    }
}
