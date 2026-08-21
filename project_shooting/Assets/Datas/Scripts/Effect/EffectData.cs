using UnityEngine;

/// <summary>
/// エフェクトの種類
/// </summary>
public enum EffectType
{
    EXPLOSION  //爆発エフェクト
}

//エフェクトデータ
[CreateAssetMenu(fileName = "EffectData",menuName = "Effect/EffectDataを作成")]

//エフェクトデータ
public class EffectData : ScriptableObject
{
    [Header("エフェクトの種類")]
    public EffectType type;

    [Header("エフェクトのPrefab")]
    public GameObject prefab;
}
