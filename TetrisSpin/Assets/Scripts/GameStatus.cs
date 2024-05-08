using System.Collections.Generic;
using UnityEngine;

///// ゲームステータスに関するスクリプト /////


// ↓このスクリプトで可能なこと↓ //

// ゲームの状態を管理(状態が変化する時は、他のスクリプトで計算される)


public class GameStatus : MonoBehaviour
{
    private bool GameOver; // ゲームオーバの判定

    private bool BackToBack = false; // BackToBackの判定

    private int Ren = 0; // Renの判定

    private List<int> LineClearCountHistory = new List<int>(); // ライン消去の履歴を記録するリスト

    // 向きの定義 //
    public string North { get; } = "NORTH"; // 初期(未回転)状態をNORTHとして、
    public string East { get; } = "EAST"; // 右回転後の向きをEAST
    public string South { get; } = "SOUTH"; // 左回転後の向きをWEST
    public string West { get; } = "WEST"; // 2回右回転または左回転した時の向きをSOUTHとする

    // ミノの回転後と回転前の向き //
    private string MinoAngleAfter = "NORTH"; // 初期値はNORTHの状態
    private string MinoAngleBefore = "NORTH"; // 初期値はNORTHの状態 // SRSで必要

    // 最後に行ったスーパーローテーションシステム(SRS)の段階を表す変数 //
    private int LastSRS = 0; // SRSが使用されていないときは0, 1〜4の時は、SRSの段階を表す

    // ゲッタープロパティ //
    public bool gameOver
    {
        get { return GameOver; }
    }
    public bool backToBack
    {
        get { return BackToBack; }
    }
    public int ren
    {
        get { return Ren; }
    }
    public List<int> lineClearCountHistory
    {
        get { return LineClearCountHistory; }
    }
    public string minoAngleAfter
    {
        get { return MinoAngleAfter; }
    }
    public string minoAngleBefore
    {
        get { return MinoAngleBefore; }
    }
    public int lastSRS
    {
        get { return LastSRS; }
    }

    // 干渉するスクリプト //
    Spawner spawner;

    // インスタンス化 //
    private void Awake()
    {
        spawner = FindObjectOfType<Spawner>();
    }

    // ゲームオーバー判定をオンにする関数 //
    public void Set_GameOver()
    {
        GameOver = true;
    }

    // ゲームオーバー判定をオフにする関数 //
    public void Reset_GameOver()
    {
        GameOver = false;
    }

    // BackToBack判定をオンにする関数 //
    public void Set_BackToBack()
    {
        BackToBack = true;
    }

    // BackToBack判定をオフにする関数 //
    public void Reset_BackToBack()
    {
        BackToBack = false;
    }

    // Renの値をリセットする関数 //
    public void Reset_Ren()
    {
        Ren = 0;
    }

    // Renの値を1増加させる関数 //
    public void IncreaseRen()
    {
        Ren++;
    }

    // ライン消去数
    public void AddLineClearCountHistory(int _LineClearCount, int _MinoPutNumber)
    {
        LineClearCountHistory.Add(_LineClearCount);
    }

    // ミノの向きを初期化する関数 //
    public void Reset_Angle()
    {
        MinoAngleBefore = "NORTH";
        MinoAngleAfter = "NORTH";
    }

    // MinoAngleAfterのリセットをする関数 //
    public void Reset_MinoAngleAfter()
    {
        MinoAngleAfter = MinoAngleBefore;
    }

    // 通常回転のリセットをする関数 //
    public void Reset_Rotate()
    {
        //通常回転が右回転だった時
        if ((MinoAngleBefore == North && MinoAngleAfter == East) ||
        (MinoAngleBefore == East && MinoAngleAfter == South) ||
        (MinoAngleBefore == South && MinoAngleAfter == West) ||
        (MinoAngleBefore == West && MinoAngleAfter == North))
        {
            //左回転で回転前の状態に戻す
            spawner.activeMino.Rotateleft();
        }
        //通常回転が左回転だった時
        else
        {
            //右回転で回転前の状態に戻す
            spawner.activeMino.RotateRight();
        }
    }

    // MinoAngleAfterの更新をする関数 //
    public void UpdateMinoAngleAfter(string _RotateDirection)
    {
        if (_RotateDirection == "RotateRight") // 右回転の時
        {
            switch (MinoAngleAfter)
            {
                case "NORTH":
                    MinoAngleAfter = East;
                    break;
                case "EAST":
                    MinoAngleAfter = South;
                    break;
                case "SOUTH":
                    MinoAngleAfter = West;
                    break;
                case "WEST":
                    MinoAngleAfter = North;
                    break;
            }
        }
        else if (_RotateDirection == "RotateLeft") // 左回転の時
        {
            switch (MinoAngleAfter)
            {
                case "NORTH":
                    MinoAngleAfter = West;
                    break;
                case "EAST":
                    MinoAngleAfter = North;
                    break;
                case "SOUTH":
                    MinoAngleAfter = East;
                    break;
                case "WEST":
                    MinoAngleAfter = South;
                    break;
            }
        }
    }

    // MinoAngleBeforeの更新をする関数 //
    public void UpdateMinoAngleBefore()
    {
        MinoAngleBefore = MinoAngleAfter;
    }

    // LastSRSの値をリセットする関数 //
    public void Reset_LastSRS()
    {
        LastSRS = 0;
    }

    // LastSRSの値を1増加させる関数 //
    public void IncreaseLastSRS()
    {
        LastSRS++;
    }
}
