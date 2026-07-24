using UnityEngine;

public class PlayerBullet : BulletBase
{
    [SerializeField]
    private int m_Damage = 30; //ƒ_ƒ[ƒW”

    protected override void OnHit(Collider2D other)
    {
        if(other.CompareTag("Enemy"))
        {
            Debug.Log("“G‚É“–‚½‚Á‚½");
            other.GetComponent<EnemyHealth>().TakeDamage(m_Damage);
            Despawn();
        }
    }
}
