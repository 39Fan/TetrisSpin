using System;
using System.Collections.Generic;
using UnityEngine;

///// Spin判定に関するスクリプト /////


// ↓このスクリプトで可能なこと↓ //

// 各Spinの判定

public class SpinCheck : MonoBehaviour
{
    // // ミノの向き //
    // private Existence North = "North";
    // private Existence East = "East";
    // private Existence South = "South";
    // private Existence West = "West";

    // Spinの種類 //
    [SerializeField] private SpinTypeNames SpinTypeName;

    // ゲッタープロパティ //
    public SpinTypeNames spinTypeName
    {
        get { return SpinTypeName; }
    }

    // 干渉するスクリプト //
    Board board;
    GameStatus gameStatus;
    Spawner spawner;

    // インスタンス化
    private void Awake()
    {
        board = FindObjectOfType<Board>();
        gameStatus = FindObjectOfType<GameStatus>();
        spawner = FindObjectOfType<Spawner>();
    }

    // SpinTypeNameをリセットする関数 //
    public void Reset_SpinTypeName()
    {
        SpinTypeName = SpinTypeNames.None;
    }

    // 各ミノのスピン判定をチェックする //
    public void CheckSpinType() // この関数は、回転時に呼び出される
    {
        switch (spawner.ActiveMinoName)
        {
            case MinoType.I_Mino:
                LogHelper.Log(LogHelper.LogLevel.Debug, "SpinCheck", "IspinCheck()", "Start");
                IspinCheck();
                LogHelper.Log(LogHelper.LogLevel.Debug, "SpinCheck", "IspinCheck()", "End");
                break;
            case MinoType.J_Mino:
                LogHelper.Log(LogHelper.LogLevel.Debug, "SpinCheck", "JspinCheck()", "Start");
                JspinCheck();
                LogHelper.Log(LogHelper.LogLevel.Debug, "SpinCheck", "JspinCheck()", "End");
                break;
            case MinoType.L_Mino:
                LogHelper.Log(LogHelper.LogLevel.Debug, "SpinCheck", "LspinCheck()", "Start");
                LspinCheck();
                LogHelper.Log(LogHelper.LogLevel.Debug, "SpinCheck", "LspinCheck()", "End");
                break;
            case MinoType.T_Mino:
                LogHelper.Log(LogHelper.LogLevel.Debug, "SpinCheck", "TspinCheck()", "Start");
                TspinCheck();
                LogHelper.Log(LogHelper.LogLevel.Debug, "SpinCheck", "TspinCheck()", "End");
                break;
            default:
                // Debug.LogError("[SpinCheck CheckSpinType()] ActiveMinoNameが特定できません。");
                break;
        }
    }

    // public void CheckSpinType() // この関数は、回転時に呼び出される
    // {
    //     if (spawner.ActiveMinoName == "I_Mino")
    //     {
    //         IspinCheck();
    //     }
    //     if (spawner.ActiveMinoName == "J_Mino")
    //     {
    //         JspinCheck();
    //     }
    //     if (spawner.ActiveMinoName == "L_Mino")
    //     {
    //         LspinCheck();
    //     }
    //     /*else if (mino == O_mino)
    //     {
    //         return OspinCheck(_ActiveMino);
    //     }*/
    //     /*else if (mino == S_mino)
    //     {
    //         return SspinCheck(_ActiveMino);
    //     }*/
    //     if (spawner.ActiveMinoName == "T_Mino")
    //     {
    //         TspinCheck();
    //     }
    //     /*else if (mino == Z_mino)
    //     {
    //         return ZspinCheck(_ActiveMino);
    //     }*/
    //     else
    //     {
    //         // Debug.LogError("[SpinCheck CheckSpinType()] ActiveMinoNameが特定できません。");
    //     }
    // }

