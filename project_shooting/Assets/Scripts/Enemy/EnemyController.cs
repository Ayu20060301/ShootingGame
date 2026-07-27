using Unity.VisualScripting.FullSerializer;
using UnityEngine;

//敵の移動制御
public class EnemyController : MonoBehaviour
{

    [SerializeField]
    private float m_MoveDistance = 3.0f; //上下に動く距離

    [SerializeField]
    private float m_MoveSpeed = 1.0f; //移動速度

    private Transform m_CachedTransform;
    private Vector3 m_StartPosition;  //敵の開始位置

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
        Move();
    }

    /// <summary>
    /// 上下移動
    /// </summary>
    private void Move()
    {
        float newY = m_StartPosition.y +
            Mathf.Sin(Time.time * m_MoveSpeed) * m_MoveDistance;

        m_CachedTransform.position = new Vector3(

            m_StartPosition.x,
            newY,
            m_StartPosition.z
            );

    }
}
