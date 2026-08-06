using System.Collections;
using UnityEngine;
using TMPro;

//ゲーム終了処理
public class FinishController : MonoBehaviour
{
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
    /// 終了演出シーケンス
    /// </summary>
    /// <param name="isClear">クリアしているか</param>
    /// <param name="position">指定の座標</param>
    /// <param name="target">ターゲット</param>
    /// <returns></returns>
    private IEnumerator FinishSequence(bool isClear,Vector3 position,GameObject target)
    {
        GameManager.Instance.isActive = false;


        yield return new WaitForSecondsRealtime(1.0f);

        yield return isClear
            ? PlayEnemyExplosion(position)
            : PlayPlayerExplosion(position);

        DestroyTarget(target);

        yield return new WaitForSecondsRealtime(1.0f);

        PlayResult(isClear);

        yield return new WaitForSecondsRealtime(m_EndWaitTime);

        GameManager.Instance.GameEnd(isClear);
    }


    private void PlayResult(bool isClear)
    {
        BGMManager.Instance.AudioStop();

        if(isClear)
        {
            BGMManager.Instance.BGMPlay(BGMType.CLEAR);
            m_ResultText.text = "Game Clear";
        }
        else
        {
            BGMManager.Instance.BGMPlay(BGMType.GAMEOVER);
            m_ResultText.text = "Game Over";
        }

    }

    /// <summary>
    /// 敵の爆発演出
    /// </summary>
    /// <param name="position">敵のポジション</param>
    /// <returns></returns>
    private IEnumerator PlayEnemyExplosion(Vector3 position)
    {

        for (int i = 0; i < m_ExplosionCount; i++)
        {

            CreateExplosion(
                position + (Vector3)(Random.insideUnitCircle * m_ExplosionRadius)
                );

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
    /// <param name="position">プレイヤーのポジション</param>
    /// <returns></returns>
    private IEnumerator PlayPlayerExplosion(Vector3 position)
    {
        CreateExplosion(position);

        yield return new WaitForSecondsRealtime(0.1f);
    }


    /// <summary>
    /// 爆発の生成
    /// </summary>
    /// <param name="position">爆発させるポジション</param>
    private void CreateExplosion(Vector3 position)
    {
        EffectManager.Instance.PlayEffect(
            EffectType.EXPLOSION,
            position
            );

        SEManager.Instance.SEPlay(SEType.EXPLOSION);
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
