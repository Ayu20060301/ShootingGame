using UnityEditor;
using UnityEngine;

/// <summary>
/// 弾を生成するクラス
/// </summary>
public static class BulletManager
{
    /// <summary>
    /// 弾を1発生成する
    /// </summary>
    /// <typeparam name="T">生成するバレットの型</typeparam>
    /// <param name="position">発射位置</param>
    /// <param name="direction">進行方向</param>
    /// <param name="speed">弾の速度</param>
    /// <returns></returns>
    public static T CreateBullet<T>(Vector3 position,Vector2 direction, float speed,Sprite sprite) where T : BulletBase
    {
        GameObject bulletObj = new GameObject(typeof(T).Name);
        bulletObj.transform.position = position;
        bulletObj.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);

        //見た目
        SpriteRenderer spriteRenderer = bulletObj.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;

        //当たり判定
        CircleCollider2D collider = bulletObj.AddComponent<CircleCollider2D>();
        collider.isTrigger = true;
        collider.radius = 0.1f;

        //物理演算
        Rigidbody2D rb = bulletObj.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0.0f;
        rb.bodyType = RigidbodyType2D.Kinematic;

        //弾の挙動スクリプト
        T bullet = bulletObj.AddComponent<T>();
        bullet.Initialize(position, direction, speed);

        return bullet;
    }
}
