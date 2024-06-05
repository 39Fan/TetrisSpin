using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ミノの種類一覧
/// </summary>
public enum eMinoType
{
    I_Mino,
    J_Mino,
    L_Mino,
    O_Mino,
    S_Mino,
    T_Mino,
    Z_Mino
}

/// <summary>
/// ミノの向き一覧
/// </summary>
public enum eMinoDirection
{
    North, // 初期(未回転)状態
    East,  // 右回転後の向き
    South, // 2回右回転または左回転した時の向き
    West   // 左回転後の向き
}

/// <summary>
/// ミノの回転方向一覧
/// </summary>
public enum eMinoRotationDirection
{
    RotateRight,
    RotateLeft
}

/// <summary>
/// ミノの統計情報を保持する静的クラス
/// </summary>
public static class MinoMovementStats
{
    /// <summary> ミノの回転後の向き </summary>
    /// <remarks>
    /// 初期値はNorthの状態 <br/>
    /// Spin判定を確認する際、回転後の向きと回転前の向きの情報が必要なため
    /// </remarks>
    private static eMinoDirection minoAngleAfter = eMinoDirection.North;

    /// <summary> ミノの回転前の向き </summary>
    /// <remarks>
    /// 初期値はNorthの状態 <br/>
    /// Spin判定を確認する際、回転後の向きと回転前の向きの情報が必要なため
    /// </remarks>
    private static eMinoDirection minoAngleBefore = eMinoDirection.North;

    /// <summary> スーパーローテーションシステム(SRS)の段階 </summary>
    /// <remarks>
    /// SRSが使用されていないときは0, 1〜4の時は、SRSの段階を表す
    /// </remarks>
    /// <value> 0~4 </value>
    [SerializeField] private static int stepsSRS = 0;

    // ゲッタープロパティ //
    public static eMinoDirection MinoAngleAfter => minoAngleAfter;
    public static eMinoDirection MinoAngleBefore => minoAngleBefore;
    public static int StepsSRS => stepsSRS;

    /// <summary> フィールドの値を更新する関数 </summary>
    /// <param name="_minoAngleAfter"> ミノの回転後の向き </param>
    /// <param name="_minoAngleBefore"> ミノの回転前の向き </param>
    /// <param name="_stepsSRS"> SRSの段階 </param>
    /// <remarks>
    /// 指定されていない引数は現在の値を維持
    /// </remarks>
    public static void Update(eMinoDirection? _minoAngleAfter = null, eMinoDirection? _minoAngleBefore = null, int? _stepsSRS = null)
    {
        minoAngleAfter = _minoAngleAfter ?? minoAngleAfter;
        minoAngleBefore = _minoAngleBefore ?? minoAngleBefore;
        stepsSRS = _stepsSRS ?? stepsSRS;
        // TODO: ログの記入
    }

    /// <summary> デフォルトの <see cref="MinoMovementStats"/> にリセットする関数 </summary>
    public static void Reset()
    {
        minoAngleAfter = eMinoDirection.North;
        minoAngleBefore = eMinoDirection.North;
        stepsSRS = 0;
    }
}

/// <summary>
/// ミノに関するスクリプト
/// </summary>
/// <remarks>
/// - ミノの生成 <br/>
/// - 左右移動、下移動、通常回転 <br/>
/// - スーパーローテーションシステム(以後SRSと呼ぶ) <br/>
/// - ゴーストミノの位置調整
/// </remarks>
public class MinoMovement : MonoBehaviour
{
    /// <summary> 回転の可能判定 </summary>
    [SerializeField] private bool CanRotate = true; // Oミノは回転しないので、エディターでfalseに設定

    // 干渉するスクリプト //
    Board board;
    Spawner spawner;

    /// <summary>
    /// インスタンス化
    /// </summary>
    private void Awake()
    {
        board = FindObjectOfType<Board>();
        spawner = FindObjectOfType<Spawner>();
    }

    /// <summary> ミノを指定した方向に移動する関数 </summary>
    public void Move(Vector3 _MoveDirection)
    {
        transform.position += _MoveDirection;
    }

