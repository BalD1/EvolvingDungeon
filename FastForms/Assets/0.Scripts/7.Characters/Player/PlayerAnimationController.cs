using StdNounou;
using UnityEngine;

public class PlayerAnimationController : EntityAnimationControllerBase
{
    private void Update()
    {
        TryFlip(MouseUtils.GetMouseWorldPosition().x < ownerObj.transform.position.x);
    }
}
