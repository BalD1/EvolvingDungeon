using StdNounou;
using UnityEngine;

public class Room : MonoBehaviour
{
    [field: SerializeField] public string ID {  get; private set; }
    [field: SerializeField] public Transform PlayerSpawnPoint {  get; private set; }
    [field: SerializeField] public CameraController RoomCamera {  get; private set; }
    [SerializeField] private DoorToNextLevel doorToNextLevel;

    [SerializeField] private bool clearRoomOnAwake;
    [SerializeField] private GameObject[] clearConditionsObjs;
    private IRoomCondition[] roomConditions;
    private int metConditions;

    private bool initedRoomConditions = false;

    [field: SerializeField] public bool IsCleared { get; private set; }

    private void Reset()
    {
        ID = this.gameObject.name;
    }

    private void Awake()
    {
        PopulateRoomConditions();
        if (clearRoomOnAwake) ClearRoom();
    }

    private void PopulateRoomConditions()
    {
        if (initedRoomConditions) return;
        initedRoomConditions = true;
        if (!clearConditionsObjs.NotNullOrEmpty()) return;

        roomConditions = new IRoomCondition[clearConditionsObjs.Length];
        for (int i = 0; i < clearConditionsObjs.Length; i++)
        {
            if (!clearConditionsObjs[i].TryGetComponent(out IRoomCondition condition))
            {
                this.LogError($"Could not find Conditions in {clearConditionsObjs[i].gameObject.name} at idx {i} ");
                continue;
            }

            roomConditions[i] = condition;
            condition.AddListener(OnConditonMet);
            condition.Setup();
        }
    }

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

    private void OnConditonMet(IRoomCondition condition)
    {
        condition.RemoveListener(OnConditonMet);
        metConditions++;
        if (metConditions >= clearConditionsObjs.Length) ClearRoom();
    }

    public void ClearRoom()
    {
        IsCleared = true;
        doorToNextLevel.Open();
    }
}