using UnityEngine;
using UnityEngine.UI;

//残機を表示するスクリプト
public class LifeUI : MonoBehaviour
{

    [SerializeField]
    private Image[] m_LifeUI; //残機用UI

    [SerializeField]
    private int m_MaxLifes = 3; //最大残機数
  
    private int m_CurrentLife;  //現在の残機数
    

    private void Start()
    {
        m_CurrentLife = m_MaxLifes;

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
            m_LifeUI[i].enabled = i < m_CurrentLife;
        }
    }

    /// <summary>
    /// 残機を1減らす
    /// </summary>
    public void LoseLife()
    {
        m_CurrentLife = Mathf.Max(0, m_CurrentLife - 1);
        
        //UIの更新
        UpdateUI();

        //残機が0になったら
        if(m_CurrentLife <= 0)
        {
            //ゲームオーバー
            GameManager.Instance.Result();
        }

    }
}