    // Jspinの判定をする関数 //
    private void JspinCheck()
    {
        // Jspinの判定をチェックする

        // 条件
        // ①以下の図の指定されたマス(▫️)のうち3つ以上がブロックや壁である
        // ②SRSを使用している(SRSの段階が1以上)

        // North    // East    // South    // West
        //               
        //  ▫️            ▫️                      ▫️
        //  ▪️           ▫️▪️▪️▫️         ▫️          ▪️
        // ▫️▪️◆▪️▫️         ◆        ▫️▪️◆▪️▫️         ◆
        //  ▫️            ▪️           ▪️        ▫️▪️▪️▫️
        //               ▫️           ▫️          ▫️       // ◆はJミノの軸


        // 指定マスのブロックの有無情報を格納するリスト
        List<Existence> checkBlocks_ForJ = new List<Existence>();

        // 各向きに対応したオフセット //
        int[,] northOffset_ForJ = new int[,]
        {
        { 2, 0 },{ -1, -1 },{ -2, 0 },{ -1, 2 }
        };
        int[,] eastOffset_ForJ = new int[,]
        {
        { 2, 1 },{ 0, -2 },{ -1, 1 },{ 0, 2 }
        };
        int[,] southOffset_ForJ = new int[,]
        {
        { 2, 0 },{ 1, -2 },{ -2, 0 },{ 1, 1 }
        };
        int[,] westOffset_ForJ = new int[,]
        {
        { 1, -1 },{ 0, -2 },{ -2, -1 },{ 0, 2 }
        };

        // activeMinoのx,y座標
        int pos_x = Mathf.RoundToInt(spawner.ActiveMino.transform.position.x);
        int pos_y = Mathf.RoundToInt(spawner.ActiveMino.transform.position.y);

        int[,] currentOffsets = null;

        if (gameStatus.minoAngleAfter == MinoDirections.North) // Jミノが北向きの場合
        {
            currentOffsets = northOffset_ForJ;
        }
        else if (gameStatus.minoAngleAfter == MinoDirections.East) // Jミノが東向きの場合
        {
            currentOffsets = eastOffset_ForJ;
        }
        else if (gameStatus.minoAngleAfter == MinoDirections.South) // Jミノが南向きの場合
        {
            currentOffsets = southOffset_ForJ;
        }
        else if (gameStatus.minoAngleAfter == MinoDirections.West) // Jミノが西向きの場合
        {
            currentOffsets = westOffset_ForJ;
        }

        // ①を調べる
        for (int grid = 0; grid < currentOffsets.GetLength(0); grid++)
        {
            if (board.CheckGrid(pos_x + currentOffsets[grid, 0], pos_y + currentOffsets[grid, 1], spawner.ActiveMino))
            {
                checkBlocks_ForJ.Add(Existence.Exist);
            }
            else
            {
                checkBlocks_ForJ.Add(Existence.NotExist);
            }
        }

        // 条件を満たすか確認(②の確認込み)
        if (checkBlocks_ForJ.FindAll(block => block == Existence.Exist).Count >= 3 && gameStatus.stepsSRS >= 1)
        {
            SpinTypeName = SpinTypeNames.J_Spin;
        }
    }

    // Lspinの判定をする関数 //
    private void LspinCheck()
    {
        // Lspinの判定をチェックする

        // 条件
        // ①以下の図の指定されたマス(▫️)のうち3つ以上がブロックや壁である
        // ②SRSを使用している(SRSの段階が1以上)

        // North    // East    // South    // West
        //               
        //    ▫️          ▫️                      ▫️
        //    ▪️          ▪️         ▫️          ▫️▪️▪️▫️
        // ▫️▪️◆▪️▫️         ◆        ▫️▪️◆▪️▫️         ◆
        //    ▫️         ▫️▪️▪️▫️       ▪️            ▪️
        //               ▫️         ▫️            ▫️       // ◆はLミノの軸


        // 指定マスのブロックの有無情報を格納するリスト
        List<Existence> checkBlocks_ForJ = new List<Existence>();

        // 各向きに対応したオフセット //
        int[,] northOffset_ForJ = new int[,]
        {
        { 2, 0 },{ 1, -1 },{ -2, 0 },{ 1, 2 }
        };
        int[,] eastOffset_ForJ = new int[,]
        {
        { 2, -1 },{ 0, -2 },{ -1, -1 },{ 0, 2 }
        };
        int[,] southOffset_ForJ = new int[,]
        {
        { 2, 0 },{ -1, -2 },{ -2, 0 },{ -1, 1 }
        };
        int[,] westOffset_ForJ = new int[,]
        {
        { 1, 1 },{ 0, -2 },{ -2, 1 },{ 0, 2 }
        };

        // ActiveMinoのx,y座標
        int pos_x = Mathf.RoundToInt(spawner.ActiveMino.transform.position.x);
        int pos_y = Mathf.RoundToInt(spawner.ActiveMino.transform.position.y);

        int[,] currentOffsets = null;

        // Lミノの向きで処理を分岐させる
        if (gameStatus.minoAngleAfter == MinoDirections.North)
        {
            currentOffsets = northOffset_ForJ;
        }
        else if (gameStatus.minoAngleAfter == MinoDirections.East)
        {
            currentOffsets = eastOffset_ForJ;
        }
        else if (gameStatus.minoAngleAfter == MinoDirections.South)
        {
            currentOffsets = southOffset_ForJ;
        }
        else if (gameStatus.minoAngleAfter == MinoDirections.West)
        {
            currentOffsets = westOffset_ForJ;
        }

        // ①を調べる
        for (int grid = 0; grid < currentOffsets.GetLength(0); grid++)
        {
            if (board.CheckGrid(pos_x + currentOffsets[grid, 0], pos_y + currentOffsets[grid, 1], spawner.ActiveMino))
            {
                checkBlocks_ForJ.Add(Existence.Exist);
            }
            else
            {
                checkBlocks_ForJ.Add(Existence.NotExist);
            }
        }

        // 条件を満たすか確認(②の確認込み)
        if (checkBlocks_ForJ.FindAll(block => block == Existence.Exist).Count >= 3 && gameStatus.stepsSRS >= 1)
        {
            SpinTypeName = SpinTypeNames.L_Spin;
        }
    }

