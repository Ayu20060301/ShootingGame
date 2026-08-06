using UnityEngine;
using TMPro;

public class PlayTimeController : MonoBehaviour
{
    [SerializeField]
    private TMP_Text m_PlayTimeText;

    private void Update()
    {
        if (!GameManager.Instance.isActive) return;

        GameManager.Instance.playTime += Time.deltaTime;

        int minutes = Mathf.FloorToInt(GameManager.Instance.playTime / 60.0f);
        int seconds = Mathf.FloorToInt(GameManager.Instance.playTime % 60.0f);

        m_PlayTimeText.text = string.Format("Time :" + "{0:00} : {1:00}", minutes, seconds);
    }

}
