using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [field: SerializeField] public CinemachineVirtualCamera CineCamera { get; private set; }

    public void StartFollowing(Transform target)
    {
        CineCamera.Follow = target;
    }

    public void StopFollowing()
        => CineCamera.Follow = null;
}
