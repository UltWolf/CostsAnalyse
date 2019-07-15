using System;

namespace CostsAnalyse.Models.Data
{
    [Serializable]
    public class ParseState
    { 
        private int index;
        public Store Type { get; set; }

        public ParseState(int index, Store type)
        {
            this.index = index;
            this.Type = type;
        }
    }
}