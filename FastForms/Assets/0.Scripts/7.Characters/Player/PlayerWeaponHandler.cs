using StdNounou;

public class PlayerWeaponHandler : WeaponHandler
{
    protected override void EventsSubscriber()
    {
        PlayerInputsHandlerEvents.OnMouseDown += FireWeapon;
    }

    protected override void EventsUnSubscriber()
    {
        PlayerInputsHandlerEvents.OnMouseDown -= FireWeapon;
    }

    protected override void CooldownEnded()
    {
        base.CooldownEnded();
        if (PlayerInputsHandler.IsMouseDown) FireWeapon();
    }

    private void FireWeapon()
    {
        Execute(MouseUtils.GetMouseWorldPosition());
    }
}
