using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEditor;

//ポーズメニューの制御
public class PauseController : MonoBehaviour
{
    /// <summary>
    /// ポーズメニューの選択項目
    /// </summary>
    private enum MenuSelection
    {
        RESUME, //再開
        RETRY,  //リトライ
        TITLE   //タイトル
    }

    //---------------
    //定数
    //---------------

    //ポーズ中のBGM音量
    private const float PAUSE_BGM_VOLUME = 0.3f;

    //通常時のBGM音量
    private const float NORMAL_BGM_VOLUME = 1.0f;

    // スティック・十字キーの入力判定値
    private const float NAVIGATION_THRESHOLD = 0.5f;

    [Header("ポーズメニュー")]
    [SerializeField]
    private GameObject m_PauseMenu;
    [SerializeField]
    private TMP_Text m_SelectText;

    //入力
    private PlayerInputActions m_Input;

    //現在選択している項目
    private MenuSelection m_SelectIndex = MenuSelection.RESUME;

    //キーの連続入力防止
    private bool m_IsCanMove = true; 

    //ポーズ中かどうか
    private bool m_IsPause = false; 

    /// <summary>
    /// 初期化
    /// </summary>
    private void Awake()
    {
        //InputSystemを生成
        m_Input = new PlayerInputActions();

        //ポーズメニューを非表示
        m_PauseMenu.SetActive(false);

        //初期メニューを表示
        RefreshMenu();
    }


    private void OnEnable()
    {
        m_Input.UI.Enable();
        m_Input.UI.Navigate.performed += OnNavigate;
        m_Input.UI.Submit.performed += OnSubmit;
        m_Input.UI.Pause.performed += OnPause;
    }

    private void OnDisable()
    {
        m_Input.UI.Navigate.performed -= OnNavigate;
        m_Input.UI.Submit.performed -= OnSubmit;
        m_Input.UI.Pause.performed -= OnPause;

        // UI入力を無効化
        m_Input.UI.Disable();

        // Player入力も無効化
        m_Input.Player.Disable();
    }


    /// <summary>
    /// カーソル移動
    /// </summary>
    /// <param name="context">入力情報</param>
    private void OnNavigate(InputAction.CallbackContext context)
    {
        //ポーズ中でなければ操作しない
        if (!m_IsPause) return;

        float y = context.ReadValue<Vector2>().y;

        //入力が戻ったら次の入力を受け付ける
        if(Mathf.Abs(y) < NAVIGATION_THRESHOLD)
        {
            m_IsCanMove = true;
            return;
        }

        //連続入力防止
        if (!m_IsCanMove) return;

        MenuSelection previewIndex = m_SelectIndex;

        if (y > 0)
        {
            MoveSelection(-1);
        }
        else
        {
            MoveSelection(1);
        }

        //選択項目が変わった場合
        if (previewIndex != m_SelectIndex)
        {
            SEManager.Instance.SEPlay(SEType.SELECT);
            RefreshMenu();
        }

        //次の入力まで移動を受け付けない
        m_IsCanMove = false;
    }

    /// <summary>
    /// 決定キー
    /// </summary>
    /// <param name="context">入力情報</param>
    private void OnSubmit(InputAction.CallbackContext context)
    {
        //ポーズ中でなければ操作しない
        if (!m_IsPause) return;

        //シーン遷移中なら無視
        if (SceneController.Instance.IsFading) return;

        //決定音を鳴らす
        SEManager.Instance.SEPlay(SEType.DECIDE);

        switch (m_SelectIndex)
        {
            case MenuSelection.RESUME:
                ResumeGame();
                break;
            case MenuSelection.RETRY:
                RetryGame();
                break;
            case MenuSelection.TITLE:
                ReturnTitle();
                break;
        }
    }

    /// <summary>
    /// 選択項目を移動する
    /// </summary>
    /// <param name="direction">移動方向</param>
    private void MoveSelection(int direction)
    {
        int index = (int)m_SelectIndex;

        index += direction;

        //選択範囲内に制限
        index = Mathf.Clamp(
            index,
            (int)MenuSelection.RESUME,
            (int)MenuSelection.TITLE
            );

        m_SelectIndex = (MenuSelection)index;
    }

    /// <summary>
    /// ポーズキー
    /// </summary>
    /// <param name="context">入力情報</param>
    private void OnPause(InputAction.CallbackContext context)
    { 
        if(m_IsPause)
        {
            ResumeGame();
        }
        else
        {
            GamePause();
        }
    }


    /// <summary>
    /// ポーズ開始
    /// </summary>
    private void GamePause()
    {
        //非アクティブの場合は呼び出さない
        if (!GameManager.Instance.isActive) return;

        //フェード中は処理を行わない
        if (SceneController.Instance.IsFading) return;

        //ポーズ状態にする
        m_IsPause = true;

        //初期選択を「再開」に戻す
        m_SelectIndex = MenuSelection.RESUME;

        //停止
        Time.timeScale = 0.0f;

        //ポーズ中はBGM音量を下げる
        SetBGMVolume(PAUSE_BGM_VOLUME);

        //ポーズメニューを表示
        m_PauseMenu.SetActive(true);

        // Player入力を無効化
        m_Input.Player.Disable();

        // UI入力は有効にする
        m_Input.UI.Enable();

        //連打防止
        m_Input.UI.Pause.Disable();
    }

    /// <summary>
    /// ポーズ解除
    /// </summary>
    private void ResumeGame()
    {
        //ポーズ状態を解除
        m_IsPause = false;

        //カーソル移動を再び許可
        m_IsCanMove = true;

        //通常へ戻す
        Time.timeScale = 1.0f;

        //BGM音量を戻す
        SetBGMVolume(NORMAL_BGM_VOLUME);

        //ポーズメニューを非表示
        m_PauseMenu.SetActive(false);

        // Player入力を有効化
        m_Input.Player.Enable();

        // ポーズキーを有効化
        m_Input.UI.Pause.Enable();
    }

    /// <summary>
    /// BGM音量を設定する
    /// </summary>
    /// <param name="volume">音量</param>
    private void SetBGMVolume(float volume)
    {
        BGMManager.Instance.bgmAudio.volume = volume;
    }

    /// <summary>
    /// リトライ
    /// </summary>
    private void RetryGame()
    {
        ResumeGame();

        //状態をリセット
        GameManager.Instance.ResetGame();

        //メインシーンへ移動
        SceneController.Instance.LoadScene("MainScene");
    }

    /// <summary>
    /// タイトルへ戻る
    /// </summary>
    private void ReturnTitle()
    {
        ResumeGame();

        //状態をリセット
        GameManager.Instance.ResetGame();

        //タイトルシーンへ移動
        SceneController.Instance.LoadScene("TitleScene");
    }

    /// <summary>
    /// メニュー表示更新
    /// </summary>
    private void RefreshMenu()
    {
        string resumeText = m_SelectIndex == MenuSelection.RESUME ? "> " : " ";

        string retryText = m_SelectIndex == MenuSelection.RETRY ? "> " : " ";

        string titleText = m_SelectIndex == MenuSelection.TITLE ? "> " : " ";

        m_SelectText.text = $"{resumeText}再開\n" + $"{retryText}リトライ\n" + $"{titleText}タイトルへ戻る";
    }

    /// <summary>
    /// ゲーム起動中にほかのサイトや別のアプリなどに切り替えたら自動でポーズ画面を表示する
    /// </summary>
    /// <param name="pause"></param>
    private void OnApplicationPause(bool pause)
    {
        if(pause && !m_IsPause)
        {
            GamePause();
        }
    }
}
