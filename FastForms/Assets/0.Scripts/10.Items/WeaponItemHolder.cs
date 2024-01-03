using UnityEngine;

public class WeaponItemHolder : ItemHolder<SO_WeaponData>
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        this.EnteredWeaponPickupRange();
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        this.ExitedWeaponPickupRange();
    }
}
