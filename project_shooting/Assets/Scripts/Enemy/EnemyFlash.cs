using System.Collections;
using UnityEngine;

public class EnemyFlash : MonoBehaviour
{
    [SerializeField]
    private float m_FlashInterval = 0.1f; //点滅間隔
   
    [SerializeField]
    private SpriteRenderer m_SP; //敵の画像スプライト

    public void Flash()
    {
        StartCoroutine(FlashCoroutine());
    }

    //点滅させる処理
    private IEnumerator FlashCoroutine()
    {
        //点滅ループ開始
       while(true)
        {
            m_SP.enabled = false;
            yield return new WaitForSecondsRealtime(m_FlashInterval);

            m_SP.enabled = true;
            yield return new WaitForSecondsRealtime(m_FlashInterval);
        }
    }
}
