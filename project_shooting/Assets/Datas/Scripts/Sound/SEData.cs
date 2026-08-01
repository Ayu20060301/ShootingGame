using UnityEngine;

/// <summary>
/// Œø‰Ê‰¹‚Ìí—Ş
/// </summary>
public enum SEType
{
    SHOT_PLAYER,
    SHOT_ENEMY,
    DAMAGE_PLAYER,
    DAMAGE_ENEMY,
    EXPLOSION,
    DECIDE,
    SELECT
}

[CreateAssetMenu(fileName = "SEData", menuName = "Sound/SeData‚ğì¬")]

public class SEData : ScriptableObject
{
    public SEType type;
    public AudioClip clip;
}
