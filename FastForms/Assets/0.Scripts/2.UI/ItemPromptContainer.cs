using StdNounou;
using System.Collections.Generic;
using UnityEngine;

public class ItemPromptContainer : MonoBehaviourEventsHandler
{
    [SerializeField] private ItemPrompt itemPromptPF;
    [SerializeField] private RectTransform itemPromptsContainer;

    private Dictionary<WeaponItemHolder, ItemPrompt> currentItemPrompsList;

    protected override void EventsSubscriber()
    {
        ItemHolderEvents.OnEnteredWeaponPickupRange += OnEnteredWeaponPickupRange;
        ItemHolderEvents.OnExitedWeaponPickupRange += OnExitedWeaponPickupRange;
    }

    protected override void EventsUnSubscriber()
    {
        ItemHolderEvents.OnEnteredWeaponPickupRange -= OnEnteredWeaponPickupRange;
        ItemHolderEvents.OnExitedWeaponPickupRange -= OnExitedWeaponPickupRange;
    }

    protected override void Awake()
    {
        base.Awake();
        currentItemPrompsList = new Dictionary<WeaponItemHolder, ItemPrompt>();
    }

    private void OnEnteredWeaponPickupRange(WeaponItemHolder weaponItemHolder)
    {
        ItemPrompt newPrompt = itemPromptPF.Create(itemPromptsContainer);
        newPrompt.Setup(weaponItemHolder.GetItemHolderData().Data);
        currentItemPrompsList.Add(weaponItemHolder, newPrompt);
    }

    private void OnExitedWeaponPickupRange(WeaponItemHolder weaponItemHolder)
    {
        currentItemPrompsList.Remove(weaponItemHolder);
    }
}
