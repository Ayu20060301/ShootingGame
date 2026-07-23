using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

//ゲームの進行を管理するクラス
public class GameManager : MonoBehaviour
{
    //シングルトン化
    public static GameManager Instance { get; private set; }

    /// <summary>
    /// ゲームの状態
    /// </summary>
    public enum GameState
    {
        TITLE,   //タイトル
        LOADING,  //ロード中
        PLAY,     //ゲームプレイ
        PAUSED,   //ポーズ中
        GAMEOVER,  //ゲームオーバー
    }

    public GameState CurrentState { get; private set; }
    public event Action<GameState> OnStateChanged;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
    }

    private void Start()
    {
        ChangeState(GameState.TITLE);
    }


    public void ChangeState(GameState newState)
    {
        CurrentState = newState;
        OnStateChanged.Invoke(newState);

        switch(newState)
        {
            case GameState.TITLE:
                Time.timeScale = 1.0f;
                break;
            case GameState.LOADING:
                Time.timeScale = 1.0f;
                break;
            case GameState.PLAY:
                Time.timeScale = 1.0f;
                break;
            case GameState.PAUSED:
                Time.timeScale = 0.0f;
                break;
            case GameState.GAMEOVER:
                Time.timeScale = 0.0f;
                break;
        }
    }

    /// <summary>
    /// ゲームオーバー
    /// </summary>
    public void GameOver()
    {
        ChangeState(GameState.GAMEOVER);
    }

    /// <summary>
    /// ロード中
    /// </summary>
    public void Loading()
    {
        ChangeState(GameState.LOADING);
    }

    /// <summary>
    /// ゲームスタート
    /// </summary>
    public void StartGame()
    {
        ChangeState(GameState.PLAY);
    }

    /// <summary>
    /// ポーズ中
    /// </summary>
    public void Pauseing()
    {
        if(CurrentState == GameState.PAUSED)
        {
            ChangeState(GameState.PAUSED);
        }
    }

}
