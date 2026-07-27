using Unity.VisualScripting.FullSerializer;
using UnityEngine;

//ホーミング弾
public class HomingBullet : BulletBase
{
    [Header("ホーミング設定")]
    [SerializeField]
    private float m_RotateSpeed = 180.0f;  //1秒あたりの回転速度

    [SerializeField]
    private float m_MaxHomingAngle = 30.0f; //最大誘導角

    [SerializeField]
    private float m_MaxHomingTime = 15.0f; //誘導時間

    private Transform m_Target;

    private Vector2 m_StartDirection;
    private float m_HomingTimer;

    private bool m_IsHoming = false;

    /// <summary>
    /// ターゲットの設定
    /// </summary>
    /// <param name="target">対象プレイヤー</param>
    public void SetTarget(Transform target)
    {
        m_Target = target;
    }

    public override void Initialize(Vector3 position, Vector2 direction, float speed)
    {
        base.Initialize(position, direction, speed);

        m_StartDirection = direction.normalized;
        m_HomingTimer = 0.0f;
        m_IsHoming = true;
    }

    protected override void Move()
    {
        if(m_IsHoming && m_Target != null)
        {
            m_HomingTimer += Time.deltaTime;

            //15秒経過で誘導終了
            if(m_HomingTimer >= m_MaxHomingTime)
            {
                m_IsHoming = false;
            }
            else
            {
                Vector2 targetDir =
                    ((Vector2)m_Target.position - (Vector2)transform.position).normalized;

                float maxRotate =
                    m_RotateSpeed * Mathf.Deg2Rad * Time.deltaTime;

                m_Direction =
                    Vector3.RotateTowards(
                        m_Direction,
                        targetDir,
                        maxRotate,
                        0.0f
                        );

                //発射方向から30°以上曲がったら誘導終了

                float angle = Vector2.Angle(m_StartDirection, m_Direction);

                if(angle >= m_MaxHomingAngle)
                {
                    m_IsHoming = false;
                }
            }



        }

        //前進
        m_CashedTransform.position +=
            (Vector3)(m_Direction * m_Speed * Time.deltaTime);
    }

    protected override void OnHit(Collider2D other)
    {
        //プレイヤーに当たったら消す
        if(other.CompareTag("Player"))
        {
            if (other.TryGetComponent<PlayerFlash>(out var playerFlash))
            {
                playerFlash.BulletHit();
            }

            if (other.TryGetComponent<LifeUI>(out var lifeUI))
            {
                lifeUI.LoseLife();
            }
            Despawn();
        }
    }
}
