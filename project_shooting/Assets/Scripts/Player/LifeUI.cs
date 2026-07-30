using UnityEngine;
using UnityEngine.UI;

//残機を表示するスクリプト
public class LifeUI : MonoBehaviour
{
    [SerializeField]
    private Image[] m_LifeUI; //残機用UI

    [SerializeField]
    private int m_MaxLifes = 3; //最大残機数

    private void Start()
    {
        GameManager.Instance.life = m_MaxLifes;

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
            m_LifeUI[i].enabled = i < GameManager.Instance.life;
        }
    }


    /// <summary>
    /// 残機を1減らす
    /// </summary>
    public void LoseLife()
    {
        //HPが0以下ならば処理を行わない
        if (GameManager.Instance.life <= 0) return;

        //消える残機のインデックス
        int index = GameManager.Instance.life - 1;

        //UIの位置でエフェクト再生
        EffectManager.Instance.PlayEffect(
            EffectType.EXPLOSION,
            m_LifeUI[index].transform.position
            );

        //残機を減らす
        GameManager.Instance.life--;


        //UIの更新
        UpdateUI();

        if(GameManager.Instance.life <= 0)
        {
            GameManager.Instance.GameEnd(false);
        }

    }
}
