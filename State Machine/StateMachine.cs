using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> StateMachine
/// State Machine class holds the infomation for the Current State, links to the factory
/// Holds some extra functionality that could be held in the Factory. Easier to reference the State Machine itself.
/// Adapted from iHeartGameDev: https://www.youtube.com/watch?v=kV06GiJgFhc 
/// </summary>
public class StateMachine 
{
    #region  State Variables
    BaseState _currentState;
    StateFactory _states;

    // Getters and Setters
    public BaseState CurrentState {get { return _currentState;} set { _currentState = value; }}
    public StateFactory States { get{ return _states;} set {_states = value; }}
    #endregion 

    
    /// <summary> StateMachine Constructor
    /// Create a State Factory on creation and set the factory to that. pass through this on the State Factory so they look at eachother.
    /// Saves the user from having to create a factory too.
    /// </summary>  
    public StateMachine(){
        _states = new StateFactory(this);
    }

    /// <summary> StateMachine- Set Starting State
    /// As Described
    /// </summary>  
    public void SetStartingState(BaseState state){
           _currentState = state;
            // state.EnterSubState();
            state.EnterStates(null);
    }

    /// <summary> StateMachine- Factory Methods - Saves us from referencing
    /// AddState - Ass Described
    /// InitialiseSubStates - Before we can have a functioning state machine each state/substate/subsubstate etc must be initialised.
    /// </summary>  
    public void AddState(string name, BaseState baseState) => _states.AddState(name, baseState);
    

    public void InitialiseSubStates(){
        foreach(BaseState state in States.StatesList){
            state.InitialiseSubState();
        }
    }

}
