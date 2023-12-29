using StdNounou;
using UnityEngine;

public class PlayerRotator : ObjectRotator
{
    public void SetAim()
    {
        if (isUsingGamepad) RotationTowardsMovements(true);
        else RotationTowardsMouse();
    }
}
