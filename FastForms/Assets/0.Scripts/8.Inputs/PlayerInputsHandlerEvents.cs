using System;
using UnityEngine;

public static class PlayerInputsHandlerEvents
{
	public static event Action<Vector2> OnMovementsInputs;
	public static void MovementsInputs_Call(this PlayerInputsHandler inputsHandler, Vector2 value)
		=> OnMovementsInputs?.Invoke(value);

	public static event Action OnMouseDown;
	public static void MouseDown_Call(this PlayerInputsHandler inputsHandler)
		=> OnMouseDown?.Invoke();

    public static event Action OnMouseUp;
    public static void MouseUp_Call(this PlayerInputsHandler inputsHandler)
        => OnMouseUp?.Invoke();

	public static event Action OnPausePressed;
	public static void PausePressed_Call(this PlayerInputsHandler inputsHandler)
		=> OnPausePressed?.Invoke();

	public static event Action OnCloseYoungestMenu;
	public static void CloseYoungestMenu_Call(this PlayerInputsHandler inputsHandler)
		=> OnCloseYoungestMenu?.Invoke();

	public static event Action OnPickupLeft;
	public static void PickupLeft_Call(this PlayerInputsHandler inputsHandler)
		=> OnPickupLeft?.Invoke();

	public static event Action OnPickupRight;
	public static void PickupRight_Call(this PlayerInputsHandler inputsHandler)
		=> OnPickupRight?.Invoke();
}