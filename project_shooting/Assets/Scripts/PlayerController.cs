using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

//*プレイヤー制御クラス*//

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float m_MoveSpeed = 5.0f;

    private Vector2 m_MoveInput;
    private Vector2 m_BombInput;

    // Update is called once per frame
    void Update()
    {
        this.transform.position += (Vector3)m_MoveInput * m_MoveSpeed * Time.deltaTime;
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    /// <param name="context"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        //"move"のリファレンスを追加
        m_MoveInput = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// 射撃処理
    /// </summary>
    /// <param name="context"></param>
    public void OnShoot(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            Debug.Log("ショット");
        }
    }
}
