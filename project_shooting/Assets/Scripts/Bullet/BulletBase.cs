using UnityEngine;


/// <summary>
/// ’e‚Ì‹¤’Êˆ—‚ğ§Œä‚·‚éŠî’êƒNƒ‰ƒX
/// </summary>
public abstract class BulletBase : MonoBehaviour
{
    [SerializeField]
    protected float m_Speed = 10.0f;  //”­Ë‘¬“x
    [SerializeField]
    protected float m_LifeTime = 3.0f; //’e‚Ì¶‘¶ŠÔ

    protected Vector2 m_Direction = Vector2.right;
    protected Transform m_CashedTransform;
    private float m_Timer = 0.0f;

    
    private void Awake()
    {
        m_CashedTransform = this.transform;   
    }

    private void OnEnable()
    {
        m_Timer = 0.0f;
    }

    private void Update()
    {
        m_CashedTransform.position += (Vector3)(m_Direction * m_Speed * Time.deltaTime);

        m_Timer += Time.deltaTime;

        //¶‘¶ŠÔ‚ğ’´‚¦‚½ê‡’e‚ÍÁ‚¦‚é
        if(m_Timer >=m_LifeTime)
        {
            Despawn();
        }
    }

    /// <summary>
    /// ’e‚Ì‰Šú‰»
    /// </summary>
    /// <param name="position">”­ËˆÊ’u</param>
    /// <param name="direction">is•ûŒü</param>
    /// <param name="speed">’e‚Ì‘¬“x</param>
    public virtual void Initialize(Vector3 position, Vector2 direction,float speed)
    {
        m_CashedTransform.position = position;
        m_Direction = direction.normalized;
        m_Speed = speed;
        m_Timer = 0.0f;
    }


    /// <summary>
    /// ’e‚ÌˆÚ“®ˆ—
    /// </summary>
    protected virtual void Move()
    {
        m_CashedTransform.position += (Vector3)(m_Direction * m_Speed * Time.deltaTime);
    }

    /// <summary>
    /// ‹¤’Ê‚Ì“–‚½‚è”»’è
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        OnHit(other);
    }

    /// <summary>
    /// “–‚½‚Á‚½‚Ìˆ—
    /// </summary>
    /// <param name="other"></param>
    protected abstract void OnHit(Collider2D other);

    //’e‚ğÁ‚·
    protected virtual void Despawn()
    {
        gameObject.SetActive(false);
    }
}
