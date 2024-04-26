using System.Collections.Generic;
using UnityEngine;

// //ミノの回転に関するスクリプト
// //ミノの特殊回転を可能にする、スーパーローテーションシステムの関数や、
// //各ミノのSpin判定をチェックする関数がある

// //↓スーパーローテーションシステムについて解説されているサイト
// //https://tetrisch.github.io/main/srs.html

public class Rotation : MonoBehaviour
{
    //各種干渉するスクリプトの設定
    Board board;
    Calculate calculate;
    GameStatus gameStatus;
    //GameManager gameManager;
    //TetrisSpinData tetrisSpinData;
    Mino mino;

    //インスタンス化
    private void Awake()
    {
        board = FindObjectOfType<Board>();
        calculate = FindObjectOfType<Calculate>();
        //gameManager = FindObjectOfType<GameManager>();
        gameStatus = FindObjectOfType<GameStatus>();
        //tetrisSpinData = FindObjectOfType<TetrisSpinData>();
        mino = FindObjectOfType<Mino>();
    }

    //     //スーパーローテーションシステム(SRS)
    //     //通常回転ができなかった時に試す回転
    //     //4つの軌跡を辿り、ブロックや壁に衝突しなかったらそこに移動する

    //     //↓参考にした動画
    //     //https://www.youtube.com/watch?v=0OQ7mP97vdc

    //     public bool MinoSuperRotation(Mino _ActiveMino)
    //     {
    //         //初期(未回転)状態をtetrisSpinData.NORTHとして、
    //         //右回転後の向きをtetrisSpinData.EAST
    //         //左回転後の向きをtetrisSpinData.WEST
    //         //2回右回転または左回転した時の向きをtetrisSpinData.SOUTHとする
    //         // int tetrisSpinData.NORTH = gameStatus.tetrisSpinData.NORTH;
    //         // int tetrisSpinData.EAST = gameStatus.tetrisSpinData.EAST;
    //         // int tetrisSpinData.SOUTH = gameStatus.tetrisSpinData.SOUTH;
    //         // int tetrisSpinData.WEST = gameStatus.tetrisSpinData.WEST;

    //         //回転後の角度(MinoAngleAfter)の調整
    //         calculate.CalibrateMinoAngleAfter();

    //         //SRSはIミノとそれ以外のミノとで処理が違うため分けて処理する
    //         //Iミノ以外のSRS
    //         if (!_ActiveMino.name.Contains("I"))
    //         {
    //             //Debug.Log("Iミノ以外のSRS");

    //             //NORTHからtetrisSpinData.EAST
    //             //tetrisSpinData.SOUTHからtetrisSpinData.EASTに回転する時
    //             if ((gameStatus.MinoAngleBefore == tetrisSpinData.NORTH && gameStatus.MinoAngleAfter == tetrisSpinData.EAST) ||
    //                 (gameStatus.MinoAngleBefore == tetrisSpinData.SOUTH && gameStatus.MinoAngleAfter == tetrisSpinData.EAST))
    //             {
    //                 //Debug.Log("tetrisSpinData.NORTHからtetrisSpinData.EASTまたは、tetrisSpinData.SOUTHからtetrisSpinData.EASTに回転する時");

    //                 //第一法則
    //                 //左に1つ移動
    //                 _ActiveMino.MoveLeft();

    //                 //LastSRSには、SRSが成功した際の軌跡の段階を格納(TspinMiniの判定に必要)
    //                 //LastSRSは1
    //                 gameStatus.LastSRS++;

    //                 if (!board.CheckPosition(_ActiveMino))
    //                 {
    //                     //第二法則
    //                     //上に1つ移動
    //                     _ActiveMino.MoveUp();

    //                     //LastSRSは2
    //                     gameStatus.LastSRS++;

    //                     if (!board.CheckPosition(_ActiveMino))
    //                     {
    //                         //第三法則
    //                         //右に1つ移動、下に3つ移動
    //                         _ActiveMino.MoveRight();
    //                         _ActiveMino.MoveDown();
    //                         _ActiveMino.MoveDown();
    //                         _ActiveMino.MoveDown();

    //                         //LastSRSは3
    //                         gameStatus.LastSRS++;

