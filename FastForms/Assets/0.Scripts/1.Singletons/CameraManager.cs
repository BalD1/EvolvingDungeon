using UnityEngine;
using StdNounou;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] private CameraController currentController;
    [SerializeField] private Camera mainCam;
    [SerializeField] private Camera currentCam;

    public static Camera CurrentCam { get => Instance.currentCam; }

    protected override void EventsSubscriber()
    {
        RoomEvents.OnRoomLoaded += OnRoomLoaded;
    }
    
    protected override void EventsUnSubscriber()
    {
        RoomEvents.OnRoomLoaded -= OnRoomLoaded;
    }

    protected override void Awake()
    {
        base.Awake();

        if (mainCam == null) mainCam = Camera.main;
        if (currentCam == null) currentCam = mainCam;
    }

    private void OnRoomLoaded(Room room)
    {
        currentController = room.RoomCamera;
        currentCam = currentController.Camera;
        currentController.Init();
        mainCam.gameObject.SetActive(false);
    }
}