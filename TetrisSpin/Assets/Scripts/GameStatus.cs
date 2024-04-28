using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// GameManager
// ゲームの進行を制御するスクリプト

// ゲームステータス
public class GameStatus : MonoBehaviour
{
    // public Mino ActiveMino { get; set; } // 操作中のミノ
    //public Mino_Ghost GhostMino { get; set; } // ゴーストミノ
    //public Mino[] NextMino_Array { get; set; } = new Mino[5]; //NEXT
    //public Mino HoldMino { get; set; } // HOLD

    //public List<int> SpawnMinoOrder_List { get; set; } = new List<int>(); //string型に変更予定

    private bool BackToBack = false;

    private int Ren = 0;

    private bool GameOver;

    // Spinの種類 //
    // private string SpinTypeName;

    // 底についた判定
    //public bool Bottom { get; set; } = false;

    //public bool CanNotMove { get; set; } = false;


    // ライン消去数 //
    private List<int> LineClearCountHistory = new List<int>(); // ライン消去の履歴を表すリストと
    // private int LineClearCountHistoryNumber = 0; // それに対応するカウントの変数


    /////Data.csから移動した変数↓

    //activeMinoから底までの距離を格納する変数
    //ゴーストミノの生成座標の計算で必要
    //public int Distance { get; set; }

    //生成されるミノの順番について//

    //ミノの順番はこのスクリプト内の DecideSpawnMinoOrder() で決定する
    //決定されたミノの順番をspawnMinoOrderに格納
    //生成されるミノは増え続けるため、リスト型
    //public List<int> SpawnMinoOrder { get; set; } = new List<int>();

    //ミノの回転について//

    //ミノが回転した時、回転前の向き(Before)と回転後の向き(After)を保存する変数
    //初期値はnorthの状態
    //public int MinoAngleBefore { get; set; } = 0;
    //public int MinoAngleAfter { get; set; } = 0;

    // 回転の使用を判別する変数
    // ミノのSpin判定に必要
    // Spin判定は2つあり、SpinとSpinMiniがある
    //public bool UseSpin { get; set; } = false;
    public bool UseSpinMini { get; set; } = false;

    // 最後に行ったスーパーローテーションシステム(SRS)の段階を表す変数
    // 0〜4の値が格納される
    // SRSが使用されていないときは0
    // 1〜4の時は、SRSの段階を表す
    private int LastSRS = 0;

    // ゲッタープロパティ //
    public int lastSRS
    {
        get { return LastSRS; }
    }
    public bool gameOver
    {
        get { return GameOver; }
    }
    public List<int> lineClearCountHistory
    {
        get { return LineClearCountHistory; }
    }


    //ハードドロップについて

    //操作中のミノを瞬時に最下部まで落下させる機能

    //ハードドロップが使用されたか判別する変数
    public bool UseHardDrop { get; set; } = false;

    //Holdについて//

    //操作中のミノを一時的に保持する機能
    //Holdは1回目の処理と2回目以降の処理が違う

    //1回目
    //Holdされたミノは、ゲーム画面の左上あたりに移動
    //その後、Nextミノが新しく降ってくる
    //2回目以降
    //Holdされたミノは、ゲーム画面の左上あたりに移動(1回目と同じ)
    //以前Holdしたミノが新しく降ってくる

    //Holdが使用されたか判別する変数
    //Holdを使うと、次のミノを設置するまで使用できない
    //public bool UseHold { get; set; } = false;

    //Holdが1回目かどうかを判別する変数
    //Holdが1回でも使用されるとfalseになる
    public bool FirstHold { get; set; } = true;

    //Holdされたミノの生成番号
    public int HoldMinoNumber { get; set; }

    // ミノの出現数
    public int MinoPopNumber { get; set; } = 0;

    // 向きの定義 //
    public string North { get; } = "NORTH"; // 初期(未回転)状態をNORTHとして、
    public string East { get; } = "EAST"; // 右回転後の向きをEAST
    public string South { get; } = "SOUTH"; // 左回転後の向きをWEST
    public string West { get; } = "WEST"; // 2回右回転または左回転した時の向きをSOUTHとする

    // ミノの回転後と回転前の向き //
    [SerializeField] private string MinoAngleAfter = "NORTH"; // 初期値はNORTHの状態
    [SerializeField] private string MinoAngleBefore = "NORTH"; // 初期値はNORTHの状態 // SRSで必要

    public string minoAngleAfter
    {
        get { return MinoAngleAfter; }
    }
    public string minoAngleBefore
    {
        get { return MinoAngleBefore; }
    }

    Spawner spawner;

    private void Awake()
    {
        spawner = FindObjectOfType<Spawner>();
    }


    //各種変数の初期化をする関数
    public void AllReset()
    {
        AngleReset();

        SpinResetFlag();

        UseHardDrop = false;

        //UseHold = false;
    }

    // //ミノの向きを初期化する関数
    // public void AngleReset()
    // {
    //     //MinoAngleBefore = 0;
    //     //MinoAngleAfter = 0;
    // }

    //ミノの回転フラグを無効にする関数
    public void SpinResetFlag()
    {
        //UseSpin = false;
        LastSRS = 0;
        //SpinTypeName = "None";
    }

    //ミノの回転フラグを有効にする関数
    public void SpinSetFlag()
    {
        //UseSpin = true;
        UseSpinMini = false;
        LastSRS = 0;
        //SpinTypeName = "None";
    }

    public void IncreaseLastSRS()
    {
        LastSRS++;
    }

    public void GameOverAction()
    {
        GameOver = true;
    }

    // public void SetSpinTypeName(string _NewSpinTypeName)
    // {
    //     SpinTypeName = _NewSpinTypeName;
    // }

    public void AddLineClearCountHistory(int _LineClearCount, int _MinoPutNumber)
    {
        LineClearCountHistory.Add(_LineClearCount);
    }

    // public void ResetUseHold()
    // {
    //     UseHold = false;
    // }

    // MinoAngleBeforeの更新をする関数 //
    public void UpdateMinoAngleBefore()
    {
        Debug.Log("はーい1");
        MinoAngleBefore = MinoAngleAfter;
    }

    // MinoAngleAfterのリセットをする関数 //
    public void ResetMinoAngleAfter()
    {
        MinoAngleAfter = MinoAngleBefore;
    }

    // 通常回転のリセットをする関数 //
    public void RotateReset()
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

    // ミノの向きを初期化する関数 //
    public void AngleReset()
    {
        MinoAngleBefore = "NORTH";
        MinoAngleAfter = "NORTH";
    }

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
}
