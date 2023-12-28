using UnityEngine;

[RequireComponent(typeof(PooledParticlesPlayer))]
public class PooledParticlesPlayer : MonoBehaviour
{
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private E_ParticlesPoolID particlesPoolID;

    [field: SerializeField] public float OffsetDistance { get; private set; } = 1f;

    public enum E_ParticlesPoolID
    {
        SimpleGunFire,
        ProjectileImpact,
        ProjectileEntityHit,
    }

    private void Reset()
    {
        particles = this.GetComponent<ParticleSystem>();
    }

    public PooledParticlesPlayer GetNext(Vector2 position, Quaternion quaternion)
    {
        return PoolsManager.Instance.ParticlesDictionary[particlesPoolID].GetNext(position, quaternion);
    }

    public void PlayParticles()
    {
        particles.Play();
    }

    public void OnParticleSystemStopped()
    {
        PoolsManager.Instance.ParticlesDictionary[particlesPoolID].Enqueue(this);
    }
}
