using System;
using System.Collections.Generic;

/// <summary> State Factory
/// The State Factory that handles holding the states - helps to link the Machine and each Base State
/// Handles the heavu lifting for anything regarding states
/// Adapted from iHeartGameDev: https://www.youtube.com/watch?v=kV06GiJgFhc 
/// </summary>
public class StateFactory 
{
    #region  Variables
    StateMachine _context;  /// The state machine
    private Dictionary<string, BaseState> _statesDict = new Dictionary<string, BaseState>(); /// Dictionary that holds all the states
    private List<BaseState> _stateList = new List<BaseState>();

    // Getters and Setters
    public Dictionary<string, BaseState> StatesDict { get { return _statesDict; }}
    public List<BaseState> StatesList { get { return _stateList; }}
    #endregion

    #region  State Factory Methods
    public StateFactory(StateMachine currentContext){
        _context = currentContext;
    }

    /// <summary> State Factory- AddState   
    /// Simple method to add state to dictionary.
    /// This is called in InitialiseSubStates
    /// </summary>
    public void AddState(string name, BaseState state)
    {
        _statesDict.Add(name, state);
        _stateList.Add(state);
    }

    /// <summary> State Factory- GetState   
    /// Simple method to recieve state from the factory
    /// Normally called when wanting to switch state i.e SwitchState(Factory.GetState("Whatever"))
    /// </summary>
    public BaseState GetState(string name)
    {
        BaseState stateToReturn = _statesDict[name];

        if(stateToReturn != null) return stateToReturn;
        throw new NullReferenceException($"The State {name} does not exist");
    }
    #endregion
}
