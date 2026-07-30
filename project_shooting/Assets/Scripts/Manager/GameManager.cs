using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [Header("現在の情報")]
    public int score;   //スコア
    public int bombUsed; //ボムの使用数
    public int life; //残機
    public float playTime;   //経過時間
    public bool isActive; //動いているかどうか
  
    private void Start()
    {
        Application.targetFrameRate = 60; //フレームレート

        Time.timeScale = 1.0f;
        //ゲーム開始時は動作中
        isActive = true;
    }


    public void GameEnd(bool isClear)
    {
        ResultData.isClear = isClear;
        ResultData.life = life;
        ResultData.bombUsed = bombUsed;
        ResultData.playTime = playTime;

            SceneController.Instance.LoadScene("ResultScene");

    }
}
