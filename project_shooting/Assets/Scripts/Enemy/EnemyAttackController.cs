using UnityEngine;

//敵の攻撃制御
public class EnemyAttackController : MonoBehaviour
{
    /// <summary>
    /// 攻撃フェーズ
    /// </summary>
    public enum EnemyPhase
    {
        NORMAL,    //通常
        PHASE1,    //扇形攻撃
        PHASE2     //ホーミング攻撃
    }

    private const float FAN_ANGLE = 60.0f;
    private const int ODD_BULLET_COUNT = 5;
    private const int EVEN_BULLET_COUNT = 4;

    [SerializeField]
    private EnemyPhase m_CurrentPhase = EnemyPhase.NORMAL;

    [SerializeField]
    private Transform m_FirePoint;

    [SerializeField]
    private Transform m_Player;

    [SerializeField]
    private Sprite m_NormalBulletSprite;

    [SerializeField]
    private Sprite m_HomingBulletSprite;

    [SerializeField]
    private float m_BulletSpeed = 10.0f;

    [Header("攻撃間隔")]
    [SerializeField]
    private float m_NormalInterval = 2.0f;

    [SerializeField]
    private float m_FanInterval = 1.0f;

    [SerializeField]
    private float m_HomingInterval = 7.0f;

    private float m_NormalTimer;
    private float m_FanTimer;
    private float m_HomingTimer;

    private EnemyController m_EnemyController;

    //奇数・偶数発射を切り替える
    private bool m_IsOddShot = true;

    private void Awake()
    {
        m_EnemyController = GetComponent<EnemyController>();
    }

    private void Update()
    {
        if (!GameManager.Instance.isActive)
        {
            return;
        }

        if (m_Player == null)
        {
            return;
        }

        UpdateTimer();
        Attack();
    }

    /// <summary>
    /// 現在のフェーズに応じて攻撃する
    /// </summary>
    private void Attack()
    {
        switch (m_CurrentPhase)
        {
            case EnemyPhase.NORMAL:
                NormalAttack();
                break;

            case EnemyPhase.PHASE1:
                Phase1Attack();
                break;

            case EnemyPhase.PHASE2:
                Phase2Attack();
                break;
        }
    }

    /// <summary>
    /// タイマー更新
    /// </summary>
    private void UpdateTimer()
    {
        m_NormalTimer += Time.deltaTime;
        m_FanTimer += Time.deltaTime;
        m_HomingTimer += Time.deltaTime;
    }

    /// <summary>
    /// 攻撃フェーズ変更
    /// </summary>
    public void SetPhase(EnemyPhase phase)
    {
        if (m_CurrentPhase == phase)
        {
            return;
        }

        m_CurrentPhase = phase;
        ResetTimer();
    }

    /// <summary>
    /// タイマーをリセット
    /// </summary>
    private void ResetTimer()
    {
        m_NormalTimer = 0.0f;
        m_FanTimer = 0.0f;
        m_HomingTimer = 0.0f;
    }

    /// <summary>
    /// 通常攻撃
    /// </summary>
    private void NormalAttack()
    {
        if (m_NormalTimer < m_NormalInterval)
        {
            return;
        }

        m_NormalTimer = 0.0f;
        ShootNormal();
    }

    /// <summary>
    /// フェーズ1攻撃
    /// </summary>
    private void Phase1Attack()
    {
        if (!m_EnemyController.IsStopping())
        {
            return;
        }

        if (m_FanTimer < m_FanInterval)
        {
            return;
        }

        m_FanTimer = 0.0f;
        ShootFan();
    }

    /// <summary>
    /// フェーズ2攻撃
    /// </summary>
    private void Phase2Attack()
    {
        if (m_HomingTimer < m_HomingInterval)
        {
            return;
        }

        m_HomingTimer = 0.0f;
        ShotHoming();
    }

    /// <summary>
    /// プレイヤー方向を取得
    /// </summary>
    private Vector2 GetPlayerDirection()
    {
        return (m_Player.position - m_FirePoint.position).normalized;
    }

    /// <summary>
    /// 通常弾発射
    /// </summary>
    private void ShootNormal()
    {
        SEManager.Instance.SEPlay(SEType.SHOT_ENEMY);

        BulletManager.CreateBullet<EnemyBullet>(
            m_FirePoint.position,
            GetPlayerDirection(),
            m_BulletSpeed,
            m_NormalBulletSprite);
    }

    /// <summary>
    /// 扇形弾発射
    /// </summary>
    private void ShootFan()
    {
        SEManager.Instance.SEPlay(SEType.SHOT_ENEMY);

        int bulletCount = m_IsOddShot
            ? ODD_BULLET_COUNT
            : EVEN_BULLET_COUNT;

        float startAngle = -FAN_ANGLE * 0.5f;
        float step = FAN_ANGLE / (bulletCount - 1);

        if (!m_IsOddShot)
        {
            startAngle += step * 0.5f;
        }

        Vector2 baseDirection = GetPlayerDirection();

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = startAngle + step * i;

            Vector2 direction =
                Quaternion.Euler(0, 0, angle) * baseDirection;

            BulletManager.CreateBullet<EnemyBullet>(
                m_FirePoint.position,
                direction,
                m_BulletSpeed,
                m_NormalBulletSprite);
        }

        m_IsOddShot = !m_IsOddShot;
    }

    /// <summary>
    /// ホーミング弾発射
    /// </summary>
    private void ShotHoming()
    {
        SEManager.Instance.SEPlay(SEType.HOMING);

        HomingBullet bullet =
            BulletManager.CreateBullet<HomingBullet>(
                m_FirePoint.position,
                GetPlayerDirection(),
                m_BulletSpeed,
                m_HomingBulletSprite);

        bullet.SetTarget(m_Player);
    }
}
