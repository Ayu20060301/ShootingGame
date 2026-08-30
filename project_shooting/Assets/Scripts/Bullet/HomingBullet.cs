using Unity.VisualScripting.FullSerializer;
using UnityEngine;

//ホーミング弾
public class HomingBullet : BulletBase
{
    //1秒あたりの回転速度
    private float m_RotateSpeed = 180.0f;
    
    //最大誘導角
    private float m_MaxHomingAngle = 30.0f;
    
    //誘導時間
    private float m_MaxHomingTime = 15.0f;
    
    //追尾対象
    private Transform m_Target;
    
    //発射時の進行方向
    private Vector2 m_StartDirection;
    
    //ホーミング開始からの経過時間
    private float m_HomingTimer;
    
    //現在ホーミング中か
    private bool m_IsHoming = false;
   
    /// <summary>
    /// ターゲットの設定
    /// </summary>
    /// <param name="target">追尾対象</param>
    public void SetTarget(Transform target)
    {
        m_Target = target;
    }

    /// <summary>
    /// 弾を初期化する
    /// </summary>
    /// <param name="position"></param>
    /// <param name="direction"></param>
    /// <param name="speed"></param>
    public override void Initialize(Vector3 position, Vector2 direction, float speed)
    {
        base.Initialize(position, direction, speed);

        //発射時の方向を保存
        m_StartDirection = direction.normalized;
        
        //ホーミング時間をリセット
        m_HomingTimer = 0.0f;

        //ホーミング開始
        m_IsHoming = true;
    }

    /// <summary>
    /// 弾を移動する
    /// </summary>
    protected override void Move()
    {
        if (!GameManager.Instance.isActive) return;

        //ホーミング処理
        UpdateHoming();

        //現在の方向へ移動
        MoveForward();
    }

    /// <summary>
    /// プレイヤーがホーミング弾に衝突した際の処理
    /// </summary>
    /// <param name="other">プレイヤーのCollider</param>
    protected override void OnHit(Collider2D other)
    {
        //プレイヤーに当たったら消す
        if (!other.CompareTag("Player")) return;
        
        //被弾演出
        if (other.TryGetComponent<PlayerFlash>(out var playerFlash))
        {
            playerFlash.BulletHit();
        }

        //プレイヤーのライフを減らす
        if (other.TryGetComponent<PlayerLifeUI>(out var lifeUI))
        {
            lifeUI.LoseLife();
        }

        //弾を消す
        Despawn();
    }

    /// <summary>
    /// ホーミング処理を更新する
    /// </summary>
    private void UpdateHoming()
    {
        //ホーミング終了またはターゲットが存在しない場合
        if (!m_IsHoming || m_Target == null) return;

        //ホーミング時間を更新
        m_HomingTimer += Time.deltaTime;

        //指定時間を超えたら誘導終了
        if(m_HomingTimer >= m_MaxHomingTime)
        {
            StopHoming();
            return;
        }

        //ターゲットへの方向を取得
        Vector2 targetDirection =
            ((Vector2)m_Target.position -
             (Vector2)m_CachedTransform.position).normalized;

        //ターゲット方向へ旋回
        RotateTowards(targetDirection);

        //最大誘導角度を超えた場合
        if(IsExceededHomingAngle())
        {
            StopHoming();
        }
    }

    /// <summary>
    /// 指定した方向へ徐々に旋回する
    /// </summary>
    /// <param name="targetDirection">旋回先の方向</param>
    private void RotateTowards(Vector2 targetDirection)
    {
        //現在の進行方向の角度
        float currentAngle =
            Mathf.Atan2(
                m_Direction.y,
                m_Direction.x)
            * Mathf.Rad2Deg;

        //ターゲット方向の角度
        float targetAngle =
            Mathf.Atan2(
                targetDirection.y,
                targetDirection.x)
            * Mathf.Rad2Deg;

        //指定速度でターゲット方向へ旋回
        float nextAngle =
            Mathf.MoveTowardsAngle(
                currentAngle,
                targetAngle,
                m_RotateSpeed * Time.deltaTime);

        //角度を方向ベクトルへ変換
        float radians =
            nextAngle * Mathf.Deg2Rad;

        m_Direction =
            new Vector2(
                Mathf.Cos(radians),
                Mathf.Sin(radians));
    }

    /// <summary>
    /// 発射方向から最大誘導角度を超えたら判定する
    /// </summary>
    /// <returns></returns>
    private bool IsExceededHomingAngle()
    {
        float angle =
            Vector2.Angle(
                m_StartDirection,
                m_Direction);

        return angle >= m_MaxHomingAngle;
    }

    /// <summary>
    /// ホーミングを終了する
    /// </summary>
    private void StopHoming()
    {
        m_IsHoming = false;
    }

    /// <summary>
    /// 現在の進行方向へ移動する
    /// </summary>
    private void MoveForward()
    {
        m_Direction = m_Direction.normalized;

        m_CachedTransform.position +=
            (Vector3)(
            m_Direction *
            m_Speed *
            Time.deltaTime);
    }
}
