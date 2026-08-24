using UnityEngine;
using System.Collections;
using NUnit.Framework;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "SoundDatabase", menuName = "Sound/SoundDatabaseを作成")]

public class SoundDatabase : ScriptableObject
{
    //リストに追加
    public List<BGMData> bgmData = new();
    public List<SEData> seData = new();

    //メソッドの取得
    public BGMData GetBGMData(BGMType type)
    {
        return bgmData.Find(bgmData => bgmData.type == type);
    }

    public SEData GetSEData(SEType type)
    {
        return seData.Find(seData => seData.type == type);
    }
}
