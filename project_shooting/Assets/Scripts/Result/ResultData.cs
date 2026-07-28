using UnityEngine;

//リザルトデータをまとめるクラス
public static class ResultData
{
    public static float playTime; //プレイ時間
    public static bool isClear;   //クリアしたかどうか   
    public static int bombUsed;   //ボム(爆弾)の使用数
    public static int hitCount;  //被弾回数
    public static Sprite rankSprite; //ランク用の画像     
}
