using System;

namespace Miner49er
{
    class MainClass
    {
        public static void Main(string[] args) {
            //Set up the variables 
            Miner miner = new OptimizedMiner();
            int secsPerTick = 1;
            Random myRandom = new Random(Environment.TickCount);
            int gameLengthInTics = 200;//(int)(myRandom.NextSingle() * 200) + 100;
            // run the mineM9er• loop 
            for (int tick = 0; tick < gameLengthInTics; tick++)
            {
                miner.DoEvent("tick");
                /*Console.WriteLine("Tick # " + tick);
                miner.printStatus();
                Console.WriteLine();
                Console.WriteLine("");*/
                //System.Threading.Thread.Sleep(secsPerTick * 1000);
            }

            Console.WriteLine("Ending Wealth: " + miner.getCurrentWealth());
        }
    }
}