using UnityEngine;

public class BGController : MonoBehaviour
{
    [SerializeField]
    private float m_Speed = 1.0f;

    private void Update()
    {
        this.transform.position -= new Vector3(Time.deltaTime * m_Speed, 0);

        if (this.transform.position.x <= -19.0f)
        {
            this.transform.position = new Vector3(19.0f, 0);
        }
    }
}
