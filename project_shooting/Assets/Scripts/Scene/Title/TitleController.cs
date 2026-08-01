using UnityEngine;
using UnityEngine.InputSystem;

public class TitleController : MonoBehaviour
{

    private PlayerInputActions m_Input;

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
        SEManager.Instance.SEPlay(SEType.DECIDE);
        SceneController.Instance.LoadScene("MainScene");
    }
}
