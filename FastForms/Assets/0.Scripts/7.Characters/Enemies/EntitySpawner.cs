using StdNounou;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    [SerializeField] private Entity entityPF;

    public Entity PerformSpawn()
    {
        return entityPF?.Create(parent: this.transform);
    }
}
