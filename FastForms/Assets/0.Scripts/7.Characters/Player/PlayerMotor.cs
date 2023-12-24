using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMotor : EntityMotor
{
    protected override void EventsSubscriber()
    {
        base.EventsSubscriber();
        PlayerInputsHandlerEvents.OnMovementsInputs += OnMovementsInputs;
    }

    protected override void EventsUnSubscriber()
    {
        base.EventsUnSubscriber();
        PlayerInputsHandlerEvents.OnMovementsInputs -= OnMovementsInputs;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        this.MoveByVelocity();
    }

    private void OnMovementsInputs(Vector2 inputValue)
        => ReadMovementsInputs(inputValue);
    public void ReadMovementsInputs(InputAction.CallbackContext context)
    => ReadMovementsInputs(context.ReadValue<Vector2>());
    public void ReadMovementsInputs(Vector2 value)
    {
        Velocity = value;
        if (Velocity != Vector2.zero) LastDirection = Velocity;
    }
}
