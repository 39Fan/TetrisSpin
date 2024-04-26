using System.Collections.Generic;
using System.Collections;
using UnityEngine;

///// ミノに関するスクリプト /////


// ↓このスクリプトで可能なこと↓ //

// ミノの生成
// 左右移動、下移動、通常回転
// スーパーローテーションシステム(以後SRSと呼ぶ)
// ゴーストミノの位置調整

public class Mino : MonoBehaviour
{
    // 回転していいミノかどうか //
    [SerializeField] private bool CanRotate = true; // Oミノは回転しないので、エディターでfalseに設定

    // Z軸の回転量 //
    private int RotateRightAroundZ = -90; // 右回転
    private int RotateLeftAroundZ = 90; // 左回転

    // 回転方向 //
    private string UseRotateRight = "RotateRight";
    private string UseRotateLeft = "RotateLeft";

    Board board;
    GameStatus gameStatus;
    Spawner spawner;

    private void Awake()
    {
        board = FindObjectOfType<Board>();
        gameStatus = FindObjectOfType<GameStatus>();
        spawner = FindObjectOfType<Spawner>();
    }

    // 移動用 //
    public void Move(Vector3 _MoveDirection)
    {
        transform.position += _MoveDirection;
    }

    // 移動関数を呼ぶ関数(4種類) //
    public void MoveLeft()
    {
        Move(new Vector3(-1, 0, 0));
    }

    public void MoveRight()
    {
        Move(new Vector3(1, 0, 0));
    }

    public void MoveUp()
    {
        Move(new Vector3(0, 1, 0));
    }

    public void MoveDown()
    {
        Move(new Vector3(0, -1, 0));
    }

    // 通常右回転 //
    public void RotateRight()
    {
        // 回転できるブロックかどうか
        if (CanRotate == false)
        {
            return; // Oミノは回転できないので弾かれる
        }

        if (spawner.activeMinoName != "I_Mino") // Iミノ以外の右回転
        {
            transform.Rotate(0, 0, RotateRightAroundZ);
            Debug.Log("右回転");
        }
        else // Iミノは軸が他のミノと違うため別の処理
        {
            // Iミノの軸を取得する
            Vector3 IminoAxis = AxisCheck
                (Mathf.RoundToInt(spawner.activeMino.transform.position.x), Mathf.RoundToInt(spawner.activeMino.transform.position.y));

            // IminoAxis を中心に右回転する
            transform.RotateAround(IminoAxis, Vector3.forward, RotateRightAroundZ);
        }

        // ミノの角度の調整(右回転)
        gameStatus.UpdateMinoAngleAfter(UseRotateRight);
    }

    // 通常左回転 //
    public void Rotateleft()
    {
        // 回転できるブロックかどうか
        if (CanRotate == false)
        {
            return; // Oミノは回転できないので弾かれる
        }

        // Iミノ以外の左回転
        if (!spawner.activeMino.name.Contains("I"))
        {
            transform.Rotate(0, 0, RotateLeftAroundZ);
        }

        // Iミノは軸が他のミノと違うため別の処理
        else
        {
            // Iミノの軸を取得する
            Vector3 IminoAxis = AxisCheck
                (Mathf.RoundToInt(spawner.activeMino.transform.position.x), Mathf.RoundToInt(spawner.activeMino.transform.position.y));

            // IminoAxisを中心に左回転する
            transform.RotateAround(IminoAxis, Vector3.forward, RotateLeftAroundZ);
        }

        // ミノの角度の調整(左回転)
        gameStatus.UpdateMinoAngleAfter(UseRotateLeft);
    }

    // スーパーローテーションシステム(SRS) //
    // 通常回転ができなかった時に試す回転
    // 4つの軌跡を辿り、ブロックや壁に衝突しなかったらそこに移動する

    // ↓参考にした動画
    // https://www.youtube.com/watch?v=0OQ7mP97vdc

