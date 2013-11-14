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

namespace EvolutionaryPatternSearch
{
    public partial class Form1 : Form
    {
        DocumentContainer cont;
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fdiag = new FolderBrowserDialog();
            fdiag.SelectedPath = @"C:\Users\peacemaker\C# Projects\EvolutionaryPatternSearch\EvolutionaryPatternSearch\Documents";
            DialogResult res = fdiag.ShowDialog();
            if (res != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }
            DirectoryInfo dir = new DirectoryInfo(fdiag.SelectedPath);
            List<Topic> topics = new List<Topic>();
            int countTopics = Convert.ToInt32(textBox1.Text);
            for (int i = 0; i < countTopics; i++)
            {
                topics.Add(new Topic(i.ToString()));
            }
            cont = new DocumentContainer(dir, topics);
            Random rand = new Random((int)DateTime.Now.Ticks);
            List<Word> result;
            for (int i = 0; i < 100; i++)
            {
                cont.Perform(rand);
            }

            StringBuilder sbTopic = new StringBuilder();
            string topicname = string.Empty;
            int highestamount = 0;
            foreach (Topic topic in cont.Topics)
            {
                sbTopic.Append(topic.name + ":");                
                
                foreach (Document document in cont.Documents)
                {
                    foreach (string word in document.Words.Where(w => w.Topic == topic).OrderBy(t => t.Name).Select(w =>w.Name).Distinct())
                    {                        
                        int amountword =  document.Words.Count(w => w.Name == word);
                        if(amountword > highestamount){
                            highestamount = amountword;
                            topicname = word;
                        }
                        sbTopic.Append(word + " (" + amountword + ") , ");
                    }
                }
                sbTopic.AppendLine();
                sbTopic.AppendLine();
                sbTopic.Append(topicname);
                sbTopic.AppendLine();
            }

            foreach (Document doc in cont.Documents)
            {
                sbTopic.AppendLine();
                sbTopic.Append(doc.Name + ": (");
                int wordsindoc = doc.Words.Count;                
                foreach (Topic t in cont.Topics)
                {
                    int wordsintopic = doc.Words.Count(w => w.Topic == t);
                    sbTopic.Append(wordsintopic * 100 / wordsindoc  + ",");                    
                }
                sbTopic.Append(")");
                sbTopic.AppendLine();
            }
            tbRes.Text = sbTopic.ToString();
            //double bestReturn = 100000;
            //for (int i = 0; i < 50; i++)
            //{
            //    List<Word> listres = cont.Perform(rand);
            //    if (bestReturn > cont.PerformReturn)
            //    {
            //        bestReturn = cont.PerformReturn;
            //        result = listres;
            //    }
            //} 
            //for (int j = 0; j < 50; j++)
            //{
            //    for (int i = 0; i < 50; i++)
            //    {
            //        List<Word> listres = cont.Perform(rand);
            //        if (bestReturn > cont.PerformReturn)
            //        {
            //            bestReturn = cont.PerformReturn;
            //            result = listres;
            //        }
            //    } 
               

            //}
            //cont.CreateTopics();
        }
    }
}
