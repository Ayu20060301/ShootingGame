using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

//リザルト画面の制御
public class ResultController : MonoBehaviour
{
    /// <summary>
    /// リザルトメニューの選択項目
    /// </summary>
    private enum MenuSelection
    {
        RETRY,
        TITLE
    }

    [Header("メニュー")]
    [SerializeField]
    private TMP_Text m_MenuText;

    //入力
    private PlayerInputActions m_Input; 

    //現在選択している項目
    private MenuSelection m_SelectIndex = MenuSelection.RETRY;

    /// <summary>
    /// 初期化
    /// </summary>
    private void Awake()
    {
        //InputSystemを生成
        m_Input = new PlayerInputActions();

        //リザルトBGMを再生
        BGMManager.Instance.BGMPlay(BGMType.RESULT);

        //メニューを初期表示
        RefreshMenu();
    }

    private void OnEnable()
    {
        //UI入力を有効化
        m_Input.UI.Enable();

        //入力イベントを登録
        m_Input.UI.Navigate.performed += OnNavigate;
        m_Input.UI.Submit.performed += OnSubmit;
    }

    private void OnDisable()
    {
        //入力イベントを解除
        m_Input.UI.Navigate.performed -= OnNavigate;
        m_Input.UI.Submit.performed -= OnSubmit;

        //UI入力を無効化
        m_Input.UI.Disable();
    }


    /// <summary>
    /// メニュー表示を更新
    /// </summary>
    private void RefreshMenu()
    {

        string retryText =
            m_SelectIndex == MenuSelection.RETRY ? "> " : " ";

        string titleText =
            m_SelectIndex == MenuSelection.TITLE ? "> " : " ";

        m_MenuText.text =
         $"{retryText}リトライ\n" +
         $"{titleText}タイトルへ戻る";
    }
    
    /// <summary>
    /// カーソル移動
    /// </summary>
    /// <param name="context"></param>
    private void OnNavigate(InputAction.CallbackContext context)
    {
        float y = context.ReadValue<Vector2>().y;

        //移動前の選択項目を保存
        MenuSelection previousIndex = m_SelectIndex;

        //上入力
        if (y > 0.5f)
        {
            m_SelectIndex = MenuSelection.RETRY;
        }
        //下入力
        else if (y < -0.5f)
        {
            m_SelectIndex = MenuSelection.TITLE;
        }

        //選択項目が変わった場合のみ更新
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

        //決定音を再生
        SEManager.Instance.SEPlay(SEType.DECIDE);

        //ゲーム状態をリセット
        GameManager.Instance.ResetGame();

       //選択項目に応じて処理
       switch(m_SelectIndex)
       {
            case MenuSelection.RETRY:
                RetryGame();
                break;
            case MenuSelection.TITLE:
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
