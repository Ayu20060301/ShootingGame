using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultUI : MonoBehaviour
{

    [SerializeField]
    private BestTimeController m_BestTimeController;

    [SerializeField]
    private TMP_Text m_ResultText;
    [SerializeField]
    private TMP_Text m_PlayTimeText;
    [SerializeField]
    private TMP_Text m_BestTimeText;
    [SerializeField]
    private TMP_Text m_RankText;

    private void Start()
    {
        m_ResultText.text = ResultData.isClear ? "GAME CLEAR" : "GAME OVER";

        int minutes = Mathf.FloorToInt(ResultData.playTime / 60.0f);
        int seconds = Mathf.FloorToInt(ResultData.playTime % 60.0f);

        m_PlayTimeText.text = (ResultData.isClear ? "クリア時間 : "  : "生存時間 : ")　+  $"{minutes:00}:{seconds:00}";

        //ベストタイムを更新
        UpdateBestTime();
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
        else if (time <= 90.0f)
        {
            m_RankText.text = "S";
            m_RankText.color = Color.yellow;
        }
        else if (time <= 120.0f)
        {
            m_RankText.text = "A";
            m_RankText.color = Color.green;
        }
        else if (time <= 150.0f)
        {
            m_RankText.text = "B";
            m_RankText.color = Color.cyan;
        }
        else if (time <= 180.0f)
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

    /// <summary>
    /// ベストタイムの更新
    /// </summary>
    private void UpdateBestTime()
    {
       //ゲームクリア時のみベストタイムを更新
       if(ResultData.isClear)
        {
            m_BestTimeController.SaveBestTime(ResultData.playTime);
        }

        float bestTime = m_BestTimeController.GetBestTime();

        //まだ記録がない
        if(bestTime == float.MaxValue)
        {
            m_BestTimeText.text = "ベストタイム : --:--";
            return;
        }

        int minutes = Mathf.FloorToInt(bestTime / 60.0f);
        int seconds = Mathf.FloorToInt(bestTime % 60.0f);

        m_BestTimeText.text = $"ベストタイム : {minutes:00}:{seconds:00}";
    }
}
