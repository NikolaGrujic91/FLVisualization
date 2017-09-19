using System;
using System.Threading.Tasks;

namespace FLVisualization.Populate
{
    class Program
    {
        private const string allDataURL = "https://fantasy.premierleague.com/drf/bootstrap-static";
        private const string playerDataURL = "https://fantasy.premierleague.com/drf/element-summary/";

        static void Main(string[] args)
        {
            DataParser dataParser = new DataParser(allDataURL, playerDataURL);
            Task T = new Task(dataParser.Run);
            T.Start();
            Console.ReadLine();
        }
    }
}
