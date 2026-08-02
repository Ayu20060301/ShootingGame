using UnityEngine;

public class SEManager : SingletonMonoBehaviour<SEManager>
{
    public AudioSource seAudio;


    /// <summary>
    /// Œø‰Ê‰¹‚ÌÄ¶
    /// </summary>
    /// <param name="type">Œø‰Ê‰¹‚Ìƒ^ƒCƒv</param>
    public void SEPlay(SEType type)
    {
        SEData data = DatabaseManager.Instance.soundDatabase.GetSEData(type);
        
        if(data == null)
        {
            Debug.LogWarning($"{type}‚ÌSEData‚ªŒ©‚Â‚©‚è‚Ü‚¹‚ñ");
            return;
        }

        seAudio.PlayOneShot(data.clip);

        if(Time.timeScale < 0)
        {
            seAudio.Stop();
        }

    }
}