    /// <summary> ミノを左に移動する関数 </summary>
    public void MoveLeft()
    {
        // LogHelper.Log("Start", LogHelper.eLogLevel.Debug, "Mino", "MoveLeft()");

        Move(new Vector3(-1, 0, 0));
    }
    /// <summary> ミノを右に移動する関数 </summary>
    public void MoveRight()
    {
        // LogHelper.Log("Start", LogHelper.eLogLevel.Debug, "Mino", "MoveRight()");

        Move(new Vector3(1, 0, 0));
    }
    /// <summary> ミノを上に移動する関数 </summary>
    public void MoveUp()
    {
        // LogHelper.Log("Start", LogHelper.eLogLevel.Debug, "Mino", "MoveUp()");

        Move(new Vector3(0, 1, 0));
    }
    /// <summary> ミノを下に移動する関数 </summary>
    public void MoveDown()
    {
        // LogHelper.Log("Start", LogHelper.eLogLevel.Debug, "Mino", "MoveDown()");
        Move(new Vector3(0, -1, 0));
    }

    /// <summary> ミノを右回転する関数 </summary>
    public void RotateRight()
    {
        // LogHelper.Log("Start", LogHelper.eLogLevel.Debug, "Mino", "RotateRight()");

        /// <summary> 右回転のZ軸の回転量 </summary>
        int RotateRightAroundZ = -90;

        if (CanRotate == false) // 回転できないブロックの場合
        {
            return; // Oミノは回転できないので弾かれる
        }

        if (SpawnerStats.ActiveMinoName != eMinoType.I_Mino) // Iミノ以外の右回転
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
        UpdateMinoAngleAfter(eMinoRotationDirection.RotateRight);
    }

    /// <summary> ミノを左回転する関数 </summary>
    public void RotateLeft()
    {
        // LogHelper.Log("Start", LogHelper.eLogLevel.Debug, "Mino", "Rotateleft()");

        /// <summary> 左回転のZ軸の回転量 </summary>
        int RotateLeftAroundZ = 90;

        if (CanRotate == false) // 回転できないブロックの場合
        {
            return; // Oミノは回転できないので弾かれる
        }

        // Iミノ以外の左回転
        if (SpawnerStats.ActiveMinoName != eMinoType.I_Mino)
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
        UpdateMinoAngleAfter(eMinoRotationDirection.RotateLeft);
    }

    /// <summary> Iミノの軸を計算し、Vector3で返す関数 </summary>
    /// <param name="Imino_x"> Iミノのx座標</param>
    /// <param name="Imino_y"> Iミノのy座標 </param>
    /// <returns> Iミノの軸となる座標(Vector3) </returns>
    public Vector3 AxisCheck_ForI(int Imino_x, int Imino_y) // Imino_x と Imino_y はIミノのx, y座標
    {
        LogHelper.Log(LogHelper.eLogLevel.Debug, "Mino", "AxisCheck()", "Start");

        // x軸とy軸のオフセットを宣言
        float xOffset = 0.5f;
        float yOffset = 0.5f;

        string AxisPosition; // Infoログ用

        // 回転軸は現在位置から、x軸を xOffset 動かし、y軸を yOffset 動かした座標にある
        // xOffset と yOffset の正負は回転前の向きによって変化する

        // 向きがNorthの時
        if (MinoMovementStats.MinoAngleAfter == eMinoDirection.North)
        {
            AxisPosition = $"North: Axis = ({Imino_x + xOffset}, {Imino_y - yOffset})";
            LogHelper.Log(LogHelper.eLogLevel.Info, "Mino", "AxisCheck_ForI()", AxisPosition);
            LogHelper.Log(LogHelper.eLogLevel.Debug, "Mino", "AxisCheck()", "End");
            return new Vector3(Imino_x + xOffset, Imino_y - yOffset, 0);
        }
        // 向きがEastの時
        else if (MinoMovementStats.MinoAngleAfter == eMinoDirection.East)
        {
            AxisPosition = $"East: Axis = ({Imino_x - xOffset}, {Imino_y - yOffset})";
            LogHelper.Log(LogHelper.eLogLevel.Info, "Mino", "AxisCheck_ForI()", AxisPosition);
            LogHelper.Log(LogHelper.eLogLevel.Debug, "Mino", "AxisCheck()", "End");
            return new Vector3(Imino_x - xOffset, Imino_y - yOffset, 0);
        }
        // 向きがSouthの時
        else if (MinoMovementStats.MinoAngleAfter == eMinoDirection.South)
        {
            AxisPosition = $"South: Axis = ({Imino_x - xOffset}, {Imino_y + yOffset})";
            LogHelper.Log(LogHelper.eLogLevel.Info, "Mino", "AxisCheck_ForI()", AxisPosition);
            LogHelper.Log(LogHelper.eLogLevel.Debug, "Mino", "AxisCheck()", "End");
            return new Vector3(Imino_x - xOffset, Imino_y + yOffset, 0);
        }
        // 向きがWestの時
        else
        {
            AxisPosition = $"West: Axis = ({Imino_x + xOffset}, {Imino_y + yOffset})";
            LogHelper.Log(LogHelper.eLogLevel.Info, "Mino", "AxisCheck_ForI()", AxisPosition);
            LogHelper.Log(LogHelper.eLogLevel.Debug, "Mino", "AxisCheck()", "End");
            return new Vector3(Imino_x + xOffset, Imino_y + yOffset, 0);
        }
    }

