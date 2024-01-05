using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMotor : EntityMotor
{
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        this.MoveByVelocity();
    }

    public void ForceReadMovementsInput()
        => ReadMovementsInputs(PlayerInputsHandler.Instance.MovInputsValue);
    public void ReadMovementsInputs(InputAction.CallbackContext context)
    => ReadMovementsInputs(context.ReadValue<Vector2>());
    public void ReadMovementsInputs(Vector2 value)
    {
        Velocity = value;
        if (Velocity != Vector2.zero) LastDirection = Velocity;
    }
}
