using StdNounou;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MonoStatsHandler))]
public class Bullet : MonoBehaviour, IProjectile<Bullet>
{
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private float lifetime = 10;

    [SerializeField] private MonoStatsHandler selfStats;

    private Coroutine waitCoroutine;
    private WaitForSeconds waitBeforeKill;

    private void Reset()
    {
        selfStats = this.GetComponent<MonoStatsHandler>();
    }

    private void Awake()
    {
        waitBeforeKill = new WaitForSeconds(lifetime);
    }

    public Bullet GetNext(Vector2 position, Quaternion rotation)
        => PoolsManager.Instance.BulletsPool.GetNext(position, rotation);

    public void Launch(Vector2 direction, float force, float damages, float critChances, float critMultiplier)
    {
        waitCoroutine = StartCoroutine(KillCoroutine());

        Vector2 aimTargetPosition = direction;
        Vector2 aimerPosition = this.transform.position;
        Vector2 dir = (aimTargetPosition - aimerPosition);
        dir.Normalize();
        this.body.AddForce(dir * force, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    public void Kill()
    {
        if (waitCoroutine != null)
        {
            StopCoroutine(waitCoroutine);
            waitCoroutine = null;
        }
        PoolsManager.Instance.BulletsPool.Enqueue(this);
    }

    private IEnumerator KillCoroutine()
    {
        yield return waitBeforeKill;
        waitCoroutine = null;
        Kill();
    }
}
