using System.Data;

namespace KAI.FSA;

public class PushdownFSAImpl(string name): FSAImpl(name), PushdownFSA
{
    private Stack<State> stateStack = new Stack<State>();
    public void push_state(State state)
    {
        stateStack.Push(state);
    }

    public State pop_state(State currentState)
    {
        return stateStack.Pop();
    }
    
    public override State MakeNewState(string name=null){
        return MakeNewState<StateImpl> (name);
    }
}
