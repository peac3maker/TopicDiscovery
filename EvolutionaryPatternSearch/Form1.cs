using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TwitterFeedLogger;

namespace EvolutionaryPatternSearch
{
    public partial class Form1 : Form
    {
        DocumentContainer cont;
        int topWordsNumber = 3;
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            //FolderBrowserDialog fdiag = new FolderBrowserDialog();
            //fdiag.SelectedPath = @"C:\Users\peacemaker\C# Projects\EvolutionaryPatternSearch\EvolutionaryPatternSearch\Documents";
            //DialogResult res = fdiag.ShowDialog();
            //if (res != System.Windows.Forms.DialogResult.OK)
            //{
            //    return;
            //}            
            //DirectoryInfo dir = new DirectoryInfo(fdiag.SelectedPath);
            //List<Topic> topics = new List<Topic>();
            //int countTopics = Convert.ToInt32(textBox1.Text);
            //for (int i = 0; i < countTopics; i++)
            //{
            //    topics.Add(new Topic(i.ToString()));
            //}
            //cont = new DocumentContainer(dir, topics);
            //Random rand = new Random((int)DateTime.Now.Ticks);
            //List<Word> result;
            //for (int i = 0; i < 100; i++)
            //{
            //    cont.Perform(rand);
            //}

            //StringBuilder sbTopic = new StringBuilder();    
            
            //foreach (Topic topic in cont.Topics)
            //{
            //    topic.name = string.Empty;
            //    int highestamount = 0;
            //    Dictionary<string, int> mostUsedWords = new Dictionary<string, int>();
            //    sbTopic.Append(topic.name + ":");                
                
            //    foreach (Document document in cont.Documents)
            //    {
            //        foreach (string word in document.Words.Where(w => w.Topic == topic).OrderBy(t => t.Name).Select(w =>w.Name).Distinct())
            //        {                        
            //            int amountword =  document.Words.Count(w => w.Name == word);
            //            //if(amountword > highestamount){
            //            //    highestamount = amountword;
            //            //    topicname = word;
            //            //}
            //            if (mostUsedWords.Count < topWordsNumber)
            //            {
            //                mostUsedWords.Add(word, amountword);
            //            }
            //            else
            //            {
            //                    List<KeyValuePair<string,int>> dictWords = mostUsedWords.Where(w => w.Value < amountword).ToList();
            //                    if (dictWords.Count > 0 && !mostUsedWords.ContainsKey(word))
            //                    {
            //                        KeyValuePair<string, int> wordToDelete = dictWords.First();
            //                        mostUsedWords.Remove(wordToDelete.Key);
            //                        mostUsedWords.Add(word, amountword);
            //                    }
            //                    else if (mostUsedWords.ContainsKey(word))
            //                    {
            //                        mostUsedWords[word] += amountword;
            //                    }
            //            }
            //            sbTopic.Append(word + " (" + amountword + ") , ");
            //        }
            //    }
            //    sbTopic.AppendLine();
            //    sbTopic.AppendLine();
            //    foreach (KeyValuePair<string, int> mostusedword in mostUsedWords)
            //    {
            //        sbTopic.Append(mostusedword.Key + ",");
            //        topic.name += (mostusedword.Key + ",");
            //    }
            //    sbTopic.AppendLine();
            //}

            //foreach (Document doc in cont.Documents)
            //{
            //    sbTopic.AppendLine();
            //    sbTopic.Append(doc.Name + ": (");
            //    int wordsindoc = doc.Words.Count;                
            //    foreach (Topic t in cont.Topics)
            //    {
            //        int wordsintopic = doc.Words.Count(w => w.Topic == t);
            //        sbTopic.Append(t.name+"{"+(wordsintopic * 100 / wordsindoc)  + "},");                    
            //    }
            //    sbTopic.Append(")");                
            //    sbTopic.AppendLine();
            //}
            //tbRes.Text = sbTopic.ToString();
            ////double bestReturn = 100000;
            ////for (int i = 0; i < 50; i++)
            ////{
            ////    List<Word> listres = cont.Perform(rand);
            ////    if (bestReturn > cont.PerformReturn)
            ////    {
            ////        bestReturn = cont.PerformReturn;
            ////        result = listres;
            ////    }
            ////} 
            ////for (int j = 0; j < 50; j++)
            ////{
            ////    for (int i = 0; i < 50; i++)
            ////    {
            ////        List<Word> listres = cont.Perform(rand);
            ////        if (bestReturn > cont.PerformReturn)
            ////        {
            ////            bestReturn = cont.PerformReturn;
            ////            result = listres;
            ////        }
            ////    } 
               

            ////}
            ////cont.CreateTopics();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var connectionString = "mongodb://10.0.0.17/test";

            MongoClient mongoClient = new MongoClient(connectionString);
            MongoServer mongoServer = mongoClient.GetServer();
            MongoDatabase db = mongoServer.GetDatabase("test");
            var collection = db.GetCollection<TweetItem>("TweetItems");
            DateTime dtmQuery = dtmStart.Value;
            DateTime dtmQueryLower = dtmEnd.Value ;
            var query = Query<TweetItem>.Where(t => t.CreationDate >= dtmQuery && t.CreationDate < dtmQueryLower);
            List<TweetItem> value = collection.Find(query).ToList();
            
            List<Topic> topics = new List<Topic>();
            int countTopics = Convert.ToInt32(textBox1.Text);
            for (int i = 0; i < countTopics; i++)
            {
                topics.Add(new Topic(i.ToString()));
            }
            cont = new DocumentContainer(value, topics);
            Random rand = new Random((int)DateTime.Now.Ticks);
            List<Word> result;
            for (int i = 0; i < 50; i++)
            {
                cont.Perform(rand);
            }
            List<string> mostUsedWords = new List<string>();
            foreach (Topic topic in cont.Topics)
            {

                List<Tuple<Document,Topic,Word>> words = cont.WordValues.Where(w => w.Item2 == topic).GroupBy(w => w).OrderByDescending(w => w.Count()).Take(3).Select(w=>w.Key).ToList();
                foreach (Tuple<Document, Topic, Word> word in words)
                {
                    mostUsedWords.Add(word.Item3.Name);
                }
                
            }
            StringBuilder sbTopic = new StringBuilder();
            foreach (Tuple<Document, Topic, Word> wordValue in cont.WordValues.OrderBy(w=>w.Item2))
            {
                sbTopic.AppendLine("Document: "+wordValue.Item1.Name + " Topic:"+ wordValue.Item2.name + " Word:"+ wordValue.Item3 );
            }           

            foreach (Document doc in cont.WordValues.Select(w=>w.Item1).Distinct())
            {
                sbTopic.AppendLine();
                sbTopic.Append(doc.Name + ": (");
                int wordsindoc = cont.WordValues.Count(w=>w.Item1 == doc);
                foreach (Topic t in cont.Topics)
                {
                    int wordsintopic = cont.WordValues.Count(w=>w.Item1 == doc && w.Item2 == t);
                    if(wordsindoc != 0)
                        sbTopic.Append(t.name + "{" + (wordsintopic * 100 / wordsindoc) + "},");
                }
                sbTopic.Append(")");
                sbTopic.AppendLine();
            }
            tbRes.Text = sbTopic.ToString();

        }
    }
}
