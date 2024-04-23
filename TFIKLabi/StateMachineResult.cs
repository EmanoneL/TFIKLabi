using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFIKLabi
{
    internal class StateMachineResult
    {
        public List<char> chars = new List<char>();
        public List<string> states = new List<string>();

        public void AddResult(char ch, State st) { 
            chars.Add(ch);
            states.Add(st.ToString());
        }

        public void AddResult(char ch, string st)
        {
            chars.Add(ch);
            states.Add(st);
        }

        public int getCount()
        {
            return chars.Count;
        }
    }
}
