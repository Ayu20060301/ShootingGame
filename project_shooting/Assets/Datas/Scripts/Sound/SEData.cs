using UnityEngine;

/// <summary>
/// Œø‰Ê‰¹‚Ìí—Ş
/// </summary>
public enum SEType
{
    SHOT_PLAYER,
    SHOT_ENEMY,
}

[CreateAssetMenu(fileName = "SEData", menuName = "Sound/SeData‚ğì¬")]

public class SEData : ScriptableObject
{
    public SEType type;
    public AudioClip clip;
}
