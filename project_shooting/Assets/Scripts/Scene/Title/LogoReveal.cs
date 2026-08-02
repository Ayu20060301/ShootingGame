using System.Collections;
using UnityEngine;

public class LogoReveal : MonoBehaviour
{
    [SerializeField]
    private RectTransform m_Mask;

    [SerializeField]
    private float m_Duration = 2.0f;

    private float m_MaxWidth;


    private void Start()
    {
        m_MaxWidth = m_Mask.sizeDelta.x;

        //‰¡0‚©‚çŠJŽn
        m_Mask.sizeDelta = new Vector2(0, m_Mask.sizeDelta.y);


        StartCoroutine(Reveal());
    }

    private IEnumerator Reveal()
    {
        float time = 0.0f;

        while(time < m_Duration)
        {
            time += Time.deltaTime;

            float width = Mathf.Lerp(0, m_MaxWidth, time / m_Duration);


            m_Mask.sizeDelta = new Vector2(width, m_Mask.sizeDelta.y);

            yield return null;
        }

        m_Mask.sizeDelta = new Vector2(m_MaxWidth, m_Mask.sizeDelta.y);
    }

}
