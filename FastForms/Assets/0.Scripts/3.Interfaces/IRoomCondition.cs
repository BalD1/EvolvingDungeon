using System;

public interface IRoomCondition
{
    public void Setup();
    public void AddListener(Action<IRoomCondition> listener);
    public void RemoveListener(Action<IRoomCondition> listener);
}