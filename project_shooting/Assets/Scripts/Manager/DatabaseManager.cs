using UnityEngine;

//各種データを管理するクラス
public class DatabaseManager : SingletonMonoBehaviour<DatabaseManager>
{
    [Header("各種データベースの参照")]
    public EffectDatabase effectDatabase;
    public SoundDatabase soundDatabase;
}
