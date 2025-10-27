using ESOA.Model;
using System.Collections.Generic;

namespace ESOA.Model.Constant
{
    public static class TransmissionMode
    {
        public const string Pull = "Pull";
        public const string Push = "Push"; 


        public static List<NameValuePair> List()
        {
            List<NameValuePair> result = new List<NameValuePair>();

            result.Add(new NameValuePair() { Value = Translate(Pull), Name = Pull });
            result.Add(new NameValuePair() { Value = Translate(Push), Name = Push }); 

            return result;
        }

        public static string Translate(string key)
        {
            if (string.IsNullOrEmpty(key)) return string.Empty;

            string result = key;
            switch (key)
            {
                case Pull:
                    result = "Pull";
                    break;
                case Push:
                    result = "Push";
                    break;
                default:
                    break;
            }

            return result;
        }
    }
}