    public bool SuperRotationSystem()
    {
        Debug.Log("わーい");
        //初期(未回転)状態をNorthとして、
        //右回転後の向きをEast
        //左回転後の向きをWest
        //2回右回転または左回転した時の向きをSouthとする
        // int North = North;
        // int East = East;
        // int South = South;
        // int West = West;

        //回転後の角度(gameStatus.minoAngleAfter)の調整
        //CalibrategameStatus.minoAngleAfter();

        // SRSはIミノとそれ以外のミノとで処理が違うため分けて処理する
        // Iミノ以外のSRS
        if (spawner.activeMinoName != "I_Mino")
        {
            //Debug.Log("Iミノ以外のSRS");

            if ((gameStatus.minoAngleBefore == gameStatus.North && gameStatus.minoAngleAfter == gameStatus.East) ||
                (gameStatus.minoAngleBefore == gameStatus.South && gameStatus.minoAngleAfter == gameStatus.East))   //North から East , South から East に回転する時
            {
                Debug.Log("North から East , South から East に回転する時");

                //第一法則
                //左に1つ移動
                spawner.activeMino.MoveLeft();

                //LastSRSには、SRSが成功した際の軌跡の段階を格納(TspinMiniの判定に必要)
                //LastSRSは1
                gameStatus.IncreaseLastSRS();

                if (!board.CheckPosition(spawner.activeMino))
                {
                    //第二法則
                    //上に1つ移動
                    spawner.activeMino.MoveUp();

                    //LastSRSは2
                    gameStatus.IncreaseLastSRS();

                    if (!board.CheckPosition(spawner.activeMino))
                    {
                        //第三法則
                        //右に1つ移動、下に3つ移動
                        spawner.activeMino.MoveRight();
                        spawner.activeMino.MoveDown();
                        spawner.activeMino.MoveDown();
                        spawner.activeMino.MoveDown();

                        //LastSRSは3
                        gameStatus.IncreaseLastSRS();

                        if (!board.CheckPosition(spawner.activeMino))
                        {
                            //第四法則
                            //左に1つ移動
                            spawner.activeMino.MoveLeft();

                            //LastSRSは4
                            gameStatus.IncreaseLastSRS();

                            if (!board.CheckPosition(spawner.activeMino))
                            {
                                //SRSができなかった際に回転前の状態に戻る
                                spawner.activeMino.MoveRight();
                                spawner.activeMino.MoveUp();
                                spawner.activeMino.MoveUp();

                                //通常回転のリセット
                                gameStatus.RotateReset();

                                //SRSができなかった時、falseを返す
                                return false;
                            }
                        }
                    }
                }
            }
            //WestからNorth
            //WestからSouthに回転する時
            else if ((gameStatus.minoAngleBefore == gameStatus.West && gameStatus.minoAngleAfter == gameStatus.North) ||
                (gameStatus.minoAngleBefore == gameStatus.West && gameStatus.minoAngleAfter == gameStatus.South))
            {
                Debug.Log("わーい");

                //第一法則
                //左に1つ移動
                spawner.activeMino.MoveLeft();

                //LastSRSには、SRSが成功した際の軌跡の段階を格納(TspinMiniの判定に必要)
                //LastSRSは1
                gameStatus.IncreaseLastSRS();

                if (!board.CheckPosition(spawner.activeMino))
                {
                    //第二法則
                    //下に1つ移動
                    spawner.activeMino.MoveDown();

                    //LastSRSは2
                    gameStatus.IncreaseLastSRS();

                    if (!board.CheckPosition(spawner.activeMino))
                    {
                        //第三法則
                        //右に1つ移動、上に3つ移動
                        spawner.activeMino.MoveRight();
                        spawner.activeMino.MoveUp();
                        spawner.activeMino.MoveUp();
                        spawner.activeMino.MoveUp();

                        //LastSRSは3
                        gameStatus.IncreaseLastSRS();

                        if (!board.CheckPosition(spawner.activeMino))
                        {
                            //第四法則
                            //左に1つ移動
                            spawner.activeMino.MoveLeft();

                            //LastSRSは4
                            gameStatus.IncreaseLastSRS();

                            if (!board.CheckPosition(spawner.activeMino))
                            {
                                //SRSができなかった際に回転前の状態に戻る
                                spawner.activeMino.MoveRight();
                                spawner.activeMino.MoveUp();
                                spawner.activeMino.MoveUp();

                                //通常回転のリセット
                                gameStatus.RotateReset();

                                //SRSができなかった時、falseを返す
                                return false;
                            }
                        }
                    }
                }
            }
            //EastからNorth
            //EastからSouthに回転する時
            else if ((gameStatus.minoAngleBefore == gameStatus.East && gameStatus.minoAngleAfter == gameStatus.North) ||
                (gameStatus.minoAngleBefore == gameStatus.East && gameStatus.minoAngleAfter == gameStatus.South))
            {
                Debug.Log("わーい");

                //第一法則
                //右に1つ移動
                spawner.activeMino.MoveRight();

                //LastSRSには、SRSが成功した際の軌跡の段階を格納(TspinMiniの判定に必要)
                //LastSRSは1
                gameStatus.IncreaseLastSRS();

                if (!board.CheckPosition(spawner.activeMino))
                {
                    //第二法則
                    //下に1つ移動
                    spawner.activeMino.MoveDown();

                    //LastSRSは2
                    gameStatus.IncreaseLastSRS();

                    if (!board.CheckPosition(spawner.activeMino))
                    {
                        //第三法則
                        //左に1つ移動、上に3つ移動
                        spawner.activeMino.MoveLeft();
                        spawner.activeMino.MoveUp();
                        spawner.activeMino.MoveUp();
                        spawner.activeMino.MoveUp();

                        //LastSRSは3
                        gameStatus.IncreaseLastSRS();

                        if (!board.CheckPosition(spawner.activeMino))
                        {
                            //第四法則
                            //右に1つ移動
                            spawner.activeMino.MoveRight();

                            //LastSRSは4
                            gameStatus.IncreaseLastSRS();

                            if (!board.CheckPosition(spawner.activeMino))
                            {
                                //SRSができなかった際に回転前の状態に戻る
                                spawner.activeMino.MoveLeft();
                                spawner.activeMino.MoveDown();
                                spawner.activeMino.MoveDown();

                                //通常回転のリセット
                                gameStatus.RotateReset();

                                //SRSができなかった時、falseを返す
                                return false;
                            }
                        }
                    }
                }
            }
            //NorthからWest
            //SouthからWestに回転する時
            else if ((gameStatus.minoAngleBefore == gameStatus.North && gameStatus.minoAngleAfter == gameStatus.West) ||
                (gameStatus.minoAngleBefore == gameStatus.South && gameStatus.minoAngleAfter == gameStatus.West))
            {
                Debug.Log("わーい");

                //第一法則
                //右に1つ移動
                spawner.activeMino.MoveRight();

                //LastSRSには、SRSが成功した際の軌跡の段階を格納(TspinMiniの判定に必要)
                //LastSRSは1
                gameStatus.IncreaseLastSRS();

                if (!board.CheckPosition(spawner.activeMino))
                {
                    //第二法則
                    //上に1つ移動
                    spawner.activeMino.MoveUp();

                    //LastSRSは2
                    gameStatus.IncreaseLastSRS();

                    if (!board.CheckPosition(spawner.activeMino))
                    {
                        //第三法則
                        //左に1つ移動、下に3つ移動
                        spawner.activeMino.MoveLeft();
                        spawner.activeMino.MoveDown();
                        spawner.activeMino.MoveDown();
                        spawner.activeMino.MoveDown();

                        //LastSRSは3
                        gameStatus.IncreaseLastSRS();

                        if (!board.CheckPosition(spawner.activeMino))
                        {
                            //第四法則
                            //右に1つ移動
                            spawner.activeMino.MoveRight();

                            //LastSRSは4
                            gameStatus.IncreaseLastSRS();

                            if (!board.CheckPosition(spawner.activeMino))
                            {
                                //SRSができなかった時、通常回転前の状態に戻る
                                //SRSのリセット
                                spawner.activeMino.MoveLeft();
                                spawner.activeMino.MoveUp();
                                spawner.activeMino.MoveUp();

                                //通常回転のリセット
                                gameStatus.RotateReset();

                                //SRSができなかった時、falseを返す
                                return false;
                            }
                        }
                    }
                }
            }
        }
        //IミノのSRS(かなり複雑)
        //Iミノは軸が他のミノと違うため別の処理
        //IミノはLastSRSを扱わない
        else
        {
            //Debug.Log("IミノのSRS");

            //NorthからEast
            //WestからSouthに回転する時
            if ((gameStatus.minoAngleBefore == gameStatus.North && gameStatus.minoAngleAfter == gameStatus.East) ||
                (gameStatus.minoAngleBefore == gameStatus.North && gameStatus.minoAngleAfter == gameStatus.South))
            {
                //Debug.Log("NorthからEastまたはWestからSouthに回転する時");

                //第一法則
                //左に2つ移動
                spawner.activeMino.MoveLeft();
                spawner.activeMino.MoveLeft();

                if (!board.CheckPosition(spawner.activeMino))
                {
                    //第二法則
                    //右に3つ移動
                    spawner.activeMino.MoveRight();
                    spawner.activeMino.MoveRight();
                    spawner.activeMino.MoveRight();

                    if (!board.CheckPosition(spawner.activeMino))
                    {
                        //第三法則
                        //左に3つ移動、下に1つ移動
                        spawner.activeMino.MoveLeft();
                        spawner.activeMino.MoveLeft();
                        spawner.activeMino.MoveLeft();
                        spawner.activeMino.MoveDown();

                        if (!board.CheckPosition(spawner.activeMino))
                        {
                            //第四法則
                            //右に3つ移動、上に3つ移動
                            spawner.activeMino.MoveRight();
                            spawner.activeMino.MoveRight();
                            spawner.activeMino.MoveRight();
                            spawner.activeMino.MoveUp();
                            spawner.activeMino.MoveUp();
                            spawner.activeMino.MoveUp();

                            if (!board.CheckPosition(spawner.activeMino))
                            {
                                //SRSができなかった際に回転前の状態に戻る
                                spawner.activeMino.MoveLeft();
                                spawner.activeMino.MoveDown();
                                spawner.activeMino.MoveDown();

                                //通常回転のリセット
                                gameStatus.RotateReset();

                                //SRSができなかった時、falseを返す
                                return false;
                            }
                        }
                    }
                }
            }
            //WestからNorth
            //SouthからEastに回転する時
            else if ((gameStatus.minoAngleBefore == gameStatus.West && gameStatus.minoAngleAfter == gameStatus.North) ||
                (gameStatus.minoAngleBefore == gameStatus.South && gameStatus.minoAngleAfter == gameStatus.East))
            {
                //Debug.Log("WestからNorthまたはSouthからEastに回転する時");

                //第一法則
                //右に1つ移動
                spawner.activeMino.MoveRight();

                if (!board.CheckPosition(spawner.activeMino))
                {
                    //第二法則
                    //左に3つ移動
                    spawner.activeMino.MoveLeft();
                    spawner.activeMino.MoveLeft();
                    spawner.activeMino.MoveLeft();

                    if (!board.CheckPosition(spawner.activeMino))
                    {
                        //第三法則
                        //右に3つ移動、下に2つ移動
                        spawner.activeMino.MoveRight();
                        spawner.activeMino.MoveRight();
                        spawner.activeMino.MoveRight();
                        spawner.activeMino.MoveDown();
                        spawner.activeMino.MoveDown();

                        if (!board.CheckPosition(spawner.activeMino))
                        {
                            //第四法則
                            //左に3つ移動、上に3つ移動
                            spawner.activeMino.MoveLeft();
                            spawner.activeMino.MoveLeft();
                            spawner.activeMino.MoveLeft();
                            spawner.activeMino.MoveUp();
                            spawner.activeMino.MoveUp();
                            spawner.activeMino.MoveUp();

                            if (!board.CheckPosition(spawner.activeMino))
                            {
                                //SRSができなかった際に回転前の状態に戻る
                                spawner.activeMino.MoveRight();
                                spawner.activeMino.MoveRight();
                                spawner.activeMino.MoveDown();

                                //通常回転のリセット
                                gameStatus.RotateReset();

                                //SRSができなかった時、falseを返す
                                return false;
                            }
                        }
                    }
                }
            }
            //EastからNorthまたはSouthからWestに回転する時
            else if ((gameStatus.minoAngleBefore == gameStatus.East && gameStatus.minoAngleAfter == gameStatus.North) ||
                (gameStatus.minoAngleBefore == gameStatus.South && gameStatus.minoAngleAfter == gameStatus.West))
            {
                //Debug.Log("EastからNorthまたはSouthからWestに回転する時");

                //第一法則
                //右に2つ移動
                spawner.activeMino.MoveRight();
                spawner.activeMino.MoveRight();

                if (!board.CheckPosition(spawner.activeMino))
                {
                    //第二法則
                    //左に3つ移動
                    spawner.activeMino.MoveLeft();
                    spawner.activeMino.MoveLeft();
                    spawner.activeMino.MoveLeft();

                    if (!board.CheckPosition(spawner.activeMino))
                    {
                        //第三法則
                        //右に3つ移動、上に1つ移動
                        spawner.activeMino.MoveRight();
                        spawner.activeMino.MoveRight();
                        spawner.activeMino.MoveRight();
                        spawner.activeMino.MoveUp();

                        if (!board.CheckPosition(spawner.activeMino))
                        {
                            //第四法則
                            //左に3つ移動、下に3つ移動
                            spawner.activeMino.MoveLeft();
                            spawner.activeMino.MoveLeft();
                            spawner.activeMino.MoveLeft();
                            spawner.activeMino.MoveDown();
                            spawner.activeMino.MoveDown();
                            spawner.activeMino.MoveDown();

                            if (!board.CheckPosition(spawner.activeMino))
                            {
                                //SRSができなかった際に回転前の状態に戻る
                                spawner.activeMino.MoveRight();
                                spawner.activeMino.MoveUp();
                                spawner.activeMino.MoveUp();

                                //通常回転のリセット
                                gameStatus.RotateReset();

                                //SRSができなかった時、falseを返す
                                return false;
                            }
                        }
                    }
                }
            }
            //NorthからWestまたはEastからSouthに回転する時
            else if ((gameStatus.minoAngleBefore == gameStatus.North && gameStatus.minoAngleAfter == gameStatus.West) ||
                (gameStatus.minoAngleBefore == gameStatus.East && gameStatus.minoAngleAfter == gameStatus.South))
            {
                //Debug.Log("NorthからWestまたはEastからSouthに回転する時");

                //第一法則
                //左に1つ移動
                spawner.activeMino.MoveLeft();

                if (!board.CheckPosition(spawner.activeMino))
                {
                    //第二法則
                    //右に3つ移動
                    spawner.activeMino.MoveRight();
                    spawner.activeMino.MoveRight();
                    spawner.activeMino.MoveRight();

                    if (!board.CheckPosition(spawner.activeMino))
                    {
                        //第三法則
                        //左に3つ移動、上に2つ移動
                        spawner.activeMino.MoveLeft();
                        spawner.activeMino.MoveLeft();
                        spawner.activeMino.MoveLeft();
                        spawner.activeMino.MoveUp();
                        spawner.activeMino.MoveUp();

                        if (!board.CheckPosition(spawner.activeMino))
                        {
                            //第四法則
                            //右に3つ移動、下に3つ移動
                            spawner.activeMino.MoveRight();
                            spawner.activeMino.MoveRight();
                            spawner.activeMino.MoveRight();
                            spawner.activeMino.MoveDown();
                            spawner.activeMino.MoveDown();
                            spawner.activeMino.MoveDown();

                            if (!board.CheckPosition(spawner.activeMino))
                            {
                                //SRSができなかった時、回転前の状態に戻る
                                spawner.activeMino.MoveLeft();
                                spawner.activeMino.MoveLeft();
                                spawner.activeMino.MoveUp();

                                //通常回転のリセット
                                gameStatus.RotateReset();

                                //SRSができなかった時、falseを返す
                                return false;
                            }
                        }
                    }
                }
            }
        }
        //SRSができた時、trueを返す
        return true;
    }

