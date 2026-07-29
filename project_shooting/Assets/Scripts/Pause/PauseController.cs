using UnityEngine;

public class PauseController : MonoBehaviour
{
    public void PauseGame()
    {
        //Šù‚É’âŽ~’†‚È‚ç‰½‚à‚µ‚È‚¢
        if (!GameManager.Instance.isActive) return;

        GameManager.Instance.isActive = false;
        Time.timeScale = 0.0f;
    }

    public void ResumeGame()
    {
        //Šù‚ÉƒvƒŒƒC’†‚È‚ç‰½‚à‚µ‚È‚¢
        if (GameManager.Instance.isActive) return;

        GameManager.Instance.isActive = true;
        Time.timeScale = 1.0f;
    }
}
