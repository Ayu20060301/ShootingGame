using UnityEngine;
using UnityEngine.UI;

//敵のHPゲージの表示を管理するクラス
public class EnemyHPBar : MonoBehaviour
{
    [Header("HPゲージ用のUI")]
    [SerializeField]
    private Image m_HPUI;

    [Header("HPが50%より多い場合の画像")]
    [SerializeField]
    private Sprite m_NormalSprite;

    [Header("HPが50以下の場合の画像")]
    [SerializeField]
    private Sprite m_HalfSprite;

    [Header("HPが20%以下の場合の画像")]
    [SerializeField]
    private Sprite m_CriticalSprite;

   
    /// <summary>
    /// 現在のHPに応じてHPゲージを更新する
    /// </summary>
    /// <param name="current">現在のHP</param>
    /// <param name="max">最大HP</param>
    public void SetHP(int current, int max)
    {
        //HPゲージが設定されていない場合
        if(m_HPUI == null)
        {
            Debug.LogWarning("HPゲージのImageが設定されていません");
            return;
        }

        //最大HPが0以下の場合は計算できない
        if(max <= 0)
        {
            Debug.LogWarning("最大HPは1以上に設定してください");
            return;
        }

        //現在のHP割合を0～1の範囲に制限
        float ratio = Mathf.Clamp01((float)current / max);

        //HPゲージの表示量を更新
        m_HPUI.fillAmount = ratio;

        //HP割合に応じてゲージ画像を変更
        m_HPUI.sprite = GetGaugeSprite(ratio);
    }

    /// <summary>
    /// HP割合に応じたゲージ画像を取得
    /// </summary>
    /// <param name="ratio">HPの割合</param>
    /// <returns>表示するゲージ画像</returns>
    private Sprite GetGaugeSprite(float ratio)
    {
        //HPが20%以下の場合
        if(ratio <= 0.2f)
        {
            return m_CriticalSprite;
        }

        //HPが50%以下の場合
        else if(ratio <= 0.5f)
        {
            return m_HalfSprite;
        }

        //HPが50%より多い場合
        return m_NormalSprite;
    }
}
