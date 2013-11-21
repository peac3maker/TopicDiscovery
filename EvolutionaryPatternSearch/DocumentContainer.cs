using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterFeedLogger;

namespace EvolutionaryPatternSearch
{
    public class DocumentContainer
    {
        private List<Document> documents = new List<Document>();

        public List<Document> Documents
        {
            get { return documents; }
            set { documents = value; }
        }

        private List<Topic> topics = new List<Topic>();

        public List<Topic> Topics
        {
            get { return topics; }
            set { topics = value; }
        }


        public DocumentContainer(DirectoryInfo dir, List<Topic> topics)
        {
            this.Topics = topics;
            foreach (FileInfo fi in dir.GetFiles())
            {
                documents.Add(new Document(fi,topics));
            }
        }

        public DocumentContainer(List<TweetItem> tweets, List<Topic> topics)
        {
            this.Topics = topics;
            for (int i = 0; i < tweets.Count; i++)
            {
                documents.Add(new Document(tweets[i], topics,i));
            }
        }

        public double PerformReturn = 0.0;

        public void Perform(Random rand)
        {                        
            foreach (Document doc in Documents)
            {
                foreach (Word word in doc.Words)
                {                    
                    //Dictionary<Topic, double> results = new Dictionary<Topic, double>();
                    //foreach (Topic topic in topics)
                    //{
                    //    int WinTopic = doc.Words.Where(w => w.Topic == topic).Count();
                    //    double propWordinDoc = (double)WinTopic / doc.Words.Count;
                    //    double propWordTopic = (double)GetTimesWordsAssignedTopic(word,topic) / GetAmountOfWordsAssignedTopic(word.Topic);
                    //    double res = propWordinDoc * propWordTopic;                        
                    //    results.Add(topic,res);
                    //}
                    ConcurrentDictionary<Topic, double> results = new ConcurrentDictionary<Topic, double>();
                    Parallel.ForEach(topics, topic =>
                    {
                        int WinTopic = doc.Words.Count(w => w.Topic == topic);
                        double propWordinDoc = (double)WinTopic / doc.Words.Count;
                        double propWordTopic = (double)GetTimesWordsAssignedTopic(word, topic) / GetAmountOfWordsAssignedTopic(word.Topic);
                        double res = propWordinDoc * propWordTopic;
                        results.AddOrUpdate(topic, res, (key,oldValue) => res);
                    });
                    Topic newTopic = GetNewTopic(results.ToDictionary(kvp=>kvp.Key, kvp=>kvp.Value));
                    word.Topic = newTopic;
                }
            }                        
        }

        private Topic GetNewTopic(Dictionary<Topic, double> results)
        {
            double sum = 0.0;
            Dictionary<Topic, double> fixedRes = new Dictionary<Topic, double>();
            sum = results.Sum(r=>r.Value);            
            foreach (KeyValuePair<Topic, double> result in results)
            {
                double relation = result.Value / sum;
                if (relation != 0)
                {
                    fixedRes.Add(result.Key, relation);
                }
            }
            Random rand = new Random();
            double help = rand.NextDouble();
            double i = 0.0;
            foreach (KeyValuePair<Topic, double> res in fixedRes)
            {
                i += res.Value;
                if (help <= i)
                {
                    return res.Key;
                }
            }
            return null;
            
        }

        private int GetTimesWordsAssignedTopic(Word word, Topic topic)
        {
            int amount = 0;
            foreach (Document doc in Documents)
            {
                amount += doc.Words.Count(w => w.Topic == topic && w.Name.Equals(word.Name));
            }
            return amount;
        }

        private int GetAmountOfWordsAssignedTopic(Topic topic)
        {
            int amount = 0;
            foreach (Document doc in Documents)
            {
                amount += doc.Words.Count(w => w.Topic == topic);
            }
            return amount;
        }

    }
}
