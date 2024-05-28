using System;
using System.Collections.Generic;
using UnityEngine;

///// ミノに関するスクリプト /////


// ↓このスクリプトで可能なこと↓ //

// ミノの生成
// 左右移動、下移動、通常回転
// スーパーローテーションシステム(以後SRSと呼ぶ)
// ゴーストミノの位置調整

public class Mino : MonoBehaviour
{
    // // ミノの向き //
    // private string North = "North";
    // private string East = "East";
    // private string South = "South";
    // private string West = "West";

    // 回転していいミノかどうか //
    [SerializeField] private bool CanRotate = true; // Oミノは回転しないので、エディターでfalseに設定

    // Z軸の回転量 //
    private int RotateRightAroundZ = -90; // 右回転
    private int RotateLeftAroundZ = 90; // 左回転

    // // 回転方向 //
    // private string UseRotateRight = "RotateRight";
    // private string UseRotateLeft = "RotateLeft";

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
        // LogHelper.Log("Start", LogHelper.LogLevel.Debug, "Mino", "MoveLeft()");

        Move(new Vector3(-1, 0, 0));
    }

    public void MoveRight()
    {
        // LogHelper.Log("Start", LogHelper.LogLevel.Debug, "Mino", "MoveRight()");

        Move(new Vector3(1, 0, 0));
    }

    public void MoveUp()
    {
        // LogHelper.Log("Start", LogHelper.LogLevel.Debug, "Mino", "MoveUp()");

        Move(new Vector3(0, 1, 0));
    }

    public void MoveDown()
    {
        // LogHelper.Log("Start", LogHelper.LogLevel.Debug, "Mino", "MoveDown()");

        Move(new Vector3(0, -1, 0));
    }

    // 通常右回転 //
    public void RotateRight()
    {
        // LogHelper.Log("Start", LogHelper.LogLevel.Debug, "Mino", "RotateRight()");

        if (CanRotate == false) // 回転できないブロックの場合
        {
            return; // Oミノは回転できないので弾かれる
        }

        if (spawner.ActiveMinoName != MinoType.I_Mino) // Iミノ以外の右回転
        {
            transform.Rotate(0, 0, RotateRightAroundZ);
        }
        else // Iミノは軸が他のミノと違うため別の処理
        {
            // Iミノの軸を取得する
            Vector3 IminoAxis = AxisCheck_ForI
                (Mathf.RoundToInt(spawner.ActiveMino.transform.position.x), Mathf.RoundToInt(spawner.ActiveMino.transform.position.y));

            // IminoAxis を中心に右回転する
            transform.RotateAround(IminoAxis, Vector3.forward, RotateRightAroundZ);
        }

        // ミノの角度の調整(右回転)
        gameStatus.UpdateMinoAngleAfter(MinoRotationDirections.RotateRight);
    }

    // 通常左回転 //
    public void RotateLeft()
    {
        // LogHelper.Log("Start", LogHelper.LogLevel.Debug, "Mino", "Rotateleft()");

        if (CanRotate == false) // 回転できないブロックの場合
        {
            return; // Oミノは回転できないので弾かれる
        }

        // Iミノ以外の左回転
        if (spawner.ActiveMinoName != MinoType.I_Mino)
        {
            transform.Rotate(0, 0, RotateLeftAroundZ);
        }

        // Iミノは軸が他のミノと違うため別の処理
        else
        {
            // Iミノの軸を取得する
            Vector3 IminoAxis = AxisCheck_ForI
                (Mathf.RoundToInt(spawner.ActiveMino.transform.position.x), Mathf.RoundToInt(spawner.ActiveMino.transform.position.y));

            // IminoAxisを中心に左回転する
            transform.RotateAround(IminoAxis, Vector3.forward, RotateLeftAroundZ);
        }

        // ミノの角度の調整(左回転)
        gameStatus.UpdateMinoAngleAfter(MinoRotationDirections.RotateLeft);
    }

    // Iミノの軸を計算し、Vectoe3で返す関数 //
    public Vector3 AxisCheck_ForI(int Imino_x, int Imino_y) // Imino_x と Imino_y はIミノのx, y座標
    {
        LogHelper.Log(LogHelper.LogLevel.Debug, "Mino", "AxisCheckt()", "Start");

        // x軸とy軸のオフセットを宣言
        float xOffset = 0.5f;
        float yOffset = 0.5f;

        string AxisPosition; // Infoログ用

        // 回転軸は現在位置から、x軸を xOffset 動かし、y軸を yOffset 動かした座標にある
        // xOffset と yOffset の正負は回転前の向きによって変化する

        // 向きがNorthの時
        if (gameStatus.minoAngleAfter == MinoDirections.North)
        {
            AxisPosition = $"North: Axis = ({Imino_x + xOffset}, {Imino_y - yOffset})";
            LogHelper.Log(LogHelper.LogLevel.Info, "Mino", "AxisCheck_ForI()", AxisPosition);
            LogHelper.Log(LogHelper.LogLevel.Debug, "Mino", "AxisCheckt()", "End");
            return new Vector3(Imino_x + xOffset, Imino_y - yOffset, 0);
        }
        // 向きがEastの時
        else if (gameStatus.minoAngleAfter == MinoDirections.East)
        {
            AxisPosition = $"East: Axis = ({Imino_x - xOffset}, {Imino_y - yOffset})";
            LogHelper.Log(LogHelper.LogLevel.Info, "Mino", "AxisCheck_ForI()", AxisPosition);
            LogHelper.Log(LogHelper.LogLevel.Debug, "Mino", "AxisCheckt()", "End");
            return new Vector3(Imino_x - xOffset, Imino_y - yOffset, 0);
        }
        // 向きがSouthの時
        else if (gameStatus.minoAngleAfter == MinoDirections.South)
        {
            AxisPosition = $"South: Axis = ({Imino_x - xOffset}, {Imino_y + yOffset})";
            LogHelper.Log(LogHelper.LogLevel.Info, "Mino", "AxisCheck_ForI()", AxisPosition);
            LogHelper.Log(LogHelper.LogLevel.Debug, "Mino", "AxisCheckt()", "End");
            return new Vector3(Imino_x - xOffset, Imino_y + yOffset, 0);
        }
        // 向きがWestの時
        else
        {
            AxisPosition = $"West: Axis = ({Imino_x + xOffset}, {Imino_y + yOffset})";
            LogHelper.Log(LogHelper.LogLevel.Info, "Mino", "AxisCheck_ForI()", AxisPosition);
            LogHelper.Log(LogHelper.LogLevel.Debug, "Mino", "AxisCheckt()", "End");
            return new Vector3(Imino_x + xOffset, Imino_y + yOffset, 0);
        }
    }

    // スーパーローテーションシステム(SRS) //
    // 通常回転ができなかった時に試す回転
    // 4つの軌跡を辿り、ブロックや壁に衝突しなかったらそこに移動する

    // ↓参考にした動画
    // https://www.youtube.com/watch?v=0OQ7mP97vdc

    public bool SuperRotationSystem()
    {
        LogHelper.Log(LogHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()", "Start");

        bool success = false;

        // SRSはIミノとそれ以外のミノとで処理が違うため分けて処理する
        // Iミノ以外のSRS
        if (spawner.ActiveMinoName != MinoType.I_Mino)
        {
            if ((gameStatus.minoAngleBefore == MinoDirections.North && gameStatus.minoAngleAfter == MinoDirections.East) ||
                (gameStatus.minoAngleBefore == MinoDirections.South && gameStatus.minoAngleAfter == MinoDirections.East))   // North から East , South から East に回転する時
            {
                success = TrySuperRotation(new List<Action>
            {
                () => spawner.ActiveMino.MoveLeft(), // 第一法則
                () => spawner.ActiveMino.MoveUp(),   // 第二法則
                () =>
                {
                    spawner.ActiveMino.MoveRight();
                    spawner.ActiveMino.MoveDown();
                    spawner.ActiveMino.MoveDown();
                    spawner.ActiveMino.MoveDown(); // 第三法則
                },
                () => spawner.ActiveMino.MoveLeft()  // 第四法則
            }, "NtoE, StoE");
            }
            else if ((gameStatus.minoAngleBefore == MinoDirections.West && gameStatus.minoAngleAfter == MinoDirections.North) ||
                (gameStatus.minoAngleBefore == MinoDirections.West && gameStatus.minoAngleAfter == MinoDirections.South))   // West から North , West から South に回転する時
            {
                success = TrySuperRotation(new List<Action>
            {
                () => spawner.ActiveMino.MoveLeft(), // 第一法則
                () => spawner.ActiveMino.MoveDown(), // 第二法則
                () =>
                {
                    spawner.ActiveMino.MoveRight();
                    spawner.ActiveMino.MoveUp();
                    spawner.ActiveMino.MoveUp();
                    spawner.ActiveMino.MoveUp(); // 第三法則
                },
                () => spawner.ActiveMino.MoveLeft()  // 第四法則
            }, "WtoN, WtoS");
            }
            else if ((gameStatus.minoAngleBefore == MinoDirections.East && gameStatus.minoAngleAfter == MinoDirections.North) ||
                (gameStatus.minoAngleBefore == MinoDirections.East && gameStatus.minoAngleAfter == MinoDirections.South))   // East から North , East から South に回転する時
            {
                success = TrySuperRotation(new List<Action>
            {
                () => spawner.ActiveMino.MoveRight(), // 第一法則
                () => spawner.ActiveMino.MoveDown(),  // 第二法則
                () =>
                {
                    spawner.ActiveMino.MoveLeft();
                    spawner.ActiveMino.MoveUp();
                    spawner.ActiveMino.MoveUp();
                    spawner.ActiveMino.MoveUp(); // 第三法則
                },
                () => spawner.ActiveMino.MoveRight()  // 第四法則
            }, "EtoN, EtoS");
            }
            else if ((gameStatus.minoAngleBefore == MinoDirections.North && gameStatus.minoAngleAfter == MinoDirections.West) ||
                (gameStatus.minoAngleBefore == MinoDirections.South && gameStatus.minoAngleAfter == MinoDirections.West))   // North から West , South から West に回転する時
            {
                success = TrySuperRotation(new List<Action>
            {
                () => spawner.ActiveMino.MoveRight(), // 第一法則
                () => spawner.ActiveMino.MoveUp(),    // 第二法則
                () =>
                {
                    spawner.ActiveMino.MoveLeft();
                    spawner.ActiveMino.MoveDown();
                    spawner.ActiveMino.MoveDown();
                    spawner.ActiveMino.MoveDown(); // 第三法則
                },
                () => spawner.ActiveMino.MoveRight()  // 第四法則
            }, "NtoW, StoW");
            }
        }
        // IミノのSRS(かなり複雑)
        else
        {
            if ((gameStatus.minoAngleBefore == MinoDirections.North && gameStatus.minoAngleAfter == MinoDirections.East) ||
                (gameStatus.minoAngleBefore == MinoDirections.West && gameStatus.minoAngleAfter == MinoDirections.South))  // North から East , West から South に回転する時
            {
                success = TrySuperRotation(new List<Action>
            {
                () =>
                {
                    spawner.ActiveMino.MoveLeft();
                    spawner.ActiveMino.MoveLeft(); // 第一法則
                },
                () =>
                {
                    spawner.ActiveMino.MoveRight();
                    spawner.ActiveMino.MoveRight();
                    spawner.ActiveMino.MoveRight(); // 第二法則
                },
                () =>
                {
                    spawner.ActiveMino.MoveLeft();
                    spawner.ActiveMino.MoveLeft();
                    spawner.ActiveMino.MoveLeft();
                    spawner.ActiveMino.MoveDown(); // 第三法則
                },
                () =>
                {
                    spawner.ActiveMino.MoveRight();
                    spawner.ActiveMino.MoveRight();
                    spawner.ActiveMino.MoveRight();
                    spawner.ActiveMino.MoveUp();
                    spawner.ActiveMino.MoveUp();
                    spawner.ActiveMino.MoveUp(); // 第四法則
                }
            }, "NtoE, WtoS, I");
            }
            else if ((gameStatus.minoAngleBefore == MinoDirections.West && gameStatus.minoAngleAfter == MinoDirections.North) ||
                (gameStatus.minoAngleBefore == MinoDirections.South && gameStatus.minoAngleAfter == MinoDirections.East))   // West から North , South から East に回転する時
            {
                success = TrySuperRotation(new List<Action>
            {
                () => spawner.ActiveMino.MoveRight(), // 第一法則
                () =>
                {
                    spawner.ActiveMino.MoveLeft();
                    spawner.ActiveMino.MoveLeft();
                    spawner.ActiveMino.MoveLeft(); // 第二法則
                },
                () =>
                {
                    spawner.ActiveMino.MoveRight();
                    spawner.ActiveMino.MoveRight();
                    spawner.ActiveMino.MoveRight();
                    spawner.ActiveMino.MoveDown();
                    spawner.ActiveMino.MoveDown(); // 第三法則
                },
                () =>
                {
                    spawner.ActiveMino.MoveLeft();
                    spawner.ActiveMino.MoveLeft();
                    spawner.ActiveMino.MoveLeft();
                    spawner.ActiveMino.MoveUp();
                    spawner.ActiveMino.MoveUp();
                    spawner.ActiveMino.MoveUp(); // 第四法則
                }
            }, "WtoN, StoE, I");
            }
            else if ((gameStatus.minoAngleBefore == MinoDirections.East && gameStatus.minoAngleAfter == MinoDirections.North) ||
                (gameStatus.minoAngleBefore == MinoDirections.South && gameStatus.minoAngleAfter == MinoDirections.West))   // East から North , South から West に回転する時
            {
                success = TrySuperRotation(new List<Action>
            {
                () =>
                {
                    spawner.ActiveMino.MoveRight();
                    spawner.ActiveMino.MoveRight(); // 第一法則
                },
                () =>
                {
                    spawner.ActiveMino.MoveLeft();
                    spawner.ActiveMino.MoveLeft();
                    spawner.ActiveMino.MoveLeft(); // 第二法則
                },
                () =>
                {
                    spawner.ActiveMino.MoveRight();
                    spawner.ActiveMino.MoveRight();
                    spawner.ActiveMino.MoveRight();
                    spawner.ActiveMino.MoveUp(); // 第三法則
                },
                () =>
                {
                    spawner.ActiveMino.MoveLeft();
                    spawner.ActiveMino.MoveLeft();
                    spawner.ActiveMino.MoveLeft();
                    spawner.ActiveMino.MoveDown();
                    spawner.ActiveMino.MoveDown();
                    spawner.ActiveMino.MoveDown(); // 第四法則
                }
            }, "EtoN, StoW, I");
            }
            else if ((gameStatus.minoAngleBefore == MinoDirections.North && gameStatus.minoAngleAfter == MinoDirections.West) ||
                (gameStatus.minoAngleBefore == MinoDirections.East && gameStatus.minoAngleAfter == MinoDirections.South))   // North から West , East から South に回転する時
            {
                success = TrySuperRotation(new List<Action>
            {
                () => spawner.ActiveMino.MoveLeft(), // 第一法則
                () =>
                {
                    spawner.ActiveMino.MoveRight();
                    spawner.ActiveMino.MoveRight();
                    spawner.ActiveMino.MoveRight(); // 第二法則
                },
                () =>
                {
                    spawner.ActiveMino.MoveLeft();
                    spawner.ActiveMino.MoveLeft();
                    spawner.ActiveMino.MoveLeft();
                    spawner.ActiveMino.MoveUp();
                    spawner.ActiveMino.MoveUp(); // 第三法則
                },
                () =>
                {
                    spawner.ActiveMino.MoveRight();
                    spawner.ActiveMino.MoveRight();
                    spawner.ActiveMino.MoveRight();
                    spawner.ActiveMino.MoveDown();
                    spawner.ActiveMino.MoveDown();
                    spawner.ActiveMino.MoveDown(); // 第四法則
                }
            }, "NtoW, EtoS, I");
            }
        }

        LogHelper.Log(LogHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()", "End");
        return success; // SRSが成功したかどうかを返す
    }

    private bool TrySuperRotation(List<Action> rotationSteps, string direction)
    {
        Vector3 originalPosition = spawner.ActiveMino.transform.position; // 現在の位置を保存
        for (int step = 0; step < rotationSteps.Count; step++)
        {
            gameStatus.IncreaseStepsSRS();

            rotationSteps[step]();

            if (board.CheckPosition(spawner.ActiveMino))
            {
                LogHelper.Log(LogHelper.LogLevel.Info, "Mino", "TrySuperRotation()", $"Success SRS = {gameStatus.stepsSRS}, {direction}");
                LogHelper.Log(LogHelper.LogLevel.Debug, "Mino", "TrySuperRotation()", "End");
                return true;
            }
        }

        // 全てのステップが失敗した場合、回転前の状態に戻す
        spawner.ActiveMino.transform.position = originalPosition;
        gameStatus.Reset_Rotate();
        LogHelper.Log(LogHelper.LogLevel.Info, "Mino", "TrySuperRotation()", $"Failure SRS, {direction}");
        LogHelper.Log(LogHelper.LogLevel.Debug, "Mino", "TrySuperRotation()", "End");
        return false;
    }



    // public bool SuperRotationSystem()
    // {
    //     LogHelper.Log(LogHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()", "Start");

    //     // SRSはIミノとそれ以外のミノとで処理が違うため分けて処理する
    //     // Iミノ以外のSRS
    //     if (spawner.ActiveMinoName != "I_Mino")
    //     {
    //         //Debug.Log("Iミノ以外のSRS");

    //         if ((gameStatus.minoAngleBefore == North && gameStatus.minoAngleAfter == East) ||
    //             (gameStatus.minoAngleBefore == South && gameStatus.minoAngleAfter == East))   // North から East , South から East に回転する時
    //         {
    //             // 第一法則
    //             spawner.ActiveMino.MoveLeft(); // 左に1つ移動

    //             // SRSの段階数を格納(TspinMiniの判定に必要)
    //             gameStatus.IncreaseLastSRS(); // 1

    //             if (!board.CheckPosition(spawner.ActiveMino))
    //             {
    //                 // 第二法則
    //                 spawner.ActiveMino.MoveUp(); // 上に1つ移動

    //                 gameStatus.IncreaseLastSRS(); // 2

    //                 if (!board.CheckPosition(spawner.ActiveMino))
    //                 {
    //                     // 第三法則
    //                     spawner.ActiveMino.MoveRight(); // 右に1つ移動
    //                     spawner.ActiveMino.MoveDown();
    //                     spawner.ActiveMino.MoveDown();
    //                     spawner.ActiveMino.MoveDown(); // 下に3つ移動

    //                     gameStatus.IncreaseLastSRS(); // 3

    //                     if (!board.CheckPosition(spawner.ActiveMino))
    //                     {
    //                         // 第四法則
    //                         spawner.ActiveMino.MoveLeft(); // 左に1つ移動

    //                         gameStatus.IncreaseLastSRS(); // 4

    //                         if (!board.CheckPosition(spawner.ActiveMino))
    //                         {
    //                             // SRSができなかった時、回転前の状態に戻る
    //                             spawner.ActiveMino.MoveRight();
    //                             spawner.ActiveMino.MoveUp();
    //                             spawner.ActiveMino.MoveUp();

    //                             gameStatus.Reset_Rotate(); // 通常回転のリセット

    //                             LogHelper.Log("Failure SRS, NtoE, StoE", LogHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");

    //                             return false; // SRSができなかった時、falseを返す
    //                         }
    //                         LogHelper.Log("Success SRS = 4, NtoE, StoE", LogHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
    //                     }
    //                     LogHelper.Log("Success SRS = 3, NtoE, StoE", LogHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
    //                 }
    //                 LogHelper.Log("Success SRS = 2, NtoE, StoE", LogHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
    //             }
    //             LogHelper.Log("Success SRS = 1, NtoE, StoE", LogHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
    //         }
    //         else if ((gameStatus.minoAngleBefore == West && gameStatus.minoAngleAfter == North) ||
    //             (gameStatus.minoAngleBefore == West && gameStatus.minoAngleAfter == South))   // West から North , West から South に回転する時
    //         {
    //             // 第一法則
    //             spawner.ActiveMino.MoveLeft(); // 左に1つ移動

    //             // SRSの段階数を格納(TspinMiniの判定に必要)
    //             gameStatus.IncreaseLastSRS(); // 1

    //             if (!board.CheckPosition(spawner.ActiveMino))
    //             {
    //                 // 第二法則
    //                 spawner.ActiveMino.MoveDown(); // 下に1つ移動

    //                 gameStatus.IncreaseLastSRS(); // 2

    //                 if (!board.CheckPosition(spawner.ActiveMino))
    //                 {
    //                     // 第三法則
    //                     spawner.ActiveMino.MoveRight(); // 右に1つ移動
    //                     spawner.ActiveMino.MoveUp();
    //                     spawner.ActiveMino.MoveUp();
    //                     spawner.ActiveMino.MoveUp(); // 上に3つ移動

    //                     gameStatus.IncreaseLastSRS(); // 3

    //                     if (!board.CheckPosition(spawner.ActiveMino))
    //                     {
    //                         // 第四法則
    //                         spawner.ActiveMino.MoveLeft(); // 左に1つ移動

    //                         gameStatus.IncreaseLastSRS(); // 4

    //                         if (!board.CheckPosition(spawner.ActiveMino))
    //                         {
    //                             //SRSができなかった際に回転前の状態に戻る
    //                             spawner.ActiveMino.MoveRight();
    //                             spawner.ActiveMino.MoveDown();
    //                             spawner.ActiveMino.MoveDown();

    //                             gameStatus.Reset_Rotate(); // 通常回転のリセット

    //                             LogHelper.Log("Failure SRS, WtoN, WtoS", LogHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");

    //                             return false; // SRSができなかった時、falseを返す
    //                         }
    //                         LogHelper.Log("Success SRS = 4, WtoN, WtoS", LogHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
    //                     }
    //                     LogHelper.Log("Success SRS = 3, WtoN, WtoS", LogHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
    //                 }
    //                 LogHelper.Log("Success SRS = 2, WtoN, WtoS", LogHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
    //             }
    //             LogHelper.Log("Success SRS = 1, WtoN, WtoS", LogHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
    //         }
    //         else if ((gameStatus.minoAngleBefore == East && gameStatus.minoAngleAfter == North) ||
    //             (gameStatus.minoAngleBefore == East && gameStatus.minoAngleAfter == South))   // East から North , East から South に回転する時
    //         {
    //             // 第一法則
    //             spawner.ActiveMino.MoveRight(); // 右に1つ移動

    //             // SRSの段階数を格納(TspinMiniの判定に必要)
    //             gameStatus.IncreaseLastSRS(); // 1

    //             if (!board.CheckPosition(spawner.ActiveMino))
    //             {
    //                 // 第二法則
    //                 spawner.ActiveMino.MoveDown(); // 下に1つ移動

    //                 gameStatus.IncreaseLastSRS(); // 2

    //                 if (!board.CheckPosition(spawner.ActiveMino))
    //                 {
    //                     // 第三法則
    //                     spawner.ActiveMino.MoveLeft(); // 左に1つ移動
    //                     spawner.ActiveMino.MoveUp();
    //                     spawner.ActiveMino.MoveUp();
    //                     spawner.ActiveMino.MoveUp(); // 上に3つ移動

    //                     gameStatus.IncreaseLastSRS(); // 3

    //                     if (!board.CheckPosition(spawner.ActiveMino))
    //                     {
    //                         // 第四法則
    //                         spawner.ActiveMino.MoveRight(); // 右に1つ移動

    //                         gameStatus.IncreaseLastSRS(); // 4

    //                         if (!board.CheckPosition(spawner.ActiveMino))
    //                         {
    //                             // SRSができなかった際に回転前の状態に戻る
    //                             spawner.ActiveMino.MoveLeft();
    //                             spawner.ActiveMino.MoveDown();
    //                             spawner.ActiveMino.MoveDown();

    //                             gameStatus.Reset_Rotate(); // 通常回転のリセット

    //                             LogHelper.Log("Failure SRS, EtoN, EtoS", LogHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");

    //                             return false; // SRSができなかった時、falseを返す
    //                         }
    //                         LogHelper.Log("Success SRS = 4, EtoN, EtoS", LogHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
    //                     }
    //                     LogHelper.Log("Success SRS = 3, EtoN, EtoS", LogHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
    //                 }
    //                 LogHelper.Log("Success SRS = 2, EtoN, EtoS", LogHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
    //             }
    //             LogHelper.Log("Success SRS = 1, EtoN, EtoS", LogHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
    //         }
    //         else if ((gameStatus.minoAngleBefore == North && gameStatus.minoAngleAfter == West) ||
    //             (gameStatus.minoAngleBefore == South && gameStatus.minoAngleAfter == West))   // North から West , South から West に回転する時
    //         {
    //             // 第一法則
    //             spawner.ActiveMino.MoveRight(); // 右に1つ移動

    //             // SRSの段階数を格納(TspinMiniの判定に必要)
    //             gameStatus.IncreaseLastSRS(); // 1

    //             if (!board.CheckPosition(spawner.ActiveMino))
    //             {
    //                 // 第二法則
    //                 spawner.ActiveMino.MoveUp(); // 上に1つ移動

    //                 gameStatus.IncreaseLastSRS(); // 2

    //                 if (!board.CheckPosition(spawner.ActiveMino))
    //                 {
    //                     // 第三法則
    //                     spawner.ActiveMino.MoveLeft(); // 左に1つ移動
    //                     spawner.ActiveMino.MoveDown();
    //                     spawner.ActiveMino.MoveDown();
    //                     spawner.ActiveMino.MoveDown(); // 下に3つ移動

    //                     gameStatus.IncreaseLastSRS(); // 3

    //                     if (!board.CheckPosition(spawner.ActiveMino))
    //                     {
    //                         // 第四法則
    //                         spawner.ActiveMino.MoveRight(); // 右に1つ移動

    //                         gameStatus.IncreaseLastSRS(); // 4

    //                         if (!board.CheckPosition(spawner.ActiveMino))
    //                         {
    //                             // SRSができなかった時、通常回転前の状態に戻る
    //                             spawner.ActiveMino.MoveLeft();
    //                             spawner.ActiveMino.MoveUp();
    //                             spawner.ActiveMino.MoveUp();

    //                             gameStatus.Reset_Rotate(); // 通常回転のリセット

    //                             LogHelper.Log("Failure SRS, NtoW, StoW", LogHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");

    //                             return false; // SRSができなかった時、falseを返す
    //                         }
    //                         LogHelper.Log("Success SRS = 4, NtoW, StoW", LogHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
    //                     }
    //                     LogHelper.Log("Success SRS = 3, NtoW, StoW", LogHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
    //                 }
    //                 LogHelper.Log("Success SRS = 2, NtoW, StoW", LogHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
    //             }
    //             LogHelper.Log("Success SRS = 1, NtoW, StoW", LogHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
    //         }
    //     }
    //     // IミノのSRS(かなり複雑)
    //     else
    //     {
    //         //Debug.Log("IミノのSRS");

    //         if ((gameStatus.minoAngleBefore == North && gameStatus.minoAngleAfter == East) ||
    //             (gameStatus.minoAngleBefore == West && gameStatus.minoAngleAfter == South))  // North から East , West から South に回転する時
    //         {
    //             // 第一法則
    //             spawner.ActiveMino.MoveLeft();
    //             spawner.ActiveMino.MoveLeft(); // 左に2つ移動

    //             if (!board.CheckPosition(spawner.ActiveMino))
    //             {
    //                 // 第二法則
    //                 spawner.ActiveMino.MoveRight();
    //                 spawner.ActiveMino.MoveRight();
    //                 spawner.ActiveMino.MoveRight(); // 右に3つ移動

    //                 if (!board.CheckPosition(spawner.ActiveMino))
    //                 {
    //                     // 第三法則
    //                     spawner.ActiveMino.MoveLeft();
    //                     spawner.ActiveMino.MoveLeft();
    //                     spawner.ActiveMino.MoveLeft(); // 左に3つ移動
    //                     spawner.ActiveMino.MoveDown(); // 下に1つ移動

    //                     if (!board.CheckPosition(spawner.ActiveMino))
    //                     {
    //                         // 第四法則
    //                         spawner.ActiveMino.MoveRight();
    //                         spawner.ActiveMino.MoveRight();
    //                         spawner.ActiveMino.MoveRight(); // 右に3つ移動
    //                         spawner.ActiveMino.MoveUp();
    //                         spawner.ActiveMino.MoveUp();
    //                         spawner.ActiveMino.MoveUp(); // 上に3つ移動

    //                         if (!board.CheckPosition(spawner.ActiveMino))
    //                         {
    //                             // SRSができなかった際に回転前の状態に戻る
    //                             spawner.ActiveMino.MoveLeft();
    //                             spawner.ActiveMino.MoveDown();
    //                             spawner.ActiveMino.MoveDown();

    //                             gameStatus.Reset_Rotate(); // 通常回転のリセット

    //                             LogHelper.Log("Failure SRS, NtoE, WtoS, I", LogHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");

    //                             return false; // SRSができなかった時、falseを返す
    //                         }
    //                         LogHelper.Log("Success SRS = 4, NtoE, WtoS, I", LogHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
    //                     }
    //                     LogHelper.Log("Success SRS = 3, NtoE, WtoS, I", LogHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
    //                 }
    //                 LogHelper.Log("Success SRS = 2, NtoE, WtoS, I", LogHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
    //             }
    //             LogHelper.Log("Success SRS = 1, NtoE, WtoS, I", LogHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
    //         }
    //         else if ((gameStatus.minoAngleBefore == West && gameStatus.minoAngleAfter == North) ||
    //             (gameStatus.minoAngleBefore == South && gameStatus.minoAngleAfter == East))   // West から North , South から East に回転する時
    //         {
    //             // 第一法則
    //             spawner.ActiveMino.MoveRight(); // 右に1つ移動

    //             if (!board.CheckPosition(spawner.ActiveMino))
    //             {
    //                 // 第二法則
    //                 spawner.ActiveMino.MoveLeft();
    //                 spawner.ActiveMino.MoveLeft();
    //                 spawner.ActiveMino.MoveLeft(); // 左に3つ移動

    //                 if (!board.CheckPosition(spawner.ActiveMino))
    //                 {
    //                     // 第三法則
    //                     spawner.ActiveMino.MoveRight();
    //                     spawner.ActiveMino.MoveRight();
    //                     spawner.ActiveMino.MoveRight(); // 右に3つ移動
    //                     spawner.ActiveMino.MoveDown();
    //                     spawner.ActiveMino.MoveDown(); // 下に2つ移動

    //                     if (!board.CheckPosition(spawner.ActiveMino))
    //                     {
    //                         // 第四法則
    //                         spawner.ActiveMino.MoveLeft();
    //                         spawner.ActiveMino.MoveLeft();
    //                         spawner.ActiveMino.MoveLeft(); // 左に3つ移動
    //                         spawner.ActiveMino.MoveUp();
    //                         spawner.ActiveMino.MoveUp();
    //                         spawner.ActiveMino.MoveUp(); // 上に3つ移動

    //                         if (!board.CheckPosition(spawner.ActiveMino))
    //                         {
    //                             // SRSができなかった際に回転前の状態に戻る
    //                             spawner.ActiveMino.MoveRight();
    //                             spawner.ActiveMino.MoveRight();
    //                             spawner.ActiveMino.MoveDown();

    //                             gameStatus.Reset_Rotate(); // 通常回転のリセット

    //                             LogHelper.Log("Failure SRS, WtoN, StoE, I", LogHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");

    //                             return false; // SRSができなかった時、falseを返す
    //                         }
    //                         LogHelper.Log("Success SRS = 4, WtoN, StoE, I", LogHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
    //                     }
    //                     LogHelper.Log("Success SRS = 3, WtoN, StoE, I", LogHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
    //                 }
    //                 LogHelper.Log("Success SRS = 2, WtoN, StoE, I", LogHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
    //             }
    //             LogHelper.Log("Success SRS = 1, WtoN, StoE, I", LogHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
    //         }
    //         else if ((gameStatus.minoAngleBefore == East && gameStatus.minoAngleAfter == North) ||
    //             (gameStatus.minoAngleBefore == South && gameStatus.minoAngleAfter == West))   // East から North , South から West に回転する時
    //         {
    //             // 第一法則
    //             spawner.ActiveMino.MoveRight();
    //             spawner.ActiveMino.MoveRight(); // 右に2つ移動

    //             if (!board.CheckPosition(spawner.ActiveMino))
    //             {
    //                 // 第二法則
    //                 spawner.ActiveMino.MoveLeft();
    //                 spawner.ActiveMino.MoveLeft();
    //                 spawner.ActiveMino.MoveLeft(); // 左に3つ移動

    //                 if (!board.CheckPosition(spawner.ActiveMino))
    //                 {
    //                     // 第三法則
    //                     spawner.ActiveMino.MoveRight();
    //                     spawner.ActiveMino.MoveRight();
    //                     spawner.ActiveMino.MoveRight(); // 右に3つ移動
    //                     spawner.ActiveMino.MoveUp(); // 上に1つ移動

    //                     if (!board.CheckPosition(spawner.ActiveMino))
    //                     {
    //                         // 第四法則
    //                         spawner.ActiveMino.MoveLeft();
    //                         spawner.ActiveMino.MoveLeft();
    //                         spawner.ActiveMino.MoveLeft(); // 左に3つ移動
    //                         spawner.ActiveMino.MoveDown();
    //                         spawner.ActiveMino.MoveDown();
    //                         spawner.ActiveMino.MoveDown(); // 下に3つ移動

    //                         if (!board.CheckPosition(spawner.ActiveMino))
    //                         {
    //                             // SRSができなかった際に回転前の状態に戻る
    //                             spawner.ActiveMino.MoveRight();
    //                             spawner.ActiveMino.MoveUp();
    //                             spawner.ActiveMino.MoveUp();

    //                             gameStatus.Reset_Rotate(); // 通常回転のリセット

    //                             LogHelper.Log("Failure SRS, EtoN, StoW, I", LogHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");

    //                             return false; // SRSができなかった時、falseを返す
    //                         }
    //                         LogHelper.Log("Success SRS = 4, EtoN, StoW, I", LogHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
    //                     }
    //                     LogHelper.Log("Success SRS = 3, EtoN, StoW, I", LogHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
    //                 }
    //                 LogHelper.Log("Success SRS = 2, EtoN, StoW, I", LogHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
    //             }
    //             LogHelper.Log("Success SRS = 1, EtoN, StoW, I", LogHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
    //         }
    //         else if ((gameStatus.minoAngleBefore == North && gameStatus.minoAngleAfter == West) ||
    //             (gameStatus.minoAngleBefore == East && gameStatus.minoAngleAfter == South))   // North から West , East から South に回転する時
    //         {
    //             // 第一法則
    //             spawner.ActiveMino.MoveLeft(); // 左に1つ移動

    //             if (!board.CheckPosition(spawner.ActiveMino))
    //             {
    //                 // 第二法則
    //                 spawner.ActiveMino.MoveRight();
    //                 spawner.ActiveMino.MoveRight();
    //                 spawner.ActiveMino.MoveRight(); // 右に3つ移動

    //                 if (!board.CheckPosition(spawner.ActiveMino))
    //                 {
    //                     // 第三法則
    //                     spawner.ActiveMino.MoveLeft();
    //                     spawner.ActiveMino.MoveLeft();
    //                     spawner.ActiveMino.MoveLeft(); // 左に3つ移動
    //                     spawner.ActiveMino.MoveUp();
    //                     spawner.ActiveMino.MoveUp(); // 上に2つ移動

    //                     if (!board.CheckPosition(spawner.ActiveMino))
    //                     {
    //                         // 第四法則
    //                         spawner.ActiveMino.MoveRight();
    //                         spawner.ActiveMino.MoveRight();
    //                         spawner.ActiveMino.MoveRight(); // 右に3つ移動
    //                         spawner.ActiveMino.MoveDown();
    //                         spawner.ActiveMino.MoveDown();
    //                         spawner.ActiveMino.MoveDown(); // 下に3つ移動

    //                         if (!board.CheckPosition(spawner.ActiveMino))
    //                         {
    //                             // SRSができなかった時、回転前の状態に戻る
    //                             spawner.ActiveMino.MoveLeft();
    //                             spawner.ActiveMino.MoveLeft();
    //                             spawner.ActiveMino.MoveUp();

    //                             gameStatus.Reset_Rotate(); // 通常回転のリセット

    //                             LogHelper.Log(LogHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()", "End");

    //                             return false; // SRSができなかった時、falseを返す
    //                         }
    //                         LogHelper.Log(LogHelper.LogLevel.Info, "Mino", "SuperRotationSystem()", "Success SRS = 4, NtoW, EtoS, I");
    //                     }
    //                     LogHelper.Log(LogHelper.LogLevel.Info, "Mino", "SuperRotationSystem()", "Success SRS = 3, NtoW, EtoS, I");
    //                 }
    //                 LogHelper.Log(LogHelper.LogLevel.Info, "Mino", "SuperRotationSystem()", "Success SRS = 2, NtoW, EtoS, I");
    //             }
    //             LogHelper.Log(LogHelper.LogLevel.Info, "Mino", "SuperRotationSystem()", "Success SRS = 1, NtoW, EtoS, I");
    //         }
    //     }
    //     LogHelper.Log(LogHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()", "End");
    //     return true; // SRSができた時、trueを返す
    // }
}
