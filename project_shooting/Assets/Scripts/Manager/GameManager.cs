using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

//ゲーム全体の状態や情報を管理するクラス
public class GameManager : SingletonMonoBehaviour<GameManager>
{
    //デフォルトの最大残機数
    private const int DEFAULT_MAX_LIFE = 3;
    
    //敵の最大HP
    private const int DEFAULT_ENEMY_HP = 10000;
    
    //最大ボム数
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

        //ゲーム情報を初期化
        InitializeGame();
    }

    /// <summary>
    /// ゲーム情報を初期化する
    /// </summary>
    private void InitializeGame()
    {
        Time.timeScale = 1.0f;

        //ゲームを開始状態にする
        isActive = true;

        //残機を初期化
        maxLife = DEFAULT_MAX_LIFE;
        currentLife = maxLife;

        //敵HPを初期化
        maxEnemyHP = DEFAULT_ENEMY_HP;
        currentEnemyHP = maxEnemyHP;

        //ボム数を初期化
        maxBomb = DEFAULT_MAX_BOMB;
        currentBomb = maxBomb;

        //経過時間をリセット
        playTime = 0.0f;
    }

    /// <summary>
    /// ゲームの状態を初期状態に戻す
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
        //リザルト用のデータを保存
        ResultData.isClear = isClear;
        ResultData.playTime = playTime;

        //リザルトシーンへ移動
        SceneController.Instance.LoadScene("ResultScene");
    }
}