    /// <summary> MinoAngleAfterの更新をする関数 </summary>
    /// <param name="_rotateDirection"> 回転方向 </param>
    public void UpdateMinoAngleAfter(eMinoRotationDirection _rotateDirection)
    {
        switch (_rotateDirection)
        {
            case eMinoRotationDirection.RotateRight:
                switch (MinoMovementStats.MinoAngleAfter)
                {
                    case eMinoDirection.North:
                        MinoMovementStats.Update(_minoAngleAfter: eMinoDirection.East);
                        break;
                    case eMinoDirection.East:
                        MinoMovementStats.Update(_minoAngleAfter: eMinoDirection.South);
                        break;
                    case eMinoDirection.South:
                        MinoMovementStats.Update(_minoAngleAfter: eMinoDirection.West);
                        break;
                    case eMinoDirection.West:
                        MinoMovementStats.Update(_minoAngleAfter: eMinoDirection.North);
                        break;
                }
                break;
            case eMinoRotationDirection.RotateLeft:
                switch (MinoMovementStats.MinoAngleAfter)
                {
                    case eMinoDirection.North:
                        MinoMovementStats.Update(_minoAngleAfter: eMinoDirection.West);
                        break;
                    case eMinoDirection.East:
                        MinoMovementStats.Update(_minoAngleAfter: eMinoDirection.North);
                        break;
                    case eMinoDirection.South:
                        MinoMovementStats.Update(_minoAngleAfter: eMinoDirection.East);
                        break;
                    case eMinoDirection.West:
                        MinoMovementStats.Update(_minoAngleAfter: eMinoDirection.South);
                        break;
                }
                break;
        }
    }

    /// <summary> MinoAngleAfter の値を MinoAngleBefore に変更する関数 </summary>
    public void UpdateMinoAngleAfterToMinoAngleBefore()
    {
        MinoMovementStats.Update(_minoAngleAfter: MinoMovementStats.MinoAngleBefore);
    }

    /// <summary> MinoAngleBefore の値を MinoAngleAfter に変更する関数 </summary>
    public void UpdateMinoAngleBeforeToMinoAngleAfter()
    {
        MinoMovementStats.Update(_minoAngleBefore: MinoMovementStats.MinoAngleAfter);
    }

    /// <summary> 通常回転のリセットをする関数 </summary>
    public void ResetRotate()
    {
        // 通常回転が右回転だった時
        if ((MinoMovementStats.MinoAngleAfter == eMinoDirection.North && MinoMovementStats.MinoAngleAfter == eMinoDirection.East) ||
        (MinoMovementStats.MinoAngleAfter == eMinoDirection.East && MinoMovementStats.MinoAngleAfter == eMinoDirection.South) ||
        (MinoMovementStats.MinoAngleAfter == eMinoDirection.South && MinoMovementStats.MinoAngleAfter == eMinoDirection.West) ||
        (MinoMovementStats.MinoAngleAfter == eMinoDirection.West && MinoMovementStats.MinoAngleAfter == eMinoDirection.North))
        {
            spawner.ActiveMino.RotateLeft(); // 左回転で回転前の状態に戻す
        }
        else // 通常回転が左回転だった時
        {
            spawner.ActiveMino.RotateRight(); // 右回転で回転前の状態に戻す
        }
    }

