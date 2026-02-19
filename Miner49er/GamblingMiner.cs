using System;
using KAI.FSA; // Use the namespace from your FSAImpl definition

namespace Miner49er
{
    /// <summary>
    /// This class implements the Miner as described by the first state transition table
    /// </summary>
    public class GamblingMiner : FSAImpl, Miner
    {
        private const int POCKET_SIZE = 5;
        private const int MAX_THIRST = 15;
        
        /// Amount of gold nuggest in the miner's pockets ...
        public int gold = 0;
        /// How thirsty the miner is ...
        public int thirst = 0;
        /// How many gold nuggets the miner has in the bank ...
        public int bank = 0;

        // The following variables are each oen of the defiend states the miner cna be in.
        State miningState;
        State drinkingState;
        State bankingState;
        State gamblingState;

        // FIXED: Added : base("SimpleMiner") to resolve the FSAImpl constructor error
        public GamblingMiner() : base("SimpleMiner")
        {
            // FIXED: Using PascalCase to match your FSAImpl.MakeNewState method
            miningState = MakeNewState("Mining");
            drinkingState = MakeNewState("Drinking");
            bankingState = MakeNewState("Banking");
            gamblingState = MakeNewState("Blackjack");

            // set mining transitions
            /*miningState.addTransition("tick",
                new ConditionDelegate[] { new ConditionDelegate(this.parched) },
                new ActionDelegate[] { }, drinkingState);*/
            
            miningState.addTransition("tick",
                new ConditionDelegate[] { new ConditionDelegate(this.pocketsFull) },
                new ActionDelegate[] { new ActionDelegate(this.incrementThirst) }, gamblingState);
            
            miningState.addTransition("tick",
                new ConditionDelegate[] { }, 
                new ActionDelegate[] { new ActionDelegate(this.incrementThirst), new ActionDelegate(this.dig) }, miningState);

            // set drinking transitions
            drinkingState.addTransition("tick",
                new ConditionDelegate[] { new ConditionDelegate(this.isThirsty) },
                new ActionDelegate[] { new ActionDelegate(this.takeDrink) }, drinkingState);

            drinkingState.addTransition("tick",
                new ConditionDelegate[] { new ConditionDelegate(this.canGamble), new ConditionDelegate(this.feelLikeGambling) },
                new ActionDelegate[] { }, gamblingState);
            
            drinkingState.addTransition("tick",
                new ConditionDelegate[] { },
                new ActionDelegate[] { new ActionDelegate(this.incrementThirst) }, miningState);
            
            //set gambling transitions
            gamblingState.addTransition("tick",
                new ConditionDelegate[] { new ConditionDelegate(this.feelLikeGambling), new ConditionDelegate(this.pocketsNotEmpty) },
                new ActionDelegate[] { new ActionDelegate(this.incrementThirst), new ActionDelegate(this.gamble) }, gamblingState);
            
            gamblingState.addTransition("tick",
                new ConditionDelegate[] { new ConditionDelegate(this.pocketsNotEmpty) },
                new ActionDelegate[] { new ActionDelegate(this.incrementThirst) }, bankingState);
            
            gamblingState.addTransition("tick",
                new ConditionDelegate[] { new ConditionDelegate(this.isThirsty) },
                new ActionDelegate[] { }, drinkingState);
            
            gamblingState.addTransition("tick",
                new ConditionDelegate[] { },
                new ActionDelegate[] { new ActionDelegate(this.incrementThirst) }, miningState);


            // set banking transitions
            bankingState.addTransition("tick",
                new ConditionDelegate[] { new ConditionDelegate(this.pocketsNotEmpty) },
                new ActionDelegate[] { new ActionDelegate(this.depositGold) }, bankingState);

            bankingState.addTransition("tick",
                new ConditionDelegate[] { new ConditionDelegate(this.isThirsty) },
                new ActionDelegate[] {  }, drinkingState);
            
            bankingState.addTransition("tick",
                new ConditionDelegate[] { },
                new ActionDelegate[] { new ActionDelegate(this.incrementThirst) }, miningState);

            // FIXED: Using PascalCase to match your FSAImpl.SetCurrentState method
            SetCurrentState(miningState);
        }

        /// <summary>
        /// This is a condition that tests to see if the miner is so thirsty that he cannot dig
        /// </summary>
        private Boolean parched(FSA fsa)
        {
            if (thirst >= MAX_THIRST)
            {
                Console.WriteLine("Too thirsty too work.");
            }
            return thirst >= MAX_THIRST;
        }
        
        /// <summary>
        /// This is a condition that tests to see if the miner is so thirsty that he cannot dig
        /// </summary>
        private Boolean isThirsty(FSA fsa)
        {
            return thirst >= MAX_THIRST-POCKET_SIZE-1;
        }

        /// <summary>
        /// An action that decrements the miner's thirst ...
        /// </summary>
        private void takeDrink(FSA fsa)
        {
            thirst -= 1;
            Console.WriteLine("Glug glug glug");
        }

        /// <summary>
        /// An action that decrements the gold in the miner's pockets and increments the gold in the bank ...
        /// </summary>
        private void depositGold(FSA fsa)
        {
            gold -= 1;
            bank += 1;
            Console.WriteLine("deposit a gold nugget");
        }

        /// <summary>
        /// This implements the Miner.getCurrentWealth() call ...
        /// </summary>
        public int getCurrentWealth()
        {
            return bank + gold;
        }

        // --- Previously extracted methods ---

        private void dig(FSA fsa)
        {
            gold++;
            Console.WriteLine("Miner is digging.");
        }
        
        private void gamble(FSA fsa)
        {
            gold--;
            Random random = new Random();
            if (random.NextSingle() < 0.5)
            {
                Console.WriteLine("Miner won gambling.");
                gold += 4;
                return;
            }
            Console.WriteLine("Miner lost gambling.");
        }

        private void incrementThirst(FSA fsa)
        {
            thirst++;
        }

        private Boolean pocketsFull(FSA fsa) => gold >= POCKET_SIZE;

        private Boolean pocketsNotEmpty(FSA fsa) => gold > 0;

        private Boolean thirsty(FSA fsa) => thirst > 0;

        private Boolean canGamble(FSA fsa) => bank > 10;

        private Boolean feelLikeGambling(FSA fsa)
        {
            Random random = new Random();
            if (random.NextSingle() < 0.9)
            {
                return true;
            }

            return false;
        }

        public void printStatus()
        {
            Console.WriteLine("Thirst: "+thirst+" Gold: "+gold+" Bank: "+bank);
        }
    }
}