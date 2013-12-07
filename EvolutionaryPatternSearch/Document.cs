using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TwitterFeedLogger;

namespace EvolutionaryPatternSearch
{
    public class Document
    {       

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public Document(string name)
        {
            this.Name = name;
        }

    }
}
