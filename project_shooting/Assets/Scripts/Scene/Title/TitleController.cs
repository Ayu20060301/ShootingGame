using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using System.Collections;

//タイトル画面の制御
public class TitleController : MonoBehaviour
{
    /// <summary>
    /// 終了メニューの選択項目
    /// </summary>
    private enum MenuSelection
    {
        YES,   //はい
        NO     //いいえ
    }

    //ボタンを押した際の演出の時間
    private const float BUTTON_PRESSED_TIME = 0.1f;

    //入力
    private PlayerInputActions m_Input; 

    [Header("終了確認メニュー")]
    [SerializeField]
    private GameObject m_QuitMenu;
    [SerializeField]
    private TMP_Text m_QuitMenuText;

    [Header("スタートボタン")]
    [SerializeField]
    private Button m_StartButton;

    //現在選択している項目
    private MenuSelection m_SelectIndex = MenuSelection.YES;

    //終了メニューが開いているか
    private bool m_IsMenuOpen = false; 

    private void Awake()
    {
        //InputSystemを生成
        m_Input = new PlayerInputActions();

        //終了確認メニューを非表示
        m_QuitMenu.SetActive(false);

        //初期表示をUIへ反映
        RefreshQuitMenu();
    }


    private void OnEnable()
    {
        //UI入力を有効化
        m_Input.UI.Enable();

        //入力イベントを登録
        m_Input.UI.Navigate.performed += OnNavigate;
        m_Input.UI.Submit.performed += OnSubmit;
        m_Input.UI.Cancel.performed += OnCancel;
    }

    private void OnDisable()
    {
        //入力イベントを解除
        m_Input.UI.Navigate.performed -= OnNavigate;
        m_Input.UI.Submit.performed -= OnSubmit;
        m_Input.UI.Cancel.performed -= OnCancel;

        //UI入力を無効化
        m_Input.UI.Disable();
    }

    /// <summary>
    /// 終了メニューの表示を更新する
    /// </summary>
    private void RefreshQuitMenu()
    {
        string yesText = m_SelectIndex == MenuSelection.YES ? "> " : " ";
        string noText = m_SelectIndex == MenuSelection.NO ? "> " : " ";

        m_QuitMenuText.text = $"{yesText}はい\n" + $"{noText}いいえ\n";
    }

    /// <summary>
    /// 終了メニューを開く
    /// </summary>
    private void OpenQuitMenu()
    {
        m_IsMenuOpen = true;
        m_SelectIndex = MenuSelection.YES;

        m_QuitMenu.SetActive(true);

        //初期選択を「はい」に戻す
        RefreshQuitMenu();

        //メニュー表示中はcancelの二重処理を防ぐ
        m_Input.UI.Cancel.Disable();
    }

    /// <summary>
    /// 終了メニューを閉じる
    /// </summary>
    private void CloseQuitMenu()
    {
        m_IsMenuOpen = false;
        m_QuitMenu.SetActive(false);

        //Cancel入力を再び有効化
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

        //移動前の選択位置を保存
        MenuSelection previewIndex = m_SelectIndex;

        //上入力
        if(y > 0.5f)
        {
            m_SelectIndex = MenuSelection.YES;
        }
        //下入力
        else if(y < -0.5f)
        {
            m_SelectIndex = MenuSelection.NO;
        }

        //選択項目が変わった時だけ更新
        if(previewIndex != m_SelectIndex)
        {
            SEManager.Instance.SEPlay(SEType.SELECT);

            RefreshQuitMenu();
        }
    }

    /// <summary>
    /// 決定入力
    /// </summary>
    /// <param name="context"></param>
    private void OnSubmit(InputAction.CallbackContext context)
    {
        //シーン遷移中なら無視
        if (SceneController.Instance.IsFading) return;

        //決定SEを再生
        SEManager.Instance.SEPlay(SEType.DECIDE);

        //終了メニューが開いていない場合はゲーム開始
        if (!m_IsMenuOpen)
        {
            StartGame();
            return;
        }

        //終了確認メニューを決定
        ExecuteQuitMenu();
    }

    /// <summary>
    /// メニュー入力
    /// </summary>
    /// <param name="context"></param>
    private void OnCancel(InputAction.CallbackContext context)
    {
        // シーン遷移中なら無視
        if (SceneController.Instance.IsFading) return;

        //メニューが開いてなければ開く
        if (!m_IsMenuOpen)
        {
            OpenQuitMenu();
           
        }
        //メニューが開いていれば閉じる
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
        //スタートボタンのPressed演出
        StartCoroutine(PressedButton());

        //メインシーンへ移動
        SceneController.Instance.LoadScene("MainScene");
    }

    /// <summary>
    /// スタートボタンを押したときの演出
    /// </summary>
    /// <returns></returns>
    private IEnumerator PressedButton()
    {
        //押し込み時の色に変更
        m_StartButton.image.color = m_StartButton.colors.pressedColor;

        //一定時間待機
        yield return new WaitForSecondsRealtime(BUTTON_PRESSED_TIME);

        //通常時の色に戻す
        m_StartButton.image.color = m_StartButton.colors.normalColor;
    }

    /// <summary>
    /// 終了メニューの決定処理
    /// </summary>
    private void ExecuteQuitMenu()
    {
        //「はい」が選択されている場合
        if(m_SelectIndex == MenuSelection.YES)
        {
            QuitGame();
        }
        //「いいえ」が選択されている場合
        else
        {
            CloseQuitMenu();
        }
    }

    /// <summary>
    /// ゲームを終了する
    /// </summary>
    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();  
#endif
    }

    /// <summary>
    /// ゲーム起動中にほかのサイトや別のアプリなどに切り替えたら自動で終了メニューを表示する
    /// </summary>
    /// <param name="pause"></param>
    private void OnApplicationPause(bool pause)
    {
        if(pause && !m_IsMenuOpen)
        {
            OpenQuitMenu();
        }
    }
}
