using UnityEngine;

public class BestTimeController : MonoBehaviour
{
    private const int RANK_MAX = 5;
    private const string BEST_TIME_KEY = "BestTime";

   
    /// <summary>
    /// ランキングを保存
    /// </summary>
    /// <param name="currentTime">クリアタイム</param>
    /// <returns></returns>
    public bool SaveBestTime(float currentTime)
    {
        float[] times = GetBestTimes();

        int insertIndex = -1;

        //入る順位を探す
        for(int i = 0; i < RANK_MAX; i++)
        {
            if(currentTime < times[i])
            {
                insertIndex = i;
                break;
            }
        }

        //ランク外
        if (insertIndex == -1) return false;


        //下へずらす
        for(int i = RANK_MAX - 1; i > insertIndex; i--)
        {
            times[i] = times[i - 1];
        }

        //新しいタイムを挿入
        times[insertIndex] = currentTime;

        //保存
        for(int i = 0; i < RANK_MAX; i++)
        {
            PlayerPrefs.SetFloat(BEST_TIME_KEY + i, times[i]);
        }

        PlayerPrefs.Save();

        Debug.Log($"{insertIndex + 1}位にランクイン");

        //1位ならニューレコード
        return insertIndex == 0;
    }

    /// <summary>
    /// 1位のタイムを取得
    /// </summary>
    /// <returns></returns>
    public float GetBestTime()
    {
        return GetBestTimes()[0];
    }

    /// <summary>
    /// ランキングを取得
    /// </summary>
    /// <returns></returns>
    public float[] GetBestTimes()
    {
        float[] times = new float[RANK_MAX];

        for(int i = 0; i < RANK_MAX; i++)
        {
            times[i] = PlayerPrefs.GetFloat("BestTime" + i, float.MaxValue);
        }

        return times;
    }

    /// <summary>
    /// ランキングをリセット
    /// </summary>
    public void ResetBestTime()
    {
        
        for(int i = 0; i < RANK_MAX; i++)
        {
            PlayerPrefs.DeleteKey(BEST_TIME_KEY + i);
        }

        PlayerPrefs.Save();

        Debug.Log("ベストタイムをリセットしました");
    }

    private void OnApplicationQuit()
    {
#if UNITY_EDITOR
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
#endif
    }

}