    //                         if (!board.CheckPosition(_ActiveMino))
    //                         {
    //                             //第四法則
    //                             //左に1つ移動
    //                             _ActiveMino.MoveLeft();

    //                             //LastSRSは4
    //                             gameStatus.LastSRS++;

    //                             if (!board.CheckPosition(_ActiveMino))
    //                             {
    //                                 //SRSができなかった際に回転前の状態に戻る
    //                                 _ActiveMino.MoveRight();
    //                                 _ActiveMino.MoveUp();
    //                                 _ActiveMino.MoveUp();

    //                                 //通常回転のリセット
    //                                 calculate.RotateReset();

    //                                 //SRSができなかった時、falseを返す
    //                                 return false;
    //                             }
    //                         }
    //                     }
    //                 }
    //             }
    //             //tetrisSpinData.WESTからtetrisSpinData.NORTH
    //             //tetrisSpinData.WESTからtetrisSpinData.SOUTHに回転する時
    //             else if ((gameStatus.MinoAngleBefore == tetrisSpinData.WEST && gameStatus.MinoAngleAfter == tetrisSpinData.NORTH) ||
    //                 (gameStatus.MinoAngleBefore == tetrisSpinData.WEST && gameStatus.MinoAngleAfter == tetrisSpinData.SOUTH))
    //             {
    //                 //Debug.Log("tetrisSpinData.WESTからtetrisSpinData.NORTHまたは、tetrisSpinData.WESTからtetrisSpinData.SOUTHに回転する時");

    //                 //第一法則
    //                 //左に1つ移動
    //                 _ActiveMino.MoveLeft();

    //                 //LastSRSには、SRSが成功した際の軌跡の段階を格納(TspinMiniの判定に必要)
    //                 //LastSRSは1
    //                 gameStatus.LastSRS++;

    //                 if (!board.CheckPosition(_ActiveMino))
    //                 {
    //                     //第二法則
    //                     //下に1つ移動
    //                     _ActiveMino.MoveDown();

    //                     //LastSRSは2
    //                     gameStatus.LastSRS++;

    //                     if (!board.CheckPosition(_ActiveMino))
    //                     {
    //                         //第三法則
    //                         //右に1つ移動、上に3つ移動
    //                         _ActiveMino.MoveRight();
    //                         _ActiveMino.MoveUp();
    //                         _ActiveMino.MoveUp();
    //                         _ActiveMino.MoveUp();

    //                         //LastSRSは3
    //                         gameStatus.LastSRS++;

    //                         if (!board.CheckPosition(_ActiveMino))
    //                         {
    //                             //第四法則
    //                             //左に1つ移動
    //                             _ActiveMino.MoveLeft();

    //                             //LastSRSは4
    //                             gameStatus.LastSRS++;

    //                             if (!board.CheckPosition(_ActiveMino))
    //                             {
    //                                 //SRSができなかった際に回転前の状態に戻る
    //                                 _ActiveMino.MoveRight();
    //                                 _ActiveMino.MoveUp();
    //                                 _ActiveMino.MoveUp();

    //                                 //通常回転のリセット
    //                                 calculate.RotateReset();

    //                                 //SRSができなかった時、falseを返す
    //                                 return false;
    //                             }
    //                         }
    //                     }
    //                 }
    //             }
    //             //tetrisSpinData.EASTからtetrisSpinData.NORTH
    //             //tetrisSpinData.EASTからtetrisSpinData.SOUTHに回転する時
    //             else if ((gameStatus.MinoAngleBefore == tetrisSpinData.EAST && gameStatus.MinoAngleAfter == tetrisSpinData.NORTH) ||
    //                 (gameStatus.MinoAngleBefore == tetrisSpinData.EAST && gameStatus.MinoAngleAfter == tetrisSpinData.SOUTH))
    //             {
    //                 //Debug.Log("tetrisSpinData.EASTからtetrisSpinData.NORTHまたは、tetrisSpinData.EASTからtetrisSpinData.SOUTHに回転する時");

    //                 //第一法則
    //                 //右に1つ移動
    //                 _ActiveMino.MoveRight();

