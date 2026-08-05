using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

//ボムのUIを表示するスクリプト
public class PlayerBombUI : MonoBehaviour
{
    [SerializeField]
    private GameObject m_BombPrefab; //ボムUIのプレハブ
    [SerializeField]
    private Transform m_BombParent; //横並びにする親
    private List<Image> m_BombUI = new();

    private void Start()
    {
        GameManager.Instance.currentBomb = GameManager.Instance.maxBomb;

        //UIを生成
        CreateBombUI();

        //UIの更新
        UpdateUI();
    }

    /// <summary>
    /// ボムUIの生成
    /// </summary>
    private void CreateBombUI()
    {
        m_BombUI.Clear();

        for(int i = 0; i < GameManager.Instance.maxBomb; i++)
        {
            GameObject obj = Instantiate(m_BombPrefab, m_BombParent);

            Image image = obj.GetComponent<Image>();

            m_BombUI.Add(image);
        }
    }

    /// <summary>
    /// UIを更新
    /// </summary>
    private void UpdateUI()
    {
        for(int i = 0; i < m_BombUI.Count; i++)
        {
            m_BombUI[i].enabled = i < GameManager.Instance.currentBomb;
        }
    }
}
