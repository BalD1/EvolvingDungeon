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

    protected override void Awake()
    {
        Owner = ownerObj.GetComponent<IComponentHolder>();
        base.Awake();
    }

    protected virtual void Start()
    {
        States = new Dictionary<StateEnum, IState>();
        SetupStates();
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
    protected abstract void SetupStates();

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

    protected override void OnDestroy()
    {
        CurrentState?.ExitState();
        base.OnDestroy();
    }
}
