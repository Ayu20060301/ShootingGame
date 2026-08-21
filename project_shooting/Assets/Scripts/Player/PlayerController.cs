using UnityEngine;
using UnityEngine.InputSystem;

//プレイヤー制御クラス
public class PlayerController : MonoBehaviour
{

    [Header("移動設定")]
    [SerializeField]
     private float m_MoveSpeed = 5.0f; //移動速度
    [SerializeField]
    private float m_SlowMoveSpeedRatio = 0.5f; //低速移動時の移動倍率

    [Header("移動範囲")]
    float m_XLimit = 8.0f;
    float m_YLimit = 4.5f;

    private Rigidbody2D m_Rigidbody2D;
    private Transform m_CachedTransform;
    private Vector2 m_MoveInput;
    private bool m_IsSlowMode;  //低速移動の切り替え

    [Header("弾の設定")]
    [SerializeField]
    private Sprite m_BulletSprite; //弾の見た目
    [SerializeField]
    private Transform m_MuzzlePoint;
    [SerializeField]
    private float m_ShootInterval = 0.1f;
    [SerializeField]
    private float m_BulletSpeed = 10.0f; //弾の速度
    private float m_ShootTimer;
    private bool m_IsShooting; //発射しているかどうか

    [SerializeField]
    private PlayerBombController m_BombController;

    private void Start()
    {
        m_CachedTransform = this.transform;
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (!GameManager.Instance.isActive) return;

        //発射間隔のクールタイムを更新する
        HandleShootTimer();

        //射撃入力中かどうかを見て、発射処理を呼び出す
        HandleShooting();

    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.isActive) return;

        //移動処理
        Move();
    }

    /// <summary>
    /// プレイヤー移動処理
    /// </summary>
    private void Move()
    {
        float moveSpeed = GetCurrentMoveSpeed();

        Vector2 nextPos =
            m_Rigidbody2D.position + 
            m_MoveInput * moveSpeed *
            Time.fixedDeltaTime;

        //画面外に出ないように制限
        nextPos.x = 
            Mathf.Clamp(nextPos.x, - m_XLimit,m_XLimit);
        nextPos.y = 
            Mathf.Clamp(nextPos.y, -m_YLimit,m_YLimit);

        m_Rigidbody2D.MovePosition(nextPos);
    }

    /// <summary>
    /// 現在の移動速度を取得
    /// </summary>
    /// <returns></returns>
    private float GetCurrentMoveSpeed()
    {
        return m_IsSlowMode
            ? m_MoveSpeed * m_SlowMoveSpeedRatio
            : m_MoveSpeed;
    }


    /// <summary>
    /// 発射間隔のクールタイムを更新する
    /// </summary>
    private void HandleShootTimer()
    {
        if (m_ShootTimer <= 0.0f) return;

        m_ShootTimer -= Time.deltaTime;
    }

    /// <summary>
    /// 射撃入力中かどうかを見て、発射処理を呼び出す
    /// </summary>
    private void HandleShooting()
    {
        if (!m_IsShooting) return;

        Shoot();
    }

    /// <summary>
    /// プレイヤー弾を生成する
    /// </summary>
    private void Shoot()
    {
        if (m_ShootTimer > 0.0f) return;

        SEManager.Instance.SEPlay(SEType.SHOT_PLAYER);

        Vector3 spawnPos = m_MuzzlePoint != null ?
            m_MuzzlePoint.position : m_CachedTransform.position;

        BulletManager.CreateBullet<PlayerBullet>(spawnPos,
            Vector2.right, m_BulletSpeed, m_BulletSprite);

        m_ShootTimer = m_ShootInterval;
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    /// <param name="context"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        //"Move"アクションの値を反映
        m_MoveInput = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// 低速移動入力
    /// </summary>
    /// <param name="context"></param>
    public void OnSlowMode(InputAction.CallbackContext context)
    {

        if (context.started)
        {
            m_IsSlowMode = true;
        }
        else if(context.canceled)
        {
            m_IsSlowMode = false;
        }
    }

    /// <summary>
    /// 射撃入力
    /// </summary>
    /// <param name="context"></param>
    public void OnShoot(InputAction.CallbackContext context)
    {
        if (Time.timeScale == 0.0f) return;

        if ((context.started))
        {
            m_IsShooting = true;
        }
        else if(context.canceled)
        {
            m_IsShooting = false;
        }
    }

    /// <summary>
    /// ボム入力
    /// </summary>
    /// <param name="context"></param>
    public void OnBomb(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        if (!GameManager.Instance.isActive) return;

        m_BombController.UseBomb();
    }
}
