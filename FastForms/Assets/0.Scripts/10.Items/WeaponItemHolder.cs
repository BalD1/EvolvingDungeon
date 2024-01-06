using System;
using UnityEngine;

public class WeaponItemHolder : ItemHolder<SO_WeaponData>
{
    protected override void EventsSubscriber()
    {
    }

    protected override void EventsUnSubscriber()
    {
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        this.EnteredWeaponPickupRange();

        PlayerInputsHandlerEvents.OnPickupLeft += OnLeftPickupInput;
        PlayerInputsHandlerEvents.OnPickupRight += OnRightPickupInput;
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        this.ExitedWeaponPickupRange();

        PlayerInputsHandlerEvents.OnPickupLeft -= OnLeftPickupInput;
        PlayerInputsHandlerEvents.OnPickupRight -= OnRightPickupInput;
    }

    private void OnLeftPickupInput()
    {
        base.Interact();
    }
    private void OnRightPickupInput()
    {

    }

}
