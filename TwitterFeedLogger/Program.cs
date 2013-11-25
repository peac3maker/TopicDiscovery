using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ComplexEventProcessing;
using Microsoft.ComplexEventProcessing.Linq;
using MongoDB;
using MongoDB.Driver;


namespace TwitterFeedLogger
{
    /// <summary>
    /// This sample StreamInsight application belongs to the StreamInsight blog
    /// post series that I published on www.12qw.ch.
    /// 
    /// Have fun and be creative!
    /// 
    /// 3.10.2013 Manuel Meyer
    /// </summary>
    class Program
    {
        private static MongoClient mongoClient;
        private static MongoServer mongoServer;
        static void Main(string[] args)
        {
            var connectionString = "mongodb://10.0.0.17/test";

            mongoClient = new MongoClient(connectionString);
            mongoServer = mongoClient.GetServer();            
            
            using (Server server = Server.Create("Luca"))
            {
                Application app = server.CreateApplication("TwitterAnalyzer");

                //Define the data source
                var twitterStreamable = app.DefineObservable(() =>
                    new TwitterStream()).ToPointStreamable(
                    tweet => PointEvent<TweetItem>.
                        CreateInsert(tweet.CreationDate, tweet),
                    AdvanceTimeSettings.IncreasingStartTime);


                ////**** Example 1: Pass-through
                var consoleObserver = app.DefineObserver(() =>
                    Observer.Create<PointEvent<TweetItem>>(ConsoleWritePointNoCTI));

                //var binding = twitterStreamable.Bind(consoleObserver);



                ////**** Example 2: Tweets per second
                //var consoleObserver = app.DefineObserver(() =>
                //    Observer.Create<PointEvent<long>>(ConsoleWritePointNoCTI));

                ////Number of Tweets per second
                //var query = from window in twitterStreamable.
                //                TumblingWindow(TimeSpan.FromSeconds(1))
                //            select window.Count();

                //var binding = query.Bind(consoleObserver);



                ////**** Example 3: Tweets per second grouped by language
                //var consoleObserver = app.DefineObserver(() =>
                //    Observer.Create<PointEvent<LanguageSummary>>(ConsoleWritePointNoCTI));

                ////Tweets per second grouped by language
                //var query = from t in twitterStreamable
                //            group t by t.Language into perLanguage
                //            from window in perLanguage.
                //                TumblingWindow(TimeSpan.FromSeconds(5))
                //            select new LanguageSummary
                //            {
                //                Language = perLanguage.Key,
                //                Count = window.Count(),
                //            };

                //var binding = query.Bind(consoleObserver);

                //Tweets of language
                var query = from t in twitterStreamable
                            where t.Language.Equals("en")
                            select t;

                var binding = query.Bind(consoleObserver);


                ////Example 4: Top 5 Languages with most Tweets every 5 seconds
                //var consoleObserver = app.DefineObserver(() =>
                //    Observer.Create<PointEvent<LanguageSummary>>(ConsoleWritePointNoCTI));

                ////Tweets per second grouped by language
                //var query = from t in twitterStreamable
                //            group t by t.Language into perLanguage
                //            from window in perLanguage.
                //                TumblingWindow(TimeSpan.FromSeconds(5))
                //            select new LanguageSummary
                //            {
                //                Language = perLanguage.Key,
                //                Count = window.Count(),
                //            };
                
                //var query2 = from win in query.SnapshotWindow()
                //             from e in
                //                 (from e in win
                //                  orderby e.Count descending
                //                  select e).Take(5)
                //             select e;

                //var binding = query2.Bind(consoleObserver);
                
                
                
                

                ////**** Example 5: Person with most Followers every 3 seconds
                //var consoleObserver = app.DefineObserver(() =>
                //    Observer.Create<PointEvent<MostPopularTweet>>(ConsoleWritePointNoCTI));

                //var mostFollowers = from win in twitterStreamable.
                //                        TumblingWindow(TimeSpan.FromSeconds(3))
                //                    from e in
                //                        (from e in win
                //                         orderby e.Followers descending
                //                         select e).Take(1)
                //                    select new MostPopularTweet
                //                    {
                //                        Followers = e.Followers,
                //                        Friends = e.Friends,
                //                        Text = e.Text,
                //                        User = e.User,
                //                        Language = e.Language
                //                    };

                //var binding = mostFollowers.Bind(consoleObserver);


                ////**** Example 6: Person with most followers AND most friends in 3 second window
                //var consoleObserver = app.DefineObserver(() =>
                //    Observer.Create<PointEvent<MostPopularTweet>>(ConsoleWritePointNoCTI));

                //var mostFollowers = from win in twitterStreamable.
                //                        TumblingWindow(TimeSpan.FromSeconds(3))
                //                    from e in
                //                        (from e in win
                //                         orderby e.Followers descending
                //                         select e).Take(1)
                //                    select new MostPopularTweet
                //                    {
                //                        Followers = e.Followers,
                //                        Friends = e.Friends,
                //                        Text = e.Text,
                //                        User = e.User,
                //                        Language = e.Language
                //                    };
                
                //var mostFriends = from win in twitterStreamable.
                //                     TumblingWindow(TimeSpan.FromSeconds(3))
                //                  from e in
                //                      (from e in win
                //                       orderby e.Friends descending
                //                       select e).Take(1)
                //                  select new MostPopularTweet
                //                  {
                //                      Followers = e.Followers,
                //                      Friends = e.Friends,
                //                      Text = e.Text,
                //                      User = e.User,
                //                      Language = e.Language
                //                  };

                //var mostPopGuyOnTwitter = from e1 in mostFollowers
                //                          join e2 in mostFriends
                //                          on e1.User equals e2.User
                //                          select e1;

                //var binding = mostPopGuyOnTwitter.Bind(consoleObserver);

                using (binding.Run())
                {
                    Console.ReadLine();
                }
            }
        }

        public static void ConsoleWritePointNoCTI<T>(PointEvent<T> e)
        {
            // Exclude EventKind.Cti
            if (e.EventKind == EventKind.Insert)
            {
                //Console.WriteLine("INSERT <{0}> {1}",
                //    e.StartTime.DateTime, e.Payload.ToString());      
                MongoDatabase db = mongoServer.GetDatabase("test");
                var collection = db.GetCollection<TweetItem>("TweetItems");
                collection.Insert(e.Payload);                
            }
        }
    }
}
