using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionaryPatternSearch
{
    public class Topic
    {
        public string name;
        public Topic(string name)
        {
            this.name = name;
            WordsInTopic = -1;
        }

        public int WordsInTopic { get; set; }        
    }
}
