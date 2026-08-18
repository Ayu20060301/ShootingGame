using UnityEngine;
using TMPro;
using UnityEngine.Rendering;

//タイトル画面のランキングUIを管理するクラス
public class TitleRankingUI : MonoBehaviour
{
    [SerializeField]
    private BestTimeController m_BestTimeController;
    [SerializeField]
    private TMP_Text[] m_RankTexts;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateRanking();
    }

    /// <summary>
    /// ランキングUIの更新
    /// </summary>
    private void UpdateRanking()
    {
        //ベストタイムを取得
        float[] times = m_BestTimeController.GetBestTimes();

        //UIとランキングの小さい方に合わせて処理する
        int rankCount = Mathf.Min(
            times.Length,
            m_RankTexts.Length
            );

        for(int i = 0; i < rankCount; i++)
        {
            //ランキング表示を更新
            m_RankTexts[i].text = CreateRankText(i, times[i]);
        }
    }

    /// <summary>
    /// ランキング表示用の文字列を作成する
    /// </summary>
    /// <param name="rankIndex">ランキングのインデックス</param>
    /// <param name="time">クリアタイム</param>
    /// <returns>ランキング表示文字列</returns>
    private string CreateRankText(int rankIndex, float time)
    {
        int rank = rankIndex + 1;

        //タイムが未登録の場合
        if(time == float.MaxValue)
        {
            return $"{rank}位 --:--";
        }

        //秒から分・秒へ変換
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);


        return $"{rank}位 {minutes:00}:{seconds:00}";
    }
}
