using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetinCore.Interfaces;

namespace TwitterFeedLogger
{
    public class TweetItem
    {
        public ObjectId _id { get; set; }
        public string User { get; set; }
        public string Text { get; set; }
        public DateTime CreationDate { get; set; }
        public string Language { get; set; }
        public int Followers { get; set; }
        public int Friends { get; set; }

        public TweetItem() { }

        public TweetItem(ITweet tweet)
        {
            Text = tweet.Text;
            CreationDate = tweet.CreatedAt;
            User = tweet.Creator.ScreenName;
            if (tweet.Creator != null)
            {
                Language = tweet.Creator.Lang;
                if (tweet.Creator.FollowersCount.HasValue)
                {
                    Followers = tweet.Creator.FollowersCount.Value;
                }

                if (tweet.Creator.FriendsCount.HasValue)
                {
                    Friends = tweet.Creator.FriendsCount.Value;
                }
            }
        }

        public override string ToString()
        {
            return User + ": " + Text;
        }
    }
}
