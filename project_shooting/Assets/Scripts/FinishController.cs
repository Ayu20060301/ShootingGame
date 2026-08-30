using System.Collections;
using UnityEngine;
using TMPro;


public class FinishController : MonoBehaviour
{

    //------------------
    //定数
    //------------------

    // 終了演出開始までの待機時間
    private const float FINISH_START_WAIT = 1.0f;

    // 結果表示前の待機時間
    private const float RESULT_WAIT_TIME = 1.0f;

    // 爆発演出終了後の待機時間
    private const float EXPLOSION_END_WAIT = 0.1f;

    // 最後の大爆発のサイズ
    private const float FINAL_EXPLOSION_SCALE = 6.0f;


    [Header("爆発回数")]
    [SerializeField]
    private int m_ExplosionCount = 7; 

    [Header("爆発間隔")]
    [SerializeField]
    private float m_ExplosionInterval = 0.35f;

    [Header("爆発位置のランダム範囲")]
    [SerializeField]
    private float m_ExplosionRadius = 1.0f; 

    [Header("リザルト画面へ移動するまでの待機時間")]
    [SerializeField]
    private float m_EndWaitTime = 8.0f;

    [Header("リザルトテキスト表示")]
    [SerializeField]
    private TMP_Text m_ResultText;

    //終了処理がすでに開始されているか
    private bool m_IsFinished = false;

    /// <summary>
    /// ゲーム終了処理
    /// </summary>
    /// <param name="isClear">クリアしているかどうか</param>
    /// <param name="position">終了演出を行う座標</param>
    /// <param name="target">削除する対象オブジェクト</param>

    public void Finish(bool isClear,Vector3 position, GameObject target)
    {
        //終了処理がすでに開始されている場合は無視
        if (m_IsFinished) return; 
        
        //終了処理開始
        m_IsFinished = true;
        
        StartCoroutine(FinishSequence(isClear, position, target));
    }

    /// <summary>
    /// 終了演出シーケンス
    /// </summary>
    /// <param name="isClear">クリアしているか</param>
    /// <param name="position">終了演出を行う座標</param>
    /// <param name="target">削除する対象オブジェクト</param>
    /// <returns></returns>
    private IEnumerator FinishSequence(bool isClear,Vector3 position,GameObject target)
    {
        //ゲーム中の処理を停止
        GameManager.Instance.isActive = false;

        //終了演出開始まで少し待機
        yield return new WaitForSeconds(FINISH_START_WAIT);

        //クリア・ゲームオーバーに応じた爆発処理
        if(isClear)
        {
            yield return PlayEnemyExplosion(position);
        }
        else
        {
            yield return PlayPlayerExplosion(position); 
        }

        //対象オブジェクトを削除
        DestroyTarget(target);

        //結果表示まで待機
        yield return new WaitForSeconds(RESULT_WAIT_TIME);

        //Clear / Game Overを表示
        PlayResult(isClear);

        //結果画面へ移動するまで待機
        yield return new WaitForSeconds(m_EndWaitTime);

        //ゲーム終了
        GameManager.Instance.GameEnd(isClear);
    }

    /// <summary>
    /// Clear / Game Overの演出を行う
    /// </summary>
    /// <param name="isClear">ゲームをクリアしたか</param>
    private void PlayResult(bool isClear)
    {
        //現在のBGMを停止
        BGMManager.Instance.AudioStop();

        if(isClear)
        {
            //クリア時のBGMと文字を設定
            BGMManager.Instance.BGMPlay(BGMType.CLEAR);
            m_ResultText.text = "Game Clear";
        }
        else
        {
            //ゲームオーバー時のBGM知二を設定
            BGMManager.Instance.BGMPlay(BGMType.GAMEOVER);
            m_ResultText.text = "Game Over";
        }
    }

    /// <summary>
    /// 敵の爆発演出
    /// </summary>
    /// <param name="position">敵の座標</param>
    /// <returns></returns>
    private IEnumerator PlayEnemyExplosion(Vector3 position)
    {
        //複数回爆発させる
        for (int i = 0; i < m_ExplosionCount; i++)
        {
            Vector3 explosionPosition = position + (Vector3)(Random.insideUnitCircle * m_ExplosionRadius);

            CreateExplosion(explosionPosition);

            yield return new WaitForSeconds(m_ExplosionInterval);
        }

        //最後の大爆発まで少し待機
        yield return new WaitForSeconds(RESULT_WAIT_TIME);

        //最後に中央で大爆発
        EffectManager.Instance.PlayEffect(EffectType.EXPLOSION,position,Vector3.one * FINAL_EXPLOSION_SCALE);

        //爆発SEを再生
        SEManager.Instance.SEPlay(SEType.EXPLOSION);

        //終了演出開始まで待機する
        yield return new WaitForSeconds(EXPLOSION_END_WAIT);
    }

    /// <summary>
    /// プレイヤーの爆発演出
    /// </summary>
    /// <param name="position">プレイヤーの座標</param>
    /// <returns></returns>
    private IEnumerator PlayPlayerExplosion(Vector3 position)
    {
        //プレイヤーの位置で爆発
        CreateExplosion(position);

        yield return new WaitForSeconds(EXPLOSION_END_WAIT);
    }


    /// <summary>
    /// 爆発の生成
    /// </summary>
    /// <param name="position">爆発させるポジション</param>
    private void CreateExplosion(Vector3 position)
    {
        //爆発エフェクトを生成
        EffectManager.Instance.PlayEffect(EffectType.EXPLOSION,position);

        SEManager.Instance.SEPlay(SEType.EXPLOSION);
    }


    /// <summary>
    /// オブジェクト削除
    /// </summary>
    /// <param name="target">削除対象のオブジェクト</param>
    private void DestroyTarget(GameObject target)
    {
        if (target == null) return;

        Destroy(target);
    }
}
