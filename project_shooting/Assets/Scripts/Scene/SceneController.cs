using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : SingletonMonoBehaviour<SceneController>
{
    /// <summary>
    /// シーン遷移
    /// </summary>
    /// <param name="sceneName">シーン名</param>
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
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
