using UnityEngine;

/// <summary>
/// シーンをまたいで使用するSingletonの基底クラス
/// </summary>
/// <typeparam name="T">Singletonとして管理するクラス</typeparam>
public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T m_Instance;

    /// <summary>
    /// Singletonインスタンスを取得する
    /// </summary>
    public static T Instance
    {
        get
        {
            //まだインスタンスが存在しない場合
            if (m_Instance == null)
            {
                //シーン上から対象のコンポーネントを検索
                m_Instance = (T)FindFirstObjectByType(typeof(T));

                //シーン上にも存在しない場合
                if (m_Instance == null)
                {
                    Debug.LogError($"{typeof(T).Name}がシーン上に存在しません。");
                }
            }

            return m_Instance;
        }
    }

    /// <summary>
    /// Singletonの初期化
    /// </summary>
    protected virtual void Awake()
    {
        //まだSingletonが存在しない場合
        if(m_Instance == null)
        {
            //自分自身をSingletonとして登録
            m_Instance = this as T;

            //シーンをまたいでも破棄されないようにする
            DontDestroyOnLoad(this.gameObject);
        }
        //すでに別のSingletonが存在する場合
        else if(m_Instance != this)
        {
            //重複したGameObjectを削除
            Destroy(this.gameObject);
        }
    }
}
