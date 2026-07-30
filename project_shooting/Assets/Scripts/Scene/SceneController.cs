using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneController : SingletonMonoBehaviour<SceneController>
{


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// シーン読み込み完了時
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void OnSceneLoaded(Scene scene,LoadSceneMode mode)
    {
        ChangeBGM(scene.name);  
    }


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

    /// <summary>
    /// シーンごとにBGMを変える
    /// </summary>
    /// <param name="sceneName"></param>
    private void ChangeBGM(string sceneName)
    {
        //シーンごとにBGMを切り替える
        switch (sceneName)
        {
            case "TitleScene":
                BGMManager.Instance.BGMPlay(BGMType.TITLE);
                break;
            case "MainScene":
                BGMManager.Instance.BGMPlay(BGMType.GAME);
                break;
        }
    }

}
