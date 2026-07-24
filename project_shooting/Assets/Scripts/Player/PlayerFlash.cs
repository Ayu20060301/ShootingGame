using System.Collections;
using UnityEngine;

public class PlayerFlash : MonoBehaviour
{

    [SerializeField]
    private float m_FlashInterval = 0.1f; //点滅間隔
    [SerializeField]
    private int m_LoopCount = 60;

    private SpriteRenderer m_SP; //プレイヤーの画像スプライト

    private PolygonCollider2D m__Py2D;

    private bool m_IsHit; //当たったかどうかのフラグ

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //コンポーネントの取得
        m_SP = GetComponent<SpriteRenderer>();
        m__Py2D = GetComponent<PolygonCollider2D>();
    }

    /// <summary>
    /// 弾との当たり判定
    /// </summary>
    public void BulletHit()
    {
        //Hitしていたら処理を行わない
        if (m_IsHit)
        {
            return;
        }

        //コルーチンを開始
        StartCoroutine(HitCoroutine());
    }

    //点滅させる処理
    private IEnumerator HitCoroutine()
    {
        //当たりフラグをtrueに変更
        m_IsHit = true;

        //無敵中は当たり判定を無効化
        m__Py2D.enabled = false;

        //点滅ループ開始
        for(int i = 0; i < m_LoopCount; i++)
        {

            yield return new WaitForSeconds(m_FlashInterval);

            //spriteRendererをオフ
            m_SP.enabled = false;

            yield return new WaitForSeconds(m_FlashInterval);

            //spriteRendererをオン
            m_SP.enabled = true;
        }

        //点滅ループが抜けたら当たりフラグをfalse
        m_IsHit = false;

        //当たり判定を有効化
        m__Py2D.enabled = true;
    }
}
