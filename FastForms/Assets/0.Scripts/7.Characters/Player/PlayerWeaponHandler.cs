using UnityEngine;
using StdNounou;

public class PlayerWeaponHandler : WeaponHandler
{
    protected override void EventsSubscriber()
    {
        PlayerInputsHandlerEvents.OnMouseDown += Execute;
    }

    protected override void EventsUnSubscriber()
    {
        PlayerInputsHandlerEvents.OnMouseDown -= Execute;
    }

    protected override void OnCooldownEnded()
    {
        if (PlayerInputsHandler.IsMouseDown) Execute();
    }
}
