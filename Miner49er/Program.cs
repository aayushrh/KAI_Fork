using System;

namespace Miner49er
{
    class MainClass
    {
        public static void Main(string[] args) {
            //Set up the variables 
            Miner miner = new SimpleMiner();
            int secsPerTick = 1;
            int gameLengthInTics = 60;
            // run the mineM9er• loop 
            for (int tick = 0; tick < gameLengthInTics; tick++)
            {
                Console.WriteLine("Tick # " + tick);
                miner.DoEvent("tick");
                miner.printStatus();
                Console.WriteLine();
                Console.WriteLine("");
                System.Threading.Thread.Sleep(secsPerTick * 1000);
            }

            Console.WriteLine("Ending Wealth: " + miner.getCurrentWealth());
        }
    }
}