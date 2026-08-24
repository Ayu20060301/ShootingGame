using UnityEngine;

//BGMを管理するクラス
public class BGMManager : SingletonMonoBehaviour<BGMManager>
{
    //BGMを再生するAudioSource
    public AudioSource bgmAudio;

    /// <summary>
    /// BGMの再生
    /// </summary>
    /// <param name="type">BGMの種類</param>
    public void BGMPlay(BGMType type)
    {
        //データベースから指定されたBGMデータを取得
        BGMData data = DatabaseManager.Instance.soundDatabase.GetBGMData(type);

        //BGMデータが存在しない場合
        if(data == null)
        {
            Debug.LogWarning($"{type}のBGMDataが見つかりません");

            return;
        }

        //AudioSourceにBGMの設定を反映
        bgmAudio.loop = data.loop;
        bgmAudio.clip = data.clip;

        //BGMを再生
        bgmAudio.Play();
    }

    /// <summary>
    /// BGMの再生を止める
    /// </summary>
    public void AudioStop()
    {
        bgmAudio.Stop();
    }
}
