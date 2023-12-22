using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputsHandler : MonoBehaviour
{
    [field: SerializeField] public PlayerInput InputsComponent {  get; private set; }
    [field: SerializeField, ReadOnly] public Vector2 MovInputsValue { get; private set; }

    public static bool IsMouseDown { get; private set; }

    private void Reset()
    {
        InputsComponent = this.GetComponent<PlayerInput>();
    }

    public void OnMovements(InputAction.CallbackContext context)
    {
        MovInputsValue = context.ReadValue<Vector2>();
        this.MovementsInputs_Call(MovInputsValue);
    }

    public void OnMouse(InputAction.CallbackContext context)
    {
        if (context.performed) return;
        if (context.started)
        {
            IsMouseDown = true;
            this.MouseDown_Call();
            return;
        }
        IsMouseDown = false;
        this.MouseUp_Call();
    }
}
