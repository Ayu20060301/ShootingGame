using System.Collections;
using UnityEngine;

//--------------------
//グリッチ演出クラス
//ロゴアニメーション
//--------------------
public class LogoAnimation : MonoBehaviour
{
    //Shaderのプロパティ名
    private const string GLITCH_INTENSITY_PROPERTY = "_GlitchIntensity";

    //グリッチ発生間隔
    private const float GLITCH_INTERVAL_MIN = 2.0f;
    private const float GLITCH_INTERVAL_MAX = 4.0f;

    [Header("マテリアルを参照する")]
    [SerializeField]
    private Material m_GlitchMaterial;

    [Header("通常時のグリッチ強度")]
    [SerializeField]
    private float m_NormalIntensity = 0.01f;

    [Header("グリッチ発生時の強度")]
    [SerializeField]
    private float m_GlitchIntensity = 0.15f;

    [Header("グリッチ継続時間")]
    [SerializeField]
    private float m_GlitchTime = 0.15f;


    //ShaderプロパティID
    private static readonly int GlitchIntensityID = Shader.PropertyToID(GLITCH_INTENSITY_PROPERTY);

    private void Start()
    {
        //グリッチ演出を開始
        StartCoroutine(GlitchLoop());
    }

    /// <summary>
    /// グリッチ演出を繰り返す
    /// </summary>
    /// <returns></returns>
    private IEnumerator GlitchLoop()
    {
        while(true)
        {
            //通常状態に戻す
            SetGlitchIntensity(m_NormalIntensity);

            //次のグリッチまでランダムに待機
            float waitTime = Random.Range(GLITCH_INTERVAL_MIN,GLITCH_INTERVAL_MAX); 

            //2～4秒待つ
            yield return new WaitForSecondsRealtime(waitTime);

            //ノイズ音を再生
            SEManager.Instance.SEPlay(SEType.NOISE);

            //一瞬だけ強くグリッチ
            SetGlitchIntensity(m_GlitchIntensity);

            //グリッチを一定時間維持
            yield return new WaitForSecondsRealtime(m_GlitchTime);
        }
    }

    /// <summary>
    /// Shaderのグリッチ強度を変更する
    /// </summary>
    /// <param name="intensity">グリッチ強度</param>
    private void SetGlitchIntensity(float intensity)
    {
        m_GlitchMaterial.SetFloat(GlitchIntensityID,intensity);
    }
}
