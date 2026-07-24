using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBar : MonoBehaviour
{
    [SerializeField]
    private Image m_HPUI; //HPÉQÅ[ÉWópÇÃUI

    public void SetHP(int current, int max)
    {
        m_HPUI.fillAmount = (float)current / max;
    }
}
