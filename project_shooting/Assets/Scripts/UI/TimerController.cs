using UnityEngine;
using TMPro;


public class TimerController : MonoBehaviour
{
    [SerializeField]
    private TMP_Text m_TimeText;

    private void Update()
    {
        GameManager.Instance.timer += Time.deltaTime;

        int minutes = Mathf.FloorToInt(GameManager.Instance.timer / 60.0f);
        int seconds = Mathf.FloorToInt(GameManager.Instance.timer % 60.0f);

        m_TimeText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }
}
