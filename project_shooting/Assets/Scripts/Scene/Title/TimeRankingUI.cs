using UnityEngine;
using TMPro;

public class TimeRankingUI : MonoBehaviour
{
    [SerializeField]
    private TimeRankingController m_TimeRankingController;
    [SerializeField]
    private TMP_Text[] m_RankTexts;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateRanking();
    }

    /// <summary>
    /// ランキングの更新
    /// </summary>
    private void UpdateRanking()
    {

        float[] times = m_TimeRankingController.GetBestTimes();

        for (int i = 0; i < m_RankTexts.Length; i++)
        {
            if (times[i] == float.MaxValue)
            {
                m_RankTexts[i].text = $"{i + 1}位 --:--";
            }
            else
            {
                int minutes = Mathf.FloorToInt(times[i] / 60);
                int seconds = Mathf.FloorToInt(times[i] % 60);

                m_RankTexts[i].text =
                    $"{i + 1}位 {minutes:00}:{seconds:00}";
            }
        }
    }
}
