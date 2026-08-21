using UnityEngine;

/// <summary>
/// BGMの種類
/// </summary>
public enum BGMType
{
    TITLE,   //タイトル画面
    GAME,    //ゲーム画面
    RESULT,  //リザルト画面
    CLEAR,   //ゲームクリア時のBGM
    GAMEOVER //ゲームオーバー時のBGM
}

//BGMデータ
[CreateAssetMenu(fileName = "BGMData", menuName = "Sound/BGMDataを作成")]

public class BGMData : ScriptableObject
{
    [Header("BGMの種類")]
    public BGMType type;

    [Header("BGM")]
    public AudioClip clip;

    [Header("BGMがループするか")]
    public bool loop = true;
}
