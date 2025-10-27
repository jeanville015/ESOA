using ESOA.Model;
using System.Collections.Generic;

namespace ESOA.Model.Constant
{
    public static class VatStatus
    {
        public const string TwelvePercent = "12%";
        public const string ZeroRated = "Zero-Rated";
        public const string Exempt = "Exempt";


        public static List<NameValuePair> List()
        {
            List<NameValuePair> result = new List<NameValuePair>();

            result.Add(new NameValuePair() { Value = Translate(TwelvePercent), Name = TwelvePercent });
            result.Add(new NameValuePair() { Value = Translate(ZeroRated), Name = ZeroRated });
            result.Add(new NameValuePair() { Value = Translate(Exempt), Name = Exempt });

            return result;
        }

        public static string Translate(string key)
        {
            if (string.IsNullOrEmpty(key)) return string.Empty;

            string result = key;
            switch (key)
            {
                case TwelvePercent:
                    result = "12%";
                    break;
                case ZeroRated:
                    result = "Zero-Rated";
                    break;
                case Exempt:
                    result = "Exempt";
                    break;
                default:
                    break;
            }

            return result;
        }
    }
}