    //                 //LastSRSには、SRSが成功した際の軌跡の段階を格納(TspinMiniの判定に必要)
    //                 //LastSRSは1
    //                 gameStatus.LastSRS++;

    //                 if (!board.CheckPosition(_ActiveMino))
    //                 {
    //                     //第二法則
    //                     //下に1つ移動
    //                     _ActiveMino.MoveDown();

    //                     //LastSRSは2
    //                     gameStatus.LastSRS++;

    //                     if (!board.CheckPosition(_ActiveMino))
    //                     {
    //                         //第三法則
    //                         //左に1つ移動、上に3つ移動
    //                         _ActiveMino.MoveLeft();
    //                         _ActiveMino.MoveUp();
    //                         _ActiveMino.MoveUp();
    //                         _ActiveMino.MoveUp();

    //                         //LastSRSは3
    //                         gameStatus.LastSRS++;

    //                         if (!board.CheckPosition(_ActiveMino))
    //                         {
    //                             //第四法則
    //                             //右に1つ移動
    //                             _ActiveMino.MoveRight();

    //                             //LastSRSは4
    //                             gameStatus.LastSRS++;

    //                             if (!board.CheckPosition(_ActiveMino))
    //                             {
    //                                 //SRSができなかった際に回転前の状態に戻る
    //                                 _ActiveMino.MoveLeft();
    //                                 _ActiveMino.MoveDown();
    //                                 _ActiveMino.MoveDown();

    //                                 //通常回転のリセット
    //                                 calculate.RotateReset();

    //                                 //SRSができなかった時、falseを返す
    //                                 return false;
    //                             }
    //                         }
    //                     }
    //                 }
    //             }
    //             //tetrisSpinData.NORTHからtetrisSpinData.WEST
    //             //tetrisSpinData.SOUTHからtetrisSpinData.WESTに回転する時
    //             else if ((gameStatus.MinoAngleBefore == tetrisSpinData.NORTH && gameStatus.MinoAngleAfter == tetrisSpinData.WEST) ||
    //                 (gameStatus.MinoAngleBefore == tetrisSpinData.SOUTH && gameStatus.MinoAngleAfter == tetrisSpinData.WEST))
    //             {
    //                 //Debug.Log("tetrisSpinData.NORTHからtetrisSpinData.WESTまたはtetrisSpinData.SOUTHからtetrisSpinData.WESTに回転する時");

    //                 //第一法則
    //                 //右に1つ移動
    //                 _ActiveMino.MoveRight();

    //                 //LastSRSには、SRSが成功した際の軌跡の段階を格納(TspinMiniの判定に必要)
    //                 //LastSRSは1
    //                 gameStatus.LastSRS++;

    //                 if (!board.CheckPosition(_ActiveMino))
    //                 {
    //                     //第二法則
    //                     //上に1つ移動
    //                     _ActiveMino.MoveUp();

    //                     //LastSRSは2
    //                     gameStatus.LastSRS++;

    //                     if (!board.CheckPosition(_ActiveMino))
    //                     {
    //                         //第三法則
    //                         //左に1つ移動、下に3つ移動
    //                         _ActiveMino.MoveLeft();
    //                         _ActiveMino.MoveDown();
    //                         _ActiveMino.MoveDown();
    //                         _ActiveMino.MoveDown();

    //                         //LastSRSは3
    //                         gameStatus.LastSRS++;

    //                         if (!board.CheckPosition(_ActiveMino))
    //                         {
    //                             //第四法則
    //                             //右に1つ移動
    //                             _ActiveMino.MoveRight();

    //                             //LastSRSは4
    //                             gameStatus.LastSRS++;

    //                             if (!board.CheckPosition(_ActiveMino))
    //                             {
    //                                 //SRSができなかった時、通常回転前の状態に戻る
    //                                 //SRSのリセット
    //                                 _ActiveMino.MoveLeft();
    //                                 _ActiveMino.MoveUp();
    //                                 _ActiveMino.MoveUp();

    //                                 //通常回転のリセット
    //                                 calculate.RotateReset();

