using StdNounou;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour, IProjectile<Bullet>
{
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private float lifetime = 10;

    private Coroutine waitCoroutine;
    private WaitForSeconds waitBeforeKill;

    private SO_WeaponBehavior.S_TotalStats totalStats;

    private int currentPiercingCount = 0;

    private Vector2 currentDirection;

    private void Reset()
    {
        body = this.GetComponent<Rigidbody2D>();
    }

    private void Awake()
    {
        waitBeforeKill = new WaitForSeconds(lifetime);
    }

    public Bullet GetNext(Vector2 position, Quaternion rotation)
        => PoolsManager.Instance.BulletsPool.GetNext(position, rotation);

    public void Launch(Vector2 direction, ref SO_WeaponBehavior.S_TotalStats totalStats)
    {
        this.totalStats = totalStats;

        waitCoroutine = StartCoroutine(KillCoroutine());

        Vector2 aimTargetPosition = direction;
        Vector2 aimerPosition = this.transform.position;
        currentDirection = (aimTargetPosition - aimerPosition);
        currentDirection.Normalize();
        this.body.AddForce(currentDirection * totalStats.GetFinalStat(IStatContainer.E_StatType.Speed), ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent<IDamageable>(out IDamageable damageable)) return;

        damageable.TryInflictDamages(CreateDamagesData());
        currentPiercingCount++;
        if (currentPiercingCount >= totalStats.GetFinalStat(IStatContainer.E_StatType.Piercing)) this.Kill();
    }

    private IDamageable.DamagesData CreateDamagesData()
    {
        float finalDamages = totalStats.GetFinalStat(IStatContainer.E_StatType.BaseDamages);
        bool isCrit = RandomExtensions.PercentageChance(totalStats.GetFinalStat(IStatContainer.E_StatType.CritChances));

        if (isCrit) finalDamages *= totalStats.GetFinalStat(IStatContainer.E_StatType.CritMultiplier);

        return new IDamageable.DamagesData(
            damagerTeam: totalStats.OwnerStats.GetTeam(),
            damagesType: totalStats.DamageType,
            damages: finalDamages,
            isCrit: isCrit,
            damageDirection: currentDirection,
            knockbackForce: totalStats.GetFinalStat(IStatContainer.E_StatType.Knockback)
            );
    }

    public void Kill()
    {
        if (waitCoroutine != null)
        {
            StopCoroutine(waitCoroutine);
            waitCoroutine = null;
        }
        currentPiercingCount = 0;
        PoolsManager.Instance.BulletsPool.Enqueue(this);
    }

    private IEnumerator KillCoroutine()
    {
        yield return waitBeforeKill;
        waitCoroutine = null;
        Kill();
    }
}
