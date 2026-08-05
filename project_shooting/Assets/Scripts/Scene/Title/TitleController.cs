using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TitleController : MonoBehaviour
{

    private PlayerInputActions m_Input;

    [SerializeField]
    private Button m_Button;

   

    private void Awake()
    {
        m_Input = new PlayerInputActions();
    }


    private void OnEnable()
    {
        m_Input.UI.Enable();
        m_Input.UI.Submit.performed += OnSubmit;
    }

    private void OnDisable()
    {
        m_Input.UI.Submit.performed -= OnSubmit;
        m_Input.UI.Disable();
    }


    private void OnSubmit(InputAction.CallbackContext context)
    {
        

        //ƒV[ƒ“‘JˆÚ’†‚È‚ç–³‹
        if (SceneController.Instance.IsFading) return;

        //Œˆ’è‰¹‚ğ–Â‚ç‚·
        SEManager.Instance.SEPlay(SEType.DECIDE);
        SceneController.Instance.LoadScene("MainScene");  
    }
}
