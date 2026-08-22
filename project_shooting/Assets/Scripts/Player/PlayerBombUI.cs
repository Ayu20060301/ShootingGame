using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

//ボム数を表示するクラス
public class PlayerBombUI : MonoBehaviour
{
    [Header("ボム1個分を表示するUIPrefab")]
    [SerializeField]
    private GameObject m_BombUIPrefab;

    [Header("ボムUIを配置する親Transform")]
    [SerializeField]
    private Transform m_BombParent;

    private List<Image> m_BombUI = new(); //生成したボムUIのImageを管理するリスト


    private void Start()
    {
        //最大ボム数分のUIをあらかじめ生成する
        CreateBombUI(); 

        //現在のボム所持数に合わせて表示状態を更新する
        UpdateUI();

        //初期状態では暗転させない
        SetDim(false);
    }

    /// <summary>
    /// ボムUIを生成する
    /// </summary>
    private void CreateBombUI()
    {
        m_BombUI.Clear();

        for(int i = 0; i < GameManager.Instance.maxBomb; i++)
        {
            GameObject obj = Instantiate(m_BombUIPrefab, m_BombParent);

            //PrefabからImageコンポーネントを取得する
            Image image = obj.GetComponent<Image>();

            if(image == null)
            {
                Debug.LogWarning("Bomb UI PrefabにImageコンポーネントがありません", obj);

                continue;
            }

            m_BombUI.Add(image);
        }
    }

    /// <summary>
    ///現在のボム所持数に合わせてUIの表示状態を更新する
    /// </summary>
    public void UpdateUI()
    {
        int currentBomb = GameManager.Instance.currentBomb;

        for(int i = 0; i < m_BombUI.Count; i++)
        {
            //所持しているボムの数だけ表示し、それ以外は非表示にする
            bool isActive = i < currentBomb;

            m_BombUI[i].enabled = isActive;

        }
    }

    /// <summary>
    /// ボム発動中の暗転表示を切り替える
    /// </summary>
    /// <param name="isDimmed">暗転させる場合はtrue</param>
    public void SetDim(bool isDimmed)
    {
        //暗転時のAlpha値
        float alpha = 0.1f;

        for(int i = 0; i < m_BombUI.Count; i++)
        {
            Image image = m_BombUI[i];

            //現在の色を取得する
            Color color = image.color;

            //暗転時はAlphaを下げる
            color.a = isDimmed ? alpha : 1.0f;

            //変更した色をImageに反映する
            image.color = color;
        }
    }
}