    /// <summary> ミノの向きを初期化する関数 </summary>
    public void ResetAngle()
    {
        MinoMovementStats.Update(_minoAngleAfter: eMinoDirection.North, _minoAngleBefore: eMinoDirection.North);
    }

    /// <summary> StepsSRSの値をリセットする関数 </summary>
    public void ResetStepsSRS()
    {
        MinoMovementStats.Update(_stepsSRS: 0);
    }

    /// <summary> スーパーローテーションシステム(SRS)の計算をする関数 </summary>
    /// <remarks>
    /// 通常回転ができなかった時に試す特殊回転 <br/>
    /// 4つの軌跡を辿り、ブロックや壁に衝突しなかったらそこに移動する <br/>
    /// Iミノとそれ以外のミノとで処理が異なる <br/>
    /// <br/>
    /// ↓参考にした動画 <br/>
    /// https://www.youtube.com/watch?v=0OQ7mP97vdc
    /// </remarks>
    /// <returns> 成功したかどうか(true or false) </returns>
    public bool SuperRotationSystem()
    {
        LogHelper.Log(LogHelper.eLogLevel.Debug, "Mino", "SuperRotationSystem()", "Start");

        /// <summary> SRSの成功失敗の判定 </summary>
        bool success = false;

        // Iミノ以外のSRS
        if (SpawnerStats.ActiveMinoName != eMinoType.I_Mino)
        {
            if ((MinoMovementStats.MinoAngleBefore == eMinoDirection.North && MinoMovementStats.MinoAngleAfter == eMinoDirection.East) ||
                (MinoMovementStats.MinoAngleBefore == eMinoDirection.South && MinoMovementStats.MinoAngleAfter == eMinoDirection.East))   // North から East , South から East に回転する時
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
            else if ((MinoMovementStats.MinoAngleBefore == eMinoDirection.West && MinoMovementStats.MinoAngleAfter == eMinoDirection.North) ||
                (MinoMovementStats.MinoAngleBefore == eMinoDirection.West && MinoMovementStats.MinoAngleAfter == eMinoDirection.South))   // West から North , West から South に回転する時
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
            else if ((MinoMovementStats.MinoAngleBefore == eMinoDirection.East && MinoMovementStats.MinoAngleAfter == eMinoDirection.North) ||
                (MinoMovementStats.MinoAngleBefore == eMinoDirection.East && MinoMovementStats.MinoAngleAfter == eMinoDirection.South))   // East から North , East から South に回転する時
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
            else if ((MinoMovementStats.MinoAngleBefore == eMinoDirection.North && MinoMovementStats.MinoAngleAfter == eMinoDirection.West) ||
                (MinoMovementStats.MinoAngleBefore == eMinoDirection.South && MinoMovementStats.MinoAngleAfter == eMinoDirection.West))   // North から West , South から West に回転する時
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
            if ((MinoMovementStats.MinoAngleBefore == eMinoDirection.North && MinoMovementStats.MinoAngleAfter == eMinoDirection.East) ||
                (MinoMovementStats.MinoAngleBefore == eMinoDirection.West && MinoMovementStats.MinoAngleAfter == eMinoDirection.South))  // North から East , West から South に回転する時
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
            else if ((MinoMovementStats.MinoAngleBefore == eMinoDirection.West && MinoMovementStats.MinoAngleAfter == eMinoDirection.North) ||
                (MinoMovementStats.MinoAngleBefore == eMinoDirection.South && MinoMovementStats.MinoAngleAfter == eMinoDirection.East))   // West から North , South から East に回転する時
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
            else if ((MinoMovementStats.MinoAngleBefore == eMinoDirection.East && MinoMovementStats.MinoAngleAfter == eMinoDirection.North) ||
                (MinoMovementStats.MinoAngleBefore == eMinoDirection.South && MinoMovementStats.MinoAngleAfter == eMinoDirection.West))   // East から North , South から West に回転する時
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
            else if ((MinoMovementStats.MinoAngleBefore == eMinoDirection.North && MinoMovementStats.MinoAngleAfter == eMinoDirection.West) ||
                (MinoMovementStats.MinoAngleBefore == eMinoDirection.East && MinoMovementStats.MinoAngleAfter == eMinoDirection.South))   // North から West , East から South に回転する時
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

        LogHelper.Log(LogHelper.eLogLevel.Debug, "Mino", "SuperRotationSystem()", "End");
        return success; // SRSが成功したかどうかを返す
    }

    /// <summary> SRSの各ステップを試す関数 </summary>
    /// <param name="rotationSteps"> 各ステップのアクションリスト </param>
    /// <param name="direction"> 方向の説明 </param>
    /// <returns> 成功したかどうか(true or false) </returns>
    private bool TrySuperRotation(List<Action> rotationSteps, string direction)
    {
        Vector3 originalPosition = spawner.ActiveMino.transform.position; // 現在の位置を保存

        for (int step = 0; step < rotationSteps.Count; step++)
        {
            MinoMovementStats.Update(_stepsSRS: MinoMovementStats.StepsSRS + 1);
            rotationSteps[step]();

            if (board.CheckPosition(spawner.ActiveMino))
            {
                LogHelper.Log(LogHelper.eLogLevel.Info, "Mino", "TrySuperRotation()", $"Success SRS = {MinoMovementStats.StepsSRS}, {direction}");
                LogHelper.Log(LogHelper.eLogLevel.Debug, "Mino", "TrySuperRotation()", "End");
                return true;
            }
        }

        // 全てのステップが失敗した場合、回転前の状態に戻す
        spawner.ActiveMino.transform.position = originalPosition;
        ResetRotate();
        LogHelper.Log(LogHelper.eLogLevel.Info, "Mino", "TrySuperRotation()", $"Failure SRS, {direction}");
        LogHelper.Log(LogHelper.eLogLevel.Debug, "Mino", "TrySuperRotation()", "End");
        return false;
    }

    /// <summary> MinoAngleAfter の値を返す関数 </summary>
    /// <returns> MinoAngleAfter(eMinoDirection) </returns>
    public eMinoDirection GetMinoAngleAfter()
    {
        return MinoMovementStats.MinoAngleAfter;
    }

    /// <summary> MinoAngleBefore の値を返す関数 </summary>
    /// <returns> MinoAngleBefore(eMinoDirection) </returns>
    public eMinoDirection GetMinoAngleBefore()
    {
        return MinoMovementStats.MinoAngleBefore;
    }

    /// <summary> StepsSRS の値を返す関数 </summary>
    /// <returns> StepsSRS(int) </returns>
    public int GetStepsSRS()
    {
        return MinoMovementStats.StepsSRS;
    }
}

/////////////////// 旧コード ///////////////////

// // ミノの向き //
// private string North = "North";
// private string East = "East";
// private string South = "South";
// private string West = "West";

// // 回転方向 //
// private string UseRotateRight = "RotateRight";
// private string UseRotateLeft = "RotateLeft";

// public bool SuperRotationSystem()
// {
//     LogHelper.Log(LogHelper.eLogLevel.Debug, "Mino", "SuperRotationSystem()", "Start");

//     // SRSはIミノとそれ以外のミノとで処理が違うため分けて処理する
//     // Iミノ以外のSRS
//     if (spawner.ActiveMinoName != "I_Mino")
//     {
//         //Debug.Log("Iミノ以外のSRS");

//         if ((MinoMovementStats.MinoAngleBefore == North && MinoMovementStats.MinoAngleAfter == East) ||
//             (MinoMovementStats.MinoAngleBefore == South && MinoMovementStats.MinoAngleAfter == East))   // North から East , South から East に回転する時
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

//                             LogHelper.Log("Failure SRS, NtoE, StoE", LogHelper.eLogLevel.Debug, "Mino", "SuperRotationSystem()");

//                             return false; // SRSができなかった時、falseを返す
//                         }
//                         LogHelper.Log("Success SRS = 4, NtoE, StoE", LogHelper.eLogLevel.Debug, "Mino", "SuperRotationSystem()");
//                     }
//                     LogHelper.Log("Success SRS = 3, NtoE, StoE", LogHelper.eLogLevel.Debug, "Mino", "SuperRotationSystem()");
//                 }
//                 LogHelper.Log("Success SRS = 2, NtoE, StoE", LogHelper.eLogLevel.Debug, "Mino", "SuperRotationSystem()");
//             }
//             LogHelper.Log("Success SRS = 1, NtoE, StoE", LogHelper.eLogLevel.Debug, "Mino", "SuperRotationSystem()");
//         }
//         else if ((MinoMovementStats.MinoAngleBefore == West && MinoMovementStats.MinoAngleAfter == North) ||
//             (MinoMovementStats.MinoAngleBefore == West && MinoMovementStats.MinoAngleAfter == South))   // West から North , West から South に回転する時
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

//                             LogHelper.Log("Failure SRS, WtoN, WtoS", LogHelper.eLogLevel.Debug, "Mino", "SuperRotationSystem()");

//                             return false; // SRSができなかった時、falseを返す
//                         }
//                         LogHelper.Log("Success SRS = 4, WtoN, WtoS", LogHelper.eLogLevel.Debug, "Mino", "SuperRotationSystem()");
//                     }
//                     LogHelper.Log("Success SRS = 3, WtoN, WtoS", LogHelper.eLogLevel.Debug, "Mino", "SuperRotationSystem()");
//                 }
//                 LogHelper.Log("Success SRS = 2, WtoN, WtoS", LogHelper.eLogLevel.Debug, "Mino", "SuperRotationSystem()");
//             }
//             LogHelper.Log("Success SRS = 1, WtoN, WtoS", LogHelper.eLogLevel.Debug, "Mino", "SuperRotationSystem()");
//         }
//         else if ((MinoMovementStats.MinoAngleBefore == East && MinoMovementStats.MinoAngleAfter == North) ||
//             (MinoMovementStats.MinoAngleBefore == East && MinoMovementStats.MinoAngleAfter == South))   // East から North , East から South に回転する時
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

//                             LogHelper.Log("Failure SRS, EtoN, EtoS", LogHelper.eLogLevel.Debug, "Mino", "SuperRotationSystem()");

//                             return false; // SRSができなかった時、falseを返す
//                         }
//                         LogHelper.Log("Success SRS = 4, EtoN, EtoS", LogHelper.eLogLevel.Debug, "Mino", "SuperRotationSystem()");
//                     }
//                     LogHelper.Log("Success SRS = 3, EtoN, EtoS", LogHelper.eLogLevel.Debug, "Mino", "SuperRotationSystem()");
//                 }
//                 LogHelper.Log("Success SRS = 2, EtoN, EtoS", LogHelper.eLogLevel.Debug, "Mino", "SuperRotationSystem()");
//             }
//             LogHelper.Log("Success SRS = 1, EtoN, EtoS", LogHelper.eLogLevel.Debug, "Mino", "SuperRotationSystem()");
//         }
//         else if ((MinoMovementStats.MinoAngleBefore == North && MinoMovementStats.MinoAngleAfter == West) ||
//             (MinoMovementStats.MinoAngleBefore == South && MinoMovementStats.MinoAngleAfter == West))   // North から West , South から West に回転する時
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

//                             LogHelper.Log("Failure SRS, NtoW, StoW", LogHelper.eLogLevel.Debug, "Mino", "SuperRotationSystem()");

//                             return false; // SRSができなかった時、falseを返す
//                         }
//                         LogHelper.Log("Success SRS = 4, NtoW, StoW", LogHelper.eLogLevel.Debug, "Mino", "SuperRotationSystem()");
//                     }
//                     LogHelper.Log("Success SRS = 3, NtoW, StoW", LogHelper.eLogLevel.Debug, "Mino", "SuperRotationSystem()");
//                 }
//                 LogHelper.Log("Success SRS = 2, NtoW, StoW", LogHelper.eLogLevel.Debug, "Mino", "SuperRotationSystem()");
//             }
//             LogHelper.Log("Success SRS = 1, NtoW, StoW", LogHelper.eLogLevel.Debug, "Mino", "SuperRotationSystem()");
//         }
//     }
//     // IミノのSRS(かなり複雑)
//     else
//     {
//         //Debug.Log("IミノのSRS");

//         if ((MinoMovementStats.MinoAngleBefore == North && MinoMovementStats.MinoAngleAfter == East) ||
//             (MinoMovementStats.MinoAngleBefore == West && MinoMovementStats.MinoAngleAfter == South))  // North から East , West から South に回転する時
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

//                             LogHelper.Log("Failure SRS, NtoE, WtoS, I", LogHelper.eLogLevel.Debug, "Mino", "SuperRotationSystem()");

//                             return false; // SRSができなかった時、falseを返す
//                         }
//                         LogHelper.Log("Success SRS = 4, NtoE, WtoS, I", LogHelper.eLogLevel.Debug, "Mino", "SuperRotationSystem()");
//                     }
//                     LogHelper.Log("Success SRS = 3, NtoE, WtoS, I", LogHelper.eLogLevel.Debug, "Mino", "SuperRotationSystem()");
//                 }
//                 LogHelper.Log("Success SRS = 2, NtoE, WtoS, I", LogHelper.eLogLevel.Debug, "Mino", "SuperRotationSystem()");
//             }
//             LogHelper.Log("Success SRS = 1, NtoE, WtoS, I", LogHelper.eLogLevel.Debug, "Mino", "SuperRotationSystem()");
//         }
//         else if ((MinoMovementStats.MinoAngleBefore == West && MinoMovementStats.MinoAngleAfter == North) ||
//             (MinoMovementStats.MinoAngleBefore == South && MinoMovementStats.MinoAngleAfter == East))   // West から North , South から East に回転する時
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

//                             LogHelper.Log("Failure SRS, WtoN, StoE, I", LogHelper.eLogLevel.Debug, "Mino", "SuperRotationSystem()");

//                             return false; // SRSができなかった時、falseを返す
//                         }
//                         LogHelper.Log("Success SRS = 4, WtoN, StoE, I", LogHelper.eLogLevel.Debug, "Mino", "SuperRotationSystem()");
//                     }
//                     LogHelper.Log("Success SRS = 3, WtoN, StoE, I", LogHelper.eLogLevel.Debug, "Mino", "SuperRotationSystem()");
//                 }
//                 LogHelper.Log("Success SRS = 2, WtoN, StoE, I", LogHelper.eLogLevel.Debug, "Mino", "SuperRotationSystem()");
//             }
//             LogHelper.Log("Success SRS = 1, WtoN, StoE, I", LogHelper.eLogLevel.Debug, "Mino", "SuperRotationSystem()");
//         }
//         else if ((MinoMovementStats.MinoAngleBefore == East && MinoMovementStats.MinoAngleAfter == North) ||
//             (MinoMovementStats.MinoAngleBefore == South && MinoMovementStats.MinoAngleAfter == West))   // East から North , South から West に回転する時
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

//                             LogHelper.Log("Failure SRS, EtoN, StoW, I", LogHelper.eLogLevel.Debug, "Mino", "SuperRotationSystem()");

//                             return false; // SRSができなかった時、falseを返す
//                         }
//                         LogHelper.Log("Success SRS = 4, EtoN, StoW, I", LogHelper.eLogLevel.Debug, "Mino", "SuperRotationSystem()");
//                     }
//                     LogHelper.Log("Success SRS = 3, EtoN, StoW, I", LogHelper.eLogLevel.Debug, "Mino", "SuperRotationSystem()");
//                 }
//                 LogHelper.Log("Success SRS = 2, EtoN, StoW, I", LogHelper.eLogLevel.Debug, "Mino", "SuperRotationSystem()");
//             }
//             LogHelper.Log("Success SRS = 1, EtoN, StoW, I", LogHelper.eLogLevel.Debug, "Mino", "SuperRotationSystem()");
//         }
//         else if ((MinoMovementStats.MinoAngleBefore == North && MinoMovementStats.MinoAngleAfter == West) ||
//             (MinoMovementStats.MinoAngleBefore == East && MinoMovementStats.MinoAngleAfter == South))   // North から West , East から South に回転する時
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

//                             LogHelper.Log(LogHelper.eLogLevel.Debug, "Mino", "SuperRotationSystem()", "End");

//                             return false; // SRSができなかった時、falseを返す
//                         }
//                         LogHelper.Log(LogHelper.eLogLevel.Info, "Mino", "SuperRotationSystem()", "Success SRS = 4, NtoW, EtoS, I");
//                     }
//                     LogHelper.Log(LogHelper.eLogLevel.Info, "Mino", "SuperRotationSystem()", "Success SRS = 3, NtoW, EtoS, I");
//                 }
//                 LogHelper.Log(LogHelper.eLogLevel.Info, "Mino", "SuperRotationSystem()", "Success SRS = 2, NtoW, EtoS, I");
//             }
//             LogHelper.Log(LogHelper.eLogLevel.Info, "Mino", "SuperRotationSystem()", "Success SRS = 1, NtoW, EtoS, I");
//         }
//     }
//     LogHelper.Log(LogHelper.eLogLevel.Debug, "Mino", "SuperRotationSystem()", "End");
//     return true; // SRSができた時、trueを返す
// }


/// <summary>
// /// ミノの統計情報を保持する構造体
// /// </summary>
// public struct MinoMovementStats
// {
//     /// <summary> ミノの回転後の向き </summary>
//     /// <remarks>
//     /// 初期値はNorthの状態 <br/>
//     /// Spin判定を確認する際、回転後の向きと回転前の向きの情報が必要なため
//     /// </remarks>
//     private eMinoDirection minoAngleAfter;

//     /// <summary>ミノの回転前の向き
//     /// <remarks>
//     /// 初期値はNorthの状態 <br/>
//     /// Spin判定を確認する際、回転後の向きと回転前の向きの情報が必要なため
//     /// </remarks>
//     private eMinoDirection minoAngleBefore;

//     /// <summary> スーパーローテーションシステム(SRS)の段階 </summary>
//     /// <remarks>
//     /// SRSが使用されていないときは0, 1〜4の時は、SRSの段階を表す
//     /// </remarks>
//     /// <value> 0~4 </value>
//     [SerializeField] private int stepsSRS;

//     // ゲッタープロパティ //
//     public eMinoDirection MinoAngleAfter => minoAngleAfter;
//     public eMinoDirection MinoAngleBefore => minoAngleBefore;
//     public int StepsSRS => stepsSRS;

//     /// <summary> デフォルトコンストラクタ </summary>
//     public MinoMovementStats(eMinoDirection _minoAngleAfter, eMinoDirection _minoAngleBefore, int _stepsSRS)
//     {
//         minoAngleAfter = _minoAngleAfter;
//         minoAngleBefore = _minoAngleBefore;
//         stepsSRS = _stepsSRS;
//     }

//     /// <summary> デフォルトの <see cref="MinoMovementStats"/> を作成する関数 </summary>
//     /// <returns>
//     /// デフォルト値で初期化された <see cref="MinoMovementStats"/> のインスタンス
//     /// </returns>
//     public static MinoMovementStats CreateDefault()
//     {
//         return new MinoMovementStats
//         {
//             minoAngleAfter = eMinoDirection.North,
//             minoAngleBefore = eMinoDirection.North,
//             stepsSRS = 0
//         };
//     }

//     /// <summary> 指定されたフィールドの値を更新する関数 </summary>
//     /// <param name="_minoAngleAfter"> ミノの回転後の向き </param>
//     /// <param name="_minoAngleBefore"> ミノの回転前の向き </param>
//     /// <param name="_stepsSRS"> SRSの段階 </param>
//     /// <returns> 更新された <see cref="MinoMovementStats"/> の新しいインスタンス </returns>
//     /// <remarks>
//     /// 指定されていない引数は現在の値を維持
//     /// </remarks>
//     public MinoMovementStats Update(eMinoDirection? _minoAngleAfter = null, eMinoDirection? _minoAngleBefore = null, int? _stepsSRS = null)
//     {
//         var updatedStats = new MinoMovementStats(
//             _minoAngleAfter ?? minoAngleAfter,
//             _minoAngleBefore ?? minoAngleBefore,
//             _stepsSRS ?? stepsSRS
//         );
//         // TODO: ログの記入
//         return updatedStats;
//     }
// }

/////////////////////////////////////////////////////////