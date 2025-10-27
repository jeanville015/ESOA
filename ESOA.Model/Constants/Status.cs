using ESOA.Model;
using System.Collections.Generic;

namespace ESOA.Model.Constant
{
    public static class Status
    {
        public const string Active = "Active";
        public const string Suspended = "Suspended";
        public const string Inactive = "Inactive";
        public const string Terminated = "Terminated";


        public static List<NameValuePair> List()
        {
            List<NameValuePair> result = new List<NameValuePair>();

            result.Add(new NameValuePair() { Value = Translate(Active), Name = Active });
            result.Add(new NameValuePair() { Value = Translate(Suspended), Name = Suspended }); 
            result.Add(new NameValuePair() { Value = Translate(Inactive), Name = Inactive }); 
            result.Add(new NameValuePair() { Value = Translate(Terminated), Name = Terminated });

            return result;
        }

        public static string Translate(string key)
        {
            if (string.IsNullOrEmpty(key)) return string.Empty;

            string result = key;
            switch (key)
            {
                case Active:
                    result = "Active";
                    break;
                case Suspended:
                    result = "Suspended";
                    break;
                case Inactive:
                    result = "Inactive";
                    break;
                case Terminated:
                    result = "Terminated";
                    break;
                default:
                    break;
            }

            return result;
        }
    }
}
