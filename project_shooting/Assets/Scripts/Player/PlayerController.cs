using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// プレイヤー制御クラス
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("移動設定")]
    [SerializeField]
    private float m_MoveSpeed = 5.0f;   //移動速度
    [SerializeField]
    private float m_SlowMoveSpeedRatio = 0.5f; //低速移動時の移動倍率

    //X座標とY座標の上限
    float m_XLimit = 8.0f;
    float m_YLimit = 4.5f;

    private Rigidbody2D m_Rigidbody2D;
    private Transform m_CashedTransform;
    private Vector2 m_MoveInput;
    private bool m_IsSlowMode;  //低速移動の切り替え

    [Header("弾の設定")]
    [SerializeField]
    private Sprite m_BulletSprite; //弾の見た目
    [SerializeField]
    private Transform m_MuzzlePoint;
    [SerializeField]
    private float m_ShotInterval = 0.1f;
    [SerializeField]
    private float m_BulletSpeed = 10.0f; //弾の速度
    private float m_ShotTimer;
    private bool m_IsShooting;


    [Header("被弾時の点滅設定")]
    [SerializeField]
    private SpriteRenderer m_SpriteRenderer;
    [SerializeField]
    private float m_FlashInterval = 0.1f; //点滅の間隔
    [SerializeField]
    int m_LoopCount; //ループカウント
    private PolygonCollider2D m_PolygonCollider2D; //コライダーをON/OFFするためのPolygonCollider2D
    private bool m_IsHit;   //当たったかどうかのフラグ
    

    private void Start()
    {
        m_CashedTransform = this.transform;
        m_Rigidbody2D = GetComponent<Rigidbody2D>();   
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_PolygonCollider2D = GetComponent<PolygonCollider2D>();
    }


    // Update is called once per frame
    void Update()
    {
      
        //発射間隔のクールタイムを更新する
        HandleShootTimer();

        //射撃入力中かどうかを見て、発射処理を呼び出す
        HandleShooting();
    }

    private void FixedUpdate()
    {
        Move();
    }

    /// <summary>
    /// 入力に応じて移動させる
    /// </summary>
    private void Move()
    {
        float speed = m_IsSlowMode
         ? m_MoveSpeed * m_SlowMoveSpeedRatio
         : m_MoveSpeed;

        Vector2 nextPos =
            m_Rigidbody2D.position + m_MoveInput * speed * Time.fixedDeltaTime;

        // 画面外に出ないよう制限
        nextPos.x = Mathf.Clamp(nextPos.x, -m_XLimit, m_XLimit);
        nextPos.y = Mathf.Clamp(nextPos.y, -m_YLimit, m_YLimit);

        m_Rigidbody2D.MovePosition(nextPos);
    }

    /// <summary>
    /// 発射間隔のクールタイムを更新する
    /// </summary>
    private void HandleShootTimer()
    {
        if(m_ShotTimer > 0.0f)
        {
            m_ShotTimer -= Time.deltaTime;
        }
    }

    /// <summary>
    /// 射撃入力中かどうかを見て、発射処理を呼び出す
    /// </summary>
    private void HandleShooting()
    {
        if(m_IsShooting)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (m_ShotTimer > 0.0f) return;

        Vector3 spawnPos = m_MuzzlePoint != null ? m_MuzzlePoint.position : m_CashedTransform.position;
        BulletManager.CreateBullet<PlayerBullet>(spawnPos, Vector2.right, m_BulletSpeed, m_BulletSprite);

        m_ShotTimer = m_ShotInterval;
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
    /// 低速移動の切り替え
    /// </summary>
    /// <param name="context"></param>
    public void OnSlowMode(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            m_IsSlowMode = true;
        }
        else if(context.canceled)
        {
            m_IsSlowMode = false;
        }
    }

    /// <summary>
    /// 弾の発射処理
    /// </summary>
    /// <param name="context"></param>
    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            m_IsShooting = true;
        }
        else if(context.canceled)
        {
            m_IsShooting = false;
        }
    }

    //当たった時の処理
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Hitしていたら処理を行わない
        if (m_IsHit) return;

        //コルーチンを開始
        StartCoroutine(HitCoroutine());
    }

    //点滅させる処理
    private IEnumerator HitCoroutine()
    {
        //当たりフラグをtrueに変更
        m_IsHit = true;

        //点滅ループ開始
        for (int i = 0; i < m_LoopCount; i++)
        {
            yield return new WaitForSeconds(m_FlashInterval);
            //spriteRendererをオフ
            m_SpriteRenderer.enabled = false;
        }

        //点滅ループが抜けたら当たりフラグをfalse
        m_IsHit = false;
    }
}
