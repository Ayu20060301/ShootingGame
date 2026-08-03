using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [Header("現在の情報")]
    public int currentLife; //現在の残機
    public int maxLife; //現在の残機
    public int currentEnemyHP;  //現在の敵のHP
    public int maxEnemyHP;  //最大の敵のHP
    public float playTime;   //経過時間
    public bool isActive; //動いているかどうか
  
    private void Start()
    {

        Time.timeScale = 1.0f;
        //ゲーム開始時は動作中
        isActive = true;

        maxLife = 3;
        playTime = 0.0f;
        maxEnemyHP = 10000;

    }


    public void ResetGame()
    {
        isActive = true;

        maxLife = 3;
        playTime = 0.0f;
        maxEnemyHP = 10000;
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
