using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;



public class GameManager : SingletonMonoBehaviour<GameManager>
{


    [Header("現在の情報")]
    public int score;   //スコア
    public int bombUsed; //ボムの使用数
    public int hitCount; //被弾数
    public float playTime;   //経過時間
    public Sprite rankSprite;

    private void Start()
    {
        Application.targetFrameRate = 60; //フレームレート
    }

    /// <summary>
    /// ゲーム終了
    /// </summary>
    /// <param name="isClear">クリアしたかどうか</param>
    public void GameEnd(bool isClear)
    {
        ResultData.playTime = playTime;
        ResultData.isClear = isClear;
        ResultData.bombUsed = bombUsed;
        ResultData.hitCount = hitCount;

        //リザルトシーンに遷移
        SceneController.Instance.LoadScene("ResultScene");
    }
}
