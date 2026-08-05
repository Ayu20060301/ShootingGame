using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

//リザルト画面の制御
public class ResultController : MonoBehaviour
{
    [Header("定数")]
    private const int RETRY = 0;
    private const int TITLE = 1;

    [SerializeField]
    private TMP_Text m_MenuText;

    private PlayerInputActions m_Input;

    private int m_SelectIndex = RETRY; //現在選択している項目

    /// <summary>
    /// 初期化
    /// </summary>
    private void Awake()
    {
        m_Input = new PlayerInputActions();

        //リザルトBGMを再生
        BGMManager.Instance.BGMPlay(BGMType.RESULT);

        RefreshMenu();
    }

    private void OnEnable()
    {
        m_Input.UI.Enable();
        m_Input.UI.Navigate.performed += OnNavigate;
        m_Input.UI.Submit.performed += OnSubmit;
    }

    private void OnDisable()
    {
        m_Input.UI.Navigate.performed -= OnNavigate;
        m_Input.UI.Submit.performed -= OnSubmit;
        m_Input.UI.Disable();
    }


    /// <summary>
    /// メニュー表示を更新
    /// </summary>
    private void RefreshMenu()
    {
        m_MenuText.text =
         (m_SelectIndex == 0 ? "> " : " ") + "リトライ\n" +
         (m_SelectIndex == 1 ? "> " : " ") + "タイトルへ戻る";
    }
    
    /// <summary>
    /// カーソル移動
    /// </summary>
    /// <param name="context"></param>
    private void OnNavigate(InputAction.CallbackContext context)
    {
        float y = context.ReadValue<Vector2>().y;

        int previousIndex = m_SelectIndex;

        if (y > 0.5f)
        {
            m_SelectIndex = RETRY;
        }
        else if (y < -0.5f)
        {
            m_SelectIndex = TITLE;
        }

        //カーソルが変わった時だけ再生
        if(previousIndex != m_SelectIndex)
        {
            SEManager.Instance.SEPlay(SEType.SELECT);
            RefreshMenu();
        }
    }

    /// <summary>
    /// 決定
    /// </summary>
    /// <param name="context"></param>
    private void OnSubmit(InputAction.CallbackContext context)
    {
        //シーン遷移中なら無視
        if (SceneController.Instance.IsFading) return;

        //決定音を鳴らす
        SEManager.Instance.SEPlay(SEType.DECIDE);

        GameManager.Instance.ResetGame();

       switch(m_SelectIndex)
        {
            case RETRY:
                RetryGame();
                break;
            case TITLE:
                ReturnTitle();
                break;
        }
    }

    /// <summary>
    /// リトライ
    /// </summary>
    private void RetryGame()
    {
        SceneController.Instance.LoadScene("MainScene");
    }

    /// <summary>
    /// タイトルへ戻る
    /// </summary>
    private void ReturnTitle()
    {
        SceneController.Instance.LoadScene("TitleScene");
    }

}
