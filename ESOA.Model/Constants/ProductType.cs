using ESOA.Model;
using System.Collections.Generic;

namespace ESOA.Model.Constant
{
    public static class ProductType
    {
        public const string ALL = "ALL";
        public const string IPP = "IPP";
        public const string PP_SC = "PP/SC"; 
        public const string RTA = "RTA"; 
        public const string SNS = "SNS"; 
        public const string IPPX = "IPPX"; 


        public static List<NameValuePair> List()
        {
            List<NameValuePair> result = new List<NameValuePair>();

            result.Add(new NameValuePair() { Value = Translate(ALL), Name = ALL });
            result.Add(new NameValuePair() { Value = Translate(IPP), Name = IPP });
            result.Add(new NameValuePair() { Value = Translate(PP_SC), Name = PP_SC });
            result.Add(new NameValuePair() { Value = Translate(RTA), Name = RTA });
            result.Add(new NameValuePair() { Value = Translate(SNS), Name = SNS });
            result.Add(new NameValuePair() { Value = Translate(IPPX), Name = IPPX });

            return result;
        }

        public static string Translate(string key)
        {
            if (string.IsNullOrEmpty(key)) return string.Empty;

            string result = key;
            switch (key)
            {
                case ALL:
                    result = "ALL";
                    break;
                case IPP:
                    result = "IPP";
                    break;
                case PP_SC:
                    result = "PP/SC";
                    break;
                case RTA:
                    result = "RTA";
                    break;
                case SNS:
                    result = "SNS";
                    break;
                case IPPX:
                    result = "IPPX";
                    break;
                default:
                    break;
            }

            return result;
        }
    }
}
