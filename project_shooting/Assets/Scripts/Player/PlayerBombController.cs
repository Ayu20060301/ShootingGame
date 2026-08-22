using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBombController : MonoBehaviour
{
    [Header("生成するボムのPrefab")]
    [SerializeField]
    private GameObject m_BombPrefab;

    [Header("ボムを生成する位置")]
    [SerializeField]
    private Transform m_BombSpawnPoint;

    [Header("爆発エフェクトが表示されている時間")]
    [SerializeField]
    private float m_EffectTime = 0.5f;

    [Header("ボムが点滅する時間")]
    [SerializeField]
    private float m_BlinkInterval = 0.1f; 

    [Header("ボム数を表示するUI")]
    [SerializeField]
    private PlayerBombUI m_BombUI;

    private bool m_IsActive; //現在ボムの演出中かどうか

   

    private void Start()
    {
        //ゲーム開始時はボムを最大数まで所持させる
        GameManager.Instance.currentBomb = GameManager.Instance.maxBomb;

        m_BombUI.UpdateUI();
    }

    /// <summary>
    /// ボムを使用する
    /// </summary>
    public void UseBomb()
    {
        //ゲーム中でなければ処理を行わない
        if (!GameManager.Instance.isActive) return;


        //ボム演出中でなければ使用しない
        if (m_IsActive) return;

        //ボムが残っていなければ使用しない
        if (GameManager.Instance.currentBomb <= 0) return;

        //ボムを1個消費
        GameManager.Instance.currentBomb--;

        //UIを更新
        if(m_BombUI != null)
        {
            m_BombUI.UpdateUI();
        }

        //ボムを発動
        ActivateBomb();
    }

    /// <summary>
    /// ボム演出を開始する
    /// </summary>
    private void ActivateBomb()
    {
        //演出中なら開始しない
        if (m_IsActive) return;

        StartCoroutine(BombSequence());
    }

    /// <summary>
    /// ボム演出全体を管理するコルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator BombSequence()
    {

        m_IsActive = true;

        //ボム発動中はUIを薄暗くする
        m_BombUI.SetDim(true);

        //ボムを生成する
        GameObject bomb = SpawnBomb();

        //生成に失敗した場合は演出を終了する
        if(bomb == null)
        {
            m_IsActive = false;
            yield break;
        }

        //ボムの位置を保存
        Vector3 bombPosition = bomb.transform.position;

        //爆発前にボムを点滅させる
        yield return StartCoroutine(BlinkBomb(bomb, 1.0f));
        
        //爆発直前にボムを拡大する
        yield return StartCoroutine(ScaleBomb(bomb));

        //敵弾を消去する
        ClearEnemyBullets();

        //ボム全体を削除する
        Destroy(bomb);

        //爆発SEを再生する
        SEManager.Instance.SEPlay(SEType.BOMB_EXPLOSION);

        //爆発エフェクトを生成する
        EffectManager.Instance.PlayEffect(EffectType.EXPLOSION,bombPosition, Vector3.one * 10.0f);

        //爆発演出が終わるまで待機する
        yield return new WaitForSeconds(m_EffectTime);

        //ボム発動終了後、UIを通常表示に戻す
        m_BombUI.SetDim(false);

        m_IsActive = false;
    }


   /// <summary>
   /// ボムを生成する
   /// </summary>
   /// <returns>生成したボム</returns>
    private GameObject SpawnBomb()
    {
        //プレハブが設定されていない場合は生成できない
        if(m_BombPrefab == null)
        {
            Debug.LogError("BombPrefabが設定されていません。");
            return null;
        }

        //プレイヤーの位置に生成する
        Vector3 spawnPosition = this.transform.position;

        //正誠一が指定されている場合はそちらを使用する
        if(m_BombSpawnPoint != null)
        {
            spawnPosition = m_BombSpawnPoint.position;
        }

        return Instantiate(m_BombPrefab, spawnPosition, Quaternion.identity);
    }


    /// <summary>
    /// 敵弾を全て消す
    /// </summary>
    private void ClearEnemyBullets()
    {
        BulletBase[] bullets = FindObjectsByType<BulletBase>(FindObjectsSortMode.None);

        foreach(BulletBase bullet in bullets)
        {
            //EnemyBulletとHomingBulletを対象にする
            if(bullet is EnemyBullet || bullet is HomingBullet)
            {
                bullet.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// ボムを点滅させる
    /// </summary>
    /// <param name="bomb">点滅させるボム</param>
    /// <param name="duration">点滅する時間</param>
    /// <returns></returns>
    private IEnumerator BlinkBomb(GameObject bomb,float duration)
    {

        SpriteRenderer spriteRenderer = bomb.GetComponent<SpriteRenderer>();

        //SpriteRendererがなければ点滅できない
        if(spriteRenderer == null)
        {
            Debug.LogWarning("ボムにSpriteRendererがありません。");
            yield break;
        }

        float elapsedTime = 0.0f;
        bool isVisible = true;

        while(elapsedTime < duration)
        {
            //表示状態を切り替える
            isVisible = !isVisible;
            spriteRenderer.enabled = isVisible;

            //次の点滅まで待機する
            yield return new WaitForSeconds(m_BlinkInterval);

            elapsedTime += m_BlinkInterval;
        }

        //演出終了時は必ず表示状態に戻す
        spriteRenderer.enabled = true;
    }

    /// <summary>
    /// 爆発直前に一瞬だけ大きくする
    /// </summary>
    /// <param name="bomb">拡大するボム</param>
    /// <returns></returns>
    private IEnumerator ScaleBomb(GameObject bomb)
    {
        //元のスケールを保存
        Vector3 originalScale = bomb.transform.localScale;

        //最終的なスケール
        Vector3 targetScale = originalScale * 1.5f;

        //拡大に書ける時間
        float duration = 0.1f;

        //経過時間
        float elapsedTime = 0.0f;

        //徐々に大きくする
        while(elapsedTime < duration)
        {
            //0～1の割合を計算
            float t = elapsedTime / duration;

            //元の大きさから目標の大きさまで徐々に拡大
            bomb.transform.localScale = Vector3.Lerp(originalScale, targetScale,t);

            //時間を進める
            elapsedTime += Time.deltaTime;

            //待機
            yield return null;
        }

        //最終的な大きさに合わせる
        bomb.transform.localScale = targetScale;
    }
}
