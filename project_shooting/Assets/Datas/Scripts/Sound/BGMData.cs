using UnityEngine;

/// <summary>
/// BGM‚Ìí—Ş
/// </summary>
public enum BGMType
{
    TITLE,   
    GAME,
    CLEAR,
    GAMEOVER
}

[CreateAssetMenu(fileName = "BGMData", menuName = "Sound/BGMData‚ğì¬")]

public class BGMData : ScriptableObject
{
    public BGMType type;
    public AudioClip clip;
    public bool loop = true;
}
