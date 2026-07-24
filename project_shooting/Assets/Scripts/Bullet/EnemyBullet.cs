using UnityEngine;

//ìGíe
public class EnemyBullet : BulletBase
{
    protected override void OnHit(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            if (other.TryGetComponent<PlayerFlash>(out var playerFlash))
            {
                playerFlash.BulletHit();
            }

            if (other.TryGetComponent<LifeUI>(out var lifeUI))
            {
                lifeUI.LoseLife();
            }

            //íeé©ëÃÇÕè¡Ç¶ÇÈ
            Despawn();
        }
    }
}
