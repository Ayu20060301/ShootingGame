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
    private TMP_Text m_NewRecordText;
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
        else if (time <= 100.0f)
        {
            m_RankText.text = "A";
            m_RankText.color = Color.green;
        }
        else if (time <= 110.0f)
        {
            m_RankText.text = "B";
            m_RankText.color = Color.cyan;
        }
        else if (time <= 120.0f)
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

        //保存前のベストタイムを取得
        float oldBestTime = m_BestTimeController.GetBestTime();

        bool isNewRecord = false;

       //ゲームクリア時のみベストタイムを更新
       if(ResultData.isClear)
       {
           //初記録または過去記録より早い場合
           if(ResultData.playTime < oldBestTime)
           {
                isNewRecord = true;
           }

            m_BestTimeController.SaveBestTime(ResultData.playTime);
       }

       //ニューレコード表示
       if(isNewRecord)
       {
            m_NewRecordText.text = "NEW RECORD!";
       }
       else
       {
            m_NewRecordText.text = string.Empty;
       }
    

        //更新後のベストタイムを取得
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
