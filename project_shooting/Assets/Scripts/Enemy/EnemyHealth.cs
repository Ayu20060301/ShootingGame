using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField]
    private int m_MaxHP = 10000;

    [SerializeField]
    private int m_CurrentHP;

    [SerializeField]
    private EnemyAttackController m_EnemyAttackController;
    [SerializeField]
    private EnemyController m_EnemyController;

    [Header("HPバー")]
    [SerializeField]
    private EnemyHPBar m_EnemyHPBar;


    private float m_DecreaseSpeed = 30000.0f; //1秒あたりに減らすHP量

    private float m_DisplayedHP; //徐々に減る値

    private bool m_IsDead = false;


    private EnemyAttackController.EnemyPhase m_CurrentPhase = EnemyAttackController.EnemyPhase.NORMAL;

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

            CheckPhase();
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

        CheckPhase();

        if(m_CurrentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {

        m_IsDead = true;

        Destroy(this.gameObject);

        GameManager.Instance.GameEnd(true);

    }

    private void CheckPhase()
    {
        float ratio = (float)m_CurrentHP / m_MaxHP;

        EnemyAttackController.EnemyPhase nextPhase;

        if(ratio <= 0.2f)
        {
            nextPhase = EnemyAttackController.EnemyPhase.PHASE2;
        }
        else if(ratio <= 0.5f)
        {
            nextPhase = EnemyAttackController.EnemyPhase.PHASE1;
        }
        else
        {
            nextPhase = EnemyAttackController.EnemyPhase.NORMAL;
        }

        if(nextPhase != m_CurrentPhase)
        {
            m_CurrentPhase = nextPhase;

            //攻撃フェーズ変更
            m_EnemyAttackController.SetPhase(nextPhase);

            //移動フェーズ変更
            m_EnemyController.SetPhase(nextPhase);
        }
    }

}
