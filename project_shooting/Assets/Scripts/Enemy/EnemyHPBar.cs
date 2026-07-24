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

   

    public void SetHP(int current, int max)
    {
         float ratio = (float)current / max;

        Sprite targetSprite;
        if(ratio <= 0.2f)
        {
            targetSprite = m_CriticalSprite;
        }
        else if(ratio <= 0.5f)
        {
            targetSprite = m_HalfSprite;
        }
        else
        {
            targetSprite = m_NormalSprite;
        }

        m_HPUI.fillAmount = ratio;
        m_HPUI.sprite = targetSprite;
    }
}
