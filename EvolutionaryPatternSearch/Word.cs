using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionaryPatternSearch
{
    public class Word
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private int count;

        public int Count
        {
            get { return count; }
            set { count = value; }
        }
        private double probability;

        public double Probability
        {
            get { return probability; }
            set { probability = value; }
        }

        private Topic topic;

        public Topic Topic
        {
            get { return topic; }
            set { topic = value; }
        }


        public Word(string name, int count, double probability, Topic topic)
        {
            this.Name = name;
            this.Count = count;
            this.Probability = probability;
            this.Topic = topic;
        }

        public Word(string name)
        {
            this.Name = name;
        }
        
    }
}
