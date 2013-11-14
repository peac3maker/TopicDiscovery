using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EvolutionaryPatternSearch
{
    public class Document
    {
        private List<Word> words = new List<Word>();

        public List<Word> Words
        {
            get { return words; }
            set { words = value; }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private List<Topic> topics = new List<Topic>();

        public List<Topic> Topics
        {
            get { return topics; }
            set { topics = value; }
        }

        public Document(FileInfo file, List<Topic> topics)
        {
            Topics = topics;
            Random rand = new Random();
            this.Name = file.Name;
            using (TextReader rd = file.OpenText())
            {
                string line;
                while((line = rd.ReadLine()) != null){
                    foreach (string wordFound in GetWords(line))
                    {
                        if(StopWords.stopWords.Contains(wordFound)){
                            continue;
                        }                       
                            //random topic
                            int topicId = rand.Next(0, topics.Count);
                            words.Add(new Word(wordFound,1,0.0,topics[topicId]));
                    }
                }
            }
        }

        static string[] GetWords(string input)
        {
            MatchCollection matches = Regex.Matches(input, @"\b[\w']*\b");
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
    }
}
