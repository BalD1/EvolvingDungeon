using UnityEngine;

[RequireComponent(typeof(PooledParticlesPlayer))]
public class PooledParticlesPlayer : MonoBehaviour
{
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private readonly E_ParticlesPoolID particlesPoolID;

    public enum E_ParticlesPoolID
    {
        Simple,
    }

    private void Reset()
    {
        particles = this.GetComponent<ParticleSystem>();
    }

    public PooledParticlesPlayer GetNext(Vector2 position, Quaternion quaternion)
    {
        //Vector3 angle = quaternion.eulerAngles;
        //angle.x = -90f;
        //quaternion.eulerAngles = angle;
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
