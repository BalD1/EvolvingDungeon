
using System;

public abstract class State_Turret_Base : IState, IDisposable
{
    protected FSM_Turret ownerFSM;

    public State_Turret_Base(FSM_Turret ownerFSM)
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

    public virtual void Dispose() { }
}
