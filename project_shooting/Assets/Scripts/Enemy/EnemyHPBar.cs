using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBar : MonoBehaviour
{
    [SerializeField]
    private Image m_HPUI; //HPゲージ用のUI

    [Header("ゲージ画像")]
    [SerializeField]
    private Sprite m_NormalSprite; //通常時
    [SerializeField]
    private Sprite m_HalfSprite;  //50%以下
    [SerializeField]
    private Sprite m_CriticalSprite;  //20%以下

   
    /// <summary>
    /// HPゲージを更新
    /// </summary>
    /// <param name="current">現在のHP</param>
    /// <param name="max">最大HP</param>
    public void SetHP(int current, int max)
    {

        if (m_HPUI == null || max <= 0) return;

        float ratio = Mathf.Clamp01((float)current / max);

        m_HPUI.fillAmount = ratio;
        m_HPUI.sprite = GetGaugeSprite(ratio);
    }

    /// <summary>
    /// HP割合に応じたゲージ画像を取得
    /// </summary>
    /// <param name="ratio">HPの割合</param>
    /// <returns></returns>
    private Sprite GetGaugeSprite(float ratio)
    {

        if(ratio <= 0.2f)
        {
            return m_CriticalSprite;
        }
        else if(ratio <= 0.5f)
        {
            return m_HalfSprite;
        }

        return m_NormalSprite;

    }
}
