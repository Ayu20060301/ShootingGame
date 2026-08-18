using UnityEngine;

//敵が発射する弾
public class EnemyBullet : BulletBase
{
    /// <summary>
    /// 弾がプレイヤーに命中した時の処理
    /// </summary>
    /// <param name="other">プレイヤーのCollider</param>
    protected override void OnHit(Collider2D other)
    {
        //プレイヤー以外に当たった場合は何もしない
        if(!other.CompareTag("Player"))
        {
            return;
        }

        //プレイヤーの被弾演出
        if (other.TryGetComponent<PlayerFlash>(out PlayerFlash playerFlash))
        {
            playerFlash.BulletHit();
        }

        //プレイヤーのライフを減らす
        if (other.TryGetComponent<PlayerLifeUI>(out PlayerLifeUI lifeUI))
        {
            lifeUI.LoseLife();
        }

        //弾を消す
        Despawn();
    }
}
