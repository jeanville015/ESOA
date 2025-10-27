using ESOA.Model;
using System.Collections.Generic;

namespace ESOA.Model.Constant
{
    public static class Role
    {
        public const string Verifier = "Verifier";
        public const string Reviewer = "Reviewer";
        public const string Admin = "Admin";
        public const string Viewer = "Viewer";
        public const string Superuser = "Superuser";


        public static List<NameValuePair> List()
        {
            List<NameValuePair> result = new List<NameValuePair>();

            result.Add(new NameValuePair() { Value = Translate(Verifier), Name = Verifier });
            result.Add(new NameValuePair() { Value = Translate(Reviewer), Name = Reviewer });
            result.Add(new NameValuePair() { Value = Translate(Admin), Name = Admin });
            result.Add(new NameValuePair() { Value = Translate(Viewer), Name = Viewer });
            result.Add(new NameValuePair() { Value = Translate(Superuser), Name = Superuser });

            return result;
        }

        public static string Translate(string key)
        {
            if (string.IsNullOrEmpty(key)) return string.Empty;

            string result = key;
            switch (key)
            {
                case Verifier:
                    result = "Verifier";
                    break;
                case Reviewer:
                    result = "Reviewer";
                    break;
                case Admin:
                    result = "Admin";
                    break;
                case Viewer:
                    result = "Viewer";
                    break;
                case Superuser:
                    result = "Superuser";
                    break;
                default:
                    break;
            }

            return result;
        }
    }
}
