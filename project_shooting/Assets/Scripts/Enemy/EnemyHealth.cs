using UnityEngine;

//敵のHPやダメージ処理を管理するクラス
public class EnemyHealth : MonoBehaviour
{
    [Header("敵撃破時の終了演出を管理するクラス")]
    [SerializeField]
    private FinishController m_FinishController;

    [Header("敵の攻撃を管理するクラス")]
    [SerializeField]
    private EnemyAttackController m_EnemyAttackController;

    [Header("敵の移動制御")]
    [SerializeField]
    private EnemyController m_EnemyController;

    [Header("敵のHPゲージ表示")]
    [SerializeField]
    private EnemyHPBar m_EnemyHPBar;

    [Header("敵がダメージ受けた際のエフェクト処理")]
    [SerializeField]
    private EnemyDamageEffect m_EnemyDamageEffect;

    //1秒あたりに減らすHP量
    private float m_DecreaseSpeed = 30000.0f;

    //徐々に減る値
    private float m_DisplayedHP;

    //敵が死亡しているかどうか
    private bool m_IsDead = false;

    //現在の敵のフェーズ
    private EnemyAttackController.EnemyPhase m_CurrentPhase = EnemyAttackController.EnemyPhase.NORMAL;

    private void Start()
    {
        //最大HPを初期表示HPとして設定
        m_DisplayedHP = GameManager.Instance.maxEnemyHP;
        
        //HPバーを初期状態に設定
        m_EnemyHPBar.SetHP(GameManager.Instance.currentEnemyHP, GameManager.Instance.maxEnemyHP);
    }

    private void Update()
    {
        UpdateHPDisplay();
    }

    /// <summary>
    /// ダメージを受ける処理
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    public void TakeDamage(int damage)
    {
        //既に死亡している場合は処理をしない
        if (m_IsDead) return;

        //敵のHPを減少
        GameManager.Instance.currentEnemyHP -= damage;
        GameManager.Instance.currentEnemyHP = Mathf.Max(GameManager.Instance.currentEnemyHP, 0);

        //ダメージ音を再生
        SEManager.Instance.SEPlay(SEType.DAMAGE_ENEMY);
        
        //ダメージエフェクトを再生
        m_EnemyDamageEffect.Flashed();

        //HPに応じてフェーズを確認
        CheckPhase();

        //HPが0になった場合
        if(GameManager.Instance.currentEnemyHP <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// HPバーの表示を徐々に現在HPへ近づける
    /// </summary>
    private void  UpdateHPDisplay()
    {
        //今現在のHP
        int currentHP = GameManager.Instance.currentEnemyHP;

        //表示HPが実際のHPより大きい場合のみ減少
        if(m_DisplayedHP <= currentHP) return;

        //表示HPを徐々に減少
        m_DisplayedHP -= m_DecreaseSpeed * Time.deltaTime;

        //実際のHPを下回らないように制限
        m_DisplayedHP = Mathf.Max(m_DisplayedHP, currentHP);

        //HPバーを更新
        m_EnemyHPBar.SetHP(Mathf.RoundToInt(m_DisplayedHP), GameManager.Instance.maxEnemyHP);
    }

    /// <summary>
    /// 死亡処理
    /// </summary>
    private void Die()
    {
        //死亡済みの場合は処理をしない
        if (m_IsDead) return;

        //死亡状態にする
        m_IsDead = true;

        //敵のColliderを無効化にする
        DisableCollider();

        //敵の攻撃を停止
        m_EnemyAttackController.enabled = false;

        //敵の移動を停止
        m_EnemyController.enabled = false;

        //終了演出開始
        m_FinishController.Finish(true, transform.position,gameObject);
    }

    /// <summary>
    /// 敵のColliderを無効化にする
    /// </summary>
    private void DisableCollider()
    {
        Collider2D col = GetComponent<Collider2D>();

        if(col == null) return;

        col.enabled = false;
    }

    /// <summary>
    /// 現在のHP割合に応じてフェーズを変更する
    /// </summary>
    private void CheckPhase()
    {
        //現在のHP割合を取得
        float hpRatio = (float)GameManager.Instance.currentEnemyHP / GameManager.Instance.maxEnemyHP;

        //HP割合から次のフェーズを取得
        EnemyAttackController.EnemyPhase nextPhase = GetPhaseFromHP(hpRatio);

        //フェーズが変化していない場合は処理をしない
        if (nextPhase == m_CurrentPhase) return;

        //現在のフェーズを更新
        m_CurrentPhase = nextPhase;

        //攻撃フェーズを変更
        m_EnemyAttackController.SetPhase(nextPhase);

        //移動フェーズを変更
        m_EnemyController.SetPhase(nextPhase);
    }

    /// <summary>
    /// HP割合に応じたフェーズを取得する
    /// </summary>
    /// <param name="hpRatio">現在のHP割合</param>
    /// <returns>対応する敵のフェーズ</returns>
    private EnemyAttackController.EnemyPhase GetPhaseFromHP(float hpRatio)
    {
        //HPが20%以下の場合
        if(hpRatio <= 0.2f)
        {
            return EnemyAttackController.EnemyPhase.PHASE2;
        }

        //HPが50%以下の場合
        if(hpRatio <= 0.5f)
        {
            return EnemyAttackController.EnemyPhase.PHASE1;
        }

        //HPが59%より多い場合
        return EnemyAttackController.EnemyPhase.NORMAL;
    }
}
