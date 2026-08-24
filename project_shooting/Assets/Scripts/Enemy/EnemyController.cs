using UnityEngine;

//敵の移動制御
public class EnemyController : MonoBehaviour
{

    //フェーズ1で加速する際の速度倍率
    private const float PHASE1_ACCELERATION = 3.0f;          //加速倍率
    
    //フェーズ1の移動時間に対する加速開始位置
    private const float PHASE1_ACCELERATION_START = 0.7f;    //加速開始割合
    
    //目的地へ到達判定に使用する距離
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

    private float m_ElapsedTime; //現在の移動・停止時間
    private float m_MoveTime;    //サイン波移動に使用する経過時間

    private bool m_IsStopping;  //敵が停止中かどうか
    private bool m_IsReturning;  //元の位置に戻ったかどうか

    private Transform m_CachedTransform; //キャッシュしたTransform

    private Vector3 m_StartPosition;   //サイン波移動の基準となる座標
    private Vector3 m_HomePosition;    //敵が帰還する基準座標
    private Vector3 m_Phase2TargetPosition; //フェーズ2で移動する目的地

    //現在のフェーズ
    private EnemyAttackController.EnemyPhase m_CurrentPhase = 
        EnemyAttackController.EnemyPhase.NORMAL;

    //次に移行するフェーズ
    private EnemyAttackController.EnemyPhase m_NextPhase;

    /// <summary>
    /// 敵が停止中かどうか
    /// </summary>
    public bool IsStopping => m_IsStopping;

    /// <summary>
    /// 開始時の座標を保存
    /// </summary>
    private void Start()
    {
        //Transformをキャッシュ
        m_CachedTransform = transform;

        //現在位置をホームポジションとして保存
        m_HomePosition = m_CachedTransform.position;
        
        //通常移動の基準座標を設定
        m_StartPosition = m_HomePosition;
    }

    private void Update()
    {
        //ゲームが停止している場合は処理を行わない
        if (!GameManager.Instance.isActive) return;
       
        //フェーズ変更のため帰還中の場合
        if (m_IsReturning)
        {
            ReturnToHome();
            return;
        }

        //現在のフェーズに応じて移動
        MovePhase();
    }

   /// <summary>
   /// 敵の移動フェーズを変更する
   /// </summary>
   /// <param name="phase">変更先のフェーズ</param>
    public void SetPhase(EnemyAttackController.EnemyPhase phase)
    {
        //同じフェーズの場合は変更しない
        if (m_CurrentPhase == phase) return;
        
        //変更先のフェーズを変更
        m_NextPhase = phase;
        
        //一度元の位置へ戻る
        m_IsReturning = true;
    }

    /// <summary>
    /// 現在のフェーズに応じた移動を実行する
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

    //----------------
    //各フェーズの移動
    //----------------

    /// <summary>
    /// 通常移動
    /// </summary>
    private void NormalMove()
    {
        //サイン波を使用してY座標を計算
        float newY =
            m_StartPosition.y +
            Mathf.Sin(Time.time * m_MoveSpeed) * m_MoveDistance;

        //Y座標を更新
        SetPositionY(newY);
    }

    /// <summary>
    /// 加速と停止を繰り返すフェーズ1移動
    /// </summary>
    private void Phase1Move()
    {
        //経過時間を更新
        m_ElapsedTime += Time.deltaTime;

        //停止中の場合
        if (m_IsStopping)
        {
            //停止時間が終了した場合
            if (m_ElapsedTime >= m_StopDuration)
            {
                //再び移動を開始
                m_IsStopping = false;
                
                //経過時間をリセット
                m_ElapsedTime = 0.0f;
            }

            return;
        }

        //通常時の移動速度
        float speed = m_MoveSpeed;


        //指定時間を超えた場合は加速
        if (m_ElapsedTime >= m_MoveDuration * PHASE1_ACCELERATION_START)
        {
            speed *= PHASE1_ACCELERATION;
        }

        //移動用の経過時間を更新
        m_MoveTime += Time.deltaTime * speed;

        //サイン波を使用してY座標を計算
        float newY =m_StartPosition.y + Mathf.Sin(m_MoveTime) * m_MoveDistance;

        //Y座標を更新
        SetPositionY(newY);

        //移動時間が終了した場合
        if (m_ElapsedTime >= m_MoveDuration)
        {
            //敵を停止状態にする
            m_IsStopping = true;
            
            //停止時間計測のためリセット
            m_ElapsedTime = 0.0f;
        }
    }

    /// <summary>
    /// ランダムな目的地へ移動するフェーズ2の移動処理
    /// </summary>
    private void Phase2Move()
    {
        //目的地へ移動
        m_CachedTransform.position =
            Vector3.MoveTowards(
                m_CachedTransform.position,
                m_Phase2TargetPosition,
                m_Phase2MoveSpeed * Time.deltaTime);

        //目的地に到達した場合
        if (Vector3.Distance(m_CachedTransform.position,m_Phase2TargetPosition) < ARRIVAL_DISTANCE)
        {
            //次のランダムな目的地を設定
            SetRandomTarget();
        }
    }

    /// <summary>
    /// 帰還処理
    /// </summary>
    private void ReturnToHome()
    {
        //元の位置へ移動
        m_CachedTransform.position =
            Vector3.MoveTowards(
                m_CachedTransform.position,
                m_HomePosition,
                m_Phase2MoveSpeed * Time.deltaTime);

        //元に位置に到達した場合
        if (Vector3.Distance(m_CachedTransform.position,m_HomePosition) < ARRIVAL_DISTANCE)
        {
            CompleteReturn();
        }
    }

    /// <summary>
    /// 帰還完了後に正確な位置を設定
    /// </summary>
    private void CompleteReturn()
    {
        //座標の誤差を防ぐため正確な位置を設定
        m_CachedTransform.position = m_HomePosition;

        //帰還状態を解除
        m_IsReturning = false;
        
        //次のフェーズへ変更
        m_CurrentPhase = m_NextPhase;

        //移動の基準座標をリセット
        m_StartPosition = m_HomePosition;

        //移動状態を初期化
        ResetMoveState();

        //フェーズ2の場合は最初の目的地を設定
        if (m_CurrentPhase == EnemyAttackController.EnemyPhase.PHASE2)
        {
            SetRandomTarget();
        }
    }

    /// <summary>
    /// 移動状態をリセット
    /// </summary>
    private void ResetMoveState()
    {
        //経過時間をリセット
        m_ElapsedTime = 0.0f;
        
        //サイン波移動用の時間をリセット
        m_MoveTime = 0.0f;
        
        //停止状態を解除
        m_IsStopping = false;
    }

   /// <summary>
   /// 敵のY座標のみ変更する
   /// </summary>
   /// <param name="y">設定するY座標</param>
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
        //元の位置を基準にランダムなY座標を取得
        float randomY = Random.Range(
            -m_MoveDistance,
            m_MoveDistance);

        //新しい移動先を設定
        m_Phase2TargetPosition =
            new Vector3(
                m_StartPosition.x,
                m_StartPosition.y + randomY,
                m_StartPosition.z);
    }
}
