using System;

public static class RoomEvents
{
	public static event Action<Room> OnRoomLoaded;
	public static void RoomLoaded(this Room room)
		=> OnRoomLoaded?.Invoke(room);

    public static event Action<Room> OnRoomUnloaded;
    public static void RoomUnloaded(this Room room)
        => OnRoomUnloaded?.Invoke(room);
}