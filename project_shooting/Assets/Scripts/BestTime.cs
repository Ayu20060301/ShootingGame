using UnityEngine;

//ベストタイムランキングを管理するクラス
public class BestTime : MonoBehaviour
{
    //---------------
    //定数
    //---------------

    //ランキングの最大登録数
    private const int RANK_MAX = 5;

    //PlayerPrefsで使用するキーのベース
    private const string BEST_TIME_KEY = "BestTime";

    /// <summary>
    /// ランキングを保存
    /// </summary>
    /// <param name="currentTime">クリアタイム</param>
    /// <returns>1位にランクインした場合はtrue、それ以外はfalse</returns>
    public bool SaveBestTime(float currentTime)
    {
        //現在のランキングを取得
        float[] times = GetBestTimes();

        //今回のクリアタイムを挿入する位置
        int insertIndex = FindInsertIndex(times, currentTime);

        //ランク外の場合は保存しない
        if (insertIndex == -1) return false;

        //挿入位置より下のランキングを1つずつ後ろへ移動
        ShiftRanking(times, insertIndex);

        //新しいタイムをランキングへ追加
        times[insertIndex] = currentTime;

        //更新したランキングを保存
        SaveRanking(times);

        Debug.Log($"{insertIndex + 1}位にランクイン");

        //1位に入った場合はニューレコード
        return insertIndex == 0;
    }

    /// <summary>
    /// タイムを挿入する位置を検索する
    /// </summary>
    /// <param name="times">現在のランキング</param>
    /// <param name="currentTime">今回のクリアタイム</param>
    /// <returns>挿入位置。ランク外の場合は-1</returns>
    private int FindInsertIndex(float[] times,float currentTime)
    {
        for(int i = 0; i < RANK_MAX; i++)
        {
            //既存タイムより速ければ、その位置に入る
            if(currentTime < times[i])
            {
                return i;
            }
        }

        //ランク外
        return -1;
    }

    /// <summary>
    /// ランキングを下へずらす
    /// </summary>
    /// <param name="times">ランキング</param>
    /// <param name="insertIndex">新しいタイムを挿入する位置</param>
    private void ShiftRanking(float[] times, int insertIndex)
    {
        //最下位から順番に1つ後ろへ移動
        for(int i = RANK_MAX - 1; i > insertIndex; i--)
        {
            times[i] = times[i - 1];
        }
    }

    /// <summary>
    /// 1位のタイムを取得
    /// </summary>
    /// <returns>1位のタイム</returns>
    public float GetBestTime()
    {
        return GetBestTimes()[0];
    }

    /// <summary>
    /// ランキングを取得
    /// </summary>
    /// <returns>ランキングのタイム</returns>
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
    /// ランキングをPlayerPrefsへ保存する
    /// </summary>
    /// <param name="times">保存するランキング</param>
    private void SaveRanking(float[] times)
    {
        for(int i = 0; i < RANK_MAX; i++)
        {
            PlayerPrefs.SetFloat(GetTimeKey(i), times[i]);
        }

        //PlayerPrefsへ変更内容を反映
        PlayerPrefs.Save();
    }

    /// <summary>
    /// ランキング用のPlayerPrefsキーを取得する
    /// </summary>
    /// <param name="rankIndex">ランキングのインデックス</param>
    /// <returns>PlayerPrefsのキー</returns>
    private string GetTimeKey(int rankIndex)
    {
        return BEST_TIME_KEY + rankIndex;
    }

#if UNITY_EDITOR
    /// <summary>
    /// Unityエディタ終了時にランキングデータを削除する
    /// </summary>
    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
#endif
}