    // // Lspinの判定をする関数
    // public void LspinCheck()
    // {
    //     // Lspinの判定をチェックする

    //     //////////////////////
    //     // 条件
    //     // ①以下の図の指定されたマス(▫️)のうち3つ以上がブロックや壁である
    //     // ②SRSを使用している(SRSの段階が1以上)

    //     // North    // East    // South    // West
    //     //               
    //     //  ▫️            ▫️                      ▫️
    //     //  ▪️           ▫️▪️▪️▫️         ▫️          ▪️
    //     // ▫️▪️◆▪️▫️         ◆        ▫️▪️◆▪️▫️         ◆
    //     //  ▫️            ▪️           ▪️        ▫️▪️▪️▫️
    //     //               ▫️           ▫️          ▫️       // ◆はJミノの軸


    //     List<Existence> checkBlocks_ForJ = new List<Existence>(); // IspinMiniの判定に必要なリスト

    //     // 各向きに対応したオフセット //
    //     int[,] northOffset_ForJ = new int[,]
    //     {
    //         { 2, 0 },{ -1, -1 },{ -2, 0 },{ -1, 2 }
    //     };
    //     int[,] eastOffset_ForJ = new int[,]
    //     {
    //         { 2, 1 },{ 0, -2 },{ -1, 1 },{ 0, 2 }
    //     };
    //     int[,] southOffset_ForJ = new int[,]
    //     {
    //         { 2, 0 },{ 1, -2 },{ -2, 0 },{ 1, 1 }
    //     };
    //     int[,] westOffset_ForJ = new int[,]
    //     {
    //         { 1, -1 },{ 0, -2 },{ -2, -1 },{ 0, 2 }
    //     };

    //     int pos_x = Mathf.RoundToInt(spawner.activeMino.transform.position.x);
    //     int pos_y = Mathf.RoundToInt(spawner.activeMino.transform.position.y);

