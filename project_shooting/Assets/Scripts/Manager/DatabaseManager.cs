using UnityEngine;

public class DatabaseManager : SingletonMonoBehaviour<DatabaseManager>
{
    [Header("各種データベースの参照")]
    public EffectDatabase effectDatabase;
    public SoundDatabase soundDatabase;
    public PlayerDatabase playerDatabase;
}