    // Iミノの軸を計算し、Vectoe3で返す関数 //
    public Vector3 AxisCheck(int Imino_x, int Imino_y) // Imino_x と Imino_y はIミノのx, y座標
    {
        //Debug.Log("AxisCheck");

        // xとyのオフセットを宣言
        float xOffset = 0.5f;
        float yOffset = 0.5f;

        // 回転軸は現在位置から、x軸を xOffset 動かし、y軸を yOffset 動かした座標にある
        // xOffset と yOffset の正負は回転前の向きによって変化する

        // 回転前の向きがNorthの時
        if (gameStatus.minoAngleBefore == gameStatus.North)
        {
            return new Vector3(Imino_x + xOffset, Imino_y - yOffset, 0);
        }
        //回転前の向きがEastの時
        else if (gameStatus.minoAngleBefore == gameStatus.East)
        {
            return new Vector3(Imino_x - xOffset, Imino_y - yOffset, 0);
        }
        //回転前の向きがSouthの時
        else if (gameStatus.minoAngleBefore == gameStatus.South)
        {
            return new Vector3(Imino_x - xOffset, Imino_y + yOffset, 0);
        }
        //回転前の向きがWestの時
        else
        {
            return new Vector3(Imino_x + xOffset, Imino_y + yOffset, 0);
        }
    }







