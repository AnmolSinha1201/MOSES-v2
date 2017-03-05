using System;
using System.Text.RegularExpressions;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var visitor = new MOSES.Visitor();
            var asd = new MOSESParser.Parser();
            asd.visitor = visitor;
            asd.Test();
        }
    }
}
