using System.Collections.Generic;
using UnityEngine;

//複数のスクリプトに干渉するデータをまとめて扱うスクリプト
//計算する関数も扱う

// // 不変の値
// public class ConstantData : MonoBehaviour
// {
//     // ミノの種類
//     string[] MinoTypes { get; } = new string[]
//     {
//         "I_mino", "J_mino", "L_mino", "O_mino", "S_mino", "T_mino", "Z_mino"
//     };

//     //ミノのPrefabs
//     //順番は(I, J, L, O, S, T, Z)
//     public Mino[] Minos;

//     //ゴーストミノについて//

//     //ゴーストミノとは、操作中のミノをそのままドロップした時、またはハードドロップした時に
//     //設置される想定の場所を薄く表示するミノのこと
//     //これを実装することで、テトリスのプレイが格段にしやすくなる

//     //ゴーストミノのPrefabs
//     //順番は(I, J, L, O, S, T, Z)
//     public Mino_Ghost[] Minos_Ghost;

//     //ミノの向きについて//

//     //GameManagerとRotationで用いる
//     //初期(未回転)状態をnorthとして、
//     //右回転後の向きをeast
//     //左回転後の向きをwest
//     //2回右回転または左回転した時の向きをsouthとする
//     private int North { get; } = 0;
//     private int East { get; } = 90;
//     private int South { get; } = 180;
//     private int West { get; } = 270;

//     //新しくミノが降ってくる時の初期座標
//     private Vector3 SpawnMinoPosition { get; } = new Vector3(4, 20, 0);

//     //Holdされたミノの座標(画面左上に配置)
//     private Vector3 HoldMinoPosition { get; } = new Vector3(-3, 17, 0);
// }

public class Calculate : MonoBehaviour
{
    // //干渉するスクリプトの設定
    // Board board;
    // //GameManager gameManager;
    // GameStatus gameStatus;
    // Spawner spawner;
    // TetrisSpinData tetrisSpinData;
    // Mino mino;

    //テトリミノの基本情報について//

    //テトリスには合計7種類のテトリミノが存在する (以後テトリミノをミノと呼ぶことにする)
    //各ミノはアルファベットで呼ばれる(I, J, L, O, S, T, Z)

    //ゲーム進行は、この7種類のミノをランダムに並べて生成し、
    //7種類全てが生成された時に、また新しく7種類のミノをランダムに並べて生成していくことを繰り返す
    //この法則を『七種一巡の法則』と呼ぶ

    //ミノのPrefabsをminosに格納
    //順番は(I, J, L, O, S, T, Z)
    /////////public Mino[] minos;

    //プログラム内での判別は数値で行う
    // I_mino = 0
    // J_mino = 1
    // L_mino = 2
    // O_mino = 3
    // S_mino = 4
    // T_mino = 5
    // Z_mino = 6

    //例
    //minos[5]→T_mino

    //対応するミノの数値を格納する変数を宣言
    /////////public int order;


    //ゴーストミノについて//

    //ゴーストミノとは、操作中のミノをそのままドロップした時、またはハードドロップした時に
    //設置される想定の場所を薄く表示するミノのこと
    //これを実装することで、テトリスのプレイが格段にしやすくなる

    //ゴーストミノのPrefabsをminos_Ghostに格納
    //順番は(I, J, L, O, S, T, Z)
    /////////public Mino_Ghost[] minos_ghost;

    //activeMinoから底までの距離を格納する変数
    //ゴーストミノの生成座標の計算で必要
    /////////public intgameStatus.Distance;


    //生成されるミノの順番について//

    //ミノの順番はこのスクリプト内の DecideSpawnMinoOrder() で決定する
    //決定されたミノの順番をspawnMinoOrderに格納
    //生成されるミノは増え続けるため、リスト型
    /////////public List<int> spawn_mino_order = new List<int>();


    //ミノの生成番号について//

    //何番目に生成されたミノか、で管理する
    /////////public int count = 0;


    //ミノの生成座標//

    //新しくミノが降ってくる時の初期座標
    /////////public Vector3 spawnMinoPosition = new Vector3(4, 20, 0);


