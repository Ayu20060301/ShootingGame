using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

//弾の生成を管理するクラス
public static class BulletManager
{
    //Rigidbody2Dの重力
    private const float GRAVITY_SCALE = 0.0f;

    //弾の回転補正
    private const float ROTATION_OFFSET = 90.0f;

    /// <summary>
    /// 弾を1発生成する
    /// </summary>
    /// <typeparam name="T">生成する弾の型</typeparam>
    /// <param name="position">発射位置</param>
    /// <param name="direction">進行方向</param>
    /// <param name="speed">弾の速度</param>
    /// <param name="sprite">弾のスプライト</param>
    /// <returns>生成した弾</returns>
    public static T CreateBullet<T>(Vector3 position,Vector2 direction, float speed,Sprite sprite) where T : BulletBase
    {
        //弾のGameObjectを生成
        GameObject bulletObj = new GameObject(typeof(T).Name);

        //Transformを取得
        Transform bulletTransform = bulletObj.transform;

        //発射位置を設定
        bulletTransform.position = position;

        //弾の向きを設定
        SetupRotation(bulletTransform, direction);

        //SpriteRendererを設定
        SetupSpriteRenderer(bulletObj, sprite);

        //Colliderを設定
        SetupCollider(bulletObj);

        //Rigidbodyを設定
        SetupRigidbody(bulletObj);

        //弾の制御コンポーネントを追加
        T bullet = bulletObj.AddComponent<T>();

        //弾を初期化
        bullet.Initialize(position, direction, speed);

        return bullet;
    }

    /// <summary>
    /// 弾の進行に合わせて回転させる
    /// </summary>
    /// <param name="transform">トランスフォーム</param>
    /// <param name="direction">進行方向</param>
    private static void SetupRotation(Transform transform,Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y,direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0.0f,0.0f,angle + ROTATION_OFFSET);
    }

    /// <summary>
    /// 弾の当たり判定を設定する
    /// </summary>
    /// <param name="bulletObject">弾のGameObject</param>
    /// <param name="sprite">弾のスプライト</param>
    private static void SetupSpriteRenderer(GameObject bulletObject,Sprite sprite)
    {
        SpriteRenderer spriteRenderer = bulletObject.AddComponent<SpriteRenderer>();

        spriteRenderer.sprite = sprite;
    }

    /// <summary>
    /// 弾の当たり判定を設定する
    /// </summary>
    /// <param name="bulletObject">弾のGameObject</param>
    private static void SetupCollider(GameObject bulletObject)
    {
        CircleCollider2D collider = bulletObject.AddComponent<CircleCollider2D>();

        collider.isTrigger = true;
    }

    /// <summary>
    /// 弾の物理設定を行う
    /// </summary>
    /// <param name="bulletObject">弾のGameObject</param>
    private static void SetupRigidbody(GameObject bulletObject)
    {
        Rigidbody2D rigidbody = bulletObject.AddComponent<Rigidbody2D>();

        rigidbody.gravityScale = GRAVITY_SCALE;
        rigidbody.bodyType = RigidbodyType2D.Kinematic;
    }
}
