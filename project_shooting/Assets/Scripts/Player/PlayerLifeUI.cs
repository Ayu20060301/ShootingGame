using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

//残機を表示するスクリプト
public class PlayerLifeUI : MonoBehaviour
{
    [SerializeField]
    private GameObject m_LifePrefab; //残機アイコンのプレハブ
    [SerializeField]
    private Transform m_LifeParent; //横並びにする親
    private List<Image> m_LifeUI = new();
    [SerializeField]
    private FinishController m_FinishController;


    private void Start()
    {
        GameManager.Instance.currentLife = GameManager.Instance.maxLife;

        //UIを生成
        CreateLifeUI();

        //UIの更新
        UpdateUI();

    }

    /// <summary>
    /// 残機UIを生成
    /// </summary>
    private void CreateLifeUI()
    {
        m_LifeUI.Clear();

        for(int i = 0; i< GameManager.Instance.maxLife; i++)
        {
            GameObject obj = Instantiate(m_LifePrefab, m_LifeParent);

            Image image = obj.GetComponent<Image>();

            m_LifeUI.Add(image);
        }


    }

    /// <summary>
    /// UIを更新
    /// </summary>
    private void UpdateUI()
    {
        for(int i = 0; i < m_LifeUI.Count; i++)
        {
            m_LifeUI[i].enabled = i < GameManager.Instance.currentLife;
        }
    }

    /// <summary>
    /// 残機を1減らす
    /// </summary>
    public void LoseLife()
    {
        //HPが0以下ならば処理を行わない
        if (GameManager.Instance.currentLife <= 0) return;

        //消える残機のインデックス
        int index = GameManager.Instance.currentLife - 1;


        SEManager.Instance.SEPlay(SEType.DAMAGE_PLAYER);

        //残機を減らす
        GameManager.Instance.currentLife--;


        //UIの更新
        UpdateUI();

        //残機が0になったらゲームオーバー
        if(GameManager.Instance.currentLife <= 0)
        {
            PlayerController player = FindFirstObjectByType<PlayerController>();

            if(player != null)
            {
                m_FinishController.Finish(false, player.transform.position,player.gameObject);
            }
            else
            {
                GameManager.Instance.GameEnd(false);
            }
        }
    }

 
}