    //ミノの向きについて//

    //GameManagerとRotationで用いる
    //初期(未回転)状態をnorthとして、
    //右回転後の向きをeast
    //左回転後の向きをwest
    //2回右回転または左回転した時の向きをsouthとする
    // public int north = 0;
    // public int east = 90;
    // public int south = 180;
    // public int west = 270;

    // //ミノの回転について//

    // //ミノが回転した時、回転前の向き(Before)と回転後の向き(After)を保存する変数
    // //初期値はnorthの状態
    // public int minoAngleBefore = 0;
    // public int minoAngleAfter = 0;

    // // 回転の使用を判別する変数
    // // ミノのSpin判定に必要
    // // Spin判定は2つあり、SpinとSpinMiniがある
    // public bool useSpin = false;
    // public bool spinMini = false;

    // // 最後に行ったスーパーローテーションシステム(SRS)の段階を表す変数
    // // 0〜4の値が格納される
    // // SRSが使用されていないときは0
    // // 1〜4の時は、SRSの段階を表す
    // public int lastSRS;


    // //ハードドロップについて

    // //操作中のミノを瞬時に最下部まで落下させる機能

    // //ハードドロップが使用されたか判別する変数
    // public bool hardDrop = false;


    //Nextについて//

    //次にどのミノが生成されるかを確認できる機能
    //ゲーム画面右側に表示される
    //このTetrisSpinでは、表示されるNextの数を5つにする

    //Nextに表示されるミノのGameObjectを格納
    /////////public Mino[] nextBlocks = new Mino[5];


    // //Holdについて//

    // //操作中のミノを一時的に保持する機能
    // //Holdは1回目の処理と2回目以降の処理が違う

    // //1回目
    // //Holdされたミノは、ゲーム画面の左上あたりに移動
    // //その後、Nextミノが新しく降ってくる
    // //2回目以降
    // //Holdされたミノは、ゲーム画面の左上あたりに移動(1回目と同じ)
    // //以前Holdしたミノが新しく降ってくる

    // //Holdが使用されたか判別する変数
    // //Holdを使うと、次のミノを設置するまで使用できない
    // public bool useHold = false;

    // //Holdが1回目かどうかを判別する変数
    // //Holdが1回でも使用されるとfalseになる
    // public bool firstHold = true;

    // //Holdされたミノの生成番号
    // public int holdMinoCount;

    //Holdされたミノの座標(画面左上に配置)
    ////////public Vector3 holdMinoPosition = new Vector3(-3, 17, 0);


    // //テトリスのゲームフィールドについて//

    // //ゲームフィールドは高さ20マス、幅10マスに設定
    // //headerは、ゲームオーバーの判定に必要
    // //height - header で高さを表現
    // public int height = 40, width = 10, header = 20;


    //ゲーム画面のテキストについて//

    //表示できるテキストの一覧
    //変数宣言の文法上、実際にゲーム画面に表示するテキストと変数名が合致しない場合がある
    // public int one_Line_Clear = 1;
    // public int two_Line_Clear = 2;
    // public int three_Line_Clear = 3;
    // public int Tetris = 4;
    // public int Ispin = 5;
    // public int Ispin_Single = 6;
    // public int Ispin_Double = 7;
    // public int Ispin_Triple = 8;
    // public int Ispin_Quattro = 9;
    // public int Jspin = 10;
    // public int Jspin_Single = 11;
    // public int Jspin_Double = 12;
    // public int Jspin_Triple = 13;
    // public int Lspin = 14;
    // public int Lspin_Single = 15;
    // public int Lspin_Double = 16;
    // public int Lspin_Triple = 17;
    // public int Ospin = 18;
    // public int Ospin_Single = 19;
    // public int Ospin_Double = 20;
    // public int Ospin_Triple = 21;
    // public int Sspin = 22;
    // public int Sspin_Single = 23;
    // public int Sspin_Double = 24;
    // public int Sspin_Triple = 25;
    // public int Tspin = 26;
    // public int Tspin_Single = 27;
    // public int Tspin_Double = 28;
    // public int Tspin_Triple = 29;
    // public int Tspin_Mini = 30;
    // public int Tspin_Double_Mini = 31;
    // public int Zspin = 32;
    // public int Zspin_Single = 33;
    // public int Zspin_Double = 34;
    // public int Zspin_Triple = 35;



