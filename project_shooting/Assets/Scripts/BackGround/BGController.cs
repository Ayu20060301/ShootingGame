using UnityEngine;

public class BGController : MonoBehaviour
{
    [SerializeField]
    private float m_Speed = 1.0f; //スクロール速度

    private void Update()
    {
        //背景を左へ移動
        this.transform.position -= new Vector3(Time.deltaTime * m_Speed, 0);

        //画面外へ出たら右側へ戻す
        if (this.transform.position.x <= -19.0f)
        {
            this.transform.position = new Vector3(19.0f, 0);
        }
    }
}
