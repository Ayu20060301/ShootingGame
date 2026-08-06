using System.Collections;
using UnityEngine;

//グリッチ演出クラス
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

            //2～4秒待つ
            yield return new WaitForSecondsRealtime(Random.Range(2.0f, 4.0f));

            //ノイズ音を再生
            SEManager.Instance.SEPlay(SEType.NOISE);

            //一瞬だけ強くグリッチ
            m_GlitchMaterial.SetFloat("_GlitchIntensity", m_GlitchIntensity);

            yield return new WaitForSecondsRealtime(m_GlitchTime);
        }
    }
}
