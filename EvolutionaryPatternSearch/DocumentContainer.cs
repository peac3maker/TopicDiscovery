using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TwitterFeedLogger;

namespace EvolutionaryPatternSearch
{
    public class DocumentContainer
    {
        private List<Document> documents = new List<Document>();

        List<Tuple<Document, Topic, Word>> wordValues = new List<Tuple<Document, Topic, Word>>();

        public List<Tuple<Document, Topic, Word>> WordValues
        {
            get { return wordValues; }
            set { wordValues = value; }
        }

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
            Random rand = new Random((int)DateTime.Now.Ticks);
            this.Topics = topics;
            foreach (FileInfo fi in dir.GetFiles())
            {
                Document doc = new Document(fi.Name);
                foreach (Word word in GetWords(fi, rand))
                {
                    int topicId = rand.Next(0, topics.Count);
                    Topic topic = topics[topicId];
                    wordValues.Add(new Tuple<Document, Topic, Word>(doc, topic, word));
                }
            }
        }

        public List<Word> GetWords(FileInfo file, Random rand)
        {
            List<Word> words = new List<Word>();
            using (TextReader rd = file.OpenText())
            {
                string line;
                while ((line = rd.ReadLine()) != null)
                {
                    foreach (string wordFound in GetWords(line))
                    {
                        if (StopWords.stopWords.Contains(wordFound))
                        {
                            continue;
                        }
                        //random topic                        
                        words.Add(new Word(wordFound));
                    }
                }
            }
            return words;
        }

        public List<Word> GetWords(TweetItem tweet, Random rand)
        {
            List<Word> words = new List<Word>();
            string line = tweet.Text;
            foreach (string wordFound in GetWords(line))
            {
                if (StopWords.stopWords.Contains(wordFound))
                {
                    continue;
                }
                //random topic                        
                words.Add(new Word(wordFound));
            }
            return words;
        }

        static string[] GetWords(string input)
        {
            MatchCollection matches = Regex.Matches(input, @"\b[\w{3,}']*\b");
            var words = from m in matches.Cast<Match>()
                        where !string.IsNullOrEmpty(m.Value)
                        select TrimSuffix(m.Value).Trim().ToLower();
            return words.ToArray();
        }

        static string TrimSuffix(string word)
        {
            int apostropheLoc = word.IndexOf('\'');
            if (apostropheLoc != -1)
            {
                word = word.Substring(0, apostropheLoc);
            }
            return word;
        }       

        public DocumentContainer(List<TweetItem> tweets, List<Topic> topics)
        {
            Random rand = new Random((int)DateTime.Now.Ticks);
            this.Topics = topics;
            for (int i = 0; i < tweets.Count; i++)
            {
                Document doc = new Document(i.ToString());
                foreach (Word word in GetWords(tweets[i], rand))
                {
                    int topicId = rand.Next(0, topics.Count);
                    Topic topic = topics[topicId];
                    wordValues.Add(new Tuple<Document, Topic, Word>(doc, topic, word));
                }
            }
        }

        public double PerformReturn = 0.0;

        public void Perform(Random rand)
        {                        
            for (int i = 0; i<wordValues.Count; i++)
            {                    
                Tuple<Document,Topic,Word> wordValue  =  wordValues[i];
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
                        //int WinTopic = doc.Words.Count(w => w.Topic == topic);
                        int WinTopic = wordValues.Count(w => w.Item2 == topic && w.Item1 == wordValue.Item1);
                        double propWordinDoc = (double)WinTopic / wordValues.Count(w=>w.Item1 == wordValue.Item1);
                        if (topic.WordsInTopic == -1)
                            topic.WordsInTopic = wordValues.Count(w => w.Item2 == wordValue.Item2);
                        int timeWordAssignedTopic = wordValues.Count(w => w.Item2 == wordValue.Item2 && w.Item3.Name.Equals(wordValue.Item3.Name));
                        double propWordTopic = (double)timeWordAssignedTopic / topic.WordsInTopic;
                        //double propWordTopic = (double)GetTimesWordsAssignedTopic(word, topic) / topic.WordsInTopic; 
                        double res = propWordinDoc * propWordTopic;
                        results.AddOrUpdate(topic, res, (key,oldValue) => res);
                    });
                    Topic newTopic = GetNewTopic(results.ToDictionary(kvp=>kvp.Key, kvp=>kvp.Value));
                    wordValue.Item2.WordsInTopic--;
                    wordValue = new Tuple<Document,Topic,Word>(wordValue.Item1,newTopic,wordValue.Item3);
                    newTopic.WordsInTopic++;
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
            Random rand = new Random((int)DateTime.Now.Ticks);
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

    }
}
