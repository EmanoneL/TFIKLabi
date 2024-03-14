using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFIKLabi
{
    class RegResult
    {
        string result;
        int start;
        int stringNum;

        public RegResult(string res, int st, int num)
        {
            result = res;
            start = st;
            stringNum = num;
        }
    }
}
