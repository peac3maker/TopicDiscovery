using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Streaminvi;
using TweetinCore.Interfaces.TwitterToken;
using TwitterToken;

namespace TwitterFeedLogger
{
    class TwitterStream : IObservable<TweetItem>
    {
        public TwitterStream()
        {
            myObservers = new List<IObserver<TweetItem>>();
            Task.Factory.StartNew(Start);
        }

        private List<IObserver<TweetItem>> myObservers;

        public IDisposable Subscribe(IObserver<TweetItem> observer)
        {
            if (!myObservers.Contains(observer))
            {
                myObservers.Add(observer);
            }
            return new Unsubscriber(observer, myObservers);
        }

        private void Start()
        {
            string userKey = "";
            string userSecret = "";
            string consumerKey = ";
            string consumerSecret = "";

            IToken token = new Token(userKey, userSecret,
                consumerKey, consumerSecret);
            SimpleStream stream = new
                SimpleStream("https://stream.twitter.com/1.1/statuses/sample.json");
            stream.StartStream(token, tweet => OnNewTweet(tweet));
        }

        private void OnNewTweet(TweetinCore.Interfaces.ITweet tweet)
        {
            TweetItem ti = new TweetItem(tweet);
            foreach (IObserver<TweetItem> observer in myObservers)
            {
                if (observer != null)
                {
                    observer.OnNext(ti);
                }
            }
        }
    }
}
