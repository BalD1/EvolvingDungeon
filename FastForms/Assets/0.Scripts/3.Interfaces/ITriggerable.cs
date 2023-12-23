using System;
using UnityEngine;

public interface ITriggerable
{
    public IComponentHolder GetComponentHolder();

    public void OnTrigger(TriggerEventArgs eventArgs);

}

public class TriggerEventArgs : EventArgs
{
    public GameObject triggerer;
    public TriggerEventArgs(GameObject triggerer)
    {
        this.triggerer = triggerer;
    }
}