using System;
using UnityEngine;
using StdNounou;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] private CameraController mainCamController;

    private PlayerCharacter currentPlayer;

    protected override void EventsSubscriber()
    {
        PlayerCharacterEvents.OnPlayerCreated += OnPlayerCreated;
    }
    
    protected override void EventsUnSubscriber()
    {
        PlayerCharacterEvents.OnPlayerCreated -= OnPlayerCreated;
    }

    protected override void Awake()
    {
        base.Awake();
        if (mainCamController == null)
        {
            GameObject controllerObj = ResourcesObjectLoader.GetWorldPrefabs().GetAsset("CameraController").Create();
            mainCamController = controllerObj.GetComponent<CameraController>();
            mainCamController.CineCamera.MoveToTopOfPrioritySubqueue();
        }
    }

    private void OnPlayerCreated(PlayerCharacter player)
    {
        currentPlayer = player;
        mainCamController.StartFollowing(currentPlayer.transform);
    }
}