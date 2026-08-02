using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneController : SingletonMonoBehaviour<SceneController>
{

    [SerializeField]
    private CanvasGroup m_FadeCanvas;

    [SerializeField]
    private float m_FadeTime = 0.5f;

    protected override void Awake()
    {
        base.Awake();

        Application.targetFrameRate = 60; //フレームレートの設定

        if(m_FadeCanvas != null)
        {
            m_FadeCanvas.alpha = 0.0f;
        }
    }


    private IEnumerator Start()
    {
        //起動時にフェードイン
        yield return Fade(1.0f, 0.0f);
    }

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

        //フェードアウト
        yield return Fade(0.0f, 1.0f);

        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;

        //読み込み完了まで待機
        while(async.progress < 0.9f)
        {
            yield return null;
        }

        //シーン切り替え
        async.allowSceneActivation = true;

        while(!async.isDone)
        {
            yield return null;
        }

        //フェードイン
        yield return Fade(1.0f, 0.0f);
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

    private IEnumerator Fade(float start, float end)
    {
        if (m_FadeCanvas == null) yield break;

        float time = 0.0f;
        m_FadeCanvas.alpha = start;

        while(time < m_FadeTime)
        {
            time += Time.deltaTime;
            m_FadeCanvas.alpha = Mathf.Lerp(start, end, time / m_FadeTime);
            yield return null;
        }

        m_FadeCanvas.alpha = end;
    }
}
