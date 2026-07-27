using UnityEngine;

public class EnemyAttackController : MonoBehaviour
{

    public enum EnemyPhase
    {
        NORMAL,
        PHASE1,
        PHASE2
    }

    [Header("現在のフェーズ")]
    [SerializeField]
    private EnemyPhase m_CurrentPhase = EnemyPhase.NORMAL;

    [Header("発射位置")]
    [SerializeField]
    private Transform m_FirePoint;

    [SerializeField]
    private Transform m_Player;

    [Header("弾")]
    [SerializeField]
    private Sprite m_NormalBulletSprite;

    [SerializeField]
    private Sprite m_HomingBulletSprite;

    [SerializeField]
    private float m_BulletSpeed = 10.0f;


    [Header("攻撃間隔")]
    [SerializeField]
    private float m_NormalInterval = 2.0f;  //通常弾

    [SerializeField]
    private float m_FanInterval = 3.0f;

    [SerializeField]
    private float m_HomingInterval = 7.0f;

    private float m_NormalTimer;
    private float m_FanTimer;
    private float m_HomingTimer;

    //扇形を奇数・偶数で切り替える
    private bool m_IsOddShot = true;

    // Update is called once per frame
    void Update()
    {
        m_NormalTimer += Time.deltaTime;
        m_FanTimer += Time.deltaTime;
        m_HomingTimer += Time.deltaTime;

        //通常弾
        if(m_NormalTimer >= m_NormalInterval)
        {
            ShotNormal();
            m_NormalTimer = 0.0f;
        }

        //phase1以降
        if(m_CurrentPhase >= EnemyPhase.PHASE1)
        {
            if(m_FanTimer >= m_FanInterval)
            {
                ShotFan();
                m_FanTimer = 0.0f;
            }
        }

        //phase2以降
        if(m_CurrentPhase >= EnemyPhase.PHASE2)
        {
            if(m_HomingTimer >= m_HomingInterval)
            {
                ShotHoming();
                m_HomingTimer = 0.0f;
            }
        }
    }

    /// <summary>
    /// フェーズ変更
    /// </summary>
    /// <param name="phase">現在のフェーズ</param>
    public void SetPhase(EnemyPhase phase)
    {
        m_CurrentPhase = phase;
    }

    /// <summary>
    /// 通常弾
    /// </summary>
    private void ShotNormal()
    {
        Vector2 dir = (m_Player.position - m_FirePoint.position).normalized;

        BulletManager.CreateBullet<EnemyBullet>(
            m_FirePoint.position,
            dir,
            m_BulletSpeed,
            m_NormalBulletSprite
            );
    }

    /// <summary>
    /// 扇形弾
    /// </summary>
    private void ShotFan()
    {
        int bulletCount = m_IsOddShot ? 5 : 4;
        float angleRange = 60.0f;

        float startAngle = -angleRange / 2.0f;
        float step = angleRange / (bulletCount - 1);

        //偶数の場合は左右対称になるよう半歩ずらす
        if(!m_IsOddShot)
        {
            startAngle += step / 2.0f;
        }

        Vector2 baseDir = (m_Player.position - m_FirePoint.position).normalized;

        for(int i = 0; i < bulletCount; i++)
        {
            float angle = startAngle + step * i;

            Vector2 dir = Quaternion.Euler(0, 0, angle) * baseDir;

            BulletManager.CreateBullet<EnemyBullet>(
                m_FirePoint.position,
                dir,
                m_BulletSpeed,
                m_NormalBulletSprite
            );
        }

        m_IsOddShot = !m_IsOddShot;
    
    }

    /// <summary>
    /// ホーミング弾
    /// </summary>
    private void  ShotHoming()
    {
        Vector2 dir = (m_Player.position - m_FirePoint.position).normalized;

        HomingBullet bullet =
            BulletManager.CreateBullet<HomingBullet>(
                m_FirePoint.position,
                dir,
                m_BulletSpeed,
                m_HomingBulletSprite
                );

        bullet.SetTarget(m_Player);
    }
}
