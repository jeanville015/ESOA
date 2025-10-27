using ESOA.Model;
using System.Collections.Generic;

namespace ESOA.Model.Constant
{
    public static class SFModeOfSettlement
    {
        public const string AutoCollectFromAFC = "Auto Collect From AFC";
        public const string Billed = "Billed";
        public const string DailyDeposit = "Daily Deposit";


        public static List<NameValuePair> List()
        {
            List<NameValuePair> result = new List<NameValuePair>();

            result.Add(new NameValuePair() { Value = Translate(AutoCollectFromAFC), Name = AutoCollectFromAFC });
            result.Add(new NameValuePair() { Value = Translate(Billed), Name = Billed });
            result.Add(new NameValuePair() { Value = Translate(DailyDeposit), Name = DailyDeposit });

            return result;
        }

        public static string Translate(string key)
        {
            if (string.IsNullOrEmpty(key)) return string.Empty;

            string result = key;
            switch (key)
            {
                case AutoCollectFromAFC:
                    result = "Auto Collect From AFC";
                    break;
                case Billed:
                    result = "Billed";
                    break;
                case DailyDeposit:
                    result = "DailyDeposit";
                    break;
                default:
                    break;
            }

            return result;
        }
    }
}
