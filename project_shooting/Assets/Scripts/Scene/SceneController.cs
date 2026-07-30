using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{

    public static SceneController Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    /// <summary>
    /// シーン遷移
    /// </summary>
    /// <param name="sceneName">シーン名</param>
    public static void LoadScene(string sceneName)
    {
        Instance.StartCoroutine(Instance.LoadSceneAsync(sceneName));
    }

    /// <summary>
    /// シーン遷移を非同期で読み込む
    /// </summary>
    /// <param name="sceneName">シーン名</param>
    /// <returns></returns>
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);

        //読み込み完了まで待機
        while(!async.isDone)
        {
            yield return null;
        }
    }
}
