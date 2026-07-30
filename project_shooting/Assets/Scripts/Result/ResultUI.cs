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
    private TMP_Text m_LifeText;
    [SerializeField]
    private TMP_Text m_RankText;

    private void Start()
    {
        m_ResultText.text = ResultData.isClear ? "GAME CLEAR" : "GAME OVER";

        int minutes = Mathf.FloorToInt(ResultData.playTime / 60.0f);
        int seconds = Mathf.FloorToInt(ResultData.playTime % 60.0f);

        m_TimeText.text = (ResultData.isClear ? "クリア時間 : "  : "生存時間 : ")　+  $"{minutes:00}:{seconds:00}";

        m_BombText.text = "使用したボムの数 : " +  ResultData.bombUsed.ToString();
        m_LifeText.text = "残りの残機 : " + ResultData.life.ToString();

        //ランクの更新
        UpdateRank();
    }

    /// <summary>
    /// ランクの更新
    /// </summary>
    private void UpdateRank()
    {
        float time = ResultData.playTime;

        if (!ResultData.isClear)
        {
            m_RankText.text = "D";
            m_RankText.color = Color.gray;
        }
        else if (time <= 60.0f)
        {
            m_RankText.text = "S";
            m_RankText.color = Color.yellow;
        }
        else if (time <= 90.0f)
        {
            m_RankText.text = "A";
            m_RankText.color = Color.green;
        }
        else if (time <= 120.0f)
        {
            m_RankText.text = "B";
            m_RankText.color = Color.cyan;
        }
        else if (time <= 150.0f)
        {
            m_RankText.text = "C";
            m_RankText.color = Color.magenta;
        }
        else
        {
            m_RankText.text = "D";
            m_RankText.color = Color.gray;
        }
    }
}
