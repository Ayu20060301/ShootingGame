using UnityEditor.Rendering.LookDev;
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

    private Transform m_CashedTransform;
    private Vector2 m_MoveInput;
    private bool m_IsSlowMode;  //低速移動の切り替え

    private void Awake()
    {
        m_CashedTransform = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //移動処理
        Move();
    }


    /// <summary>
    /// 入力に応じて移動させる
    /// </summary>
    private void Move()
    {
        float speed = m_IsSlowMode ? m_MoveSpeed * m_SlowMoveSpeedRatio : m_MoveSpeed;
        Vector3 delta = (Vector3)m_MoveInput * speed * Time.deltaTime;
        m_CashedTransform.position += delta;
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
}
