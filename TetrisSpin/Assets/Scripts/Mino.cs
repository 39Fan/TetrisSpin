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
        DebugHelper.Log("Start", DebugHelper.LogLevel.Debug, "Mino", "MoveLeft()");

        Move(new Vector3(-1, 0, 0));
    }

    public void MoveRight()
    {
        DebugHelper.Log("Start", DebugHelper.LogLevel.Debug, "Mino", "MoveRight()");

        Move(new Vector3(1, 0, 0));
    }

    public void MoveUp()
    {
        DebugHelper.Log("Start", DebugHelper.LogLevel.Debug, "Mino", "MoveUp()");

        Move(new Vector3(0, 1, 0));
    }

    public void MoveDown()
    {
        DebugHelper.Log("Start", DebugHelper.LogLevel.Debug, "Mino", "MoveDown()");

        Move(new Vector3(0, -1, 0));
    }

    // 通常右回転 //
    public void RotateRight()
    {
        DebugHelper.Log("Start", DebugHelper.LogLevel.Debug, "Mino", "RotateRight()");

        if (CanRotate == false) // 回転できないブロックの場合
        {
            return; // Oミノは回転できないので弾かれる
        }

        if (spawner.activeMinoName != "I_Mino") // Iミノ以外の右回転
        {
            transform.Rotate(0, 0, RotateRightAroundZ);
        }
        else // Iミノは軸が他のミノと違うため別の処理
        {
            // Iミノの軸を取得する
            Vector3 IminoAxis = AxisCheck_ForI
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
        DebugHelper.Log("Start", DebugHelper.LogLevel.Debug, "Mino", "Rotateleft()");

        if (CanRotate == false) // 回転できないブロックの場合
        {
            return; // Oミノは回転できないので弾かれる
        }

        // Iミノ以外の左回転
        if (spawner.activeMinoName != "I_Mino")
        {
            transform.Rotate(0, 0, RotateLeftAroundZ);
        }

        // Iミノは軸が他のミノと違うため別の処理
        else
        {
            // Iミノの軸を取得する
            Vector3 IminoAxis = AxisCheck_ForI
                (Mathf.RoundToInt(spawner.activeMino.transform.position.x), Mathf.RoundToInt(spawner.activeMino.transform.position.y));

            // IminoAxisを中心に左回転する
            transform.RotateAround(IminoAxis, Vector3.forward, RotateLeftAroundZ);
        }

        // ミノの角度の調整(左回転)
        gameStatus.UpdateMinoAngleAfter(UseRotateLeft);
    }

    // Iミノの軸を計算し、Vectoe3で返す関数 //
    public Vector3 AxisCheck_ForI(int Imino_x, int Imino_y) // Imino_x と Imino_y はIミノのx, y座標
    {
        DebugHelper.Log("Start", DebugHelper.LogLevel.Debug, "Mino", "AxisCheckt()");

        // x軸とy軸のオフセットを宣言
        float xOffset = 0.5f;
        float yOffset = 0.5f;

        string infoMessage; // Infoログ用

        // 回転軸は現在位置から、x軸を xOffset 動かし、y軸を yOffset 動かした座標にある
        // xOffset と yOffset の正負は回転前の向きによって変化する

        // 向きがNorthの時
        if (gameStatus.minoAngleAfter == gameStatus.North)
        {
            infoMessage = $"North: Axis = ({Imino_x + xOffset}, {Imino_y - yOffset})";
            DebugHelper.Log(infoMessage, DebugHelper.LogLevel.Info, "Mino", "AxisCheck_ForI()"); // Infoログ

            return new Vector3(Imino_x + xOffset, Imino_y - yOffset, 0);
        }
        // 向きがEastの時
        else if (gameStatus.minoAngleAfter == gameStatus.East)
        {
            infoMessage = $"East: Axis = ({Imino_x - xOffset}, {Imino_y - yOffset})";
            DebugHelper.Log(infoMessage, DebugHelper.LogLevel.Info, "Mino", "AxisCheck_ForI()"); // Infoログ

            return new Vector3(Imino_x - xOffset, Imino_y - yOffset, 0);
        }
        // 向きがSouthの時
        else if (gameStatus.minoAngleAfter == gameStatus.South)
        {
            infoMessage = $"South: Axis = ({Imino_x - xOffset}, {Imino_y + yOffset})";
            DebugHelper.Log(infoMessage, DebugHelper.LogLevel.Info, "Mino", "AxisCheck_ForI()"); // Infoログ

            return new Vector3(Imino_x - xOffset, Imino_y + yOffset, 0);
        }
        // 向きがWestの時
        else
        {
            infoMessage = $"West: Axis = ({Imino_x + xOffset}, {Imino_y + yOffset})";
            DebugHelper.Log(infoMessage, DebugHelper.LogLevel.Info, "Mino", "AxisCheck_ForI()"); // Infoログ

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
        DebugHelper.Log("Start", DebugHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");

        // SRSはIミノとそれ以外のミノとで処理が違うため分けて処理する
        // Iミノ以外のSRS
        if (spawner.activeMinoName != "I_Mino")
        {
            //Debug.Log("Iミノ以外のSRS");

            if ((gameStatus.minoAngleBefore == gameStatus.North && gameStatus.minoAngleAfter == gameStatus.East) ||
                (gameStatus.minoAngleBefore == gameStatus.South && gameStatus.minoAngleAfter == gameStatus.East))   // North から East , South から East に回転する時
            {
                // 第一法則
                spawner.activeMino.MoveLeft(); // 左に1つ移動

                // SRSの段階数を格納(TspinMiniの判定に必要)
                gameStatus.IncreaseLastSRS(); // 1

                if (!board.CheckPosition(spawner.activeMino))
                {
                    // 第二法則
                    spawner.activeMino.MoveUp(); // 上に1つ移動

                    gameStatus.IncreaseLastSRS(); // 2

                    if (!board.CheckPosition(spawner.activeMino))
                    {
                        // 第三法則
                        spawner.activeMino.MoveRight(); // 右に1つ移動
                        spawner.activeMino.MoveDown();
                        spawner.activeMino.MoveDown();
                        spawner.activeMino.MoveDown(); // 下に3つ移動

                        gameStatus.IncreaseLastSRS(); // 3

                        if (!board.CheckPosition(spawner.activeMino))
                        {
                            // 第四法則
                            spawner.activeMino.MoveLeft(); // 左に1つ移動

                            gameStatus.IncreaseLastSRS(); // 4

                            if (!board.CheckPosition(spawner.activeMino))
                            {
                                // SRSができなかった時、回転前の状態に戻る
                                spawner.activeMino.MoveRight();
                                spawner.activeMino.MoveUp();
                                spawner.activeMino.MoveUp();

                                gameStatus.Reset_Rotate(); // 通常回転のリセット

                                DebugHelper.Log("Failure SRS, N to E, S to E", DebugHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");

                                return false; // SRSができなかった時、falseを返す
                            }
                            DebugHelper.Log("Success SRS = 4, N to E, S to E", DebugHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
                        }
                        DebugHelper.Log("Success SRS = 3, N to E, S to E", DebugHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
                    }
                    DebugHelper.Log("Success SRS = 2, N to E, S to E", DebugHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
                }
                DebugHelper.Log("Success SRS = 1, N to E, S to E", DebugHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
            }
            else if ((gameStatus.minoAngleBefore == gameStatus.West && gameStatus.minoAngleAfter == gameStatus.North) ||
                (gameStatus.minoAngleBefore == gameStatus.West && gameStatus.minoAngleAfter == gameStatus.South))   // West から North , West から South に回転する時
            {
                // 第一法則
                spawner.activeMino.MoveLeft(); // 左に1つ移動

                // SRSの段階数を格納(TspinMiniの判定に必要)
                gameStatus.IncreaseLastSRS(); // 1

                if (!board.CheckPosition(spawner.activeMino))
                {
                    // 第二法則
                    spawner.activeMino.MoveDown(); // 下に1つ移動

                    gameStatus.IncreaseLastSRS(); // 2

                    if (!board.CheckPosition(spawner.activeMino))
                    {
                        // 第三法則
                        spawner.activeMino.MoveRight(); // 右に1つ移動
                        spawner.activeMino.MoveUp();
                        spawner.activeMino.MoveUp();
                        spawner.activeMino.MoveUp(); // 上に3つ移動

                        gameStatus.IncreaseLastSRS(); // 3

                        if (!board.CheckPosition(spawner.activeMino))
                        {
                            // 第四法則
                            spawner.activeMino.MoveLeft(); // 左に1つ移動

                            gameStatus.IncreaseLastSRS(); // 4

                            if (!board.CheckPosition(spawner.activeMino))
                            {
                                //SRSができなかった際に回転前の状態に戻る
                                spawner.activeMino.MoveRight();
                                spawner.activeMino.MoveDown();
                                spawner.activeMino.MoveDown();

                                gameStatus.Reset_Rotate(); // 通常回転のリセット

                                DebugHelper.Log("Failure SRS, W to N, W to S", DebugHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");

                                return false; // SRSができなかった時、falseを返す
                            }
                            DebugHelper.Log("Success SRS = 4, W to N, W to S", DebugHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
                        }
                        DebugHelper.Log("Success SRS = 3, W to N, W to S", DebugHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
                    }
                    DebugHelper.Log("Success SRS = 2, W to N, W to S", DebugHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
                }
                DebugHelper.Log("Success SRS = 1, W to N, W to S", DebugHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
            }
            else if ((gameStatus.minoAngleBefore == gameStatus.East && gameStatus.minoAngleAfter == gameStatus.North) ||
                (gameStatus.minoAngleBefore == gameStatus.East && gameStatus.minoAngleAfter == gameStatus.South))   // East から North , East から South に回転する時
            {
                // 第一法則
                spawner.activeMino.MoveRight(); // 右に1つ移動

                // SRSの段階数を格納(TspinMiniの判定に必要)
                gameStatus.IncreaseLastSRS(); // 1

                if (!board.CheckPosition(spawner.activeMino))
                {
                    // 第二法則
                    spawner.activeMino.MoveDown(); // 下に1つ移動

                    gameStatus.IncreaseLastSRS(); // 2

                    if (!board.CheckPosition(spawner.activeMino))
                    {
                        // 第三法則
                        spawner.activeMino.MoveLeft(); // 左に1つ移動
                        spawner.activeMino.MoveUp();
                        spawner.activeMino.MoveUp();
                        spawner.activeMino.MoveUp(); // 上に3つ移動

                        gameStatus.IncreaseLastSRS(); // 3

                        if (!board.CheckPosition(spawner.activeMino))
                        {
                            // 第四法則
                            spawner.activeMino.MoveRight(); // 右に1つ移動

                            gameStatus.IncreaseLastSRS(); // 4

                            if (!board.CheckPosition(spawner.activeMino))
                            {
                                // SRSができなかった際に回転前の状態に戻る
                                spawner.activeMino.MoveLeft();
                                spawner.activeMino.MoveDown();
                                spawner.activeMino.MoveDown();

                                gameStatus.Reset_Rotate(); // 通常回転のリセット

                                DebugHelper.Log("Failure SRS, E to N, E to S", DebugHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");

                                return false; // SRSができなかった時、falseを返す
                            }
                            DebugHelper.Log("Success SRS = 1, E to N, E to S", DebugHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
                        }
                        DebugHelper.Log("Success SRS = 1, E to N, E to S", DebugHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
                    }
                    DebugHelper.Log("Success SRS = 1, E to N, E to S", DebugHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
                }
                DebugHelper.Log("Success SRS = 1, E to N, E to S", DebugHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
            }
            else if ((gameStatus.minoAngleBefore == gameStatus.North && gameStatus.minoAngleAfter == gameStatus.West) ||
                (gameStatus.minoAngleBefore == gameStatus.South && gameStatus.minoAngleAfter == gameStatus.West))   // North から West , South から West に回転する時
            {
                // 第一法則
                spawner.activeMino.MoveRight(); // 右に1つ移動

                // SRSの段階数を格納(TspinMiniの判定に必要)
                gameStatus.IncreaseLastSRS(); // 1

                if (!board.CheckPosition(spawner.activeMino))
                {
                    // 第二法則
                    spawner.activeMino.MoveUp(); // 上に1つ移動

                    gameStatus.IncreaseLastSRS(); // 2

                    if (!board.CheckPosition(spawner.activeMino))
                    {
                        // 第三法則
                        spawner.activeMino.MoveLeft(); // 左に1つ移動
                        spawner.activeMino.MoveDown();
                        spawner.activeMino.MoveDown();
                        spawner.activeMino.MoveDown(); // 下に3つ移動

                        gameStatus.IncreaseLastSRS(); // 3

                        if (!board.CheckPosition(spawner.activeMino))
                        {
                            // 第四法則
                            spawner.activeMino.MoveRight(); // 右に1つ移動

                            gameStatus.IncreaseLastSRS(); // 4

                            if (!board.CheckPosition(spawner.activeMino))
                            {
                                // SRSができなかった時、通常回転前の状態に戻る
                                spawner.activeMino.MoveLeft();
                                spawner.activeMino.MoveUp();
                                spawner.activeMino.MoveUp();

                                gameStatus.Reset_Rotate(); // 通常回転のリセット

                                DebugHelper.Log("Failure SRS, N to W, S to W", DebugHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");

                                return false; // SRSができなかった時、falseを返す
                            }
                            DebugHelper.Log("Success SRS = 4, N to W, S to W", DebugHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
                        }
                        DebugHelper.Log("Success SRS = 3, N to W, S to W", DebugHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
                    }
                    DebugHelper.Log("Success SRS = 2, N to W, S to W", DebugHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
                }
                DebugHelper.Log("Success SRS = 1, N to W, S to W", DebugHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
            }
        }
        // IミノのSRS(かなり複雑)
        else
        {
            //Debug.Log("IミノのSRS");

            if ((gameStatus.minoAngleBefore == gameStatus.North && gameStatus.minoAngleAfter == gameStatus.East) ||
                (gameStatus.minoAngleBefore == gameStatus.West && gameStatus.minoAngleAfter == gameStatus.South))  // North から East , West から South に回転する時
            {
                // 第一法則
                spawner.activeMino.MoveLeft();
                spawner.activeMino.MoveLeft(); // 左に2つ移動

                if (!board.CheckPosition(spawner.activeMino))
                {
                    // 第二法則
                    spawner.activeMino.MoveRight();
                    spawner.activeMino.MoveRight();
                    spawner.activeMino.MoveRight(); // 右に3つ移動

                    if (!board.CheckPosition(spawner.activeMino))
                    {
                        // 第三法則
                        spawner.activeMino.MoveLeft();
                        spawner.activeMino.MoveLeft();
                        spawner.activeMino.MoveLeft(); // 左に3つ移動
                        spawner.activeMino.MoveDown(); // 下に1つ移動

                        if (!board.CheckPosition(spawner.activeMino))
                        {
                            // 第四法則
                            spawner.activeMino.MoveRight();
                            spawner.activeMino.MoveRight();
                            spawner.activeMino.MoveRight(); // 右に3つ移動
                            spawner.activeMino.MoveUp();
                            spawner.activeMino.MoveUp();
                            spawner.activeMino.MoveUp(); // 上に3つ移動

                            if (!board.CheckPosition(spawner.activeMino))
                            {
                                // SRSができなかった際に回転前の状態に戻る
                                spawner.activeMino.MoveLeft();
                                spawner.activeMino.MoveDown();
                                spawner.activeMino.MoveDown();

                                gameStatus.Reset_Rotate(); // 通常回転のリセット

                                DebugHelper.Log("Failure SRS, N to E, W to S, I", DebugHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");

                                return false; // SRSができなかった時、falseを返す
                            }
                            DebugHelper.Log("Success SRS = 4, N to E, W to S, I", DebugHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
                        }
                        DebugHelper.Log("Success SRS = 3, N to E, W to S, I", DebugHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
                    }
                    DebugHelper.Log("Success SRS = 2, N to E, W to S, I", DebugHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
                }
                DebugHelper.Log("Success SRS = 1, N to E, W to S, I", DebugHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
            }
            else if ((gameStatus.minoAngleBefore == gameStatus.West && gameStatus.minoAngleAfter == gameStatus.North) ||
                (gameStatus.minoAngleBefore == gameStatus.South && gameStatus.minoAngleAfter == gameStatus.East))   // West から North , South から East に回転する時
            {
                // 第一法則
                spawner.activeMino.MoveRight(); // 右に1つ移動

                if (!board.CheckPosition(spawner.activeMino))
                {
                    // 第二法則
                    spawner.activeMino.MoveLeft();
                    spawner.activeMino.MoveLeft();
                    spawner.activeMino.MoveLeft(); // 左に3つ移動

                    if (!board.CheckPosition(spawner.activeMino))
                    {
                        // 第三法則
                        spawner.activeMino.MoveRight();
                        spawner.activeMino.MoveRight();
                        spawner.activeMino.MoveRight(); // 右に3つ移動
                        spawner.activeMino.MoveDown();
                        spawner.activeMino.MoveDown(); // 下に2つ移動

                        if (!board.CheckPosition(spawner.activeMino))
                        {
                            // 第四法則
                            spawner.activeMino.MoveLeft();
                            spawner.activeMino.MoveLeft();
                            spawner.activeMino.MoveLeft(); // 左に3つ移動
                            spawner.activeMino.MoveUp();
                            spawner.activeMino.MoveUp();
                            spawner.activeMino.MoveUp(); // 上に3つ移動

                            if (!board.CheckPosition(spawner.activeMino))
                            {
                                // SRSができなかった際に回転前の状態に戻る
                                spawner.activeMino.MoveRight();
                                spawner.activeMino.MoveRight();
                                spawner.activeMino.MoveDown();

                                gameStatus.Reset_Rotate(); // 通常回転のリセット

                                DebugHelper.Log("Failure SRS, W to N, S to E, I", DebugHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");

                                return false; // SRSができなかった時、falseを返す
                            }
                            DebugHelper.Log("Success SRS = 4, W to N, S to E, I", DebugHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
                        }
                        DebugHelper.Log("Success SRS = 3, W to N, S to E, I", DebugHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
                    }
                    DebugHelper.Log("Success SRS = 2, W to N, S to E, I", DebugHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
                }
                DebugHelper.Log("Success SRS = 1, W to N, S to E, I", DebugHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
            }
            else if ((gameStatus.minoAngleBefore == gameStatus.East && gameStatus.minoAngleAfter == gameStatus.North) ||
                (gameStatus.minoAngleBefore == gameStatus.South && gameStatus.minoAngleAfter == gameStatus.West))   // East から North , South から West に回転する時
            {
                // 第一法則
                spawner.activeMino.MoveRight();
                spawner.activeMino.MoveRight(); // 右に2つ移動

                if (!board.CheckPosition(spawner.activeMino))
                {
                    // 第二法則
                    spawner.activeMino.MoveLeft();
                    spawner.activeMino.MoveLeft();
                    spawner.activeMino.MoveLeft(); // 左に3つ移動

                    if (!board.CheckPosition(spawner.activeMino))
                    {
                        // 第三法則
                        spawner.activeMino.MoveRight();
                        spawner.activeMino.MoveRight();
                        spawner.activeMino.MoveRight(); // 右に3つ移動
                        spawner.activeMino.MoveUp(); // 上に1つ移動

                        if (!board.CheckPosition(spawner.activeMino))
                        {
                            // 第四法則
                            spawner.activeMino.MoveLeft();
                            spawner.activeMino.MoveLeft();
                            spawner.activeMino.MoveLeft(); // 左に3つ移動
                            spawner.activeMino.MoveDown();
                            spawner.activeMino.MoveDown();
                            spawner.activeMino.MoveDown(); // 下に3つ移動

                            if (!board.CheckPosition(spawner.activeMino))
                            {
                                // SRSができなかった際に回転前の状態に戻る
                                spawner.activeMino.MoveRight();
                                spawner.activeMino.MoveUp();
                                spawner.activeMino.MoveUp();

                                gameStatus.Reset_Rotate(); // 通常回転のリセット

                                DebugHelper.Log("Failure SRS, E to N, S to W, I", DebugHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");

                                return false; // SRSができなかった時、falseを返す
                            }
                            DebugHelper.Log("Success SRS = 4, E to N, S to W, I", DebugHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
                        }
                        DebugHelper.Log("Success SRS = 3, E to N, S to W, I", DebugHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
                    }
                    DebugHelper.Log("Success SRS = 2, E to N, S to W, I", DebugHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
                }
                DebugHelper.Log("Success SRS = 1, E to N, S to W, I", DebugHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
            }
            else if ((gameStatus.minoAngleBefore == gameStatus.North && gameStatus.minoAngleAfter == gameStatus.West) ||
                (gameStatus.minoAngleBefore == gameStatus.East && gameStatus.minoAngleAfter == gameStatus.South))   // North から West , East から South に回転する時
            {
                // 第一法則
                spawner.activeMino.MoveLeft(); // 左に1つ移動

                if (!board.CheckPosition(spawner.activeMino))
                {
                    // 第二法則
                    spawner.activeMino.MoveRight();
                    spawner.activeMino.MoveRight();
                    spawner.activeMino.MoveRight(); // 右に3つ移動

                    if (!board.CheckPosition(spawner.activeMino))
                    {
                        // 第三法則
                        spawner.activeMino.MoveLeft();
                        spawner.activeMino.MoveLeft();
                        spawner.activeMino.MoveLeft(); // 左に3つ移動
                        spawner.activeMino.MoveUp();
                        spawner.activeMino.MoveUp(); // 上に2つ移動

                        if (!board.CheckPosition(spawner.activeMino))
                        {
                            // 第四法則
                            spawner.activeMino.MoveRight();
                            spawner.activeMino.MoveRight();
                            spawner.activeMino.MoveRight(); // 右に3つ移動
                            spawner.activeMino.MoveDown();
                            spawner.activeMino.MoveDown();
                            spawner.activeMino.MoveDown(); // 下に3つ移動

                            if (!board.CheckPosition(spawner.activeMino))
                            {
                                // SRSができなかった時、回転前の状態に戻る
                                spawner.activeMino.MoveLeft();
                                spawner.activeMino.MoveLeft();
                                spawner.activeMino.MoveUp();

                                gameStatus.Reset_Rotate(); // 通常回転のリセット

                                DebugHelper.Log("Failure SRS, N to W, E to S, I", DebugHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");

                                return false; // SRSができなかった時、falseを返す
                            }
                            DebugHelper.Log("Success SRS = 4, N to W, E to S, I", DebugHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
                        }
                        DebugHelper.Log("Success SRS = 3, N to W, E to S, I", DebugHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
                    }
                    DebugHelper.Log("Success SRS = 2, N to W, E to S, I", DebugHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
                }
                DebugHelper.Log("Success SRS = 1, N to W, E to S, I", DebugHelper.LogLevel.Debug, "Mino", "SuperRotationSystem()");
            }
        }
        return true; // SRSができた時、trueを返す
    }
}
