using UnityEngine;
using UnityEngine.InputSystem;

//プレイヤー制御クラス
public class PlayerController : MonoBehaviour
{
    [Header("通常時の移動速度")]
    [SerializeField]
     private float m_MoveSpeed = 5.0f;

    [Header("低速移動時の速度倍率")]
    [SerializeField]
    private float m_SlowMoveSpeedRatio = 0.5f;

    //X方向の移動可能範囲
    private float m_XLimit = 8.0f;
    
    //Y方向の移動可能範囲
    private float m_YLimit = 4.5f;

    //プレイヤーのRigidbody2D
    private Rigidbody2D m_Rigidbody2D;
    
    //Transformをキャッシュ
    private Transform m_CachedTransform;
    
    //現在の移動入力
    private Vector2 m_MoveInput;
    
    //低速移動の切り替え
    private bool m_IsSlowMode;


    //---------------
    //弾の設定
    //---------------

    [Header("弾の見た目")]
    [SerializeField]
    private Sprite m_BulletSprite;

    [Header("弾の発射位置")]
    [SerializeField]
    private Transform m_MuzzlePoint;

    [Header("弾の発射間隔")]
    [SerializeField]
    private float m_ShotInterval = 0.1f;

    [Header("弾の移動速度")]
    [SerializeField]
    private float m_BulletSpeed = 10.0f;

    //次に弾を発射できるまでの時間
    private float m_ShotTimer;
    
    //射撃ボタンを押しているかどうか
    private bool m_IsShoting;

    [Header("ボム関連のスクリプト")]
    [SerializeField]
    private PlayerBombController m_BombController;

    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Start()
    {
        //Transformを取得してキャッシュ
        m_CachedTransform = this.transform;

        //Rigidbody2Dを取得
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// 毎フレームの射撃処理
    /// </summary>
    private void Update()
    {
        //ゲーム停止中なら処理をしない
        if (!GameManager.Instance.isActive) return;

        //発射間隔のクールタイムを更新する
        HandleShotTimer();

        //射撃入力中かどうかを見て、発射処理を呼び出す
        HandleShoting();
    }

    /// <summary>
    /// 物理演算に合わせた移動処理
    /// </summary>
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
        Vector2 nextPos = m_Rigidbody2D.position + m_MoveInput * moveSpeed * Time.fixedDeltaTime;

        //-----------------------
        //画面外に出ないように制限
        //-----------------------

        //x方向の移動範囲を制限
        nextPos.x = Mathf.Clamp(nextPos.x, - m_XLimit,m_XLimit);
        
        //y方向の移動範囲を制限
        nextPos.y = Mathf.Clamp(nextPos.y, -m_YLimit,m_YLimit);

        //計算した位置へ移動
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
    private void HandleShotTimer()
    {
        //クールタイムが終了している場合は何もしない
        if (m_ShotTimer <= 0.0f) return;

        ///次に発射できるまでの時間を減らす
        m_ShotTimer -= Time.deltaTime;
    }

    /// <summary>
    /// 射撃入力中かどうかを見て、発射処理を呼び出す
    /// </summary>
    private void HandleShoting()
    {
        //射撃ボタンが押されていない場合は何もしない
        if (!m_IsShoting) return;

        //弾を発射
        Shot();
    }

    /// <summary>
    /// プレイヤー弾を生成する
    /// </summary>
    private void Shot()
    {
        //まだ発射できない場合
        if (m_ShotTimer > 0.0f) return;

        //射撃SEを再生
        SEManager.Instance.SEPlay(SEType.SHOT_PLAYER);

        //発射位置を決定
        Vector3 spawnPos = m_MuzzlePoint != null ? m_MuzzlePoint.position : m_CachedTransform.position;

        //プレイヤー弾を生成
        BulletManager.CreateBullet<PlayerBullet>(spawnPos,Vector2.right, m_BulletSpeed, m_BulletSprite);

        //次の発射までクールタイムを設定
        m_ShotTimer = m_ShotInterval;
    }

    /// <summary>
    /// 移動入力を受け取る
    /// </summary>
    /// <param name="context">入力情報</param>
    public void OnMove(InputAction.CallbackContext context)
    {
        //"Move"アクションの値を反映
        m_MoveInput = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// 低速移動の入力を受け取る
    /// </summary>
    /// <param name="context">入力情報</param>
    public void OnSlowMode(InputAction.CallbackContext context)
    {
        //ボタンを押した場合
        if (context.started)
        {
            m_IsSlowMode = true;
        }

        //ボタンを離した場合
        else if(context.canceled)
        {
            m_IsSlowMode = false;
        }
    }

    /// <summary>
    /// 射撃入力を受け取る
    /// </summary>
    /// <param name="context">入力情報</param>
    public void OnShot(InputAction.CallbackContext context)
    {
        if (Time.timeScale == 0.0f) return;

        //射撃ボタンを押した場合
        if ((context.started))
        {
            m_IsShoting = true;
        }

        //射撃ボタンを離した場合
        else if(context.canceled)
        {
            m_IsShoting = false;
        }
    }

    /// <summary>
    /// ボム入力を受け取る
    /// </summary>
    /// <param name="context">入力情報</param>
    public void OnBomb(InputAction.CallbackContext context)
    {
        if (Time.timeScale == 0.0f) return;

        //ボタンを押した瞬間以外は処理をしない
        if (!context.started) return;

        //ボムを使用
        m_BombController.UseBomb();
    }
}
