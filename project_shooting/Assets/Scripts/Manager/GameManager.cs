using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameManager : SingletonMonoBehaviour<GameManager>
{

    private const int DEFAULT_MAX_LIFE = 3;
    private const int DEFAULT_ENEMY_HP = 10000;
    private const int DEFAULT_MAX_BOMB = 3;


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

    [Header("ゲーム情報")]
    public float playTime;   //経過時間
    public bool isActive; //動いているかどうか
  
    protected override void Awake()
    {
        base.Awake();

        InitializeGame();
    }

    /// <summary>
    /// ゲーム初期化
    /// </summary>
    private void InitializeGame()
    {
        Time.timeScale = 1.0f;

        isActive = true;

        maxLife = DEFAULT_MAX_LIFE;
        currentLife = maxLife;

        maxEnemyHP = DEFAULT_ENEMY_HP;
        currentEnemyHP = maxEnemyHP;

        maxBomb = DEFAULT_MAX_BOMB;
        currentBomb = maxBomb;

        playTime = 0.0f;
    }

    /// <summary>
    /// ゲームリセット
    /// </summary>
    public void ResetGame()
    {
        InitializeGame();
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
