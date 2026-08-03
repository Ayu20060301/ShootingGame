using UnityEngine;

public class BestTimeController : MonoBehaviour
{
    private const string BEST_TIME_KEY = "BestTime";

    /// <summary>
    /// ベストタイムを更新したか
    /// </summary>
    /// <param name="currentTime">今現在のベストタイム</param>
    /// <returns></returns>
    public bool SaveBestTime(float currentTime)
    {
        //初回は非常に大きな値を取得
        float bestTime = PlayerPrefs.GetFloat(BEST_TIME_KEY, float.MaxValue);

        if(currentTime < bestTime)
        {
            PlayerPrefs.SetFloat(BEST_TIME_KEY, currentTime);
            PlayerPrefs.Save(); //即保存
            Debug.Log("ベストタイム更新");
            return true;
        }

        return false;
    }

    /// <summary>
    /// ベストタイムをリセット
    /// </summary>
    public void ResetBestTime()
    {
        PlayerPrefs.DeleteKey(BEST_TIME_KEY);
        PlayerPrefs.Save();

        Debug.Log("ベストタイムをリセットしました");
    }

    /// <summary>
    /// ベストタイムを取得
    /// </summary>
    /// <returns></returns>
    public float GetBestTime()
    {
        return PlayerPrefs.GetFloat(BEST_TIME_KEY, float.MaxValue);
    }
}
