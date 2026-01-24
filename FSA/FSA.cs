using System;
namespace KAI.FSA
{
	/// <summary>
	/// This interface defines the publicly visible interface to a Finite State Automata
	/// Author: Jeffrey P. Kesselman
	/// </summary>
	/// 
	public interface FSA
	{
		/// <summary>
		/// This call trigger's the first transition in the current state whose
		/// event is equal to evt (case sensative) and whose conditions all resolve to true.
		/// </summary>
		/// <param name="evt">
		///  The event to process. <see cref="String"/>
		/// </param>
		/// <returns>
		/// The Transition that fired, or null if none fired
		/// </returns>
		/// 
		Transition DoEvent(String evt);
		
		/// <summary>
		/// This sets the current state of the FSA
		/// </summary>
		/// <param name="state">
		/// The current state <see cref="setCurrentState"/>
		/// </param>
		void SetCurrentState(State state);
		
		/// <summary>
		/// Creates a new state that is part of this FSA
		/// </summary>
		State MakeNewState(string name=null);

		/// <summary>
		/// Creates a new state of the passed type that is part of this FSA
		/// </summary>
		T MakeNewState<T>(string name=null) where T : State;
		
		/// <summary>
		/// Gets the current state of this FSA
		/// </summary>
		/// <returns>
		/// the current state <see cref="State"/>
		/// </returns>
		State GetCurrentState();
		
		/// <summary>
		/// Pushes a state ontoi this FSA's state stack
		/// </summary>
		/// <param name="state">
		/// the state to push <see cref="State"/>
		/// </param>
		void PushState(State state);
		
		/// <summary>
		/// Pops the last pushed state and returns it
		/// </summary>
		/// <returns>
		/// the popped State or null if the stack is empty <see cref="State"/>
		/// </returns>
		State PopState();
		
		string GetName();
	}
}