    // //インスタンス化
    // private void Awake()
    // {
    //     board = FindObjectOfType<Board>();
    //     //gameManager = GetComponent<GameManager>();
    //     gameStatus = FindObjectOfType<GameStatus>();
    //     spawner = FindObjectOfType<Spawner>();
    //     tetrisSpinData = FindObjectOfType<TetrisSpinData>();
    //     mino = FindObjectOfType<Mino>();
    // }

    // //各種変数の初期化をする関数
    // public void AllReset()
    // {
    //     AngleReset();

    //     SpinReset();

    //     hardDrop = false;

    //     useHold = false;
    // }

    // //ミノの向きを初期化する関数
    // public void AngleReset()
    // {
    //     minoAngleBefore = 0;
    //     minoAngleAfter = 0;
    // }

    // //ミノの回転のフラグを初期化する関数
    // public void SpinReset()
    // {
    //     useSpin = false;
    //     lastSRS = 0;
    // }

    // //ミノの配列を決めてspawnMinoOrderに追加する関数
    // public void DetermineSpawnMinoOrder()
    // {
    //     //七種一巡の法則実装に必要な配列

    //     //range0to6は、0から6までの整数が入ったリスト
    //     //[0,1,2,3,4,5,6] ←このようなもの
    //     //range0To6を使用して、ランダムな配列をminoOrderに格納する
    //     //[2,4,3,6,1,5,0] ←このようなもの
    //     List<int> range0to6 = new List<int>();

    //     for (int numbers = 0; numbers <= 6; numbers++)
    //     {
    //         //0から6までの整数が入ったリストの生成
    //         range0to6.Add(numbers);
    //     }

    //     //range0to6の配列がなくなるまで繰り返す
    //     while (range0to6.Count > 0)
    //     {
    //         //0からrange0to6の配列数までの範囲でランダムな数値を取得し、indexに格納
    //         int index = Random.Range(0, range0to6.Count);

    //         //indexの数値をrandomNumberに格納
    //         int randomNumber = range0to6[index];

    //         //minoOrderにrandomNumberを追加
    //         gameStatus.SpawnMinoOrder_List.Add(randomNumber);

    //         //インデックス位置の要素を削除
    //         range0to6.RemoveAt(index);
    //     }
    // }


    // //activeMinoの種類を判別する関数
    // public int CheckActiveMinoShape()
    // {
    //     int order;

    //     //ミノは7種類あるので、7回繰り返す
    //     for (order = 0; order < tetrisSpinData.MINOS.Length; order++)
    //     {
    //         //ゴーストミノの名前にactiveMinoの名前が含まれるとき
    //         if (gameStatus.ActiveMino.name.Contains(tetrisSpinData.MINOS[order].name))
    //         {
    //             //breakでこのfor文を抜けて、orderの値を保存する
    //             break;
    //         }
    //     }
    //     return order;
    // }



    // //activeMinoから他のミノ、または底までの距離を計算する関数
    // public void CheckDistance_Y()
    // {
    //     //activeMinoの各座標を格納する変数を宣言
    //     int activeMino_x = Mathf.RoundToInt(gameStatus.ActiveMino.transform.position.x);
    //     int activeMino_y = Mathf.RoundToInt(gameStatus.ActiveMino.transform.position.y);
    //     int activeMino_z = Mathf.RoundToInt(gameStatus.ActiveMino.transform.position.z);

    //     //ゲームフィールドの高さのマスの数 + 2　回繰り返す
    //     for (gameStatus.Distance = 0; gameStatus.Distance < tetrisSpinData.HEIGHT - tetrisSpinData.HEADER + 2; gameStatus.Distance++)
    //     {
    //         //activeMinoのY座標をdistanceの値だけ下に移動する
    //         gameStatus.ActiveMino.transform.position = new Vector3
    //             (activeMino_x, activeMino_y - gameStatus.Distance, activeMino_z);

