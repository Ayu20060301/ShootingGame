using UnityEngine;

/// <summary>
/// 敵の移動制御
/// </summary>
public class EnemyController : MonoBehaviour
{
    private const float PHASE1_ACCELERATION = 3.0f;          //加速倍率
    private const float PHASE1_ACCELERATION_START = 0.7f;    //加速開始割合
    private const float ARRIVAL_DISTANCE = 0.01f;            //到達判定距離

    [Header("通常移動")]
    [SerializeField]
    private float m_MoveDistance = 3.0f;

    [SerializeField]
    private float m_MoveSpeed = 1.0f;

    [Header("フェーズ1")]
    [SerializeField]
    private float m_MoveDuration = 3.0f;

    [SerializeField]
    private float m_StopDuration = 2.0f;

    [Header("フェーズ2")]
    [SerializeField]
    private float m_Phase2MoveSpeed = 10.0f;

    private float m_ElapsedTime;
    private float m_MoveTime;

    private bool m_IsStopping;
    private bool m_IsReturning;

    private Transform m_CachedTransform;

    private Vector3 m_StartPosition;
    private Vector3 m_HomePosition;
    private Vector3 m_Phase2TargetPosition;

    private EnemyAttackController.EnemyPhase m_CurrentPhase =
        EnemyAttackController.EnemyPhase.NORMAL;

    private EnemyAttackController.EnemyPhase m_NextPhase;

    /// <summary>
    /// 現在停止中か取得
    /// </summary>
    public bool IsStopping()
    {
        return m_IsStopping;
    }

    private void Awake()
    {
        m_CachedTransform = transform;
    }

    private void Start()
    {
        m_HomePosition = m_CachedTransform.position;
        m_StartPosition = m_HomePosition;
    }

    private void Update()
    {
        if (!GameManager.Instance.isActive)
        {
            return;
        }

        if (m_IsReturning)
        {
            ReturnToHome();
            return;
        }

        MovePhase();
    }

    /// <summary>
    /// フェーズ変更
    /// </summary>
    public void SetPhase(EnemyAttackController.EnemyPhase phase)
    {
        if (m_CurrentPhase == phase)
        {
            return;
        }

        m_NextPhase = phase;
        m_IsReturning = true;
    }

    /// <summary>
    /// 現在のフェーズに応じた移動
    /// </summary>
    private void MovePhase()
    {
        switch (m_CurrentPhase)
        {
            case EnemyAttackController.EnemyPhase.NORMAL:
                NormalMove();
                break;

            case EnemyAttackController.EnemyPhase.PHASE1:
                Phase1Move();
                break;

            case EnemyAttackController.EnemyPhase.PHASE2:
                Phase2Move();
                break;
        }
    }

    /// <summary>
    /// 通常移動
    /// </summary>
    private void NormalMove()
    {
        float newY =
            m_StartPosition.y +
            Mathf.Sin(Time.time * m_MoveSpeed) * m_MoveDistance;

        SetPositionY(newY);
    }

    /// <summary>
    /// フェーズ1移動
    /// </summary>
    private void Phase1Move()
    {
        m_ElapsedTime += Time.deltaTime;

        if (m_IsStopping)
        {
            if (m_ElapsedTime >= m_StopDuration)
            {
                m_IsStopping = false;
                m_ElapsedTime = 0.0f;
            }

            return;
        }

        float speed = m_MoveSpeed;

        if (m_ElapsedTime >=
            m_MoveDuration * PHASE1_ACCELERATION_START)
        {
            speed *= PHASE1_ACCELERATION;
        }

        m_MoveTime += Time.deltaTime * speed;

        float newY =
            m_StartPosition.y +
            Mathf.Sin(m_MoveTime) * m_MoveDistance;

        SetPositionY(newY);

        if (m_ElapsedTime >= m_MoveDuration)
        {
            m_IsStopping = true;
            m_ElapsedTime = 0.0f;
        }
    }

    /// <summary>
    /// フェーズ2移動
    /// </summary>
    private void Phase2Move()
    {
        m_CachedTransform.position =
            Vector3.MoveTowards(
                m_CachedTransform.position,
                m_Phase2TargetPosition,
                m_Phase2MoveSpeed * Time.deltaTime);

        if (Vector3.Distance(
            m_CachedTransform.position,
            m_Phase2TargetPosition) < ARRIVAL_DISTANCE)
        {
            SetRandomTarget();
        }
    }

    /// <summary>
    /// 帰還処理
    /// </summary>
    private void ReturnToHome()
    {
        m_CachedTransform.position =
            Vector3.MoveTowards(
                m_CachedTransform.position,
                m_HomePosition,
                m_Phase2MoveSpeed * Time.deltaTime);

        if (Vector3.Distance(
            m_CachedTransform.position,
            m_HomePosition) < ARRIVAL_DISTANCE)
        {
            CompleteReturn();
        }
    }

    /// <summary>
    /// 帰還完了
    /// </summary>
    private void CompleteReturn()
    {
        m_CachedTransform.position = m_HomePosition;

        m_IsReturning = false;
        m_CurrentPhase = m_NextPhase;

        m_StartPosition = m_HomePosition;

        ResetMoveState();

        if (m_CurrentPhase ==
            EnemyAttackController.EnemyPhase.PHASE2)
        {
            SetRandomTarget();
        }
    }

    /// <summary>
    /// 移動状態をリセット
    /// </summary>
    private void ResetMoveState()
    {
        m_ElapsedTime = 0.0f;
        m_MoveTime = 0.0f;
        m_IsStopping = false;
    }

    /// <summary>
    /// Y座標のみ変更
    /// </summary>
    private void SetPositionY(float y)
    {
        m_CachedTransform.position =
            new Vector3(
                m_StartPosition.x,
                y,
                m_StartPosition.z);
    }

    /// <summary>
    /// ランダムな移動先を設定
    /// </summary>
    private void SetRandomTarget()
    {
        float randomY = Random.Range(
            -m_MoveDistance,
            m_MoveDistance);

        m_Phase2TargetPosition =
            new Vector3(
                m_StartPosition.x,
                m_StartPosition.y + randomY,
                m_StartPosition.z);
    }
}