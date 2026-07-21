using UnityEngine;
using TMPro;


public class TimerController : MonoBehaviour
{
    [SerializeField]
    private TMP_Text m_TimeText;

    private float m_ElapsedTime = 0.0f;

    private void Update()
    {
        m_ElapsedTime += Time.deltaTime;

        int minutes = Mathf.FloorToInt(m_ElapsedTime / 60.0f);
        int seconds = Mathf.FloorToInt(m_ElapsedTime % 60.0f);

        m_TimeText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }
}
