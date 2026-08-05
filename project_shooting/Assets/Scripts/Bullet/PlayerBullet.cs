using UnityEngine;

public class PlayerBullet : BulletBase
{
    private int m_Damage = 40; //ƒ_ƒ[ƒW”

    protected override void OnHit(Collider2D other)
    {
        if(other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyHealth>().TakeDamage(m_Damage);
            Despawn();
        }
    }
}