    //                                 //SRSができなかった時、falseを返す
    //                                 return false;
    //                             }
    //                         }
    //                     }
    //                 }
    //             }
    //         }
    //         //IミノのSRS(かなり複雑)
    //         //Iミノは軸が他のミノと違うため別の処理
    //         //IミノはLastSRSを扱わない
    //         else
    //         {
    //             //Debug.Log("IミノのSRS");

    //             //tetrisSpinData.NORTHからtetrisSpinData.EAST
    //             //tetrisSpinData.WESTからtetrisSpinData.SOUTHに回転する時
    //             if ((gameStatus.MinoAngleBefore == tetrisSpinData.NORTH && gameStatus.MinoAngleAfter == tetrisSpinData.EAST) ||
    //                 (gameStatus.MinoAngleBefore == tetrisSpinData.NORTH && gameStatus.MinoAngleAfter == tetrisSpinData.SOUTH))
    //             {
    //                 //Debug.Log("tetrisSpinData.NORTHからtetrisSpinData.EASTまたはtetrisSpinData.WESTからtetrisSpinData.SOUTHに回転する時");

    //                 //第一法則
    //                 //左に2つ移動
    //                 _ActiveMino.MoveLeft();
    //                 _ActiveMino.MoveLeft();

    //                 if (!board.CheckPosition(_ActiveMino))
    //                 {
    //                     //第二法則
    //                     //右に3つ移動
    //                     _ActiveMino.MoveRight();
    //                     _ActiveMino.MoveRight();
    //                     _ActiveMino.MoveRight();

    //                     if (!board.CheckPosition(_ActiveMino))
    //                     {
    //                         //第三法則
    //                         //左に3つ移動、下に1つ移動
    //                         _ActiveMino.MoveLeft();
    //                         _ActiveMino.MoveLeft();
    //                         _ActiveMino.MoveLeft();
    //                         _ActiveMino.MoveDown();

    //                         if (!board.CheckPosition(_ActiveMino))
    //                         {
    //                             //第四法則
    //                             //右に3つ移動、上に3つ移動
    //                             _ActiveMino.MoveRight();
    //                             _ActiveMino.MoveRight();
    //                             _ActiveMino.MoveRight();
    //                             _ActiveMino.MoveUp();
    //                             _ActiveMino.MoveUp();
    //                             _ActiveMino.MoveUp();

    //                             if (!board.CheckPosition(_ActiveMino))
    //                             {
    //                                 //SRSができなかった際に回転前の状態に戻る
    //                                 _ActiveMino.MoveLeft();
    //                                 _ActiveMino.MoveDown();
    //                                 _ActiveMino.MoveDown();

    //                                 //通常回転のリセット
    //                                 calculate.RotateReset();

    //                                 //SRSができなかった時、falseを返す
    //                                 return false;
    //                             }
    //                         }
    //                     }
    //                 }
    //             }
    //             //tetrisSpinData.WESTからtetrisSpinData.NORTH
    //             //tetrisSpinData.SOUTHからtetrisSpinData.EASTに回転する時
    //             else if ((gameStatus.MinoAngleBefore == tetrisSpinData.WEST && gameStatus.MinoAngleAfter == tetrisSpinData.NORTH) ||
    //                 (gameStatus.MinoAngleBefore == tetrisSpinData.SOUTH && _ActiveMino.transform.rotation.eulerAngles.z == tetrisSpinData.EAST))
    //             {
    //                 //Debug.Log("tetrisSpinData.WESTからtetrisSpinData.NORTHまたはtetrisSpinData.SOUTHからtetrisSpinData.EASTに回転する時");

    //                 //第一法則
    //                 //右に1つ移動
    //                 _ActiveMino.MoveRight();

    //                 if (!board.CheckPosition(_ActiveMino))
    //                 {
    //                     //第二法則
    //                     //左に3つ移動
    //                     _ActiveMino.MoveLeft();
    //                     _ActiveMino.MoveLeft();
    //                     _ActiveMino.MoveLeft();

    //                     if (!board.CheckPosition(_ActiveMino))
    //                     {
    //                         //第三法則
    //                         //右に3つ移動、下に2つ移動
    //                         _ActiveMino.MoveRight();
    //                         _ActiveMino.MoveRight();
    //                         _ActiveMino.MoveRight();
    //                         _ActiveMino.MoveDown();
    //                         _ActiveMino.MoveDown();

