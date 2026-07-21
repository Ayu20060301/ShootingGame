using UnityEngine;

public class EnemyBullet : BulletBase
{
    [SerializeField]
    private int m_Damage = 30;

    protected override void OnHit(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("ƒvƒŒ[ƒ„[‚É“–‚½‚Á‚½");
            Despawn();
        }
    }
}
