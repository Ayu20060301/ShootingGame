using UnityEngine;


/// <summary>
/// 弾の共通処理を制御する基底クラス
/// </summary>
public abstract class BulletBase : MonoBehaviour
{
    //弾の発射速度
    protected float m_Speed = 5.0f; 
  
    //弾の生存時間
    protected float m_LifeTime = 3.0f;

    //弾の進行方向
    protected Vector2 m_Direction = Vector2.right;

    //Transformのキャッシュ
    protected Transform m_CachedTransform;

    //弾が生成されてからの経過時間
    private float m_Timer = 0.0f;

    
    private void Awake()
    {
        m_CachedTransform = this.transform;   
    }

    /// <summary>
    /// 弾が有効化されたときの処理
    /// </summary>
    private void OnEnable()
    {
        //経過時間をリセット
        ResetTimer();
    }

    private void Update()
    {
        //弾を移動
        Move();

        //生存時間を更新
        UpdateLifeTime();
    }

    /// <summary>
    /// 弾の初期化
    /// </summary>
    /// <param name="position">発射位置</param>
    /// <param name="direction">進行方向</param>
    /// <param name="speed">弾の移動速度</param>
    public virtual void Initialize(Vector3 position, Vector2 direction,float speed)
    {
        //発射位置を特定
        m_CachedTransform.position = position;

        //進行方向を正規化
        m_Direction = direction.normalized;
        
        //移動速度を設定
        m_Speed = speed;

        //生存時間をリセット
        ResetTimer();
    }


    /// <summary>
    /// 弾の移動処理
    /// </summary>
    protected virtual void Move()
    {
        //ゲーム中でなければ移動しない
        if (!GameManager.Instance.isActive) return;

        //進行方向へ移動
        m_CachedTransform.position += (Vector3)(m_Direction * m_Speed * Time.deltaTime);
    }

    /// <summary>
    /// 弾の生存時間を更新する
    /// </summary>
    private void UpdateLifeTime()
    {
        //ゲーム中でなければ時間を進めない
        if (!GameManager.Instance.isActive) return;

        //経過時間を計測
        m_Timer += Time.deltaTime;

        //生存時間を超えた場合は消去
        if(m_Timer >= m_LifeTime)
        {
            Despawn();
        }
    }

    /// <summary>
    /// 生存時間をリセットする
    /// </summary>
    private void ResetTimer()
    {
        m_Timer = 0.0f; 
    }

    /// <summary>
    /// 共通の当たり判定
    /// </summary>
    /// <param name="collision">接触したCollider</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        OnHit(other);
    }

    /// <summary>
    /// 弾が何かに命中した時の処理
    /// </summary>
    /// <param name="other">命中したCollider</param>
    protected abstract void OnHit(Collider2D other);

    /// <summary>
    /// 弾を非アクティブにする
    /// </summary>
    protected virtual void Despawn()
    {
        gameObject.SetActive(false);
    }
}
