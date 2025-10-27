using ESOA.Model;
using System.Collections.Generic;

namespace ESOA.Model.Constant
{
    public static class PaymentCurrency
    {
        public const string PHP = "PHP";
        public const string USD = "USD"; 


        public static List<NameValuePair> List()
        {
            List<NameValuePair> result = new List<NameValuePair>();

            result.Add(new NameValuePair() { Value = Translate(PHP), Name = PHP });
            result.Add(new NameValuePair() { Value = Translate(USD), Name = USD }); 

            return result;
        }

        public static string Translate(string key)
        {
            if (string.IsNullOrEmpty(key)) return string.Empty;

            string result = key;
            switch (key)
            {
                case PHP:
                    result = "PHP";
                    break;
                case USD:
                    result = "USD";
                    break;
                default:
                    break;
            }

            return result;
        }
    }
}