    //     if (gameStatus.minoAngleAfter == North) // Jミノが北向きの場合
    //     {
    //         // 指定のマスにブロックまたは壁があるか調べる
    //         for (int grid = 0; grid < northOffset_ForJ.Length; grid++)
    //         {
    //             if (board.CheckGrid(pos_x + northOffset_ForJ[grid, 0], pos_y + northOffset_ForJ[grid, 1], spawner.activeMino))
    //             {
    //                 checkBlocks_ForJ.Add("Exist");
    //             }
    //             else
    //             {
    //                 checkBlocks_ForJ.Add(Existence.NotExist);
    //             }
    //         }
    //     }
    //     if (gameStatus.minoAngleAfter == East) // Jミノが北向きの場合
    //     {
    //         // 指定のマスにブロックまたは壁があるか調べる
    //         for (int grid = 0; grid < eastOffset_ForJ.Length; grid++)
    //         {
    //             if (board.CheckGrid(pos_x + eastOffset_ForJ[grid, 0], pos_y + eastOffset_ForJ[grid, 1], spawner.activeMino))
    //             {
    //                 checkBlocks_ForJ.Add("Exist");
    //             }
    //             else
    //             {
    //                 checkBlocks_ForJ.Add(Existence.NotExist);
    //             }
    //         }
    //     }
    //     if (gameStatus.minoAngleAfter == South) // Jミノが北向きの場合
    //     {
    //         // 指定のマスにブロックまたは壁があるか調べる
    //         for (int grid = 0; grid < southOffset_ForJ.Length; grid++)
    //         {
    //             if (board.CheckGrid(pos_x + southOffset_ForJ[grid, 0], pos_y + southOffset_ForJ[grid, 1], spawner.activeMino))
    //             {
    //                 checkBlocks_ForJ.Add("Exist");
    //             }
    //             else
    //             {
    //                 checkBlocks_ForJ.Add(Existence.NotExist);
    //             }
    //         }
    //     }
    //     if (gameStatus.minoAngleAfter == West) // Jミノが北向きの場合
    //     {
    //         // 指定のマスにブロックまたは壁があるか調べる
    //         for (int grid = 0; grid < westOffset_ForJ.Length; grid++)
    //         {
    //             if (board.CheckGrid(pos_x + westOffset_ForJ[grid, 0], pos_y + westOffset_ForJ[grid, 1], spawner.activeMino))
    //             {
    //                 checkBlocks_ForJ.Add("Exist");
    //             }
    //             else
    //             {
    //                 checkBlocks_ForJ.Add(Existence.NotExist);
    //             }
    //         }
    //     }
    // }

    // Ispinの判定をする関数(Mini判定も計算)
    private void IspinCheck()
    {
        if (gameStatus.minoAngleAfter == MinoDirections.North || gameStatus.minoAngleAfter == MinoDirections.South) // Iミノが横向きの場合
        {
            // IspinMiniの判定をチェックする

            // 条件
            // ①Iミノを構成する各ブロックの1マス上に少なくともブロックが1つ以上存在すること
            // ②Iミノを構成する各ブロックの1マス下に少なくともブロックが3つ以上存在すること
            // ▫️▫️▫️▫️ ←①
            // ▪️▪️▪️▪️
            // ▫️▫️▫️▫️ ←②

            // 指定マスのブロックの有無情報を格納するリスト
            List<Existence> checkBlocksAbove_ForI = new List<Existence>();
            List<Existence> checkBlocksBelow_ForI = new List<Existence>();

            int yOffset = 1; // IspinMiniの判定に必要なオフセット

            // ①を調べる
            foreach (Transform item in spawner.ActiveMino.transform) // Iミノを構成するブロックそれぞれを確認する
            {
                Vector2 pos = Rounding.Round(item.position); // floatからintに値を丸める

                // 1マス上部にブロックがあるか調べる
                if (board.CheckGrid(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y + yOffset), spawner.ActiveMino))
                {
                    checkBlocksAbove_ForI.Add(Existence.Exist);
                }
                else
                {
                    checkBlocksAbove_ForI.Add(Existence.NotExist);
                }
            }

            // ②を調べる
            foreach (Transform item in spawner.ActiveMino.transform) // Iミノを構成するブロックそれぞれを確認する
            {
                Vector2 pos = Rounding.Round(item.position); // floatからintに値を丸める

                // 1マス上部にブロックがあるか調べる
                if (board.CheckGrid(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y - yOffset), spawner.ActiveMino))
                {
                    checkBlocksBelow_ForI.Add(Existence.Exist);
                }
                else
                {
                    checkBlocksBelow_ForI.Add(Existence.NotExist);
                }
            }

            // ①と②の状態を文字列に変換
            string checkBlocksAboveInfo = string.Join(", ", checkBlocksAbove_ForI);
            string checkBlocksBelowInfo = string.Join(", ", checkBlocksBelow_ForI);

            LogHelper.Log(LogHelper.LogLevel.Info, "SpinCheck", "IspinCheck()", checkBlocksAboveInfo);
            LogHelper.Log(LogHelper.LogLevel.Info, "SpinCheck", "IspinCheck()", checkBlocksBelowInfo); // Infoログ

