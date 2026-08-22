using UnityEngine;

//背景を横移動にスクロールさせるクラス
public class BGController : MonoBehaviour
{
    [Header("背景のスクロール速度")]
    [SerializeField]
    private float m_Speed = 1.0f;

    [Header("背景を戻すX座標")]
    [SerializeField]
    private float m_ResetPositionX = -19.0f;

    [Header("背景を移動させるX座標")]
    [SerializeField]
    private float m_StartPositionX = 19.0f;

    private void Update()
    {
        //現在位置を取得
        Vector3 position = this.transform.position;

        //背景を左方向へ移動
        position.x -= m_Speed * Time.deltaTime;

        //指定した位置より左へ移動した場合
        if(position.x <= m_ResetPositionX)
        {
            position.x = m_StartPositionX;
        }

        //計算した座標を反映
        this.transform.position = position;
    }
}
