using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Player/PlayerDataを作成")]

public class PlayerData : ScriptableObject
{
    public int maxLife; //最大残機
    public int currentLife; //現在の残機
    public int attack;  //攻撃
    public float moveSpeed; //移動速度
    public int maxBomb; //最大ボム数
    public int currentBomb; //現在のボム数
}
