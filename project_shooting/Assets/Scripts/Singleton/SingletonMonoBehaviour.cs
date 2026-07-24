using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T m_Instance;

    public static T Instance
    {
        get
        {
            if(m_Instance == null)
            {
                m_Instance = (T)FindObjectOfType(typeof(T));

                if(m_Instance == null)
                {
                    Debug.LogError(typeof(T) + "is nothing");
                }
            }

            return m_Instance;
        }
    }
}
