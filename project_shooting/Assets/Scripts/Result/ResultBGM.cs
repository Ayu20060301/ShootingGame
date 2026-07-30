using UnityEngine;

public class ResultBGM : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        if (ResultData.isClear == true)
        {
            BGMManager.Instance.BGMPlay(BGMType.CLEAR);
        }
        else
        {
            BGMManager.Instance.BGMPlay(BGMType.GAMEOVER);
        }
    }

}
