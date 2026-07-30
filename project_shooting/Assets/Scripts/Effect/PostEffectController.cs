using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class PostEffectController : MonoBehaviour
{
    [SerializeField]
    private Volume m_Volume;

    private Bloom m_Bloom;
    private Vignette m_Vigenette;
    private ColorAdjustments m_ColorAdjustments;

    private void Awake()
    {
        VolumeProfile profile = m_Volume.profile;

        profile.TryGet(out m_Bloom);
        profile.TryGet(out m_Vigenette);
        profile.TryGet(out m_ColorAdjustments);
    }

    /// <summary>
    /// àÍèuâÊñ ÇåıÇÁÇπÇÈ
    /// </summary>
    public void Flash()
    {
        StartCoroutine(FlashCoroutine());
    }

    private IEnumerator FlashCoroutine()
    {
        m_Bloom.intensity.value = 8.0f;
        m_ColorAdjustments.postExposure.value = 1.5f;

        yield return new WaitForSecondsRealtime(0.08f);

        m_Bloom.intensity.value = 0.5f;
        m_ColorAdjustments.postExposure.value = 0.0f;
    }
}
