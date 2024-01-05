using StdNounou;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputsHandler : PersistentSingleton<PlayerInputsHandler>
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
        this.MovementsInputs_Call(context);
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

    public void OnPause(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        this.PausePressed_Call();
    }

    public void CloseYoungestMenu(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        this.CloseYoungestMenu_Call();
    }

    public void OnPickupLeft(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        this.PickupLeft_Call();
    }
    public void OnPickupRight(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        this.PickupRight_Call();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        this.OnInteract_Call();
    }

    protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
    }

    protected override void OnSceneUnloaded(Scene scene)
    {
    }
}
