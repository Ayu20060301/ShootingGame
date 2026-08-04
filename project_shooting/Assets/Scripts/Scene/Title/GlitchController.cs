using System.Collections;
using UnityEngine;

public class GlitchController : MonoBehaviour
{
    [SerializeField]
    private Material m_GlitchMaterial;
    [SerializeField]
    private float m_NormalIntensity = 0.01f;
    [SerializeField]
    private float m_GlitchIntensity = 0.15f;
    [SerializeField]
    private float m_GlitchTime = 0.15f;

    private void Start()
    {
        StartCoroutine(GlitchLoop());
    }

    private IEnumerator GlitchLoop()
    {
        while(true)
        {
            m_GlitchMaterial.SetFloat("_GlitchIntensity", m_NormalIntensity);

            //2`4•b‘Ò‚Â
            yield return new WaitForSecondsRealtime(Random.Range(2.0f, 4.0f));

            //ƒmƒCƒY‰¹‚ğÄ¶
            SEManager.Instance.SEPlay(SEType.NOISE);

            //ˆêu‚¾‚¯‹­‚­ƒOƒŠƒbƒ`
            m_GlitchMaterial.SetFloat("_GlitchIntensity", m_GlitchIntensity);

            yield return new WaitForSecondsRealtime(m_GlitchTime);
        }
    }
}
