using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine.Rendering;

//タイトル画面の制御
public class TitleController : MonoBehaviour
{
    [Header("定数")]
    private const int YES = 0;
    private const int NO = 1;

    private PlayerInputActions m_Input;
    [SerializeField]
    private GameObject m_QuitMenu;
    [SerializeField]
    private TMP_Text m_QuitMenuText;

    private int m_SelectIndex = YES; //現在選択している項目

    private bool m_IsMenuOpen = false;  //終了メニューが開いているか 


    private void Awake()
    {
        m_Input = new PlayerInputActions();

        m_QuitMenu.SetActive(false);

        //初期表示
        RefreshQuitMenu();
    }


    private void OnEnable()
    {
        m_Input.UI.Enable();
        m_Input.UI.Navigate.performed += OnNavigate;
        m_Input.UI.Submit.performed += OnSubmit;
        m_Input.UI.Cancel.performed += OnCancel;
    }

    private void OnDisable()
    {
        m_Input.UI.Navigate.performed -= OnNavigate;
        m_Input.UI.Submit.performed -= OnSubmit;
        m_Input.UI.Cancel.performed -= OnCancel;
        m_Input.UI.Disable();
    }

    /// <summary>
    /// 終了メニューの表示を更新する
    /// </summary>
    private void RefreshQuitMenu()
    {
        m_QuitMenuText.text =
            (m_SelectIndex == YES ? "> " : " ") + "はい\n" +
            (m_SelectIndex == NO ? "> " : " ") + "いいえ\n";
    }

    /// <summary>
    /// 終了メニューを開く
    /// </summary>
    private void OpenQuitMenu()
    {
        m_IsMenuOpen = true;
        m_SelectIndex = YES;

        m_QuitMenu.SetActive(true);

        RefreshQuitMenu();

        m_Input.UI.Cancel.Disable();
    }

    /// <summary>
    /// 終了メニューを閉じる
    /// </summary>
    private void CloseQuitMenu()
    {
        m_IsMenuOpen = false;
        m_QuitMenu.SetActive(false);

        m_Input.UI.Cancel.Enable();
    }

    /// <summary>
    /// カーソル移動
    /// </summary>
    /// <param name="context"></param>
    private void OnNavigate(InputAction.CallbackContext context)
    {

        //終了メニューが開いていないときは操作しない
        if (!m_IsMenuOpen) return;

        float y = context.ReadValue<Vector2>().y;

        int previewIndex = m_SelectIndex;

        if(y > 0.5f)
        {
            m_SelectIndex = 0;
        }
        else if(y < -0.5f)
        {
            m_SelectIndex = 1;
        }

        //選択項目が変わった時だけ更新
        if(previewIndex != m_SelectIndex)
        {
            SEManager.Instance.SEPlay(SEType.SELECT);

            RefreshQuitMenu();
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

        SEManager.Instance.SEPlay(SEType.DECIDE);

        //終了メニューが開いていない場合はゲーム開始
        if (!m_IsMenuOpen)
        {
            StartGame();
            return;
        }

        ExecuteQuitMenu();
    }

    /// <summary>
    /// キャンセル
    /// </summary>
    /// <param name="context"></param>
    private void OnCancel(InputAction.CallbackContext context)
    {
        // シーン遷移中なら無視
        if (SceneController.Instance.IsFading) return;

        if (!m_IsMenuOpen)
        {
            OpenQuitMenu();
           
        }
        else
        {
            CloseQuitMenu();
        }
    }
    
    /// <summary>
    /// ゲーム開始
    /// </summary>
    private void StartGame()
    {
        SceneController.Instance.LoadScene("MainScene");
    }

    /// <summary>
    /// 終了メニューの決定処理
    /// </summary>
    private void ExecuteQuitMenu()
    {
        if(m_SelectIndex == YES)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        else
        {
            CloseQuitMenu();
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if(pause && !m_IsMenuOpen)
        {
            OpenQuitMenu();
        }
    }
}
