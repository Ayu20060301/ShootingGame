using UnityEngine;
using UnityEngine.UI;
using TMPro;

//リザルト画面のUIを制御するクラス
public class ResultUI : MonoBehaviour
{

    //------------------
    //ランク設定
    //------------------
    private const float RANK_S_TIME = 90.0f;
    private const float RANK_A_TIME = 100.0f;
    private const float RANK_B_TIME = 110.0f;
    private const float RANK_C_TIME = 120.0f;

    [Header("ベストタイム")]
    [SerializeField]
    private BestTimeController m_BestTimeController;

    [Header("リザルト")]
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
        //リザルトを表示
        UpdateResult();

        //プレイ時間を表示
        UpdatePlayTime();

        //ベストタイムを更新・表示
        UpdateBestTime();

        //ランク表示
        UpdateRank();
    }

    /// <summary>
    /// CLEAR / GAME OVERを表示する
    /// </summary>
    private void UpdateResult()
    {
        m_ResultText.text =
            ResultData.isClear
                ? "GAME CLEAR"
                : "GAME OVER";
    }

    /// <summary>
    /// プレイ時間を表示する
    /// </summary>
    private void UpdatePlayTime()
    {
        string label = ResultData.isClear
            ? "クリア時間 : "
            : "生存時間 : ";

        m_PlayTimeText.text =
            label + FormatTime(ResultData.playTime);
    }

    /// <summary>
    /// ランクの更新
    /// </summary>
    private void UpdateRank()
    {
        // ゲームオーバーの場合はDランク
        if (!ResultData.isClear)
        {
            SetRank("D", Color.gray);
            return;
        }

        float time = ResultData.playTime;

        if (time <= RANK_S_TIME)
        {
            SetRank("S", Color.yellow);
        }
        else if (time <= RANK_A_TIME)
        {
            SetRank("A", Color.green);
        }
        else if (time <= RANK_B_TIME)
        {
            SetRank("B", Color.cyan);
        }
        else if (time <= RANK_C_TIME)
        {
            SetRank("C", Color.magenta);
        }
        else
        {
            SetRank("D", Color.gray);
        }
    }

    /// <summary>
    /// ランクの文字と色を設定する
    /// </summary>
    /// <param name="rank">ランク</param>
    /// <param name="color">表示色</param>
    private void SetRank(string rank,Color color)
    {
        m_RankText.text = rank;
        m_RankText.color = color;
    }

    /// <summary>
    /// ベストタイムの更新
    /// </summary>
    private void UpdateBestTime()
    {
        bool isNewRecord = false;

        //ゲームクリア時のみランキングへ登録
        if(ResultData.isClear)
        {
            isNewRecord =
                m_BestTimeController.SaveBestTime(
                    ResultData.playTime
                    );
        }

        //NEW RECORD!を表示
        UpdateNewRecord(isNewRecord);

        //更新後のベストタイムを取得
        float bestTime = m_BestTimeController.GetBestTime();

        //ベストタイムを表示
        UpdateBestTimeText(bestTime);
    }

    /// <summary>
    /// NEW RECORD!表示を更新する
    /// </summary>
    /// <param name="isNewRecord">新記録かどうか</param>
    private void UpdateNewRecord(bool isNewRecord)
    {
        m_NewRecordText.text =
            isNewRecord
            ? "NEW RECORD!"
            : string.Empty;
    }

    /// <summary>
    /// ベストタイムを表示する
    /// </summary>
    /// <param name="bestTime">ベストタイム</param>
    private void UpdateBestTimeText(float bestTime)
    {
        //まだ記録が存在しない場合
        if(bestTime == float.MaxValue)
        {
            m_BestTimeText.text = "ベストタイム : --:--";
            return;
        }

        m_BestTimeText.text =
            $"ベストタイム : {FormatTime(bestTime)}";
    }

    /// <summary>
    /// 秒数を「00:00」形式へ変換する
    /// </summary>
    /// <param name="time">秒数</param>
    /// <returns>分:秒形式の文字列</returns>
    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60.0f);
        int seconds = Mathf.FloorToInt(time % 60.0f);

        return $"{minutes:00}:{seconds:00}";
    }
}
