using System;
using UnityEngine;

public class BaseAI : MonoBehaviour
{
    [field: SerializeField] public Transform CurrentTarget {  get; private set; }
    [field: SerializeField] public bool TargetIsPlayer { get; private set; }

    public event Action<Transform> OnTargetChanged;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            SetTarget(GameObject.FindGameObjectWithTag("Player").transform, true);
    }

    public void SetTarget(Transform target, bool isPlayer)
    {
        CurrentTarget = target;
        TargetIsPlayer = isPlayer;
        OnTargetChanged?.Invoke(target);
    }
}
