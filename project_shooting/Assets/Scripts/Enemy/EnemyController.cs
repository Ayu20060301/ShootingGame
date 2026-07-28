using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Rendering;

//敵の移動制御
public class EnemyController : MonoBehaviour
{
    [Header("通常移動設定")]
    [SerializeField]
    private float m_MoveDistance = 3.0f; //上下に動く距離

    [SerializeField]
    private float m_MoveSpeed = 1.0f; //移動速度

    private Transform m_CachedTransform;
    private Vector3 m_StartPosition;  //敵の開始位置

    //現在のフェーズ
    private EnemyAttackController.EnemyPhase m_CurrentPhase = EnemyAttackController.EnemyPhase.NORMAL;


    [SerializeField]
    private float m_MoveTime = 3.0f; //移動時間

    [SerializeField]
    private float m_StopTime = 2.0f; //停止時間

    private float m_MoveTimer;
    private float m_Phase1Timer = 0.0f;  //フェーズ1の移動・停止時間を管理するタイマー
    private bool m_IsStopping = false;   //停止中かどうか

    /// <summary>
    /// 現在停止中か取得
    /// </summary>
    /// <returns></returns>
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
        //開始位置を保存
        m_StartPosition = m_CachedTransform.position;

    }

    private void Update()
    {
        //段階ごとの移動処理
        MovePhase();
    }


    public void SetPhase(EnemyAttackController.EnemyPhase phase)
    {
        if (m_CurrentPhase == phase) return;

        m_CurrentPhase = phase;

        //フェーズ変更時に状態リセット
        m_Phase1Timer = 0.0f;
        m_IsStopping = false;

        //現在位置を新しい開始位置にする
        m_StartPosition = m_CachedTransform.position;

    }

    /// <summary>
    /// 段階ごとの移動処理
    /// </summary>
    private void MovePhase()
    {
        switch(m_CurrentPhase)
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
    /// 上下移動(通常段階)
    /// </summary>
    private void NormalMove()
    {
        float newY = m_StartPosition.y +
            Mathf.Sin(Time.time * m_MoveSpeed) * m_MoveDistance;

        m_CachedTransform.position = new Vector3(
            m_StartPosition.x,
            newY,
            m_StartPosition.z
            );
    }

    /// <summary>
    /// フェーズ1段階の移動処理
    /// </summary>
    private void Phase1Move()
    {
        //移動・停止時間を計測
        m_Phase1Timer += Time.deltaTime;

        if(m_IsStopping)
        {
            //停止時間経過で再び移動開始
            if(m_Phase1Timer >= m_StopTime)
            {
                m_IsStopping = false;
                m_Phase1Timer = 0.0f;
            }

            return;
        }

        //現在の移動速度
        float currentMoveSpeed = m_MoveSpeed;

        //停止時間の直前だけ速度アップ
        if(m_Phase1Timer >= m_MoveTime * 0.7f)
        {
            currentMoveSpeed *= 3.0f;
        }


        //-----移動処理-----

        //移動中だけ時間を進める
        m_MoveTimer += Time.deltaTime * currentMoveSpeed;

        //上下移動
        float newY = m_StartPosition.y +
           Mathf.Sin(m_MoveTimer) * m_MoveDistance;

        m_CachedTransform.position = new Vector3(
            m_StartPosition.x,
            newY,
            m_StartPosition.z
            );

        //一定時間経過したら停止
        if (m_Phase1Timer >= m_MoveTime)
        {
            m_IsStopping = true;
            m_Phase1Timer = 0.0f;
        }
    }

    /// <summary>
    /// フェーズ2段階の移動処理
    /// </summary>
    private void Phase2Move()
    {
       
    }
}
