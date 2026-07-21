using Unity.VisualScripting.FullSerializer;
using UnityEngine;

/// <summary>
/// 敵の制御
/// </summary>
public class EnemyController : MonoBehaviour
{

    [Header("移動設定")]
    [SerializeField]
    private float m_MoveDistance = 3.0f; //上下に動く距離
    [SerializeField]
    private float m_MoveSpeed = 1.0f; //移動速度

    private Transform m_CashedTransform;
    private Vector3 m_StartPosition;

    [Header("弾の設定")]
    [SerializeField]
    private Sprite m_BulletSprite; //弾の見た目
    [SerializeField]
    private Transform m_MuzzlePoint;
    [SerializeField]
    private float m_ShotInterval = 0.1f;
    [SerializeField]
    private float m_BulletSpeed = 10.0f; //弾の速度

    private float m_ShotTimer;

    [Header("ステータス")]
    [SerializeField]
    private int m_MaxHP = 100; //最大HP
    private int m_CurrentHP;  //現在のHP

    private void Awake()
    {
        m_CashedTransform = this.transform;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //開始時の位置を基準点として保存
        m_StartPosition = this.transform.position;
        m_CurrentHP = m_MaxHP;
    }

    // Update is called once per frame
    void Update()
    {
        //Sin波を使って滑らかに上下移動
        float newY = m_StartPosition.y + Mathf.Sin(Time.time * m_MoveSpeed) * m_MoveDistance;
        this.transform.position = new Vector3(m_StartPosition.x, newY, this.transform.position.z);

        //発射間隔のクールタイムを更新する
        HandleShootTimer();

        //射撃処理
        Shoot();
    }

    /// <summary>
    /// 発射間隔のクールタイムを更新する
    /// </summary>
    private void HandleShootTimer()
    {
        if(m_ShotTimer > 0.0f)
        {
            m_ShotTimer -= Time.deltaTime;
        }
    }

    /// <summary>
    /// 射撃処理
    /// </summary>
    private void Shoot()
    {
        if (m_ShotTimer > 0) return;

        Vector3 spawnPos = m_MuzzlePoint != null ? m_MuzzlePoint.position : m_CashedTransform.position;
        BulletManager.CreateBullet<EnemyBullet>(spawnPos, -Vector2.right, m_BulletSpeed, m_BulletSprite);

        m_ShotTimer = m_ShotInterval;
    }

    /// <summary>
    /// ダメージを受ける処理
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    public void TakeDamage(int damage)
    {
        m_CurrentHP -= damage;

        //敵のHPが0になったら死亡
        if(m_CurrentHP <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// 撃破時の処理
    /// </summary>
    private void Die()
    {
        Debug.Log("敵を倒した");
        Destroy(this.gameObject);
    }

}
