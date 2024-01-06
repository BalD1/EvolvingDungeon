
using System;

public abstract class State_IdleBlob_Base : IState, IDisposable
{
    protected FSM_IdleBlob ownerFSM;

    public State_IdleBlob_Base(FSM_IdleBlob ownerFSM)
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
