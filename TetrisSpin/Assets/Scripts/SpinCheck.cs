using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///// Spin判定に関するスクリプト /////


// ↓このスクリプトで可能なこと↓ //

// Spin判定と識別

public class SpinCheck : MonoBehaviour
{
    // Spinの種類 //
    private string SpinTypeName;

    public string spinTypeName
    {
        get { return SpinTypeName; }
    }

    Board board;
    GameStatus gameStatus;
    Spawner spawner;
    Mino mino;

    private void Awake() // インスタンス化
    {
        board = FindObjectOfType<Board>();
        gameStatus = FindObjectOfType<GameStatus>();
        spawner = FindObjectOfType<Spawner>();
        mino = FindObjectOfType<Mino>();
    }

    public void ResetSpinTypeName()
    {
        SpinTypeName = "None";
    }

    // 各ミノのスピン判定をチェックする //
    public int CheckSpinType() // この関数は、回転時に呼び出される
    {
        // Debug.Log("SpinTerminal");
        // Debug.Log(_ActiveMino);
        //ミノの識別()
        // int I_mino = 0;
        // int J_mino = 1;
        // int L_mino = 2;
        // int O_mino = 3;
        // int S_mino = 4;
        //int T_mino = 5;
        // int Z_mino = 6;

        //最後の動作がスピンでないならSpin判定はなし
        // if (gameStatus.UseSpin == false)
        // {
        //     return 7;
        // }
        /*else if (mino == I_mino)
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
        return 7;
    }


    //Tspinの判定をする関数
    //TspinにはMiniがあるので、それも判定する
    public void TspinCheck()
    {
        //Debug.Log("====this is TspinCheck====");

        List<string> checkBlocks_ForT = new List<string>();

        //Tミノの中心から順番に[1, 1]、[1, -1]、[-1, 1]、[-1, -1]の分だけ移動した座標にブロックや壁がないか確認する
        //ブロックや壁があった時は Exist ない時は Not Exist で_ActiveMinoCountのリストに追加されていく
        //例: checkBlocks_ForT["Exist", "Exist", "Not Exist", "Exist"]

        for (int x = 1; x >= -1; x -= 2)
        {
            for (int y = 1; y >= -1; y -= 2)
            {
                if (board.ActiveMinoCheckForTspin((int)Mathf.Round(spawner.activeMino.transform.position.x) + x, (int)Mathf.Round(spawner.activeMino.transform.position.y) + y, spawner.activeMino))
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
            else
            {
                int right_up = 0; // 右上のブロックを指す
                int right_down = 1; // 右下
                int left_up = 2; // 左上
                int left_down = 3; // 左下

                // Tミノの突起の左右が空白の時、T-Spin Mini 判定になる
                switch (gameStatus.minoAngleAfter)
                {
                    case "NORTH": // 右上と左上を確認する
                        if (checkBlocks_ForT[right_up] == "Not Exist" || checkBlocks_ForT[left_up] == "Not Exist")
                        {
                            // Debug.Log("NORTHでMini");
                            // gameStatus.UseSpinMini = true;
                            SpinTypeName = "T-Spin Mini";
                        }

                        SpinTypeName = "T-Spin"; // Tミノの底側のブロックが空白の時は T-Spin 判定になる

                        break;
                    case "EAST": // 右上と右下
                        if (checkBlocks_ForT[right_up] == "Not Exist" || checkBlocks_ForT[right_down] == "Not Exist")
                        {
                            // Debug.Log("EASTでMini");
                            // gameStatus.UseSpinMini = true;
                            SpinTypeName = "T-Spin Mini";
                        }

                        SpinTypeName = "T-Spin";

                        break;
                    case "SOUTH": // 右下と左下
                        if (checkBlocks_ForT[right_down] == "Not Exist" || checkBlocks_ForT[left_down] == "Not Exist")
                        {
                            // Debug.Log("SOUTHでMini");
                            // gameStatus.UseSpinMini = true;
                            SpinTypeName = "T-Spin Mini";
                        }

                        SpinTypeName = "T-Spin";

                        break;
                    case "WEST": // 左上と左下
                        if (checkBlocks_ForT[left_up] == "Not Exist" || checkBlocks_ForT[left_down] == "Not Exist")
                        {
                            // Debug.Log("WESTでMini");
                            // gameStatus.UseSpinMini = true;
                            SpinTypeName = "T-Spin Mini";
                        }

                        SpinTypeName = "T-Spin";

                        break;
                }
            }
        }
    }

    // if (gameStatus.lastSRS != 4) // SRSが4段階の時は
    // {
    //     if (checkBlocks_ForT.FindAll(x => x == 1).Count == 3)
    //     {
    //         switch (gameStatus.ActiveMino.transform.rotation.eulerAngles.z)
    //         {
    //             case 270:
    //                 if (checkBlocks_ForT[0] == "Not Exist" || checkBlocks_ForT[1] == "Not Exist")
    //                 {
    //                     Debug.Log("270でMini");
    //                     gameStatus.UseSpinMini = true;
    //                     return 4;
    //                 }
    //                 break;
    //             case 180:
    //                 if (checkBlocks_ForT[1] == "Not Exist" || checkBlocks_ForT[3] == "Not Exist")
    //                 {
    //                     Debug.Log("180でMini");
    //                     gameStatus.UseSpinMini = true;
    //                     return 4;
    //                 }
    //                 break;
    //             case 90:
    //                 if (checkBlocks_ForT[2] == "Not Exist" || checkBlocks_ForT[3] == "Not Exist")
    //                 {
    //                     Debug.Log("90でMini");
    //                     gameStatus.UseSpinMini = true;
    //                     return 4;
    //                 }
    //                 break;
    //             case 0:
    //                 if (checkBlocks_ForT[0] == "Not Exist" || checkBlocks_ForT[2] == "Not Exist")
    //                 {
    //                     Debug.Log("0でMini");
    //                     gameStatus.UseSpinMini = true;
    //                     return 4;
    //                 }
    //                 break;
    //         }
    //         Debug.Log("Tspin");
    //         return 4;
    //     }
    // }

    //     if (checkBlocks_ForT.FindAll(x => x == 1).Count == 4)
    //     {
    //         return 4;
    //     }
    //     return 7;
    // }


}
