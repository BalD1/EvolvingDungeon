using StdNounou;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private PlayerCharacter owner;
    [field: SerializeField] public AlphaHandler_SpriteRenderer interactionPrompt;

    private List<IInteractable> currentInteractables;

    private void Awake()
    {
        currentInteractables = new List<IInteractable>();
    }

    public void AddInterctable(IInteractable interactable)
    {
        currentInteractables.Add(interactable);
        interactionPrompt.SetAlpha(1);
    }

    public void RemoveInteractable(IInteractable interactable)
    {
        currentInteractables.Remove(interactable);
        if (currentInteractables.Count == 0) interactionPrompt.SetAlpha(0);
    }
}
