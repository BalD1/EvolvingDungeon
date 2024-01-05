
[System.Serializable]
public class State_Entity_Paused<T> : IState
            where T : System.Enum
{
    private FSM_Base<T> ownerFSM;

    public State_Entity_Paused(FSM_Base<T> ownerFSM)
    {
        this.ownerFSM = ownerFSM;
    }

    public void EventsSubscriber()
    {
    }

    public void EventsUnSubscriber()
    {
    }

    public void EnterState()
    {
        ownerFSM.ResetMotor();
    }

    public void Update()
    {
    }

    public void FixedUpdate()
    {
    }

    public void ExitState()
    {
    }

    public void Conditions()
    {
    }
}