    //         //activeMinoが他のミノにぶつかる、またはゲームフィールドからはみ出した時
    //         if (!board.CheckPosition(gameStatus.ActiveMino)) //activeMinoから底までの距離をGhostBlockPositionに格納
    //         {
    //             //activeMinoの位置を元に戻す
    //             gameStatus.ActiveMino.transform.position = new Vector3
    //                 (activeMino_x, activeMino_y, activeMino_z);

    //             //この段階でdistanceから1引いた値が、activeMinoから底までの距離となる
    //             gameStatus.Distance--;

    //             //breakでこのfor文を抜けて、distanceの値を保存する
    //             break;
    //         }

    //         //activeMinoの位置を元に戻す
    //         gameStatus.ActiveMino.transform.position = new Vector3
    //             (activeMino_x, activeMino_y, activeMino_z);
    //     }
    // }


    // //ゴーストミノの位置調整を行う関数
    // public Vector3 PositionAdjustmentGhostMino()
    // {
    //     //activeMinoから他のミノ、または底までの距離を取得
    //     CheckDistance_Y();

    //     //activeMinoの各座標を取得
    //     int activeMino_x = Mathf.RoundToInt(gameStatus.ActiveMino.transform.position.x);
    //     int activeMino_y = Mathf.RoundToInt(gameStatus.ActiveMino.transform.position.y);
    //     int activeMino_z = Mathf.RoundToInt(gameStatus.ActiveMino.transform.position.z);

    //     return new Vector3(activeMino_x, activeMino_y - gameStatus.Distance, activeMino_z);
    // }


    // //Iミノの軸を計算し、Vectoe3で返す関数
    // //Imino_xとImino_yはIミノのx, y座標
    // public Vector3 AxisCheck(int Imino_x, int Imino_y)
    // {
    //     Debug.Log("AxisCheck");
    //     //xとyのオフセットを宣言
    //     //zは関係ない
    //     float xOffset = 0.5f;
    //     float yOffset = 0.5f;

    //     //回転軸は現在位置から、x軸をxOffset動かし、y軸をyOffset動かした座標にある
    //     //xOffsetとyOffsetの正負は回転前の向きによって変化する

    //     //回転前の向きがnorthの時
    //     if (gameStatus.MinoAngleBefore == tetrisSpinData.NORTH)
    //     {
    //         return new Vector3(Imino_x + xOffset, Imino_y - yOffset, 0);
    //     }
    //     //回転前の向きがeastの時
    //     else if (gameStatus.MinoAngleBefore == tetrisSpinData.EAST)
    //     {
    //         return new Vector3(Imino_x - xOffset, Imino_y - yOffset, 0);
    //     }
    //     //回転前の向きがsouthの時
    //     else if (gameStatus.MinoAngleBefore == tetrisSpinData.SOUTH)
    //     {
    //         return new Vector3(Imino_x - xOffset, Imino_y + yOffset, 0);
    //     }
    //     //回転前の向きがwestの時
    //     //minoAngleBefore == west
    //     else
    //     {
    //         return new Vector3(Imino_x + xOffset, Imino_y + yOffset, 0);
    //     }
    // }

    // //回転後の角度(minoAnglefter)の調整
    // //Z軸で回転を行っているため、90°(east)と270°(west)はプログラム上 270, 90 と表記されているため(左右反転している)
    // public void CalibrateMinoAngleAfter()
    // {
    //     //block.transform.rotation.eulerAngles.zはZ軸の回転角度
    //     //eulerAnglesはオイラー角の意
    //     //調整前の角度
    //     int originalAngle = Mathf.RoundToInt(gameStatus.ActiveMino.transform.rotation.eulerAngles.z);

    //     if (originalAngle == tetrisSpinData.WEST)
    //     {
    //         gameStatus.MinoAngleAfter = tetrisSpinData.EAST;
    //     }
    //     else if (originalAngle == tetrisSpinData.EAST)
    //     {
    //         gameStatus.MinoAngleAfter = tetrisSpinData.WEST;
    //     }
    //     else
    //     {
    //         //修正の必要なし
    //         gameStatus.MinoAngleAfter = originalAngle;
    //     }
    // }

