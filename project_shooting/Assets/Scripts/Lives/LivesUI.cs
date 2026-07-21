using UnityEngine;
using UnityEngine.UI;

/// <summary>
///残機のUI
/// </summary>
public class LivesUI : MonoBehaviour
{
    [SerializeField]
    private PlayerLives m_PlayerLives;
    [SerializeField]
    private Image[] m_LifeUIs; //残機用UI

    private void OnEnable()
    {
        m_PlayerLives.OnLivesChanged += UpdateIcons;
        UpdateIcons(m_PlayerLives.CurrentLives);
    }

    /// <summary>
    /// UIの更新
    /// </summary>
    /// <param name="currentLives">現在の残機</param>
    private void UpdateIcons(int currentLives)
    {
        for(int i = 0; i < m_LifeUIs.Length; i++)
        {
            m_LifeUIs[i].enabled = i < currentLives;
        }
    }
}
