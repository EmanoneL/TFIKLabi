using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFIKLabi
{
    public class Leksem
    {
        public string Type;
        public string Text;
        public int Start;
        public int End;
        public int StrNum;
        public Leksem(string type, string text, int nach, int konets, int strNum)
        {
            this.Type = type;
            this.Text = text;
            this.Start = nach;
            this.End = konets;
            this.StrNum = strNum;
        }
    }
}

