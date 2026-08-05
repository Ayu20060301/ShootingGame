using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [Header("現在の情報")]

    [Header("残機")]
    public int currentLife; //現在の残機
    public int maxLife; //現在の残機

    [Header("敵のHP")]
    public int currentEnemyHP;  //現在の敵のHP
    public int maxEnemyHP;  //最大の敵のHP

    [Header("ボム(爆弾)")]
    public int currentBomb; //現在のボム数
    public int maxBomb;  //最大のボム数

    public float playTime;   //経過時間
    public bool isActive; //動いているかどうか
  
    private void Start()
    {

        Time.timeScale = 1.0f;
        //ゲーム開始時は動作中
        isActive = true;

        maxLife = 3; //最大残機
        playTime = 0.0f; //プレイ時間
        maxEnemyHP = 10000;  //敵の最大HP
        maxBomb = 3;  //最大ボム数

    }


    public void ResetGame()
    {
        isActive = true;

        maxLife = 3;
        playTime = 0.0f;
        maxEnemyHP = 10000;
        maxBomb = 3;
    }

    /// <summary>
    /// ゲーム終了処理
    /// </summary>
    /// <param name="isClear">ゲームクリアしているかどうか</param>
    public void GameEnd(bool isClear)
    {
        ResultData.isClear = isClear;
        ResultData.playTime = playTime;
        SceneController.Instance.LoadScene("ResultScene");
    }
}
