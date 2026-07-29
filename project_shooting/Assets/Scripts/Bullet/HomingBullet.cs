using Unity.VisualScripting.FullSerializer;
using UnityEngine;

//ホーミング弾
public class HomingBullet : BulletBase
{
    [Header("ホーミング設定")]
    private float m_HomingSpeed = 2.0f;
    private float m_RotateSpeed = 180.0f;  //1秒あたりの回転速度
    private float m_MaxHomingAngle = 30.0f; //最大誘導角
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
                    ((Vector2)m_Target.position -
                     (Vector2)transform.position).normalized;

                //発射方向との角度差
                float angle =
                    Vector2.Angle(m_StartDirection, targetDir);

                //±30°以上なら誘導終了
                if(angle > m_MaxHomingAngle)
                {
                    m_IsHoming = false;
                }
                else
                {
                    float currentAngle =
                        Mathf.Atan2(
                            m_Direction.y,
                            m_Direction.x)
                        * Mathf.Rad2Deg;

                    float targetAngle =
                         Mathf.Atan2(
                             targetDir.y,
                             targetDir.x)
                         * Mathf.Rad2Deg;

                    currentAngle =
                        Mathf.MoveTowardsAngle(
                            currentAngle,
                            targetAngle,
                            m_RotateSpeed * Time.deltaTime);

                    m_Direction =
                        new Vector2(
                            Mathf.Cos(currentAngle * Mathf.Rad2Deg),
                            Mathf.Sin(currentAngle * Mathf.Rad2Deg));
                }
            
            }
        }

        m_Direction = m_Direction.normalized;

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
