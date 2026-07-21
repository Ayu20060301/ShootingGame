using UnityEngine;

/// <summary>
/// 背景の制御
/// </summary>
public class BGController : MonoBehaviour
{

    [SerializeField]
    private float m_Speed = 1.0f;

    [SerializeField]
    private float m_ResetPositionX = -11.0f;  //これより左に行ったらループ

    [SerializeField]
    private float m_StartPositionX = 17.0f;  //ループ後の再スタート位置

    private Transform m_Transform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_Transform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position -= new Vector3(Time.deltaTime * m_Speed, 0.0f,0.0f);

        if(m_Transform.position.x <= m_ResetPositionX)
        {
            Vector3 pos = m_Transform.position;
            pos.x = m_StartPositionX;
            m_Transform.position = pos;
        }
    }
}
