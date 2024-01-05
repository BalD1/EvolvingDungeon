using StdNounou;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class FSM_Base<StateEnum> : MonoBehaviourEventsHandler
                where StateEnum : Enum
{
    [SerializeField] protected GameObject ownerObj;
    public IComponentHolder Owner {  get; private set; }

    [field: SerializeField, ReadOnly] public IState CurrentState { get; private set; }
    [field: SerializeField, ReadOnly] public StateEnum CurrentStateKey { get; private set; }
    [field: SerializeField] public StateEnum BaseStateKey { get; private set; }
    public Dictionary<StateEnum, IState> States { get; private set; }

    public event Action<StateEnum> OnStateChanged;

    protected State_Entity_Paused<StateEnum> pausedState;

    protected const int PAUSED_STATE_VALUE = -100;
    protected const int IDLE_STATE_VALUE = -99;

    protected override void EventsSubscriber()
    {
        ScreenFadeEvents.OnStartedHideScreen += OnStartedHideScreen;
        ScreenFadeEvents.OnEndedShowScreen += OnEndedShowScreen;
    }

    protected override void EventsUnSubscriber()
    {
        ScreenFadeEvents.OnStartedHideScreen -= OnStartedHideScreen;
        ScreenFadeEvents.OnEndedShowScreen -= OnEndedShowScreen;
    }

    protected virtual void OnStartedHideScreen()
    {
        StateEnum pausedState = EnumExtensions.FromInt<StateEnum>(PAUSED_STATE_VALUE);
        AskSwitchState(pausedState);
    }
    protected virtual void OnEndedShowScreen()
    {
        StateEnum idleState = EnumExtensions.FromInt<StateEnum>(IDLE_STATE_VALUE);
        AskSwitchState(idleState);
    }

    protected override void Awake()
    {
        Owner = ownerObj.GetComponent<IComponentHolder>();
        States = new Dictionary<StateEnum, IState>();
        SetupStates();
        base.Awake();
    }

    protected virtual void Start()
    {
        SetupComponents();
        SetToBaseState();
    }

    private void SetToBaseState()
    {
        if (!States.TryGetValue(BaseStateKey, out IState baseState) || baseState == null)
        {
            this.LogError("Could not find base state " + BaseStateKey);
            return;
        }

        CurrentState = baseState;
        CurrentStateKey = BaseStateKey;
        CurrentState.EnterState();
    }

    protected abstract void SetupComponents();
    protected virtual void SetupStates()
    {
        pausedState = new State_Entity_Paused<StateEnum>(this);
        StateEnum val = EnumExtensions.FromInt<StateEnum>(PAUSED_STATE_VALUE);
        States.Add(val, pausedState);
    }

    protected virtual void Update()
    {
        if (CurrentState == null) return;
        CurrentState.Update();
        CurrentState.Conditions();
    }

    protected virtual void FixedUpdate()
    {
        if (CurrentState == null) return;
        CurrentState.FixedUpdate();
    }

    private void PerformSwitchState(StateEnum state)
    {
        CurrentState?.ExitState();
        if (!States.TryGetValue(state, out IState newState) || newState == null)
        {
            this.LogError("Could not find state " + state);
            SetToBaseState();
            return;
        }

        CurrentState = newState;
        CurrentStateKey = state;
        CurrentState?.EnterState();
        OnStateChanged?.Invoke(state);
    }

    public void AskSwitchState(StateEnum state)
    {
        PerformSwitchState(state);
    }

    public virtual void ResetMotor()
    {
        if (Owner.HolderTryGetComponent(IComponentHolder.E_Component.Motor, out ObjectMotor motor) == IComponentHolder.E_Result.Success)
        {
            motor.SetAllVelocity(Vector2.zero);
        }
    }

    protected override void OnDestroy()
    {
        CurrentState?.ExitState();
        base.OnDestroy();
    }
}