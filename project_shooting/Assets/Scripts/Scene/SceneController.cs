using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


//シーン遷移を管理するクラス
public class SceneController : SingletonMonoBehaviour<SceneController>
{

    [SerializeField]
    private CanvasGroup m_FadeCanvas;

    [SerializeField]
    private float m_FadeTime = 0.5f;

    private bool m_IsFading = false; 

    public bool IsFading => m_IsFading;

    protected override void Awake()
    {
        base.Awake();

        //フレームレートを60FPSに設定
        Application.targetFrameRate = 60; 

        if(m_FadeCanvas != null)
        {
            m_FadeCanvas.alpha = 0.0f;
        }
    }


    private IEnumerator Start()
    {
        //起動時はフェードイン
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
    /// <param name="scene">シーン名</param>
    /// <param name="mode"></param>
    private void OnSceneLoaded(Scene scene,LoadSceneMode mode)
    {
        ChangeBGM(scene.name);  
    }


    /// <summary>
    /// シーン遷移
    /// </summary>
    /// <param name="sceneName">遷移先シーン</param>
    public void LoadScene(string sceneName)
    {
        
        //既に遷移中なら無視
        if (m_IsFading) return;

        //遷移開始
        m_IsFading = true;


        StartCoroutine(LoadSceneAsync(sceneName));
    }

    /// <summary>
    /// シーン遷移を非同期で読み込む
    /// </summary>
    /// <param name="sceneName">遷移先シーン</param>
    /// <returns></returns>
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        //フェードアウト
        yield return Fade(0.0f, 1.0f);

        //シーン読み込み
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

        //遷移完了
        m_IsFading = false;
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


    /// <summary>
    /// フェード処理
    /// </summary>
    /// <param name="start">開始アルファ値</param>
    /// <param name="end">最終的なアルファ値</param>
    /// <returns></returns>
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
