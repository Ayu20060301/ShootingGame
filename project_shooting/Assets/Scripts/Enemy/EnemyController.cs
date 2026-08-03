using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Rendering;

//敵の移動制御
public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private float m_MoveDistance = 3.0f; //上下に動く距離
    [SerializeField]
    private float m_MoveSpeed = 1.0f; //移動速度
    [SerializeField]
    private float m_MoveDuration = 3.0f;//移動時間
    [SerializeField]
    private float m_StopDuration = 2.0f; //瞬間移動後の停止時間
    [SerializeField]
    private float m_StopTimer = 0.0f; //停止時間
    [SerializeField]
    private float m_ElapsedTime = 0.0f; //移動経過時間
    [SerializeField]
    private float m_MoveTime = 0.0f;  
    [SerializeField]
    private bool m_IsStopping = false; //停止しているか
    [SerializeField]
    private Transform m_CachedTransform; //敵のtransformキャッシュ
    [SerializeField]
    private Vector3 m_StartPosition;  //移動基準位置
    [SerializeField]
    private Vector3 m_HomePosition; //初期位置
    [SerializeField]
    private bool m_IsReturning = false; //戻り中かどうかのフラグ
    [SerializeField]
    private float m_TargetChangeTime = 1.5f; //次の目標までの時間
    [SerializeField]
    private Vector3 m_Phase2TargetPosition; 
    [SerializeField]
    private float m_Phase2MoveSpeed = 10.0f;  //フェーズ2段階の移動速度
    private EnemyAttackController.EnemyPhase m_CurrentPhase = EnemyAttackController.EnemyPhase.NORMAL;  //現在のフェーズ
    private EnemyAttackController.EnemyPhase m_NextPhase;  //現在のフェーズ

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
        m_HomePosition = m_CachedTransform.position;
        m_StartPosition = m_HomePosition;
    }

    private void Update()
    {

        if (!GameManager.Instance.isActive) return;


        if(m_IsReturning)
        {
            ReturnToHome();
            return;
        }

        //段階ごとの移動処理
        MovePhase();
    }


    public void SetPhase(EnemyAttackController.EnemyPhase phase)
    {
        //同じフェーズなら処理をしない
        if (m_CurrentPhase == phase) return;

        m_NextPhase = phase;
        m_IsReturning = true;
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
        m_ElapsedTime += Time.deltaTime;

        //停止中かどうか
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


        //停止前だけ加速
        if (m_ElapsedTime >= m_MoveDuration * 0.7f)
        {
            speed *= 3.0f;
        }


        m_MoveTime += Time.deltaTime * speed;


        float newY = m_StartPosition.y +
            Mathf.Sin(m_MoveTime) *
            m_MoveDistance;


        SetPositionY(newY);


        if (m_ElapsedTime >= m_MoveDuration)
        {
            m_IsStopping = true;
            m_ElapsedTime = 0.0f;
        }
    }

    /// <summary>
    /// フェーズ2段階の移動処理
    /// </summary>
    private void Phase2Move()
    {
        m_CachedTransform.position =
            Vector3.MoveTowards(
                m_CachedTransform.position,
                m_Phase2TargetPosition,
                m_Phase2MoveSpeed * Time.deltaTime
                );


        //目的地に到着したら次の位置を設定
        if(Vector3.Distance(
            m_CachedTransform.position,
            m_Phase2TargetPosition) < 0.01f)
        {
            SetRandomTarget();
        }
    }

    /// <summary>
    /// Y座標だけ変更
    /// </summary>
    /// <param name="y">y座標</param>
    private void SetPositionY(float y)
    {
        m_CachedTransform.position =
            new Vector3(
                m_StartPosition.x,
                y,
                m_StartPosition.z
                );
    }

    /// <summary>
    /// ランダムな移動先を設定
    /// </summary>
    private void SetRandomTarget()
    {
        float randomY = Random.Range(
            -m_MoveDuration,
            m_MoveDuration
            );

        m_Phase2TargetPosition =
            new Vector3(
                m_StartPosition.x,
                m_StartPosition.y + randomY,
                m_StartPosition.z
                );
    }


    private void ReturnToHome()
    {
        m_CachedTransform.position =
            Vector3.MoveTowards(
               m_CachedTransform.position,
               m_HomePosition,
               m_Phase2MoveSpeed * Time.deltaTime
                );

        if(Vector3.Distance(m_CachedTransform.position, m_HomePosition) < 0.01f)
        {
            m_CachedTransform.position = m_HomePosition;

            m_IsReturning = false;
            m_CurrentPhase = m_NextPhase;


            //各タイマーをリセット
            m_StartPosition = m_HomePosition;
            m_ElapsedTime = 0.0f;
            m_MoveTime = 0.0f;
            m_StopTimer = 0.0f;
            m_IsStopping = false;

            if(m_CurrentPhase == EnemyAttackController.EnemyPhase.PHASE2)
            {
                SetRandomTarget();
            }
        }
    }
}
