using UnityEngine;

//敵の攻撃制御
public class EnemyAttackController : MonoBehaviour
{
    /// <summary>
    /// 敵の攻撃フェーズ
    /// </summary>
    public enum EnemyPhase
    {
        NORMAL,    //通常攻撃
        PHASE1,    //扇形攻撃
        PHASE2     //ホーミング攻撃
    }

    //扇形攻撃の最大角度
    private const float FAN_ANGLE = 60.0f;

    //奇数回目の発射回数
    private const int ODD_BULLET_COUNT = 5;

    //偶数回目の発射数
    private const int EVEN_BULLET_COUNT = 4;

    //現在の攻撃のフェーズ
    private EnemyPhase m_CurrentPhase = EnemyPhase.NORMAL;

    [Header("敵の移動制御")]
    [SerializeField]
    private EnemyController m_EnemyController;

    [Header("弾を発射する位置")]
    [SerializeField]
    private Transform m_FirePoint;

    [Header("攻撃対象のプレイヤー")]
    [SerializeField]
    private Transform m_Player;

    [Header("通常弾に使用するSprite")]
    [SerializeField]
    private Sprite m_NormalBulletSprite;

    [Header("ホーミング弾に使用するSprite")]
    [SerializeField]
    private Sprite m_HomingBulletSprite;

    [Header("弾の移動速度")]
    [SerializeField]
    private float m_BulletSpeed = 10.0f;

    [Header("通常攻撃時の発射する弾の間隔")]
    [SerializeField]
    private float m_NormalInterval = 2.0f;

    [Header("扇形攻撃時の発射する弾の間隔")]
    [SerializeField]
    private float m_FanInterval = 1.0f;

    [Header("ホーミング攻撃の発射する弾の感覚")]
    [SerializeField]
    private float m_HomingInterval = 7.0f;

    //通常攻撃用タイマー
    private float m_NormalTimer;
    
    //扇形攻撃用タイマー
    private float m_FanTimer;
    
    //ホーミング攻撃用タイマー
    private float m_HomingTimer;

    //奇数・偶数発射を切り替える
    private bool m_IsOddShot = true;

    private void Update()
    {
        //ゲームが進行中でない場合は攻撃しない
        if (!GameManager.Instance.isActive) return;

        //プレイヤーが設定されていない場合は攻撃しない
        if (m_Player == null) return;
        
        //各攻撃用タイマーを更新
        UpdateTimer();
        
        //現在のフェーズに応じて攻撃
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
    /// 各攻撃用タイマーを更新
    /// </summary>
    private void UpdateTimer()
    {
        m_NormalTimer += Time.deltaTime;
        m_FanTimer += Time.deltaTime;
        m_HomingTimer += Time.deltaTime;
    }

    /// <summary>
    /// 攻撃フェーズを変更する
    /// </summary>
    /// <param name="phase">変更先のフェーズ</param>
    public void SetPhase(EnemyPhase phase)
    {
        //現在と同じフェーズの場合は変更しない
        if (m_CurrentPhase == phase) return;
        
        //攻撃フェーズを変更
        m_CurrentPhase = phase;
        
        //攻撃タイマーをリセット
        ResetTimer();
    }

    /// <summary>
    /// 全ての攻撃タイマーをリセットする
    /// </summary>
    private void ResetTimer()
    {
        m_NormalTimer = 0.0f;
        m_FanTimer = 0.0f;
        m_HomingTimer = 0.0f;
    }


    //----------------------
    //各フェーズの攻撃
    //----------------------


    /// <summary>
    /// 通常攻撃
    /// </summary>
    private void NormalAttack()
    {
        //敵が停止していない場合は攻撃しない
        if (m_NormalTimer < m_NormalInterval) return;

        //攻撃間隔に達していない場合
        if (m_FanTimer < m_FanInterval) return;
        
        //タイマーをリセット
        m_NormalTimer = 0.0f;

        //扇形弾を発射
        ShotNormal();
    }

    /// <summary>
    /// フェーズ1の扇形攻撃
    /// </summary>
    private void Phase1Attack()
    {
        //敵が停止していない場合は攻撃しない
        if (!m_EnemyController.IsStopping) return;

        //攻撃間隔に達していない場合
        if (m_FanTimer < m_FanInterval) return;

        //タイマーをリセット
        m_FanTimer = 0.0f;

        //扇形弾を発射
        ShotFan();
    }

    /// <summary>
    /// フェーズ2のホーミング攻撃
    /// </summary>
    private void Phase2Attack()
    {
        //攻撃間隔に達していない場合
        if (m_HomingTimer < m_HomingInterval) return;
        
        //タイマーをリセット
        m_HomingTimer = 0.0f;

        //ホーミング弾を発射
        ShotHoming();
    }

    /// <summary>
    /// プレイヤー方向へのベクトルを取得する
    /// </summary>
    /// <returns>プレイヤーへの正規化された方向ベクトル</returns>
    private Vector2 GetPlayerDirection()
    {
        return (m_Player.position - m_FirePoint.position).normalized;
    }

    /// <summary>
    /// 通常弾発射
    /// </summary>
    private void ShotNormal()
    {
        //敵の発射SEを再生
        SEManager.Instance.SEPlay(SEType.SHOT_ENEMY);

        //通常弾を生成
        BulletManager.CreateBullet<EnemyBullet>(
            m_FirePoint.position,
            GetPlayerDirection(),
            m_BulletSpeed,
            m_NormalBulletSprite);
    }

    /// <summary>
    /// 扇形弾発射
    /// </summary>
    private void ShotFan()
    {
        //敵の発射SEを再生
        SEManager.Instance.SEPlay(SEType.SHOT_ENEMY);

        //奇数発と偶数発を交互に切り替え
        int bulletCount = m_IsOddShot ? ODD_BULLET_COUNT : EVEN_BULLET_COUNT;

        //扇形の開始角度を計算
        float startAngle = -FAN_ANGLE * 0.5f;
        
        //弾と弾の間隔を計算
        float angleStep = FAN_ANGLE / (bulletCount - 1);

        //偶数発の場合は中心から少しずらす
        if (!m_IsOddShot)
        {
            startAngle += angleStep * 0.5f;
        }

        //プレイヤー方向を基準にする
        Vector2 baseDirection = GetPlayerDirection();

        //指定された数の弾を発射
        for (int i = 0; i < bulletCount; i++)
        {
            //各弾の角度を計算
            float angle = startAngle + angleStep * i;

            //基準方向を指定角度だけ回転
            Vector2 direction = Quaternion.Euler(0, 0, angle) * baseDirection;

            //弾を生成
            BulletManager.CreateBullet<EnemyBullet>(
                m_FirePoint.position,
                direction,
                m_BulletSpeed,
                m_NormalBulletSprite);
        }

        //次回の発射数を切り替える
        m_IsOddShot = !m_IsOddShot;
    }

    /// <summary>
    /// ホーミング弾発射
    /// </summary>
    private void ShotHoming()
    {
        //ホーミング弾のSEを再生
        SEManager.Instance.SEPlay(SEType.HOMING);

        //ホーミング弾を生成
        HomingBullet bullet =
            BulletManager.CreateBullet<HomingBullet>(
                m_FirePoint.position,
                GetPlayerDirection(),
                m_BulletSpeed,
                m_HomingBulletSprite);

        //追尾対象をプレイヤーに設定
        bullet.SetTarget(m_Player);
    }
}
