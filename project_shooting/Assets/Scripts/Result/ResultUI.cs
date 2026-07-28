using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text m_ResultText;
    [SerializeField]
    private TMP_Text m_TimeText;
    [SerializeField]
    private TMP_Text m_BombText;
    [SerializeField]
    private TMP_Text m_HitCountText;
    [SerializeField]
    private TMP_Text m_RankText;

    private void Start()
    {
        m_ResultText.text = ResultData.isClear ? "GAME CLEAR" : "GAME OVER";


        int minutes = Mathf.FloorToInt(ResultData.playTime / 60.0f);
        int seconds = Mathf.FloorToInt(ResultData.playTime % 60.0f);

        m_TimeText.text = "クリア時間 : " +  $"{minutes:00}:{seconds:00}";

        m_BombText.text = "使用したボムの数 : " +  ResultData.bombUsed.ToString();
        m_HitCountText.text = "被弾数 : " + ResultData.hitCount.ToString();
    }
}
