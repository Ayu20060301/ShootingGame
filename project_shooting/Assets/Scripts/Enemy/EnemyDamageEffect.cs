using System.Collections;
using UnityEngine;

public class EnemyDamageEffect : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer m_SpriteRenderer;

    [SerializeField]
    private float m_FlashTime = 0.1f;

    private Color m_DefaultColor;

    private Coroutine m_FlashCoroutine;

    private void Awake()
    {
        m_DefaultColor = m_SpriteRenderer.color;
    }

    public void Flashed()
    {
        if(m_FlashCoroutine != null)
        {
            StopCoroutine(m_FlashCoroutine);
        }

        m_FlashCoroutine = StartCoroutine(FlashCoroutine());
    }

    private IEnumerator FlashCoroutine()
    {
        m_SpriteRenderer.color = Color.red;

        yield return new WaitForSecondsRealtime(m_FlashTime);

        m_SpriteRenderer.color = m_DefaultColor;
    }

}
