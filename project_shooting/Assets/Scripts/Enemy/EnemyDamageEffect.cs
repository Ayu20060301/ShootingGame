using System.Collections;
using UnityEngine;

//敵がダメージを受けた際の点滅処理を管理するクラス
public class EnemyDamageEffect : MonoBehaviour
{

    [Header("ダメージに色を変更する時間")]
    [SerializeField]
    private float m_FlashTime = 0.1f;

    //敵のSpriteRenderer
    private SpriteRenderer m_SpriteRenderer;

    //通常時のSpriteの色
    private Color m_DefaultColor;

    //現在実行中の点滅コルーチン
    private Coroutine m_FlashCoroutine;

    private void Start()
    {
        //コーポネントの取得
        m_SpriteRenderer = GetComponent<SpriteRenderer>();

        //通常時の色を保存
        m_DefaultColor = m_SpriteRenderer.color;
    }

    /// <summary>
    /// ダメージ時の点滅演出を開始
    /// </summary>
    public void Flashed()
    {
        //既に点滅中なら最初からやり直す
        if(m_FlashCoroutine != null)
        {
            //現在の点滅演出を停止
            StopCoroutine(m_FlashCoroutine);

            //元の色に戻す
            m_SpriteRenderer.color = m_DefaultColor;
        }

        //新しい点滅演出を開始
        m_FlashCoroutine = StartCoroutine(FlashCoroutine());
    }

    /// <summary>
    /// 一瞬だけ赤く点滅させる
    /// </summary>
    /// <returns>コルーチン</returns>
    private IEnumerator FlashCoroutine()
    {
        //ダメージ時の色に変更
        m_SpriteRenderer.color = Color.red;

        //指定時間待機
        yield return new WaitForSecondsRealtime(m_FlashTime);

        //通常時の色に戻す
        m_SpriteRenderer.color = m_DefaultColor;

        //コルーチンの参照を解除
        m_FlashCoroutine = null;
    }
}