    //                         if (!board.CheckPosition(_ActiveMino))
    //                         {
    //                             //第四法則
    //                             //左に3つ移動、上に3つ移動
    //                             _ActiveMino.MoveLeft();
    //                             _ActiveMino.MoveLeft();
    //                             _ActiveMino.MoveLeft();
    //                             _ActiveMino.MoveUp();
    //                             _ActiveMino.MoveUp();
    //                             _ActiveMino.MoveUp();

    //                             if (!board.CheckPosition(_ActiveMino))
    //                             {
    //                                 //SRSができなかった際に回転前の状態に戻る
    //                                 _ActiveMino.MoveRight();
    //                                 _ActiveMino.MoveRight();
    //                                 _ActiveMino.MoveDown();

    //                                 //通常回転のリセット
    //                                 calculate.RotateReset();

    //                                 //SRSができなかった時、falseを返す
    //                                 return false;
    //                             }
    //                         }
    //                     }
    //                 }
    //             }
    //             //tetrisSpinData.EASTからtetrisSpinData.NORTHまたはtetrisSpinData.SOUTHからtetrisSpinData.WESTに回転する時
    //             else if ((gameStatus.MinoAngleBefore == tetrisSpinData.EAST && gameStatus.MinoAngleAfter == tetrisSpinData.NORTH) ||
    //                 (gameStatus.MinoAngleBefore == tetrisSpinData.SOUTH && gameStatus.MinoAngleAfter == tetrisSpinData.WEST))
    //             {
    //                 //Debug.Log("tetrisSpinData.EASTからtetrisSpinData.NORTHまたはtetrisSpinData.SOUTHからtetrisSpinData.WESTに回転する時");

    //                 //第一法則
    //                 //右に2つ移動
    //                 _ActiveMino.MoveRight();
    //                 _ActiveMino.MoveRight();

    //                 if (!board.CheckPosition(_ActiveMino))
    //                 {
    //                     //第二法則
    //                     //左に3つ移動
    //                     _ActiveMino.MoveLeft();
    //                     _ActiveMino.MoveLeft();
    //                     _ActiveMino.MoveLeft();

    //                     if (!board.CheckPosition(_ActiveMino))
    //                     {
    //                         //第三法則
    //                         //右に3つ移動、上に1つ移動
    //                         _ActiveMino.MoveRight();
    //                         _ActiveMino.MoveRight();
    //                         _ActiveMino.MoveRight();
    //                         _ActiveMino.MoveUp();

    //                         if (!board.CheckPosition(_ActiveMino))
    //                         {
    //                             //第四法則
    //                             //左に3つ移動、下に3つ移動
    //                             _ActiveMino.MoveLeft();
    //                             _ActiveMino.MoveLeft();
    //                             _ActiveMino.MoveLeft();
    //                             _ActiveMino.MoveDown();
    //                             _ActiveMino.MoveDown();
    //                             _ActiveMino.MoveDown();

    //                             if (!board.CheckPosition(_ActiveMino))
    //                             {
    //                                 //SRSができなかった際に回転前の状態に戻る
    //                                 _ActiveMino.MoveRight();
    //                                 _ActiveMino.MoveUp();
    //                                 _ActiveMino.MoveUp();

    //                                 //通常回転のリセット
    //                                 calculate.RotateReset();

    //                                 //SRSができなかった時、falseを返す
    //                                 return false;
    //                             }
    //                         }
    //                     }
    //                 }
    //             }
    //             //tetrisSpinData.NORTHからtetrisSpinData.WESTまたはtetrisSpinData.EASTからtetrisSpinData.SOUTHに回転する時
    //             else if ((gameStatus.MinoAngleBefore == tetrisSpinData.NORTH && gameStatus.MinoAngleAfter == tetrisSpinData.WEST) ||
    //                 (gameStatus.MinoAngleBefore == tetrisSpinData.EAST && gameStatus.MinoAngleAfter == tetrisSpinData.SOUTH))
    //             {
    //                 //Debug.Log("tetrisSpinData.NORTHからtetrisSpinData.WESTまたはtetrisSpinData.EASTからtetrisSpinData.SOUTHに回転する時");

