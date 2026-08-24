using System.Collections;
using UnityEngine;

//プレイヤーの被弾時の無敵・点滅処理
public class PlayerFlash : MonoBehaviour
{
    [Header("スプライトを切り替える間隔")]
    [SerializeField]
    private float m_FlashInterval = 0.1f;

    [Header("点滅回数")]
    [SerializeField]
    private int m_LoopCount = 60; 

    //プレイヤーの画像スプライト
    private SpriteRenderer m_SpriteRenderer;

    //プレイヤーのCollider
    private PolygonCollider2D m_Collider;

    //現在無敵状態か
    private bool m_IsInvincible; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //コンポーネントの取得
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_Collider = GetComponent<PolygonCollider2D>();
    }

    /// <summary>
    /// 弾との当たり判定
    /// </summary>
    public void BulletHit()
    {
        //すでに無敵中なら処理しない
        if (m_IsInvincible) return;
        

        //無敵・点滅処理を開始
        StartCoroutine(HitCoroutine());
    }

    /// <summary>
    /// 点滅させる処理
    /// </summary>
    /// <returns></returns>
    private IEnumerator HitCoroutine()
    {
        //無敵状態にする
        m_IsInvincible = true;

        //無敵中は敵弾との当たり判定を無効にする
        SetColliderEnabled(false);

        //点滅ループ開始
        for(int i = 0; i < m_LoopCount; i++)
        {
            //非表示
            SetSpriteVisible(false);

            yield return new WaitForSeconds(m_FlashInterval);

            //表示
            SetSpriteVisible(true);
            
            yield return new WaitForSeconds(m_FlashInterval);
        }


        //演出終了後はスプライトを表示のままにする
        SetSpriteVisible(true);

        //当たり判定を再び有効化する
        SetColliderEnabled(true);

        //点滅ループが抜けたら当たりフラグをfalse
        m_IsInvincible = false;
    }

    /// <summary>
    /// プレイヤーのスプライト表示状態を変更する
    /// </summary>
    /// <param name="isVisible">表示する場合はtrue</param>
    private void SetSpriteVisible(bool isVisible)
    {
        if (m_SpriteRenderer == null) return;

        m_SpriteRenderer.enabled = isVisible;
    }

    /// <summary>
    /// プレイヤーの当たり判定の有効・無効を切り替える
    /// </summary>
    /// <param name="isEnabled">有効にする場合はtrue</param>
    private void SetColliderEnabled(bool isEnabled)
    {
        if (m_Collider == null) return;

        m_Collider.enabled = isEnabled;
    }
}
