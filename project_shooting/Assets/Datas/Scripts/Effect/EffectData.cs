using UnityEngine;


//エフェクトの種類
public enum EffectType
{
    EXPLOSION  //爆発
}

[CreateAssetMenu(fileName = "EffectData",menuName = "Effect/EffectDataを作成")]

public class EffectData : ScriptableObject
{
    
    public EffectType type;
    public GameObject prefab;
}
