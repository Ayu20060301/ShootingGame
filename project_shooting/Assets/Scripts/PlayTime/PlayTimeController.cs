using UnityEngine;
using TMPro;

//プレイ時間の計測・表示を管理するクラス
public class PlayTimeController : MonoBehaviour
{
    [SerializeField]
    private TMP_Text m_PlayTimeText;

    private void Update()
    {
        //ゲーム中でなければ時間を進めない
        if (!GameManager.Instance.isActive) return;

        //プレイ時間を加算
        GameManager.Instance.playTime += Time.deltaTime;

        //UIを更新
        UpdatePlayTime();
    }

    /// <summary>
    /// プレイ時間の表示を更新する
    /// </summary>
    private void UpdatePlayTime()
    {
        float playTime = GameManager.Instance.playTime;

        m_PlayTimeText.text =
            $"Time : {FormatTime(playTime)}";
    }

    /// <summary>
    /// 秒数を「00:00」形式へ変換する
    /// </summary>
    /// <param name="time">秒数</param>
    /// <returns>分 : 秒形式の文字列</returns>
    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60.0f);
        int seconds = Mathf.FloorToInt(time % 60.0f);

        return $"{minutes:00} : {seconds:00}";
    }
}
