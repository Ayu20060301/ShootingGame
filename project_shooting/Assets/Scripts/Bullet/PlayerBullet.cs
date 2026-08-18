using UnityEngine;

//ƒvƒŒƒCƒ„[‚ª”­Ë‚·‚é’e
public class PlayerBullet : BulletBase
{
    //’e‚Ìƒ_ƒ[ƒW
    private const int DAMAGE = 40;

    /// <summary>
    /// ’e‚ª“G‚É–½’†‚µ‚½‚Ìˆ—
    /// </summary>
    /// <param name="other">“G‚ÌCollider</param>
    protected override void OnHit(Collider2D other)
    {
        //“GˆÈŠO‚É“–‚½‚Á‚½ê‡‚Í‰½‚à‚µ‚È‚¢
        if(!other.CompareTag("Enemy"))
        {
            return;
        }

        //EnemyHealth‚ğæ“¾‚µ‚Äƒ_ƒ[ƒW‚ğ—^‚¦‚é
        if(other.TryGetComponent<EnemyHealth>(out EnemyHealth enemyHealth))
        {
            enemyHealth.TakeDamage(DAMAGE);
        }

        //’e‚ğÁ‚·
        Despawn();
    }
}
