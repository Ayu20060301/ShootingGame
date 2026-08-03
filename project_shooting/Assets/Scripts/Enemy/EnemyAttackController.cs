using UnityEngine;

public class EnemyAttackController : MonoBehaviour
{

    /// <summary>
    /// 攻撃段階の種類
    /// </summary>
    public enum EnemyPhase
    {
        NORMAL,  //通常状態
        PHASE1,  //フェーズ1に入った状態(扇形攻撃)
        PHASE2   //フェーズ2に入った状態(ホーミング攻撃)
    }

    [SerializeField]
    private EnemyPhase m_CurrentPhase = EnemyPhase.NORMAL; //現在のフェーズ
    [SerializeField]
    private Transform m_FirePoint; //発射位置
    [SerializeField]
    private Transform m_Player;  //プレイヤーの座標
    [SerializeField]
    private Sprite m_NormalBulletSprite; //弾のスプライト
    [SerializeField]
    private Sprite m_HomingBulletSprite; //ホーミングのスプライト弾
    [SerializeField]
    private float m_BulletSpeed = 10.0f; //弾の速度
    [SerializeField]
    private float m_NormalInterval = 2.0f;  //通常弾
    [SerializeField]
    private float m_FanInterval = 1.0f; //扇形弾の間隔
    [SerializeField]
    private float m_HomingInterval = 7.0f; //ホーミング弾が発射する感覚
    private float m_NormalTimer;
    private float m_FanTimer;
    private float m_HomingTimer;

    private EnemyController m_EnemyController; 

    //扇形を奇数・偶数で切り替える
    private bool m_IsOddShot = true;

    private void Start()
    {
        //コーポネントの取得
        m_EnemyController = GetComponent<EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!GameManager.Instance.isActive) return;

        //プレイヤーがいなければ攻撃しない
        if (m_Player == null) return;


        //タイマー更新
        UpdateTimer();

        //段階ごとの攻撃処理
        switch(m_CurrentPhase)
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
    /// フェーズ変更
    /// </summary>
    /// <param name="phase">現在のフェーズ</param>
    public void SetPhase(EnemyPhase phase)
    {
        if (m_CurrentPhase == phase) return;

        m_CurrentPhase = phase;

        ResetTimer();
    }


    /// <summary>
    /// タイマーのリセット
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
        if(m_NormalTimer >= m_NormalInterval)
        {
            ShotNormal();
            m_NormalTimer = 0.0f;
        }
    }

    /// <summary>
    /// フェーズ1段階の攻撃処理
    /// </summary>
    private void Phase1Attack()
    {

        //停止中でなければ攻撃しない
        if (!m_EnemyController.IsStopping()) return;


        //扇形弾
        if(m_FanTimer >= m_FanInterval)
        {
            ShotFan();
            m_FanTimer = 0.0f;
        }
    }

    /// <summary>
    /// フェーズ2段階の攻撃処理
    /// </summary>
    private void Phase2Attack()
    {
        //ホーミング弾
        if(m_HomingTimer >= m_HomingInterval)
        {
            ShotHoming();
            m_HomingTimer = 0.0f;
        }
    }

    /// <summary>
    /// 通常弾
    /// </summary>
    private void ShotNormal()
    {

        if (m_Player == null) return;

        SEManager.Instance.SEPlay(SEType.SHOT_ENEMY);

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
        if (m_Player == null) return;

        SEManager.Instance.SEPlay(SEType.SHOT_ENEMY);

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

        if (m_Player == null) return;

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
