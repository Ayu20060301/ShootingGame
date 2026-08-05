using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerDatabase", menuName = "Player/PlayerDatabase‚ğì¬")]

public class PlayerDatabase : ScriptableObject
{
    //ƒŠƒXƒg‚Ìì¬
    public List<PlayerData> playerData = new List<PlayerData>();
}
