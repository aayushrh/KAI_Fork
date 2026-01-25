namespace KAI.FSA;

public interface PushdownFSA : FSA
{
    void push_state(State state);
    State pop_state(State currentState);

   
    
}

