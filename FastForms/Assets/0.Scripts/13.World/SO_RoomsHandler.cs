using StdNounou;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New RoomsHolder", menuName = "Scriptable/Rooms/Holder")]
public class SO_RoomsHandler : ScriptableObject
{
    private Room currentRoom;
    private Dictionary<string, Room> activeRooms = new Dictionary<string, Room>();

    private bool isRunning;

    public IEnumerator LoadRoom(Room roomToLoadPF, RoomLoader loader)
    {
        if (isRunning) yield return null;
        isRunning = true;

        if (currentRoom != null)
        {
            currentRoom.Deactivate();
            while (currentRoom.IsActive()) yield return null;
        }

        if (activeRooms.TryGetValue(roomToLoadPF.ID, out Room room))
        {
            room.Activate();
            currentRoom = room;
            yield break;
        }

        Room newRoom = roomToLoadPF.Create();
        activeRooms.Add(newRoom.ID, newRoom);
        currentRoom = newRoom;
    }
}