    // //SRS時に重複しないか判定する関数
    // //Rx, Ry はそれぞれX, Y座標のオフセット 
    // bool RotationCheck(int Rx, int Ry, Mino mino)
    // {
    //     //minoを構成する4つのブロックをそれぞれ調べる
    //     foreach (Transform item in mino.transform)
    //     {
    //         //ブロックのX, Y座標をposに格納
    //         Vector2 pos = Rounding.Round(item.position);

    //         //オフセット分移動した時、Gridの座標が負ならfalse
    //         if ((int)pos.x + Rx < 0 || (int)pos.y + Ry < 0)
    //         {
    //             Debug.Log("SRS移動先のGrid座標が負");
    //             return false;
    //         }

    //         //オフセット分移動した時、ほかのミノと重なったらfalse
    //         if (board.Grid[(int)pos.x + Rx, (int)pos.y + Ry] != null
    //             && board.Grid[(int)pos.x + Rx, (int)pos.y + Ry].parent != mino.transform)
    //         {
    //             Debug.Log("SRS先が重複");
    //             return false;
    //         }

    //         //オフセット分移動した時、ゲームフィールド外に移動していたならfalse
    //         if (!board.BoardOutCheck((int)pos.x + Rx, (int)pos.y + Ry))
    //         {
    //             Debug.Log("SRS先がゲームフィールド外");
    //             return false;
    //         }
    //     }

    //     return true;
    // }


}
