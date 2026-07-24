using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField]
    private int m_MaxHP = 60000;

    [SerializeField]
    private int m_CurrentHP;

    [Header("HPバー")]
    [SerializeField]
    private EnemyHPBar m_EnemyHPBar;

    private float m_DecreaseSpeed = 30000.0f; //1秒あたりに減らすHP量

    private float m_DisplayedHP; //徐々に減る値

    private bool m_IsDead = false; 

    private void Start()
    {
        m_CurrentHP = m_MaxHP;
        m_DisplayedHP = m_MaxHP;
        m_EnemyHPBar.SetHP(m_CurrentHP, m_MaxHP);
    }

    private void Update()
    {
        if(m_DisplayedHP > m_CurrentHP)
        {
            m_DisplayedHP -= m_DecreaseSpeed * Time.deltaTime;
            m_DisplayedHP = Mathf.Max(m_DisplayedHP, m_CurrentHP);

            m_EnemyHPBar.SetHP(Mathf.RoundToInt(m_DisplayedHP), m_MaxHP);
        }
    }

    /// <summary>
    /// ダメージを受ける処理
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    public void TakeDamage(int damage)
    {

        if (m_IsDead) return;

        m_CurrentHP -= damage;
        m_CurrentHP = Mathf.Max(m_CurrentHP, 0);
        
        if(m_CurrentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {

    }

}
