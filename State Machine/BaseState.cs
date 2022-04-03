using System.Collections.Generic;
/// <summary> BaseState - abstract class
/// Attatch this script to each concrete state and override with state logic.
/// Base State has a reference to the state machine (_ctx) and the factory - keeps it tight anc compact.
/// Adapted from iHeartGameDev: https://www.youtube.com/watch?v=kV06GiJgFhc 
/// </summary>
public abstract class BaseState
{
    #region Member Variables
    private StateMachine _ctx;
    private StateFactory _factory;

    // Getters and Setters
    protected StateMachine Ctx { get { return _ctx; }}
    protected StateFactory Factory { get { return _factory;}}
    #endregion

    #region Base State Variables
    private bool _isRootState = false;
    private BaseState _currentSubState;
    private BaseState _currentSuperState;
    private List<BaseState> _subStates = new List<BaseState>();
    private List<BaseState> _superStates = new List<BaseState>();

    // Getters and Setters
    public bool IsRootState { get{return _isRootState; } set {_isRootState = value; }}
    public BaseState CurrentSubState { get { return _currentSubState; }}
    public BaseState CurrentSuperState { get { return _currentSuperState; } set { _currentSuperState = value;}}

    public List<BaseState> SubStates { get { return _subStates; }}
    public List<BaseState> SuperStates { get { return _superStates; }}
    #endregion
    
    public BaseState(StateMachine stateMachine, StateFactory stateFactory){
        _ctx = stateMachine;
        _factory = stateFactory;
    }

    #region  Abstract State Machine Methods
    /// <summary> Enter State
    /// Pass in the state we are entering from.
    /// Override with Enter Logic
    /// </summary>
    public abstract void EnterState(BaseState fromState);

    /// <summary> Update State
    /// Override with normal Unity Update logic
    /// </summary>
    public abstract void UpdateState();

    /// <summary> Fixed Update State
    /// Override with normal Unity FixedUpdate logic
    /// </summary>
    public abstract void UpdateFixedState();

    /// <summary> Exit State
    /// Override with logic when exiting
    /// </summary>
    public abstract void ExitState();

    /// <summary> Check Switch State
    /// Override with logic to switch to other states
    /// </summary>
    public abstract void CheckSwitchStates();

    /// <summary> Initialise Sub State
    /// Override with initialising all substates into the list - use AddSubState(BaseState) here.
    /// </summary>
    public abstract void InitialiseSubState();

    /// <summary> Enter Sub States
    /// Override with logic to see what sub state you enter (if entering into a state with sub states)
    /// Logic is usually the same as the substates Check Switch States
    /// </summary>
    public abstract void EnterSubState();   // Define the logic for entering each state
    #endregion

    #region Standard Logic for all BaseStates

    /// <summary> Update States
    /// Update States is called in the State Machine.
    /// Called on this state - if this state has any substates then it is called.
    /// Every update we want to check switch state. Call here rather than in every state script.
    /// </summary>
    public void UpdateStates(){
        UpdateState();
        if(_currentSubState != null){
            _currentSubState.UpdateStates();
        }
        CheckSwitchStates();
    }
    /// <summary> Fixed Update States
    /// Same Logic as Update without the CheckSwitchState and focusing on Fixed Update
    /// </summary>

    public void UpdateFixedStates(){
        UpdateFixedState();
        if(_currentSubState != null){
            _currentSubState.UpdateFixedStates();
        }
    }

    /// <summary> Enter States
    /// Enter States is called when entering a new state
    /// Calls Enter State for each substate...
    /// Calls Enter Sub State - make sure we enter the correct sub state (In the super state)
    /// </summary>
    public void EnterStates(BaseState fromRootState){
        EnterState(fromRootState);
        EnterSubState();
        if(_currentSubState != null){
            _currentSubState.EnterStates(fromRootState);
        }
    }

    /// <summary> SwitchState
    /// Use when you want to switch state i.e SwitchState(run, this) - this (always calling from the fromState)
    /// On Switch, call the exit state on this.
    /// Call EnterStates in the new one
    /// If it is a root state, change the state machines CurrentState(this will be the root state)
    /// Else if the Current Super State exists change the Sub State for Super State
    /// </summary>
    protected void SwitchState(BaseState newState, BaseState fromState){
        // Current state exits
        ExitState();
    
        // New state enters
        newState.EnterStates(fromState);

        // Switch state of context
        if(_isRootState){
            _ctx.CurrentState = newState;
        } else if (_currentSuperState != null){
            _currentSuperState.SetSubState(newState);
        }
    }

    /// <summary> SetSuperState
    /// As described
    /// </summary>
    protected void SetSuperState(BaseState newSuperState){
        _currentSuperState = newSuperState;
    }

    /// <summary> SetSubState
    /// As described.
    /// Also set that substates superstate to this.
    /// </summary>
    protected void SetSubState(BaseState newSubState){
        _currentSubState = newSubState;
       // newSubState.SetSuperState(this);
       _currentSubState.SetSuperState(this);
    }

    /// <summary> Add Sub State
    /// Called in Initialise SubStates
    /// Adds to the list - also calls AddSuperState on the substate
    /// </summary>
    protected void AddSubState(BaseState subState){
        _subStates.Add(subState);
        subState.AddSuperState(this);
    }
    
    /// <summary> Add Super State
    /// As described
    /// </summary>
    protected void AddSuperState(BaseState superState){
        _superStates.Add(superState);
    }
    #endregion
}