    //                 //第一法則
    //                 //左に1つ移動
    //                 _ActiveMino.MoveLeft();

    //                 if (!board.CheckPosition(_ActiveMino))
    //                 {
    //                     //第二法則
    //                     //右に3つ移動
    //                     _ActiveMino.MoveRight();
    //                     _ActiveMino.MoveRight();
    //                     _ActiveMino.MoveRight();

    //                     if (!board.CheckPosition(_ActiveMino))
    //                     {
    //                         //第三法則
    //                         //左に3つ移動、上に2つ移動
    //                         _ActiveMino.MoveLeft();
    //                         _ActiveMino.MoveLeft();
    //                         _ActiveMino.MoveLeft();
    //                         _ActiveMino.MoveUp();
    //                         _ActiveMino.MoveUp();

    //                         if (!board.CheckPosition(_ActiveMino))
    //                         {
    //                             //第四法則
    //                             //右に3つ移動、下に3つ移動
    //                             _ActiveMino.MoveRight();
    //                             _ActiveMino.MoveRight();
    //                             _ActiveMino.MoveRight();
    //                             _ActiveMino.MoveDown();
    //                             _ActiveMino.MoveDown();
    //                             _ActiveMino.MoveDown();

    //                             if (!board.CheckPosition(_ActiveMino))
    //                             {
    //                                 //SRSができなかった時、回転前の状態に戻る
    //                                 _ActiveMino.MoveLeft();
    //                                 _ActiveMino.MoveLeft();
    //                                 _ActiveMino.MoveUp();

    //                                 //通常回転のリセット
    //                                 calculate.RotateReset();

    //                                 //SRSができなかった時、falseを返す
    //                                 return false;
    //                             }
    //                         }
    //                     }
    //                 }
    //             }
    //         }
    //         //SRSができた時、trueを返す
    //         return true;
    //     }

    //     //SRS時に重複しないか判定する関数
    //     //Rx, Ry はそれぞれX, Y座標のオフセット 
    //     bool RotationCheck(int Rx, int Ry, Mino mino)
    //     {
    //         //minoを構成する4つのブロックをそれぞれ調べる
    //         foreach (Transform item in mino.transform)
    //         {
    //             //ブロックのX, Y座標をposに格納
    //             Vector2 pos = Rounding.Round(item.position);

    //             //オフセット分移動した時、Gridの座標が負ならfalse
    //             if ((int)pos.x + Rx < 0 || (int)pos.y + Ry < 0)
    //             {
    //                 Debug.Log("SRS移動先のGrid座標が負");
    //                 return false;
    //             }

    //             //オフセット分移動した時、ほかのミノと重なったらfalse
    //             if (board.Grid[(int)pos.x + Rx, (int)pos.y + Ry] != null
    //                 && board.Grid[(int)pos.x + Rx, (int)pos.y + Ry].parent != mino.transform)
    //             {
    //                 Debug.Log("SRS先が重複");
    //                 return false;
    //             }

    //             //オフセット分移動した時、ゲームフィールド外に移動していたならfalse
    //             if (!board.BoardOutCheck((int)pos.x + Rx, (int)pos.y + Ry))
    //             {
    //                 Debug.Log("SRS先がゲームフィールド外");
    //                 return false;
    //             }
    //         }
    //         return true;
    //     }

    // //Tspinの判定をする関数
    // //TspinにはMiniがあるので、それも判定する
    // public int TspinCheck()
    // {
    //     Debug.Log("====this is TspinCheck====");

    //     List<int> Around_ActiveMinosCheck_ForT = new List<int>();

    //     if (Around_ActiveMinosCheck_ForT.Count != 0)
    //     {
    //         for (int i = 0; i < 4; i++)
    //         {
    //             Around_ActiveMinosCheck_ForT.RemoveAt(0);
    //         }
    //     }

