using UnityEngine;

/// <summary>
/// 効果音の種類
/// </summary>
public enum SEType
{
    SHOT_PLAYER, //プレイヤーのショット音
    SHOT_ENEMY,  //敵のショット音
    DAMAGE_PLAYER,  //プレイヤーがダメージを受けた音
    DAMAGE_ENEMY,   //敵がダメージを受けた音
    EXPLOSION,    //爆発音
    DECIDE,      //決定音
    SELECT,      //選択音
    NOISE,      //ノイズ音
    HOMING,     //ホーミング音
    BOMB_EXPLOSION, //ボムの爆発音
    BOMB_ELECTRIC   
}

//効果音のデータ
[CreateAssetMenu(fileName = "SEData", menuName = "Sound/SeDataを作成")]

public class SEData : ScriptableObject
{
    [Header("効果音の種類")]
    public SEType type;

    [Header("効果音")]
    public AudioClip clip;
}
