using System;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticlesPlayer : MonoBehaviour
{
    [SerializeField] private ParticleSystem particles;
    [field: SerializeField] public float OffsetDistance { get; private set; } = 1f;

    [SerializeField] private bool destroyOnEnd;

    public event Action OnParticlesStart;
    public event Action OnParticlesEnd;

    private void Reset()
    {
        particles = this.GetComponent<ParticleSystem>();
    }

    public void PlayParticles()
    {
        particles.Play();
        OnParticlesStart?.Invoke();
    }

    public void OnParticleSystemStopped()
    {
        OnParticlesEnd?.Invoke();
        if (destroyOnEnd) Destroy(gameObject);
    }
}
