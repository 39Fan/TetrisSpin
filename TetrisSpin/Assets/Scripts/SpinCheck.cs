using System.Collections.Generic;
using UnityEngine;

///// Spin判定に関するスクリプト /////


// ↓このスクリプトで可能なこと↓ //

// 各Spinの判定

public class SpinCheck : MonoBehaviour
{
    // Spinの種類 //
    private string SpinTypeName;

    // ゲッタープロパティ //
    public string spinTypeName
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
        SpinTypeName = "None";
    }

    // 各ミノのスピン判定をチェックする //
    public void CheckSpinType() // この関数は、回転時に呼び出される
    {
        if (spawner.activeMinoName == "I_Mino")
        {
            IspinCheck();
        }
        /*else if (mino == J_mino)
        {
            return JspinCheck(_ActiveMino);
        }*/
        /*else if (mino == L_mino)
        {
            return LspinCheck(_ActiveMino);
        }*/
        /*else if (mino == O_mino)
        {
            return OspinCheck(_ActiveMino);
        }*/
        /*else if (mino == S_mino)
        {
            return SspinCheck(_ActiveMino);
        }*/
        if (spawner.activeMinoName == "T_Mino")
        {
            TspinCheck();
        }
        /*else if (mino == Z_mino)
        {
            return ZspinCheck(_ActiveMino);
        }*/
        else
        {
            // Debug.LogError("[SpinCheck CheckSpinType()] activeMinoNameが特定できません。");
        }
    }

    // Ispinの判定をする関数(Mini判定も計算)
    public void IspinCheck()
    {
        if (gameStatus.minoAngleAfter == gameStatus.North || gameStatus.minoAngleAfter == gameStatus.South) // Iミノが横向きの場合
        {
            List<string> checkBlocksAbove_ForI = new List<string>();
            List<string> checkBlocksBelow_ForI = new List<string>(); // IspinMiniの判定に必要なリスト

            int yOffset = 1; // IspinMiniの判定に必要なオフセット


            // IspinMiniの判定をチェックする

            // 条件
            // ①Iミノを構成する各ブロックの1マス上に少なくともブロックが1つ以上存在すること
            // ②Iミノを構成する各ブロックの1マス下に少なくともブロックが3つ以上存在すること
            // ▫️▫️▫️▫️ ←①
            // ▪️▪️▪️▪️
            // ▫️▫️▫️▫️ ←②

            // ①を調べる
            foreach (Transform item in spawner.activeMino.transform) // Iミノを構成するブロックそれぞれを確認する
            {
                Vector2 pos = Rounding.Round(item.position); // floatからintに値を丸める

                // 1マス上部にブロックがあるか調べる
                if (board.CheckGrid(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y + yOffset), spawner.activeMino))
                {
                    checkBlocksAbove_ForI.Add("Exist");
                }
                else
                {
                    checkBlocksAbove_ForI.Add("Not Exist");
                }
            }

            // ②を調べる
            foreach (Transform item in spawner.activeMino.transform) // Iミノを構成するブロックそれぞれを確認する
            {
                Vector2 pos = Rounding.Round(item.position); // floatからintに値を丸める

                // 1マス上部にブロックがあるか調べる
                if (board.CheckGrid(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y - yOffset), spawner.activeMino))
                {
                    checkBlocksBelow_ForI.Add("Exist");
                }
                else
                {
                    checkBlocksBelow_ForI.Add("Not Exist");
                }
            }

            // ①と②の状態を文字列に変換
            string checkBlocksAboveInfo = string.Join(", ", checkBlocksAbove_ForI);
            string checkBlocksBelowInfo = string.Join(", ", checkBlocksBelow_ForI);

            DebugHelper.Log(checkBlocksAboveInfo, DebugHelper.LogLevel.Info, "SpinCheck", "IspinCheck()");
            DebugHelper.Log(checkBlocksBelowInfo, DebugHelper.LogLevel.Info, "SpinCheck", "IspinCheck()"); // Infoログ

            // 条件を満たすか確認
            if (checkBlocksAbove_ForI.FindAll(block => block == "Exist").Count >= 1 ||
                checkBlocksBelow_ForI.FindAll(block => block == "Exist").Count >= 3)
            {
                SpinTypeName = "I-Spin Mini";
            }
        }
        else // Iミノが縦向きの場合
        {
            List<string> checkBlocksRightSide_ForI = new List<string>();
            List<string> checkBlocksLeftSide_ForI = new List<string>();
            List<string> checkBlocksUpper_ForI = new List<string>(); // Ispinの判定に必要なリスト

            // Iミノの上部にブロックがあるか調べるために必要な変数
            int IminoTopPosition_y = board.CheckActiveMinoTopBlockPosition_y(spawner.activeMino); // Iミノの最上部のy座標

            int xOffset = 1;
            int yOffset = 1; // Ispinの判定に必要なオフセット


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

            // ①を調べる
            foreach (Transform item in spawner.activeMino.transform) // Iミノを構成するブロックそれぞれを確認する
            {
                Vector2 pos = Rounding.Round(item.position); // floatからintに値を丸める

                // 1マス右側にブロックがあるか調べる
                if (board.CheckGrid(Mathf.RoundToInt(pos.x + xOffset), Mathf.RoundToInt(pos.y), spawner.activeMino))
                {
                    checkBlocksRightSide_ForI.Add("Exist");
                }
                else
                {
                    checkBlocksRightSide_ForI.Add("Not Exist");
                }
            }
            foreach (Transform item in spawner.activeMino.transform) // Iミノを構成するブロックそれぞれを確認する
            {
                Vector2 pos = Rounding.Round(item.position); // floatからintに値を丸める

                // 1マス左側にブロックがあるか調べる
                if (board.CheckGrid(Mathf.RoundToInt(pos.x - xOffset), Mathf.RoundToInt(pos.y), spawner.activeMino))
                {
                    checkBlocksLeftSide_ForI.Add("Exist");
                }
                else
                {
                    checkBlocksLeftSide_ForI.Add("Not Exist");
                }
            }

            // ②を調べる
            while (IminoTopPosition_y + yOffset < 20) // ゲームボードの上部からはみ出すまで調べる
            {
                // Iミノの上部にブロックが存在するか1マスずつ調べていく
                if (board.CheckGrid(Mathf.RoundToInt(spawner.activeMino.transform.position.x), IminoTopPosition_y + yOffset, spawner.activeMino))
                {
                    checkBlocksUpper_ForI.Add("Exist");
                }
                else
                {
                    checkBlocksUpper_ForI.Add("Not Exist");
                }

                yOffset++;
            }


            // ①と②の状態を文字列に変換
            string checkBlocksLeftSideInfo = string.Join(", ", checkBlocksLeftSide_ForI);
            string checkBlocksUpperInfo = string.Join(", ", checkBlocksUpper_ForI);

            DebugHelper.Log(checkBlocksLeftSideInfo, DebugHelper.LogLevel.Info, "SpinCheck", "IspinCheck()");
            DebugHelper.Log(checkBlocksUpperInfo, DebugHelper.LogLevel.Info, "SpinCheck", "IspinCheck()"); // Infoログ

            // 条件を満たすか確認
            if (checkBlocksRightSide_ForI.FindAll(block => block == "Exist").Count >= 3 &&
                checkBlocksLeftSide_ForI.FindAll(block => block == "Exist").Count >= 3 &&
                checkBlocksUpper_ForI.FindAll(block => block == "Exist").Count >= 1)
            {
                SpinTypeName = "I-Spin";
            }
        }
    }

    // Tspinの判定をする関数(Mini判定も計算)
    private void TspinCheck()
    {
        List<string> checkBlocks_ForT = new List<string>(); // Tミノの中心から斜め四方のブロックの状態を確認するためのリスト

        // Tミノの中心から順番に[1, 1]、[1, -1]、[-1, 1]、[-1, -1]の分だけ移動した座標にブロックや壁がないか確認する
        // ブロックや壁があった時は Exist ない時は Not Exist で_ActiveMinoCountのリストに追加されていく
        // 例: checkBlocks_ForT["Exist", "Exist", "Not Exist", "Exist"]
        // ▫️▪️▫️
        // ▪️▪️▪️
        // ▫️ ▫️  ←四隅を調べる

        for (int x = 1; x >= -1; x -= 2)
        {
            for (int y = 1; y >= -1; y -= 2)
            {
                if (board.CheckGrid((int)Mathf.Round(spawner.activeMino.transform.position.x) + x, (int)Mathf.Round(spawner.activeMino.transform.position.y) + y, spawner.activeMino))
                {
                    checkBlocks_ForT.Add("Exist");
                }
                else
                {
                    checkBlocks_ForT.Add("Not Exist");
                }
            }
        }


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

        if (checkBlocks_ForT.FindAll(block => block == "Exist").Count >= 3) // 3マス以上ブロックや壁に埋まっている場合
        {
            if (gameStatus.lastSRS == 4) // SRSが4段階の時は T-Spin 判定になる
            {
                SpinTypeName = "T-Spin";
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
                    case "NORTH": // Tミノが北向きの時、右上と左上を確認する
                        if (checkBlocks_ForT[right_up] == "Not Exist" || checkBlocks_ForT[left_up] == "Not Exist")
                        {
                            SpinTypeName = "T-Spin Mini";
                        }
                        else
                        {
                            SpinTypeName = "T-Spin"; // Tミノの底側のブロックが空白の時は T-Spin 判定になる
                        }

                        break;

                    case "EAST": // 右上と右下
                        if (checkBlocks_ForT[right_up] == "Not Exist" || checkBlocks_ForT[right_down] == "Not Exist")
                        {
                            SpinTypeName = "T-Spin Mini";
                        }
                        else
                        {
                            SpinTypeName = "T-Spin";
                        }

                        break;

                    case "SOUTH": // 右下と左下
                        if (checkBlocks_ForT[right_down] == "Not Exist" || checkBlocks_ForT[left_down] == "Not Exist")
                        {
                            SpinTypeName = "T-Spin Mini";
                        }
                        else
                        {
                            SpinTypeName = "T-Spin";
                        }

                        break;

                    case "WEST": // 左上と左下
                        if (checkBlocks_ForT[left_up] == "Not Exist" || checkBlocks_ForT[left_down] == "Not Exist")
                        {
                            SpinTypeName = "T-Spin Mini";
                        }
                        else
                        {
                            SpinTypeName = "T-Spin";
                        }

                        break;
                }
            }
        }
    }
}
