using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFIKLabi
{
    class RegResult
    {

        public string result { get; set; }
        public string resultcontent { get; set; }
        public int start { get; set; }
        public int stringNum { get; set; }

        public RegResult(string res, string rezcontent, int st, int num)
        {
            result = res;
            resultcontent = rezcontent;
            start = st;
            stringNum = num;
        }
    }
}
