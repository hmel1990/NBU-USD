using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NBU_USD
{
    internal class Dollar
    {
        public string Exchangedate { get; set; }
        public double Rate { get; set; }

        public Dollar(string exchangedate, double rate) 
        {
            Exchangedate = exchangedate;
            Rate = rate;
        }
        public override string ToString()
        {
            return $"{Exchangedate} - {Rate}";
        }

    }
}
