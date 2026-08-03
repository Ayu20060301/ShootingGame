using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    [SerializeField]
    private FinishController m_FinishController;
  
    private EnemyAttackController m_EnemyAttackController;
    private EnemyController m_EnemyController;
    private EnemyHPBar m_EnemyHPBar;
    private EnemyDamageEffect m_EnemyDamageEffect;

    private float m_DecreaseSpeed = 30000.0f; //1秒あたりに減らすHP量

    private float m_DisplayedHP; //徐々に減る値

    private bool m_IsDead = false;


    private EnemyAttackController.EnemyPhase m_CurrentPhase = EnemyAttackController.EnemyPhase.NORMAL;

    private void Start()
    {
        //コーポネントの取得
        m_EnemyAttackController = GetComponent<EnemyAttackController>();
        m_EnemyController = GetComponent<EnemyController>();
        m_EnemyHPBar = GetComponent<EnemyHPBar>();
        m_EnemyDamageEffect = GetComponent<EnemyDamageEffect>();

        GameManager.Instance.currentEnemyHP = GameManager.Instance.maxEnemyHP;
        m_DisplayedHP = GameManager.Instance.maxEnemyHP;
        m_EnemyHPBar.SetHP(GameManager.Instance.currentEnemyHP, GameManager.Instance.maxEnemyHP);
    }

    private void Update()
    {
        if(m_DisplayedHP > GameManager.Instance.currentEnemyHP)
        {
            m_DisplayedHP -= m_DecreaseSpeed * Time.deltaTime;
            m_DisplayedHP = Mathf.Max(m_DisplayedHP, GameManager.Instance.currentEnemyHP);

           
            m_EnemyHPBar.SetHP(Mathf.RoundToInt(m_DisplayedHP), GameManager.Instance.maxEnemyHP);

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

        GameManager.Instance.currentEnemyHP -= damage;
        GameManager.Instance.currentEnemyHP = Mathf.Max(GameManager.Instance.currentEnemyHP, 0);

        SEManager.Instance.SEPlay(SEType.DAMAGE_ENEMY);
        m_EnemyDamageEffect.Flashed();

        CheckPhase();

        if(GameManager.Instance.currentEnemyHP <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// 死亡処理
    /// </summary>
    private void Die()
    {

        if (m_IsDead) return;

        m_IsDead = true;

       

        //当たり判定や攻撃を防止
        Collider2D col = GetComponent<Collider2D>();
        if(col != null)
        {
            col.enabled = false;
        }

        m_EnemyAttackController.enabled = false;
        m_EnemyController.enabled = false;


        //終了演出開始
        m_FinishController.Finish(true, transform.position,gameObject);
    }


    private void CheckPhase()
    {
        float ratio = (float)GameManager.Instance.currentEnemyHP / GameManager.Instance.maxEnemyHP;

        EnemyAttackController.EnemyPhase nextPhase;

        if(ratio <= 0.2f)
        {
            nextPhase = EnemyAttackController.EnemyPhase.PHASE2;
        }
        else if(ratio <= 0.50f)
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
