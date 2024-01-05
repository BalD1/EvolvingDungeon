
using System.Diagnostics;
using UnityEngine;

public interface IInteractable
{
    public void Interact();
}

public static class IInteractableExtensions
{
    public static void TryAddToPlayer(this IInteractable interactable, Collider2D collision)
    {
        PlayerInteractor interactor = collision.GetComponent<PlayerInteractor>();
        if (interactor == null) return;
        interactable.AddToPlayer(interactor);
    }
    public static void AddToPlayer(this IInteractable interactable, PlayerInteractor interactor)
    {
        interactor.AddInterctable(interactable);
    }

    public static void TryRemoveFromPlayer(this IInteractable interactable, Collider2D collision)
    {
        PlayerInteractor interactor = collision.GetComponent<PlayerInteractor>();
        if (interactor == null) return;
        interactable.RemoveFromPlayer(interactor);
    }
    public static void RemoveFromPlayer(this IInteractable interactable, PlayerInteractor interactor)
    {
        interactor.RemoveInteractable(interactable);
    }
}