using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


//シーン遷移を管理するクラス
public class SceneController : SingletonMonoBehaviour<SceneController>
{
    [Header("フェード用のCanvasGroup")]
    [SerializeField]
    private CanvasGroup m_FadeCanvas;

    [Header("フェード時間")]
    [SerializeField]
    private float m_FadeTime = 0.5f;

    //フェード・シーン中か
    private bool m_IsFading = false;

    /// <summary>
    /// 現在シーン遷移中か
    /// </summary>
    public bool IsFading => m_IsFading;


    protected override void Awake()
    {
        base.Awake();

        //フレームレートを60FPSに設定
        Application.targetFrameRate = 60;

        //フェード画面を透明にする
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

    /// <summary>
    /// シーン読み込み完了イベントを登録
    /// </summary>
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /// <summary>
    /// シーン読み込み完了イベントを解除
    /// </summary>
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// シーン読み込み完了時の処理
    /// </summary>
    /// <param name="scene">シーン名</param>
    /// <param name="mode"></param>
    private void OnSceneLoaded(Scene scene,LoadSceneMode mode)
    {
        ChangeBGM(scene.name);  
    }


    /// <summary>
    /// 指定したシーンへ遷移する
    /// </summary>
    /// <param name="sceneName">遷移先シーン名</param>
    public void LoadScene(string sceneName)
    {
        //既に遷移中なら無視
        if (m_IsFading) return;

        StartCoroutine(LoadSceneAsync(sceneName));
    }

    /// <summary>
    /// シーン遷移を非同期で読み込む
    /// </summary>
    /// <param name="sceneName">遷移先シーン</param>
    /// <returns></returns>
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        //遷移開始
        m_IsFading = true;

        //フェードアウト
        yield return Fade(0.0f, 1.0f);

        //シーン読み込み開始
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);

        //シーンの自動切り替えを停止
        async.allowSceneActivation = false;

        //読み込み完了まで待機
        while(async.progress < 0.9f)
        {
            yield return null;
        }

        //シーン切り替え
        async.allowSceneActivation = true;

        //シーン切り替え完了まで待機
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
    /// <param name="sceneName">シーン名</param>
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
    /// <param name="start">開始時の透明度</param>
    /// <param name="end">終了時の透明度</param>
    /// <returns></returns>
    private IEnumerator Fade(float start, float end)
    {
        //CanvasGroupが設定されていない場合
        if (m_FadeCanvas == null) yield break;

        //フェード時間が0の場合
        if(m_FadeTime <= 0.0f)
        {
            m_FadeCanvas.alpha = end;

            yield break;
        }

        float elapsedTime = 0.0f;

        //開始時の透明度を設定
        m_FadeCanvas.alpha = start;

        //フェード処理
        while(elapsedTime < m_FadeTime)
        {
            elapsedTime += Time.unscaledDeltaTime;

            float progress =　elapsedTime / m_FadeTime;

            m_FadeCanvas.alpha = Mathf.Lerp(start,end,progress);

            yield return null;
        }

        //最終値を設定
        m_FadeCanvas.alpha = end;
    }
}