            // 条件を満たすか確認
            if (checkBlocksAbove_ForI.FindAll(block => block == Existence.Exist).Count >= 1 ||
                checkBlocksBelow_ForI.FindAll(block => block == Existence.Exist).Count >= 3)
            {
                SpinTypeName = SpinTypeNames.I_SpinMini;
            }
        }
        else // Iミノが縦向きの場合
        {
            // Ispinの判定をチェックする

            // 条件
            // ①Iミノを構成する各ブロックの1マス横側にブロックが存在するか確認し、それぞれの側面で3つ以上ブロックが存在する
            // ②Iミノの上部にブロックが存在する

            //  ↓②
            //  . 
            //  .
            //  .
            //  ▫️ 
            //  ▫️ 
            // ▫️▪️▫️
            // ▫️▪️▫️
            // ▫️▪️▫️
            // ▫️▪️▫️
            // ↑ ↑①


            // 指定マスのブロックの有無情報を格納するリスト
            List<Existence> checkBlocksRightSide_ForI = new List<Existence>();
            List<Existence> checkBlocksLeftSide_ForI = new List<Existence>();
            List<Existence> checkBlocksUpper_ForI = new List<Existence>();

            // Iミノの上部にブロックがあるか調べるために必要な変数
            int IminoTopPosition_y = board.CheckActiveMinoTopBlockPosition_y(spawner.ActiveMino); // Iミノの最上部のy座標

            int xOffset = 1;
            int yOffset = 1; // Ispinの判定に必要なオフセット

            // ①を調べる
            foreach (Transform item in spawner.ActiveMino.transform) // Iミノを構成するブロックそれぞれを確認する
            {
                Vector2 pos = Rounding.Round(item.position); // floatからintに値を丸める

                // 1マス右側にブロックがあるか調べる
                if (board.CheckGrid(Mathf.RoundToInt(pos.x + xOffset), Mathf.RoundToInt(pos.y), spawner.ActiveMino))
                {
                    checkBlocksRightSide_ForI.Add(Existence.Exist);
                }
                else
                {
                    checkBlocksRightSide_ForI.Add(Existence.NotExist);
                }
            }
            foreach (Transform item in spawner.ActiveMino.transform) // Iミノを構成するブロックそれぞれを確認する
            {
                Vector2 pos = Rounding.Round(item.position); // floatからintに値を丸める

                // 1マス左側にブロックがあるか調べる
                if (board.CheckGrid(Mathf.RoundToInt(pos.x - xOffset), Mathf.RoundToInt(pos.y), spawner.ActiveMino))
                {
                    checkBlocksLeftSide_ForI.Add(Existence.Exist);
                }
                else
                {
                    checkBlocksLeftSide_ForI.Add(Existence.NotExist);
                }
            }

            // ②を調べる
            while (IminoTopPosition_y + yOffset < 20) // ゲームボードの上部からはみ出すまで調べる
            {
                // Iミノの上部にブロックが存在するか1マスずつ調べていく
                if (board.CheckGrid(Mathf.RoundToInt(spawner.ActiveMino.transform.position.x), IminoTopPosition_y + yOffset, spawner.ActiveMino))
                {
                    checkBlocksUpper_ForI.Add(Existence.Exist);
                }
                else
                {
                    checkBlocksUpper_ForI.Add(Existence.NotExist);
                }

                yOffset++;
            }


            // ①と②の状態を文字列に変換
            string checkBlocksRightSideInfo = string.Join(", ", checkBlocksRightSide_ForI);
            string checkBlocksLeftSideInfo = string.Join(", ", checkBlocksLeftSide_ForI);
            string checkBlocksUpperInfo = string.Join(", ", checkBlocksUpper_ForI);

            LogHelper.Log(LogHelper.LogLevel.Info, "SpinCheck", "IspinCheck()", checkBlocksRightSideInfo);
            LogHelper.Log(LogHelper.LogLevel.Info, "SpinCheck", "IspinCheck()", checkBlocksLeftSideInfo);
            LogHelper.Log(LogHelper.LogLevel.Info, "SpinCheck", "IspinCheck()", checkBlocksUpperInfo); // Infoログ

            // 条件を満たすか確認
            if (checkBlocksRightSide_ForI.FindAll(block => block == Existence.Exist).Count >= 3 &&
                checkBlocksLeftSide_ForI.FindAll(block => block == Existence.Exist).Count >= 3 &&
                checkBlocksUpper_ForI.FindAll(block => block == Existence.Exist).Count >= 1)
            {
                SpinTypeName = SpinTypeNames.I_Spin;
            }
        }
    }

    // Tspinの判定をする関数(Mini判定も計算)
    private void TspinCheck()
    {
        // Tミノの中心から順番に[1, 1]、[1, -1]、[-1, 1]、[-1, -1]の分だけ移動した座標にブロックや壁がないか確認する
        // ブロックや壁があった時は Exist ない時は Not Exist で_ActiveMinoCountのリストに追加されていく
        // 例: checkBlocks_ForT["Exist", "Exist", Existence.NotExist, "Exist"]
        // ▫️▪️▫️
        // ▪️▪️▪️
        // ▫️ ▫️  ←四隅を調べる

        // 指定マスのブロックの有無情報を格納するリスト
        List<Existence> checkBlocks_ForT = new List<Existence>();

        for (int x = 1; x >= -1; x -= 2)
        {
            for (int y = 1; y >= -1; y -= 2)
            {
                if (board.CheckGrid((int)Mathf.Round(spawner.ActiveMino.transform.position.x) + x, (int)Mathf.Round(spawner.ActiveMino.transform.position.y) + y, spawner.ActiveMino))
                {
                    checkBlocks_ForT.Add(Existence.Exist);
                }
                else
                {
                    checkBlocks_ForT.Add(Existence.NotExist);
                }
            }
        }

        string checkBlocksInfo = string.Join(", ", checkBlocks_ForT);

        LogHelper.Log(LogHelper.LogLevel.Info, "SpinCheck", "TspinCheck()", checkBlocksInfo); // Infoログ


        // Tspinの判定をチェックする

        // 条件
        // checkBlocks_ForTにExistが3つ以上含まれる


        // 同時にTspinMiniの判定もチェックする

        // 条件
        // Tspinの条件を満たす
        // SRSの段階が4以外
        // Tミノの突起側が Not Exist の場合
        // ▫️▪️▫️ ←この隅のどちらかが Not Exist の場合
        // ▪️▪️▪️
        // ▫️ ▫️

        if (checkBlocks_ForT.FindAll(block => block == Existence.Exist).Count >= 3) // 3マス以上ブロックや壁に埋まっている場合
        {
            if (gameStatus.stepsSRS == 4) // SRSが4段階の時は T-Spin 判定になる
            {
                SpinTypeName = SpinTypeNames.T_Spin;
            }
            else // SRSが4段階でない時
            {
                int right_up = 0; // 右上のブロックを指す
                int right_down = 1; // 右下
                int left_up = 2; // 左上
                int left_down = 3; // 左下

                // Tミノの突起の左右が空白の時、T-Spin Mini 判定になる
                switch (gameStatus.minoAngleAfter)
                {
                    case MinoDirections.North: // Tミノが北向きの時、右上と左上を確認する
                        if (checkBlocks_ForT[right_up] == Existence.NotExist || checkBlocks_ForT[left_up] == Existence.NotExist)
                        {
                            SpinTypeName = SpinTypeNames.T_SpinMini;
                        }
                        else
                        {
                            SpinTypeName = SpinTypeNames.T_Spin; // Tミノの底側のブロックが空白の時は T-Spin 判定になる
                        }

                        break;

                    case MinoDirections.East: // 右上と右下
                        if (checkBlocks_ForT[right_up] == Existence.NotExist || checkBlocks_ForT[right_down] == Existence.NotExist)
                        {
                            SpinTypeName = SpinTypeNames.T_SpinMini;
                        }
                        else
                        {
                            SpinTypeName = SpinTypeNames.T_Spin;
                        }

                        break;

                    case MinoDirections.South: // 右下と左下
                        if (checkBlocks_ForT[right_down] == Existence.NotExist || checkBlocks_ForT[left_down] == Existence.NotExist)
                        {
                            SpinTypeName = SpinTypeNames.T_SpinMini;
                        }
                        else
                        {
                            SpinTypeName = SpinTypeNames.T_Spin;
                        }

                        break;

                    case MinoDirections.West: // 左上と左下
                        if (checkBlocks_ForT[left_up] == Existence.NotExist || checkBlocks_ForT[left_down] == Existence.NotExist)
                        {
                            SpinTypeName = SpinTypeNames.T_SpinMini;
                        }
                        else
                        {
                            SpinTypeName = SpinTypeNames.T_Spin;
                        }

                        break;
                }
            }
        }
    }
}
