using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EeVeeCee1._0
{
    public class DataWrapper
    {
        public string location;
        public decimal radius;
        public string level;
        public int limit;
        public DataWrapper(string l, decimal r, string le, int lim)
        {
            location = l;
            radius = r;
            level = le;
            limit = lim;
        }
    }
}
