using System;
using UnityEngine;

public class BaseAI : MonoBehaviour
{
    [field: SerializeField] public Transform CurrentTarget {  get; private set; }
    [field: SerializeField] public bool TargetIsPlayer { get; private set; }

    public event Action<Transform> OnTargetChanged;

    public void SetTarget(Transform target, bool isPlayer)
    {
        CurrentTarget = target;
        TargetIsPlayer = isPlayer;
        OnTargetChanged?.Invoke(target);
    }
}
