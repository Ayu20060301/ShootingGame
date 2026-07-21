using System;
using UnityEngine;

public class PlayerLives : MonoBehaviour
{
    [SerializeField]
    private int m_MaxLives = 3; //最大残機
    private int m_CurrentLives; //現在の残機数

    //残機が変化した際に呼ばれる
    public event Action<int> OnLivesChanged;

    //残機が0になった場合に呼ばれる
    public event Action OnGameOver;

    public int CurrentLives => m_CurrentLives;

    private void Awake()
    {
        m_CurrentLives = m_MaxLives;
    }

    /// <summary>
    /// 残機を減らす処理
    /// </summary>
    public void LoseLofe()
    {
        if (m_CurrentLives <= 0) return;

        m_CurrentLives--;
        OnLivesChanged.Invoke(m_CurrentLives);

        if(m_CurrentLives <= 0)
        {
            OnGameOver.Invoke();
        }
        else
        {

        }
    }
}
