using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// GameManager
// ゲームの進行を制御するスクリプト

// ゲームステータス
public class GameStatus : MonoBehaviour
{
    public Mino ActiveMino { get; set; } // 操作中のミノ
    public Mino_Ghost GhostMino { get; set; } // ゴーストミノ
    public Mino[] NextMino_Array { get; set; } = new Mino[5]; //NEXT
    public Mino HoldMino { get; set; } // HOLD

    public List<int> SpawnMinoOrder_List { get; set; } = new List<int>(); //string型に変更予定

    public bool BackToBack { get; set; } = false;

    public int Ren { get; set; } = 0;

    public bool GameOver { get; set; }

    public int SpinActions { get; set; } = 7; // 修正予定

    // 底についた判定
    public bool Bottom { get; set; } = false;

    public bool CanNotMove { get; set; } = false;

    // ライン消去の履歴を表すリストと
    // それに対応するカウントの変数
    // カウントの変数は、ミノの設置で1ずつ増加する
    public List<int> LineEliminationCountHistory { get; set; } = new List<int>();
    public int LineEliminationCountHistoryNumber { get; set; } = 0; //cleanで消せるかもしれない


    /////Data.csから移動した変数↓

    //activeMinoから底までの距離を格納する変数
    //ゴーストミノの生成座標の計算で必要
    public int Distance { get; set; }

    //生成されるミノの順番について//

    //ミノの順番はこのスクリプト内の DecideSpawnMinoOrder() で決定する
    //決定されたミノの順番をspawnMinoOrderに格納
    //生成されるミノは増え続けるため、リスト型
    //public List<int> SpawnMinoOrder { get; set; } = new List<int>();

    //ミノの回転について//

    //ミノが回転した時、回転前の向き(Before)と回転後の向き(After)を保存する変数
    //初期値はnorthの状態
    public int MinoAngleBefore { get; set; } = 0;
    public int MinoAngleAfter { get; set; } = 0;

    // 回転の使用を判別する変数
    // ミノのSpin判定に必要
    // Spin判定は2つあり、SpinとSpinMiniがある
    public bool UseSpin { get; set; } = false;
    public bool UseSpinMini { get; set; } = false;

    // 最後に行ったスーパーローテーションシステム(SRS)の段階を表す変数
    // 0〜4の値が格納される
    // SRSが使用されていないときは0
    // 1〜4の時は、SRSの段階を表す
    public int LastSRS { get; set; }


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
    public bool UseHold { get; set; } = false;

    //Holdが1回目かどうかを判別する変数
    //Holdが1回でも使用されるとfalseになる
    public bool FirstHold { get; set; } = true;

    //Holdされたミノの生成番号
    public int HoldMinoNumber { get; set; }

    // ミノの出現数
    public int MinoPopNumber { get; set; } = 0;


    //各種変数の初期化をする関数
    public void AllReset()
    {
        AngleReset();

        SpinResetFlag();

        UseHardDrop = false;

        UseHold = false;
    }

    //ミノの向きを初期化する関数
    public void AngleReset()
    {
        MinoAngleBefore = 0;
        MinoAngleAfter = 0;
    }

    //ミノの回転フラグを無効にする関数
    public void SpinResetFlag()
    {
        UseSpin = false;
        LastSRS = 0;
    }

    //ミノの回転フラグを有効にする関数
    public void SpinSetFlag()
    {
        UseSpin = true;
        UseSpinMini = false;
        LastSRS = 0;
        SpinActions = 7;
    }
}