    // //通常回転のリセットをする関数
    // public void RotateReset()
    // {
    //     //通常回転が右回転だった時
    //     if ((gameStatus.MinoAngleBefore == tetrisSpinData.NORTH && gameStatus.MinoAngleAfter == tetrisSpinData.EAST) ||
    //     (gameStatus.MinoAngleBefore == tetrisSpinData.EAST && gameStatus.MinoAngleAfter == tetrisSpinData.SOUTH) ||
    //     (gameStatus.MinoAngleBefore == tetrisSpinData.SOUTH && gameStatus.MinoAngleAfter == tetrisSpinData.WEST) ||
    //     (gameStatus.MinoAngleBefore == tetrisSpinData.WEST && gameStatus.MinoAngleAfter == tetrisSpinData.NORTH))
    //     {
    //         //左回転で回転前の状態に戻す
    //         mino.Rotateleft(gameStatus.ActiveMino);
    //     }
    //     //通常回転が左回転だった時
    //     else
    //     {
    //         //右回転で回転前の状態に戻す
    //         mino.RotateRight(gameStatus.ActiveMino);
    //     }
    // }


    // //Hold機能の処理をする関数
    // public void Hold()
    // {
    //     //1回目のHold
    //     if (gameStatus.FirstHold == true)
    //     {
    //         //activeMinoを削除
    //         Destroy(gameStatus.ActiveMino.gameObject);

    //         //holdMinoCountに、activeMinoの種類の数値情報を格納
    //         //例: activeMinoが T_Mino の時、holdMinoCount = 5
    //         gameStatus.HoldMinoNumber = gameStatus.SpawnMinoOrder_List[gameStatus.MinoPopNumber];

    //         //Holdされたミノを表示
    //         gameStatus.HoldMino = spawner.SpawnHoldMino();

    //         //1回目のHoldでは、新しく生成されるミノはNext1のミノになるので、
    //         //countを1つ進める
    //         //count++;

    //         //count進行
    //         gameStatus.MinoPopNumber++;

    //         //countが7の倍数の時
    //         if (gameStatus.MinoPopNumber % 7 == 0)
    //         {
    //             //ミノの配列の補充
    //             DetermineSpawnMinoOrder();

    //             //次のActiveMinoの生成
    //             gameStatus.ActiveMino = spawner.SpawnMino(gameStatus.SpawnMinoOrder_List[gameStatus.MinoPopNumber]);
    //         }
    //         else
    //         {
    //             //次のActiveMinoの生成
    //             gameStatus.ActiveMino = spawner.SpawnMino(gameStatus.SpawnMinoOrder_List[gameStatus.MinoPopNumber]);
    //         }

    //         //activeMinoの種類を判別
    //         CheckActiveMinoShape();

    //         //変数の初期化
    //         gameStatus.AngleReset();
    //         gameStatus.SpinResetFlag();

    //         //Nextの表示
    //         spawner.SpawnNextMinos();

    //         gameStatus.FirstHold = false;
    //     }
    //     //2回目のHold
    //     else
    //     {
    //         //activeMinoを削除
    //         Destroy(gameStatus.ActiveMino.gameObject);

    //         //ホールドされていたミノをActiveMinoに戻す
    //         gameStatus.ActiveMino = spawner.SpawnMino(gameStatus.HoldMinoNumber);

    //         //activeMinoの種類を判別
    //         CheckActiveMinoShape();

    //         //1つ上のコードでactiveMinoが変化しているため、
    //         //holdMinoCountに、 "以前" のactiveMinoの種類の数値情報を格納
    //         gameStatus.HoldMinoNumber = gameStatus.SpawnMinoOrder_List[gameStatus.MinoPopNumber];

    //         //以前のホールドミノを削除
    //         Destroy(gameStatus.HoldMino.gameObject);

    //         //新しくHoldされたミノを表示
    //         gameStatus.HoldMino = spawner.SpawnHoldMino();

    //         //変数の初期化
    //         gameStatus.AngleReset();
    //         gameStatus.SpinResetFlag();
    //     }
    // }
}
