using System;
using  System.Collections.Generic;
namespace KAI.FSA
{
	/// <summary>
	/// This cAlass implements a fintie state machien that matches the FSA interface.
	/// It is intended that this class be sub-classed by variosu kinds of machines to
	/// suit their own needs
	/// </summary>
	public class FSAImpl : FSA
	{
		private List<State> stateList =
			new List<State>();
		public State currentState;
		public Stack<State> stateStack = new Stack<State>();
		private string name;
		private Boolean traceStates=false;

		public FSAImpl (string name)
		{
			this.name=name;
			
		}
		
		/// This call trigger's the first transition in the current state whose
		/// event is equal to evt (case sensative) and whose conditions all resolve to true.
		/// </summary>
		/// <param name="evt">
		///  The event to process. <see cref="String"/>
		/// </param>
		/// <returns>
		/// The transition that fire or null if no transition fired
		/// </returns>
		public virtual Transition DoEvent(String evt){
			if (currentState!=null){
				return currentState.doEvent(this,evt);	
			}
			return null;
		}
			
		public virtual State MakeNewState(string name=null){
			return MakeNewState<StateImpl> (name);
		}

		public T MakeNewState<T>(string name=null) where T : State{
			T newState =  (T)Activator.CreateInstance(typeof(T), new object[] { this, name});
			stateList.Add(newState);
			return newState;
		}
		
		public void SetCurrentState(State state){
			if (traceStates){
				Console.WriteLine("FSA "+name+" set to state "+state.GetName());
			}
			currentState = state;
		}
	protected void AddToStateList(State state){
			stateList.Add(state);	
		}
		
		/// <summary>
		/// Gets the current state of this FSA
		/// </summary>
		/// <returns>
		/// the current state <see cref="State"/>
		/// </returns>
		public State GetCurrentState(){
			return currentState;	
		}
		
		/// <summary>
		/// Pushes a state ontoi this FSA's state stack
		/// </summary>
		/// <param name="state">
		/// the state to push <see cref="State"/>
		/// </param>
		public void PushState(State state){
			stateStack.Push(state);	
		}
		
		/// <summary>
		/// Pops the last pushed state and returns it
		/// </summary>
		/// <returns>
		/// the popped State or null if the stack is empty <see cref="State"/>
		/// </returns>
		public State PopState(){
			if (stateStack.Count==0){
				return null;
			}  else {
				return stateStack.Pop();	
			}
		}
		
		public string GetName(){
			return name;
		}
	}
}

