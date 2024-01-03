using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItemHolder : ItemHolder
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        this.EnteredWeaponPickupRange();
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        this.ExitedWeaponPickupRange();
    }
}
