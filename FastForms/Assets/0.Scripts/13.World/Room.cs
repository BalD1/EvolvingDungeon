using AYellowpaper.SerializedCollections;
using System;
using UnityEngine;

public class Room : MonoBehaviour
{
    [field: SerializeField] public string ID {  get; private set; }
    [field: SerializeField] public Transform PlayerSpawnPoint {  get; private set; }
    [field: SerializeField] public CameraController RoomCamera {  get; private set; }

    public void Activate()
    {
        this.gameObject.SetActive(true);
    }
    public void Deactivate()
    {
        this.gameObject.SetActive(false);
    }
    public bool IsActive()
        => this.gameObject.activeSelf;

    private void OnEnable()
    {
        this.RoomLoaded();
    }

    private void OnDisable()
    {
        this.RoomUnloaded();
    }
}
