using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEditor;

//ポーズメニューの制御
public class PauseController : MonoBehaviour
{
    [Header("定数")]
    private const int RESUME = 0;
    private const int RETRY = 1;
    private const int TITLE = 2;

    [SerializeField]
    private GameObject m_PauseMenu;
    [SerializeField]
    private TMP_Text m_SelectText;

    private PlayerInputActions m_Input;

    private int m_SelectIndex = RESUME; //現在選択している項目 

    private bool m_IsCanMove = true; //キーの連続入力防止

    private bool m_IsPause = false; //ポーズ中かどうか

    /// <summary>
    /// 初期化
    /// </summary>
    private void Awake()
    {
        m_Input = new PlayerInputActions();

        //初期表示
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
        m_Input.UI.Disable();
    }


    /// <summary>
    /// カーソル移動
    /// </summary>
    /// <param name="context"></param>
    private void OnNavigate(InputAction.CallbackContext context)
    {

        if (!m_IsPause) return;

        float y = context.ReadValue<Vector2>().y;

        //入力が戻ったら次の入力を受け付ける
        if(Mathf.Abs(y) < 0.5f)
        {
            m_IsCanMove = true;
            return;
        }

        //連続入力防止
        if (!m_IsCanMove) return;

        int previewIndex = m_SelectIndex;

        if (y > 0)
        {
            m_SelectIndex--;
        }
        else
        {
            m_SelectIndex++;
        }

        m_SelectIndex = Mathf.Clamp(m_SelectIndex, 0, 2);

        if (previewIndex != m_SelectIndex)
        {
            SEManager.Instance.SEPlay(SEType.SELECT);
            RefreshMenu();
        }

        m_IsCanMove = false;
    }

    /// <summary>
    /// 決定
    /// </summary>
    /// <param name="context"></param>
    private void OnSubmit(InputAction.CallbackContext context)
    {

        if (!m_IsPause) return;

        //シーン遷移中なら無視
        if (SceneController.Instance.IsFading) return;

        //決定音を鳴らす
        SEManager.Instance.SEPlay(SEType.DECIDE);

        switch (m_SelectIndex)
        {
            case 0:
                ResumeGame();
                break;
            case 1:
                RetryGame();
                break;
            case 2:
                ReturnTitle();
                break;
        }
    }

    /// <summary>
    /// ポーズキー
    /// </summary>
    /// <param name="context"></param>
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

        m_IsPause = true;

        m_SelectIndex = RESUME;

        Time.timeScale = 0.0f;

        BGMManager.Instance.bgmAudio.volume = 0.3f;

        m_PauseMenu.SetActive(true);

        //連打防止
        m_Input.UI.Pause.Disable();
    }

    /// <summary>
    /// ポーズ解除
    /// </summary>
    private void ResumeGame()
    {
        m_IsPause = false;
        m_IsCanMove = true;

        Time.timeScale = 1.0f;

        BGMManager.Instance.bgmAudio.volume = 1.0f;

        m_PauseMenu.SetActive(false);

        //ESCキー・startボタンを有効
        m_Input.UI.Pause.Enable();
    }

    /// <summary>
    /// リトライ
    /// </summary>
    private void RetryGame()
    {
        ResumeGame();

        GameManager.Instance.ResetGame();

        SceneController.Instance.LoadScene("MainScene");
    }

    /// <summary>
    /// タイトルへ戻る
    /// </summary>
    private void ReturnTitle()
    {
        ResumeGame();

        GameManager.Instance.ResetGame();

        SceneController.Instance.LoadScene("TitleScene");
    }

    /// <summary>
    /// メニュー表示更新
    /// </summary>
    private void RefreshMenu()
    {
        m_SelectText.text =
            (m_SelectIndex == RESUME ? "> " : " ") + "再開\n" +
            (m_SelectIndex == RETRY ? "> " : " ") + "リトライ\n" +
            (m_SelectIndex == TITLE ? "> " : " ") + "タイトルへ戻る\n";
    }

    /// <summary>
    /// ゲーム起動中にほかのサイトや別のアプリなどに切り替えたら自動でポーズする
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
