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
    float m_XLimit = 8.0f; //X方向の移動可能範囲
    float m_YLimit = 4.5f; //Y方向の移動可能範囲

    private Rigidbody2D m_Rigidbody2D;
    private Transform m_CachedTransform; //Transformをキャッシュ
    private Vector2 m_MoveInput; //現在の移動入力
    private bool m_IsSlowMode;  //低速移動の切り替え

    [Header("弾の設定")]
    [SerializeField]
    private Sprite m_BulletSprite; //弾の見た目
    [SerializeField]
    private Transform m_MuzzlePoint; //弾の発射位置
    [SerializeField]
    private float m_ShootInterval = 0.1f; //弾の発射間隔
    [SerializeField]
    private float m_BulletSpeed = 10.0f; //弾の移動速度
    private float m_ShootTimer;
    private bool m_IsShooting; //射撃ボタンを押しているかどうか

    [SerializeField]
    private PlayerBombController m_BombController;

    private void Start()
    {
        //Transformを取得してキャッシュ
        m_CachedTransform = this.transform;

        //Rigidbody2Dを取得
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        //ゲーム停止中なら処理をしない
        if (!GameManager.Instance.isActive) return;

        //発射間隔のクールタイムを更新する
        HandleShootTimer();

        //射撃入力中かどうかを見て、発射処理を呼び出す
        HandleShooting();
    }

    private void FixedUpdate()
    {
        //ゲームが停止中なら移動しない
        if (!GameManager.Instance.isActive) return;

        //移動処理
        Move();
    }

    /// <summary>
    /// プレイヤー移動処理
    /// </summary>
    private void Move()
    {
        //現在の状態に応じた移動速度を取得
        float moveSpeed = GetCurrentMoveSpeed();

        //現在位置から入力方向へ移動した次の座標を計算
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
    /// <returns>通常移動または低速移動時の速度</returns>
    private float GetCurrentMoveSpeed()
    {

        //低速移動中の場合は速度倍率を適用
        if(m_IsSlowMode)
        {
            return m_MoveSpeed * m_SlowMoveSpeedRatio;
        }

        //通常速度
        return m_MoveSpeed;
    }


    /// <summary>
    /// 発射間隔のクールタイムを更新する
    /// </summary>
    private void HandleShootTimer()
    {
        //クールタイムが終了している場合は何もしない
        if (m_ShootTimer <= 0.0f) return;

        ///次に発射できるまでの時間を減らす
        m_ShootTimer -= Time.deltaTime;
    }

    /// <summary>
    /// 射撃入力中かどうかを見て、発射処理を呼び出す
    /// </summary>
    private void HandleShooting()
    {
        //射撃ボタンが押されていない場合は何もしない
        if (!m_IsShooting) return;

        //弾を発射
        Shoot();
    }

    /// <summary>
    /// プレイヤー弾を生成する
    /// </summary>
    private void Shoot()
    {
        if (m_ShootTimer > 0.0f) return;

        //射撃SEを再生
        SEManager.Instance.SEPlay(SEType.SHOT_PLAYER);

        //発射位置を決定
        Vector3 spawnPos = m_MuzzlePoint != null ?
            m_MuzzlePoint.position : m_CachedTransform.position;

        //プレイヤー弾を生成
        BulletManager.CreateBullet<PlayerBullet>(spawnPos,
            Vector2.right, m_BulletSpeed, m_BulletSprite);

        //次の発射までクールタイムを設定
        m_ShootTimer = m_ShootInterval;
    }

    /// <summary>
    /// 移動入力を受け取る
    /// </summary>
    /// <param name="context"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        //"Move"アクションの値を反映
        m_MoveInput = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// 低速移動の入力を受け取る
    /// </summary>
    /// <param name="context"></param>
    public void OnSlowMode(InputAction.CallbackContext context)
    {
        //ボタンを押した瞬間
        if (context.started)
        {
            m_IsSlowMode = true;
        }

        //ボタンを離した瞬間
        else if(context.canceled)
        {
            m_IsSlowMode = false;
        }
    }

    /// <summary>
    /// 射撃入力を受け取る
    /// </summary>
    /// <param name="context"></param>
    public void OnShoot(InputAction.CallbackContext context)
    {
        if (Time.timeScale == 0.0f) return;

        //射撃ボタンを押した場合
        if ((context.started))
        {
            m_IsShooting = true;
        }

        //射撃ボタンを離した場合
        else if(context.canceled)
        {
            m_IsShooting = false;
        }
    }

    /// <summary>
    /// ボム入力を受け取る
    /// </summary>
    /// <param name="context"></param>
    public void OnBomb(InputAction.CallbackContext context)
    {
        //ボタンを押した瞬間以外は処理をしない
        if (!context.started) return;

        //ゲームが停止中の場合は使用しない
        if (!GameManager.Instance.isActive) return;

        //ボムを使用
        m_BombController.UseBomb();
    }
}
