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

    private WeaponHandler.S_WeaponData weaponData;

    private int currentPiercingCount = 0;

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

    public void Launch(Vector2 direction, WeaponHandler.S_WeaponData weaponData)
    {
        this.weaponData = weaponData;

        waitCoroutine = StartCoroutine(KillCoroutine());

        Vector2 aimTargetPosition = direction;
        Vector2 aimerPosition = this.transform.position;
        Vector2 dir = (aimTargetPosition - aimerPosition);
        dir.Normalize();
        this.body.AddForce(dir * weaponData.speed, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent<IDamageable>(out IDamageable damageable)) return;

        damageable.TryInflictDamages(new IDamageable.DamagesData(weaponData));
        currentPiercingCount++;
        if (currentPiercingCount >= weaponData.piercingValue) this.Kill();
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
