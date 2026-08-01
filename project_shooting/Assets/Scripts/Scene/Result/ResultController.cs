using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class ResultController : MonoBehaviour
{

    [SerializeField]
    private TMP_Text m_MenuText;

    private PlayerInputActions m_Input;

    private int m_SelectIndex = 0; //選択中のインデックス

    private void Awake()
    {
        m_Input = new PlayerInputActions();
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


  
    private void RefreshMenu()
    {
        m_MenuText.text =
         (m_SelectIndex == 0 ? "> " : " ") + "リトライ\n" +
         (m_SelectIndex == 1 ? "> " : " ") + "タイトルへ戻る";
    }
    
    private void OnNavigate(InputAction.CallbackContext context)
    {
        float y = context.ReadValue<Vector2>().y;

        int previousIndex = m_SelectIndex;

        if (y > 0.5f)
        {
            m_SelectIndex = 0;
        }
        else if (y < -0.5f)
        {
            m_SelectIndex = 1;
        }

        //カーソルが変わった時だけ再生
        if(previousIndex != m_SelectIndex)
        {
            SEManager.Instance.SEPlay(SEType.SELECT);
        }

        RefreshMenu();
    }

    private void OnSubmit(InputAction.CallbackContext context)
    {

        SEManager.Instance.SEPlay(SEType.DECIDE);
        GameManager.Instance.ResetGame();

        if (m_SelectIndex == 0)
        {
            
            SceneController.Instance.LoadScene("MainScene");
        }
        else
        {
            SceneController.Instance.LoadScene("TitleScene");
        }
    }

}
