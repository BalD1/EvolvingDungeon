using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State_Player_Base : IState
{
    protected FSM_Player ownerFSM;

    public State_Player_Base(FSM_Player ownerFSM)
    {
        this.ownerFSM = ownerFSM;
    }

    public virtual void EnterState()
        => EventsSubscriber();

    public abstract void Update();
    public abstract void FixedUpdate();

    public virtual void ExitState()
        => EventsUnSubscriber();

    public abstract void Conditions();

    public abstract void EventsSubscriber();
    public abstract void EventsUnSubscriber();
}
