using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲームの状態
/// </summary>
public enum GameState
{
    PLAY,
    PAUSE,
    GAMEOVER
}

public class GameManager : SingletonMonoBehaviour<GameManager>
{

    [Header("ゲーム状態")]
    [SerializeField]
    private GameState m_GameState = GameState.PLAY;
    public GameState GameState => m_GameState;

    public float timer;  //時間

    private void Start()
    {
        Application.targetFrameRate = 60; //フレームレート

        timer = 0.0f;
    }

    /// <summary>
    /// ゲームを一時停止する
    /// </summary>
    public void Pause()
    {
        if (m_GameState != GameState.PLAY) return;

        m_GameState = GameState.PAUSE;

        Time.timeScale = 0.0f;
    }

    /// <summary>
    /// ゲームを再開する
    /// </summary>
    public void Resume()
    {
        if (m_GameState != GameState.PAUSE) return;

        m_GameState = GameState.PLAY;
        Time.timeScale = 1.0f;
    }

    /// <summary>
    /// リザルト処理
    /// </summary>
    public void Result()
    {
        if (m_GameState == GameState.GAMEOVER) return;

        m_GameState = GameState.GAMEOVER;
        Time.timeScale = 0.0f;
        Debug.Log("GameOver");

        SceneController.Instance.LoadScene("ResultScene");

    }
}
