using UnityEngine;

public class BGMManager : SingletonMonoBehaviour<BGMManager>
{
    public AudioSource bgmAudio;

    /// <summary>
    /// BGM‚ÌÄ¶
    /// </summary>
    /// <param name="type">BGM‚Ìí—Ş</param>
    public void BGMPlay(BGMType type)
    {
        BGMData data = DatabaseManager.Instance.soundDatabase.GetBGMData(type);


        if(data == null)
        {
            Debug.LogWarning($"{type}‚ÌBGMData‚ªŒ©‚Â‚©‚è‚Ü‚¹‚ñ");
        }

        bgmAudio.loop = data.loop;
        bgmAudio.clip = data.clip;
        bgmAudio.Play();
    }

    /// <summary>
    /// BGM‚ğÁ‚·
    /// </summary>
    public void AudioStop()
    {
        bgmAudio.Stop();
    }

}
