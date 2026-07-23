using UnityEngine;

//“G’e
public class EnemyBullet : BulletBase
{
    protected override void OnHit(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("ƒvƒŒ[ƒ„[‚É“–‚½‚Á‚½");

            Despawn();
        }
    }
}
