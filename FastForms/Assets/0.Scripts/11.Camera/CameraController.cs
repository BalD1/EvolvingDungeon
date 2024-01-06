using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [field: SerializeField] public Camera Camera { get; private set; }
    [field: SerializeField] public CinemachineVirtualCamera CineCamera { get; private set; }

    public void Init()
    {
    }

    public void StartFollowing(Transform target)
    {
        CineCamera.Follow = target;
    }

    public void StopFollowing()
        => CineCamera.Follow = null;
}
