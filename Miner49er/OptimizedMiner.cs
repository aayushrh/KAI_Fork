using System;
using KAI.FSA; // Use the namespace from your FSAImpl definition

namespace Miner49er
{
    /// <summary>
    /// This class implements the Miner as described by the first state transition table
    /// </summary>
    public class OptimizedMiner : FSAImpl, Miner
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

        // FIXED: Added : base("SimpleMiner") to resolve the FSAImpl constructor error
        public OptimizedMiner() : base("SimpleMiner")
        {
            // FIXED: Using PascalCase to match your FSAImpl.MakeNewState method
            miningState = MakeNewState("Mining");
            drinkingState = MakeNewState("Drinking");
            bankingState = MakeNewState("Banking");

            // set mining transitions
            /*miningState.addTransition("tick",
                new ConditionDelegate[] { new ConditionDelegate(this.parched) },
                new ActionDelegate[] { }, drinkingState);*/
            
            miningState.addTransition("tick",
                new ConditionDelegate[] { new ConditionDelegate(this.pocketsFull) },
                new ActionDelegate[] { new ActionDelegate(this.incrementThirst) }, bankingState);
            
            miningState.addTransition("tick",
                new ConditionDelegate[] { }, 
                new ActionDelegate[] { new ActionDelegate(this.incrementThirst), new ActionDelegate(this.dig) }, miningState);

            // set drinking transitions
            drinkingState.addTransition("tick",
                new ConditionDelegate[] { new ConditionDelegate(this.isThirsty) },
                new ActionDelegate[] { new ActionDelegate(this.takeDrink) }, drinkingState);
            
            drinkingState.addTransition("tick",
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

        private void incrementThirst(FSA fsa)
        {
            thirst++;
        }

        private Boolean pocketsFull(FSA fsa) => gold >= POCKET_SIZE;

        private Boolean pocketsNotEmpty(FSA fsa) => gold > 0;

        private Boolean thirsty(FSA fsa) => thirst > 0;

        public void printStatus()
        {
            Console.WriteLine("Thirst: "+thirst+" Gold: "+gold+" Bank: "+bank);
        }
    }
}