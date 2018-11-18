using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyURL.Utilities
{
    public class Helper
    {
        public static string ValidateURL(string URL)
        {
            if (!URL.ToLower().StartsWith("http://") && !URL.ToLower().StartsWith("https://"))
            {
                URL = "http://" + URL;
            }

            return URL;
        }

        public static string GenerateShortURL()
        {
            StringBuilder encodedURL = new StringBuilder();
            string reference = "1234567890abcdefghijklmnopqrstuvwxyz";

            Random rnd = new Random();
            for (int i = 0; i < 7; i++)
            {
                int rand = rnd.Next(0, reference.Length - 1);
                encodedURL.Append(reference[rand]);
            }

            return encodedURL.ToString();
        }
    }
}
