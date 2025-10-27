using ESOA.Model;
using System.Collections.Generic;

namespace ESOA.Model.Constant
{
    public static class SOAStatus
    {
        public const string Verified = "Verified"; 


        public static List<NameValuePair> List()
        {
            List<NameValuePair> result = new List<NameValuePair>();

            result.Add(new NameValuePair() { Value = Translate(Verified), Name = Verified }); 

            return result;
        }

        public static string Translate(string key)
        {
            if (string.IsNullOrEmpty(key)) return string.Empty;

            string result = key;
            switch (key)
            {
                case Verified:
                    result = "Verified";
                    break; 
            }

            return result;
        }
    }
}
