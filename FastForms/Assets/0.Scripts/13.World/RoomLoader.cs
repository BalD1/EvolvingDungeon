using StdNounou;
using System;
using UnityEngine;

public class RoomLoader : MonoBehaviourEventsHandler, IFadeCaller
{
    [SerializeField] private Room roomToLoadPF;
    [SerializeField] private SO_RoomsHandler roomHolder;
    [SerializeField] private bool fadeScreen = true;

    public event Action OnLoadEnded;

    private void Reset()
    {
        roomHolder = ResourcesObjectLoader.GetRoomshandlersHolder().GetAsset("MainRoomsHandler") as SO_RoomsHandler;
    }

    protected override void EventsSubscriber()
    {
        RoomEvents.OnRoomLoaded += OnRoomLoaded;
    }

    protected override void EventsUnSubscriber()
    {
        RoomEvents.OnRoomLoaded -= OnRoomLoaded;
    }

    public void OnRoomLoaded(Room room)
    {
        if (room.ID != roomToLoadPF.ID) return;
        if (fadeScreen) this.AskShowScreen();
        this.OnLoadEnded?.Invoke();
    }

    public void LoadRoom()
    {
        if (fadeScreen) this.AskHideScreen();
        else OnHideScreenEnded();
    }

    public void OnHideScreenEnded()
    {
        StartCoroutine(roomHolder.LoadRoom(roomToLoadPF, this));
    }

    public void OnShowScreenEnded()
    {
    }
}
