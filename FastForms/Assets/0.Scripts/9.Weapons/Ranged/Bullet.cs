using com.cyborgAssets.inspectorButtonPro;
using StdNounou;
using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour, IProjectile<Bullet>
{
    [SerializeField] private SO_BulletData bulletData;

    private float lifetimeTimer;

    private WaitForSeconds cooldownWait;

    private Collider2D[] overlapResult = new Collider2D[3];
    private Coroutine triggerCheckCoroutine;

    private SO_WeaponBehavior.S_TotalStats totalStats;

    private int currentPiercingCount = 0;

    private Vector2 currentDirection;

    private SO_WeaponData.S_Particles weaponParticles;
    private bool killScheduled;

    private bool isAlive = false;

    private void Awake()
    {
        cooldownWait = new WaitForSeconds(bulletData.CollisionCheckCooldown);
    }

    private void Update()
    {
        if (!isAlive) return;

        lifetimeTimer -= Time.deltaTime;
        if (lifetimeTimer <= 0)
        {
            this.Kill();
            return;
        }
        this.transform.Translate(currentDirection * totalStats.GetFinalStat(IStatContainer.E_StatType.Speed) * Time.deltaTime); 
    }

    public Bullet GetNext(Vector2 position, Quaternion rotation)
        => PoolsManager.Instance.BulletsPool.GetNext(position, Quaternion.identity);

    public void Launch(Vector2 direction, ref SO_WeaponBehavior.S_TotalStats totalStats, ref SO_WeaponData.S_Particles weaponParticles)
    {
        lifetimeTimer = bulletData.Lifetime;
        triggerCheckCoroutine = StartCoroutine(CheckForColliders());
        this.totalStats = totalStats;
        this.weaponParticles = weaponParticles;
        killScheduled = false;

        Vector2 aimTargetPosition = direction;
        Vector2 aimerPosition = this.transform.position;
        currentDirection = (aimTargetPosition - aimerPosition);
        currentDirection.Normalize();

        isAlive = true;
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
                if (!overlapResult[i].TryGetComponent<IDamageable>(out IDamageable damageable))
                {
                    killScheduled = true;
                    overlapResult[i] = null;
                    continue;
                }
                if (!damageable.TryInflictDamages(CreateDamagesData()))
                {
                    overlapResult[i] = null;
                    continue;
                }

                particlesRotation = Quaternion.FromToRotation(Vector2.up, currentDirection);
                this.weaponParticles.ImpactParticles?.GetNext(this.transform.position, particlesRotation).PlayParticles();
                if (this.weaponParticles.EntityHitParticles != null)
                {
                    Vector2 particlesPos = this.transform.position;
                    particlesPos += new Vector2(
                        x: currentDirection.x * (overlapResult[i].bounds.extents.x * weaponParticles.EntityHitParticles.OffsetDistance),
                        y: currentDirection.y * (overlapResult[i].bounds.extents.y * weaponParticles.EntityHitParticles.OffsetDistance));

                    this.weaponParticles.EntityHitParticles.GetNext(particlesPos, particlesRotation).PlayParticles();
                }

                currentPiercingCount++;
                if (currentPiercingCount >= totalStats.GetFinalStat(IStatContainer.E_StatType.Piercing)) killScheduled = true;
                overlapResult[i] = null;
            }

            if (killScheduled) this.Kill();
        }
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
        isAlive = false;
        killScheduled = false;
        lifetimeTimer = 0;
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
