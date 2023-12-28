using StdNounou;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour, IProjectile<Bullet>
{
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private SO_BulletData bulletData;

    private Timer lifetimeTimer;

    private WaitForSeconds cooldownWait;

    private Collider2D[] overlapResult = new Collider2D[10];
    private Coroutine triggerCheckCoroutine;

    private SO_WeaponBehavior.S_TotalStats totalStats;

    private int currentPiercingCount = 0;

    private Vector2 currentDirection;

    private SO_WeaponData.S_Particles weaponParticles;


    private void Reset()
    {
        body = this.GetComponent<Rigidbody2D>();
    }

    private void Awake()
    {
        cooldownWait = new WaitForSeconds(bulletData.CollisionCheckCooldown);
        lifetimeTimer = new Timer(bulletData.Lifetime, Kill);
    }

    public Bullet GetNext(Vector2 position, Quaternion rotation)
        => PoolsManager.Instance.BulletsPool.GetNext(position, rotation);

    public void Launch(Vector2 direction, ref SO_WeaponBehavior.S_TotalStats totalStats, ref SO_WeaponData.S_Particles weaponParticles)
    {
        lifetimeTimer.Reset();
        triggerCheckCoroutine = StartCoroutine(CheckForColliders());
        this.totalStats = totalStats;
        this.weaponParticles = weaponParticles;

        Vector2 aimTargetPosition = direction;
        Vector2 aimerPosition = this.transform.position;
        currentDirection = (aimTargetPosition - aimerPosition);
        currentDirection.Normalize();
        this.body.AddForce(currentDirection * totalStats.GetFinalStat(IStatContainer.E_StatType.Speed), ForceMode2D.Impulse);
    }

    private IEnumerator CheckForColliders()
    {
        while(true)
        {
            yield return cooldownWait;
            Physics2D.OverlapCircleNonAlloc(this.transform.position, bulletData.OverlapRadius, overlapResult, bulletData.TargetMask);
            Quaternion particlesRotation;

            for (int i = 0; i < overlapResult.Length; i++)
            {
                if (overlapResult[i] == null) continue;

                particlesRotation = Quaternion.FromToRotation(Vector2.up, currentDirection);
                this.weaponParticles.ImpactParticles?.GetNext(this.transform.position, particlesRotation).PlayParticles();
                if (!overlapResult[i].TryGetComponent<IDamageable>(out IDamageable damageable)) continue;
                if (this.weaponParticles.EntityHitParticles != null)
                {
                    Vector2 particlesPos = this.transform.position;
                    particlesPos += new Vector2(
                        x: currentDirection.x * (overlapResult[i].bounds.extents.x * weaponParticles.EntityHitParticles.OffsetDistance),
                        y: currentDirection.y * (overlapResult[i].bounds.extents.y * weaponParticles.EntityHitParticles.OffsetDistance));

                    this.weaponParticles.EntityHitParticles.GetNext(particlesPos, particlesRotation).PlayParticles();
                }

                damageable.TryInflictDamages(CreateDamagesData());
                currentPiercingCount++;
                if (currentPiercingCount >= totalStats.GetFinalStat(IStatContainer.E_StatType.Piercing)) this.Kill();

                overlapResult[i] = null;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        return;
        Quaternion particlesRotation = Quaternion.FromToRotation(Vector2.up, currentDirection);
        this.weaponParticles.ImpactParticles?.GetNext(this.transform.position, particlesRotation).PlayParticles();
        if (!collision.TryGetComponent<IDamageable>(out IDamageable damageable)) return;

        if (this.weaponParticles.EntityHitParticles != null)
        {
            Vector2 particlesPos = this.transform.position;
            particlesPos += new Vector2(
                x: currentDirection.x * (collision.bounds.extents.x * weaponParticles.EntityHitParticles.OffsetDistance),
                y: currentDirection.y * (collision.bounds.extents.y * weaponParticles.EntityHitParticles.OffsetDistance));

            this.weaponParticles.EntityHitParticles.GetNext(particlesPos, particlesRotation).PlayParticles();
        }

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
        lifetimeTimer.Stop();
        if (triggerCheckCoroutine != null)
        {
            StopCoroutine(triggerCheckCoroutine);
            triggerCheckCoroutine = null;
        }
        currentPiercingCount = 0;
        PoolsManager.Instance.BulletsPool.Enqueue(this);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, bulletData.OverlapRadius);
    }
}
