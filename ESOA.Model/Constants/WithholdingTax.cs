using ESOA.Model;
using System.Collections.Generic;

namespace ESOA.Model.Constant
{
    public static class WithholdingTax
    {
        public const string Yes = "Yes";
        public const string No = "No"; 


        public static List<NameValuePair> List()
        {
            List<NameValuePair> result = new List<NameValuePair>();

            result.Add(new NameValuePair() { Value = Translate(Yes), Name = Yes });
            result.Add(new NameValuePair() { Value = Translate(No), Name = No }); 

            return result;
        }

        public static string Translate(string key)
        {
            if (string.IsNullOrEmpty(key)) return string.Empty;

            string result = key;
            switch (key)
            {
                case Yes:
                    result = "Yes";
                    break;
                case No:
                    result = "No";
                    break;
                default:
                    break;
            }

            return result;
        }
    }
}