    //     //Tミノの中心から順番に[1, 1]、[1, -1]、[-1, 1]、[-1, -1]の分だけ移動した座標にブロックや壁がないか確認する関数
    //     //ブロックや壁があった時は1、ない時は0で_ActiveMinoCountのリストに追加されていく
    //     //例:Around_ActiveMinosCheck_ForT[1, 1, 0, 1]
    //     for (int x = 1; x >= -1; x -= 2)
    //     {
    //         for (int y = 1; y >= -1; y -= 2)
    //         {
    //             if (board.ActiveMinoCheckForTspin((int)Mathf.Round(mino.ActiveMino.transform.position.x) + x, (int)Mathf.Round(mino.ActiveMino.transform.position.y) + y, mino.ActiveMino))
    //             {
    //                 Around_ActiveMinosCheck_ForT.Add(1);
    //             }
    //             else
    //             {
    //                 Around_ActiveMinosCheck_ForT.Add(0);
    //             }
    //         }
    //     }

    //     if (gameStatus.LastSRS != 4)
    //     {
    //         if (Around_ActiveMinosCheck_ForT.FindAll(x => x == 1).Count == 3)
    //         {
    //             switch (gameStatus.ActiveMino.transform.rotation.eulerAngles.z)
    //             {
    //                 case 270:
    //                     if (Around_ActiveMinosCheck_ForT[0] == 0 || Around_ActiveMinosCheck_ForT[1] == 0)
    //                     {
    //                         Debug.Log("270でMini");
    //                         gameStatus.UseSpinMini = true;
    //                         return 4;
    //                     }
    //                     break;
    //                 case 180:
    //                     if (Around_ActiveMinosCheck_ForT[1] == 0 || Around_ActiveMinosCheck_ForT[3] == 0)
    //                     {
    //                         Debug.Log("180でMini");
    //                         gameStatus.UseSpinMini = true;
    //                         return 4;
    //                     }
    //                     break;
    //                 case 90:
    //                     if (Around_ActiveMinosCheck_ForT[2] == 0 || Around_ActiveMinosCheck_ForT[3] == 0)
    //                     {
    //                         Debug.Log("90でMini");
    //                         gameStatus.UseSpinMini = true;
    //                         return 4;
    //                     }
    //                     break;
    //                 case 0:
    //                     if (Around_ActiveMinosCheck_ForT[0] == 0 || Around_ActiveMinosCheck_ForT[2] == 0)
    //                     {
    //                         Debug.Log("0でMini");
    //                         gameStatus.UseSpinMini = true;
    //                         return 4;
    //                     }
    //                     break;
    //             }
    //             Debug.Log("Tspin");
    //             return 4;
    //         }
    //     }

    //     if (Around_ActiveMinosCheck_ForT.FindAll(x => x == 1).Count == 4)
    //     {
    //         return 4;
    //     }
    //     return 7;
    // }

    // //各ミノのスピン判定をチェックするターミナル
    // //この関数は、回転時とミノの設置時に呼び出される
    // public int SpinTerminal(Mino _ActiveMino)
    // {
    //     Debug.Log("SpinTerminal");
    //     Debug.Log(_ActiveMino);
    //     //ミノの識別()
    //     // int I_mino = 0;
    //     // int J_mino = 1;
    //     // int L_mino = 2;
    //     // int O_mino = 3;
    //     // int S_mino = 4;
    //     //int T_mino = 5;
    //     // int Z_mino = 6;

    //     //最後の動作がスピンでないならSpin判定はなし
    //     if (gameStatus.UseSpin == false)
    //     {
    //         return 7;
    //     }
    //     /*else if (mino == I_mino)
    //     {
    //         return IspinCheck(_ActiveMino);
    //     }*/
    //     /*else if (mino == J_mino)
    //     {
    //         return JspinCheck(_ActiveMino);
    //     }*/
    //     /*else if (mino == L_mino)
    //     {
    //         return LspinCheck(_ActiveMino);
    //     }*/
    //     /*else if (mino == O_mino)
    //     {
    //         return OspinCheck(_ActiveMino);
    //     }*/
    //     /*else if (mino == S_mino)
    //     {
    //         return SspinCheck(_ActiveMino);
    //     }*/
    //     else if (_ActiveMino.name.Contains("T_Mino"))
    //     {
    //         return TspinCheck();
    //     }
    //     /*else if (mino == Z_mino)
    //     {
    //         return ZspinCheck(_ActiveMino);
    //     }*/
    //     return 7;
    // }

}
