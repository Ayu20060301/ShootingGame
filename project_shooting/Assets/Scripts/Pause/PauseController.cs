using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEditor;

public class PauseController : MonoBehaviour
{
    [SerializeField]
    private GameObject m_PauseMenu;

    [SerializeField]
    private TMP_Text m_SelectText;

    private PlayerInputActions m_Input;

    private int m_SelectIndex = 0; 

    private bool m_IsCanMove = true;

    private bool m_IsPause = false;

    private void Awake()
    {
        m_Input = new PlayerInputActions();

        UpdateText();
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



    private void OnNavigate(InputAction.CallbackContext context)
    {

        if (!m_IsPause) return;

        float y = context.ReadValue<Vector2>().y;

        if(Mathf.Abs(y) < 0.5f)
        {
            m_IsCanMove = true;
            return;
        }

        if (!m_IsCanMove) return;

        int previewIndex = m_SelectIndex;

        if (y > 0) m_SelectIndex--;
        else m_SelectIndex++;

        m_SelectIndex = Mathf.Clamp(m_SelectIndex, 0, 2);

        if (previewIndex != m_SelectIndex)
        {
            SEManager.Instance.SEPlay(SEType.SELECT);
        }

        UpdateText();

        m_IsCanMove = false;
    }


    private void OnSubmit(InputAction.CallbackContext context)
    {

        if (!m_IsPause) return;

        SEManager.Instance.SEPlay(SEType.DECIDE);

        switch(m_SelectIndex)
        {
            case 0:
                ResumeGame();
                break;
            case 1:
                ResumeGame();
                GameManager.Instance.ResetGame();
                SceneController.Instance.LoadScene("MainScene");
                break;
            case 2:
                ResumeGame();
                GameManager.Instance.ResetGame();
                SceneController.Instance.LoadScene("TitleScene");
                break;

        }
    }

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


    private void GamePause()
    {
        //非アクティブの場合は呼び出さない
        if (!GameManager.Instance.isActive) return;

        m_IsPause = true;

        m_SelectIndex = 0;
        UpdateText();

        Time.timeScale = 0.0f;
        BGMManager.Instance.bgmAudio.volume = 0.3f;
        m_PauseMenu.SetActive(true);
    }

    private void ResumeGame()
    {
        m_IsPause = false;
        m_IsCanMove = true;

        Time.timeScale = 1.0f;
        BGMManager.Instance.bgmAudio.volume = 1.0f;
        m_PauseMenu.SetActive(false);
    }


    private void UpdateText()
    {
        m_SelectText.text =
            (m_SelectIndex == 0 ? "> " : " ") + "再開\n" +
            (m_SelectIndex == 1 ? "> " : " ") + "リトライ\n" +
            (m_SelectIndex == 2 ? "> " : " ") + "タイトルへ戻る";
    }

}
