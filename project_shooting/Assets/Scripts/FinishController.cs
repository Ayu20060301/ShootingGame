using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;


public class FinishController : MonoBehaviour
{

    [SerializeField]
    private EnemyFlash m_Flash;

    [SerializeField]
    private int m_ExplosionCount = 7;
    [SerializeField]
    private float m_ExplosionInterval = 0.35f; 
    [SerializeField]
    private float m_ExplosionRadius = 1.0f;
    [SerializeField]
    private float m_EndWaitTime = 8.0f;

    [SerializeField]
    private TMP_Text m_ResultText;

    private bool m_IsFinished = false;  //終了しているか

    /// <summary>
    /// ゲーム終了処理
    /// </summary>
    /// <param name="isClear">クリアしているかどうか</param>
    /// <param name="position">指定の座標</param>
    /// <param name="target">ターゲット</param>

    public void Finish(bool isClear,Vector3 position, GameObject target)
    {
        if (m_IsFinished) return; 
        
        m_IsFinished = true;
        
        StartCoroutine(FinishSequence(isClear, position, target));
    }

    /// <summary>
    /// ゲーム終了の順序
    /// </summary>
    /// <param name="isClear">クリアしているか</param>
    /// <param name="position">指定の座標</param>
    /// <param name="target">ターゲット</param>
    /// <returns></returns>
    private IEnumerator FinishSequence(bool isClear,Vector3 position,GameObject target)
    {
        GameManager.Instance.isActive = false;


        yield return new WaitForSecondsRealtime(1.0f);

        if(isClear)
        {
            yield return StartCoroutine(PlayEnemyExplosion(position));
        }
        else
        {
            yield return StartCoroutine(PlayPlayerExplosion(position));
        }

        DestroyTarget(target);

        yield return new WaitForSecondsRealtime(1.0f);

        BGMManager.Instance.AudioStop();

        if (isClear)
        {
            BGMManager.Instance.BGMPlay(BGMType.CLEAR);
            m_ResultText.text = "Game Clear";
        }
        else
        {
            BGMManager.Instance.BGMPlay(BGMType.GAMEOVER);
            m_ResultText.text = "Game Over";
        }


        yield return new WaitForSecondsRealtime(m_EndWaitTime);

        GameManager.Instance.GameEnd(isClear);
    }

    /// <summary>
    /// 敵の爆発演出
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    private IEnumerator PlayEnemyExplosion(Vector3 position)
    {
        //敵を点滅させる
        m_Flash.Flash();

        for (int i = 0; i < m_ExplosionCount; i++)
        {
            Vector2 offset = Random.insideUnitCircle * m_ExplosionRadius;

            EffectManager.Instance.PlayEffect(
                EffectType.EXPLOSION,
                position + (Vector3)offset
                );

            SEManager.Instance.SEPlay(SEType.EXPLOSION);

            yield return new WaitForSecondsRealtime(m_ExplosionInterval);
        }


        yield return new WaitForSecondsRealtime(1.0f);

        //最後に中央で大爆発
        EffectManager.Instance.PlayEffect(
            EffectType.EXPLOSION,
            position,
            Vector3.one * 6.0f
            );

        SEManager.Instance.SEPlay(SEType.EXPLOSION);

        yield return new WaitForSecondsRealtime(0.1f);
    }

    /// <summary>
    /// プレイヤーの爆発演出
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    private IEnumerator PlayPlayerExplosion(Vector3 position)
    {
        EffectManager.Instance.PlayEffect(
            EffectType.EXPLOSION,
            position);

        SEManager.Instance.SEPlay(SEType.EXPLOSION);

        yield return new WaitForSecondsRealtime(0.1f);
    }

    /// <summary>
    /// オブジェクト削除
    /// </summary>
    /// <param name="target">ターゲット</param>
    private void DestroyTarget(GameObject target)
    {
        if(target != null)
        {
            Destroy(target);
        }
    }

}
