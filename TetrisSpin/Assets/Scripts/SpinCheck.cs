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
        /*if (mino == I_mino)
        {
            return IspinCheck(_ActiveMino);
        }*/
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


    // Tspinの判定をする関数(Mini判定も計算)
    public void TspinCheck()
    {
        List<string> checkBlocks_ForT = new List<string>(); // Tミノの中心から斜め四方のブロックの状態を確認するためのリスト

        // Tミノの中心から順番に[1, 1]、[1, -1]、[-1, 1]、[-1, -1]の分だけ移動した座標にブロックや壁がないか確認する
        // ブロックや壁があった時は Exist ない時は Not Exist で_ActiveMinoCountのリストに追加されていく
        // 例: checkBlocks_ForT["Exist", "Exist", "Not Exist", "Exist"]

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
