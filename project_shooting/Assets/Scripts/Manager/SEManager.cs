using UnityEngine;

//効果音を管理するクラス
public class SEManager : SingletonMonoBehaviour<SEManager>
{
    //効果音を再生するAudioSource
    public AudioSource seAudio; 

    /// <summary>
    /// 効果音の再生
    /// </summary>
    /// <param name="type">効果音の種類</param>
    public void SEPlay(SEType type)
    {
        //データベースから指定された効果音データを取得する
        SEData data = DatabaseManager.Instance.soundDatabase.GetSEData(type);
        
        //効果音データが存在しない場合
        if(data == null)
        {
            Debug.LogWarning($"{type}のSEDataが見つかりません");
            return;
        }

        //効果音を再生する
        seAudio.PlayOneShot(data.clip);
    }
}
