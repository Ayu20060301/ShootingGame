using UnityEngine;
using UnityEngine.UI;

//残機を表示するスクリプト
public class LifeUI : MonoBehaviour
{
    [SerializeField]
    private Image[] m_LifeUI; //残機用UI

    [SerializeField]
    private int m_MaxLifes = 3; //最大残機数

    [SerializeField]
    private PostEffectController m_Effect;

    private void Start()
    {
        GameManager.Instance.hitCount = m_MaxLifes;

        //UIの更新
        UpdateUI();
    }

    /// <summary>
    /// UIの更新
    /// </summary>
    private void UpdateUI()
    {
        for(int i = 0; i< m_LifeUI.Length; i++)
        {
            m_LifeUI[i].enabled = i < GameManager.Instance.hitCount;
        }
    }


    /// <summary>
    /// 残機を1減らす
    /// </summary>
    public void LoseLife()
    {
        GameManager.Instance.hitCount = Mathf.Max(0, GameManager.Instance.hitCount - 1);
        
        //UIの更新
        UpdateUI();

        //残機が0になったら
        if(GameManager.Instance.hitCount <= 0)
        {

            m_Effect.BlackOut();

            //ゲームオーバー
           // GameManager.Instance.GameEnd(false);
        }
    }
}
