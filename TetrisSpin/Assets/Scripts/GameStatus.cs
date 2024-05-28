using System.Collections.Generic;
using UnityEngine;

///// ゲームステータスに関するスクリプト /////


// ↓このスクリプトで可能なこと↓ //

// ゲームの状態を管理


public class GameStatus : MonoBehaviour
{
    private bool GameOver; // ゲームオーバーの判定

    private bool BackToBack = false; // BackToBackの判定

    // private bool PerfectClear = false; // PerfectClearの判定

    private int Ren = -1; // Renの判定

    [SerializeField] private int AttackLines = 0; // 攻撃ライン数

    private List<int> LineClearCountHistory = new List<int>(); // ライン消去の履歴を記録するリスト

    // // 向きの定義 //
    // private string North = "North"; // 初期(未回転)状態をNorthとして、
    // private string East = "East"; // 右回転後の向きをEast
    // private string South = "South"; // 左回転後の向きをWest
    // private string West = "West"; // 2回右回転または左回転した時の向きをSouthとする

    // ミノの回転後と回転前の向き //
    [SerializeField] private MinoDirections MinoAngleAfter = MinoDirections.North; // 初期値はNorthの状態
    [SerializeField] private MinoDirections MinoAngleBefore = MinoDirections.North; // 初期値はNorthの状態 // SRSで必要

    // 最後に行ったスーパーローテーションシステム(SRS)の段階を表す変数 //
    [SerializeField] private int StepsSRS = 0; // SRSが使用されていないときは0, 1〜4の時は、SRSの段階を表す

    // ゲッタープロパティ //
    public bool gameOver
    {
        get { return GameOver; }
    }
    public bool backToBack
    {
        get { return BackToBack; }
    }
    // public bool perfectClear
    // {
    //     get { return PerfectClear; }
    // }
    public int ren
    {
        get { return Ren; }
    }
    public List<int> lineClearCountHistory
    {
        get { return LineClearCountHistory; }
    }
    public MinoDirections minoAngleAfter
    {
        get { return MinoAngleAfter; }
    }
    public MinoDirections minoAngleBefore
    {
        get { return MinoAngleBefore; }
    }
    public int stepsSRS
    {
        get { return StepsSRS; }
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

    // // PerfectClear判定をオンにする関数 //
    // public void Set_PerfectClear()
    // {
    //     PerfectClear = true;
    // }

    // // PerfectClear判定をオフにする関数 //
    // public void Reset_PerfectClear()
    // {
    //     PerfectClear = false;
    // }

    // Renの値をリセットする関数 //
    public void Reset_Ren()
    {
        Ren = -1;
    }

    // Renの値を1増加させる関数 //
    public void IncreaseRen()
    {
        Ren++;
    }

    // AttackLinesの値を足していく関数
    public void IncreaseAttackLines(int _addAttackLines)
    {
        AttackLines += _addAttackLines;
    }

    // ライン消去数
    public void AddLineClearCountHistory(int _LineClearCount, int _MinoPutNumber)
    {
        LineClearCountHistory.Add(_LineClearCount);
    }

    // ミノの向きを初期化する関数 //
    public void Reset_Angle()
    {
        MinoAngleBefore = MinoDirections.North;
        MinoAngleAfter = MinoDirections.North;
    }

    // MinoAngleAfterのリセットをする関数 //
    public void Reset_MinoAngleAfter()
    {
        MinoAngleAfter = MinoAngleBefore;
    }

    // 通常回転のリセットをする関数 //
    public void Reset_Rotate()
    {
        // 通常回転が右回転だった時
        if ((MinoAngleBefore == MinoDirections.North && MinoAngleAfter == MinoDirections.East) ||
        (MinoAngleBefore == MinoDirections.East && MinoAngleAfter == MinoDirections.South) ||
        (MinoAngleBefore == MinoDirections.South && MinoAngleAfter == MinoDirections.West) ||
        (MinoAngleBefore == MinoDirections.West && MinoAngleAfter == MinoDirections.North))
        {
            spawner.ActiveMino.RotateLeft(); // 左回転で回転前の状態に戻す
        }
        else // 通常回転が左回転だった時
        {
            spawner.ActiveMino.RotateRight(); // 右回転で回転前の状態に戻す
        }
    }

    // MinoAngleAfterの更新をする関数 //
    public void UpdateMinoAngleAfter(MinoRotationDirections _RotateDirection)
    {
        if (_RotateDirection == MinoRotationDirections.RotateRight) // 右回転の時
        {
            switch (MinoAngleAfter)
            {
                case MinoDirections.North:
                    MinoAngleAfter = MinoDirections.East;
                    break;
                case MinoDirections.East:
                    MinoAngleAfter = MinoDirections.South;
                    break;
                case MinoDirections.South:
                    MinoAngleAfter = MinoDirections.West;
                    break;
                case MinoDirections.West:
                    MinoAngleAfter = MinoDirections.North;
                    break;
            }
        }
        else if (_RotateDirection == MinoRotationDirections.RotateLeft) // 左回転の時
        {
            switch (MinoAngleAfter)
            {
                case MinoDirections.North:
                    MinoAngleAfter = MinoDirections.West;
                    break;
                case MinoDirections.East:
                    MinoAngleAfter = MinoDirections.North;
                    break;
                case MinoDirections.South:
                    MinoAngleAfter = MinoDirections.East;
                    break;
                case MinoDirections.West:
                    MinoAngleAfter = MinoDirections.South;
                    break;
            }
        }
    }

    // MinoAngleBeforeの更新をする関数 //
    public void UpdateMinoAngleBefore()
    {
        MinoAngleBefore = MinoAngleAfter;
    }

    // StepsSRSの値をリセットする関数 //
    public void Reset_StepsSRS()
    {
        StepsSRS = 0;
    }

    // StepsSRSの値を1増加させる関数 //
    public void IncreaseStepsSRS()
    {
        StepsSRS++;
    }
}