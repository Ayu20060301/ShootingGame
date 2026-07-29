using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [Header("現在の情報")]
    public int score;   //スコア
    public int bombUsed; //ボムの使用数
    public int hitCount; //被弾数
    public float playTime;   //経過時間
    public Sprite rankSprite;
    public bool isActive; //動いているかどうか

    private void Start()
    {
        Application.targetFrameRate = 60; //フレームレート

        Time.timeScale = 1.0f;
        //ゲーム開始時は動作中
        isActive = true;
    }

    /// <summary>
    /// ポーズ
    /// </summary>
    public void Pause()
    {

        if (!isActive) return;

        isActive = false;
        Time.timeScale = 0.0f;
    }

    /// <summary>
    /// 再開
    /// </summary>
    public void Resume()
    {
        if (isActive) return;

        isActive = true;
        Time.timeScale = 1.0f;
    }


    public void ResetGame()
    {
        playTime = 0.0f;
        bombUsed = 0;
        hitCount = 0;

    }

}
