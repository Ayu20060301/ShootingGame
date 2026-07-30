using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T m_Instance;

    public static T Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = (T)FindFirstObjectByType(typeof(T));

                if (m_Instance == null)
                {
                    Debug.LogError($"{typeof(T).Name}Ç™ÉVÅ[Éìè„Ç…ë∂ç›ÇµÇ‹ÇπÇÒÅB");
                }
            }

            return m_Instance;
        }
    }

    protected virtual void Awake()
    {
        if(m_Instance == null)
        {
            m_Instance = this as T;
            DontDestroyOnLoad(this.gameObject);
        }
        else if(m_Instance != this)
        {
            Destroy(this.gameObject);
        }
    }
}
