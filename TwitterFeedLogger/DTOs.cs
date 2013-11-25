using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterFeedLogger
{
    public class LanguageSummary
    {
        public string Language { get; set; }
        public long Count { get; set; }

        public override string ToString()
        {
            string lang = string.Empty;
            try
            {
                lang = CultureInfo.GetCultureInfo(Language).EnglishName;
            }
            catch (Exception)
            { }

            return Language + ": " + Count + ". - " + lang;
        }
    }


    public class MostPopularTweet
    {
        public long Followers { get; set; }
        public string User { get; set; }
        public string Text { get; set; }
        public string Language { get; set; }
        public long Friends { get; set; }

        public override string ToString()
        {
            return User + "(" + Followers + "/" + Friends + "): " + Text;
        }
    }


}
