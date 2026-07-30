using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EffectDatabase", menuName = "Effect/EffectDatabaseの作成")]

public class EffectDatabase : ScriptableObject
{
    //リストに追加
    public List<EffectData> effectData = new List<EffectData>();

    //メソッドの取得
    public EffectData GetEffectData(EffectType type)
    {
        return effectData.Find(effectData => effectData.type == type);
    }
}
