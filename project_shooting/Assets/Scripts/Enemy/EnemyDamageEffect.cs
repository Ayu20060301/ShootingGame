using System.Collections;
using UnityEngine;

public class EnemyDamageEffect : MonoBehaviour
{
 
    private SpriteRenderer m_SpriteRenderer;

    [SerializeField]
    private float m_FlashTime = 0.1f;

    private Color m_DefaultColor;

    private Coroutine m_FlashCoroutine;

    private void Awake()
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
            StopCoroutine(m_FlashCoroutine);
        }

        m_FlashCoroutine = StartCoroutine(FlashCoroutine());
    }

    /// <summary>
    /// 一瞬だけ赤く点滅させる
    /// </summary>
    /// <returns></returns>
    private IEnumerator FlashCoroutine()
    {
        //赤色に変更
        m_SpriteRenderer.color = Color.red;

        //指定時間待機
        yield return new WaitForSecondsRealtime(m_FlashTime);

        //元の色に戻す
        m_SpriteRenderer.color = m_DefaultColor;

        //コルーチンの参照をクリア
        m_FlashCoroutine = null;
    }

}
