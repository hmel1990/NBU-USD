using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBU_USD
{
    internal class Program
    {
         static void Main (string[] args) 
        {
            DollarList d = new DollarList();
            int x = d.FindRate();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nНужный курс доллара: \n");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(d.ListOfDollars[x]);
            Console.ResetColor();
        }
    }
}
