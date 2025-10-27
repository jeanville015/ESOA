using ESOA.Model;
using System.Collections.Generic;

namespace ESOA.Model.Constant
{
    public static class Domestic_Intl
    {
        public const string Domestic = "Domestic";
        public const string International = "International"; 


        public static List<NameValuePair> List()
        {
            List<NameValuePair> result = new List<NameValuePair>();

            result.Add(new NameValuePair() { Value = Translate(Domestic), Name = Domestic });
            result.Add(new NameValuePair() { Value = Translate(International), Name = International }); 

            return result;
        }

        public static string Translate(string key)
        {
            if (string.IsNullOrEmpty(key)) return string.Empty;

            string result = key;
            switch (key)
            {
                case Domestic:
                    result = "Domestic";
                    break;
                case International:
                    result = "International";
                    break;
                default:
                    break;
            }

            return result;
        }
    }
}
