using System;

public static class ItemHolderEvents
{
	public static event Action<WeaponItemHolder> OnEnteredWeaponPickupRange;
	public static void EnteredWeaponPickupRange(this WeaponItemHolder holder)
		=> OnEnteredWeaponPickupRange?.Invoke(holder);

    public static event Action<WeaponItemHolder> OnExitedWeaponPickupRange;
    public static void ExitedWeaponPickupRange(this WeaponItemHolder holder)
        => OnExitedWeaponPickupRange?.Invoke(holder);
}