using System.Collections.Generic;
using UnityEngine;

//ミノの回転に関するスクリプト
//ミノの特殊回転を可能にする、スーパーローテーションシステムの関数や、
//各ミノのSpin判定をチェックする関数がある

//↓スーパーローテーションシステムについて解説されているサイト
//https://tetrisch.github.io/main/srs.html

public class Rotation : MonoBehaviour
{
    //各種干渉するスクリプトの設定
    Board board;
    Data data;
    GameManager gameManager;

    //インスタンス化
    private void Awake()
    {
        board = FindObjectOfType<Board>();
        gameManager = FindObjectOfType<GameManager>();
        data = FindObjectOfType<Data>();
    }

    //スーパーローテーションシステム(SRS)
    //通常回転ができなかった時に試す回転
    //4つの軌跡を辿り、ブロックや壁に衝突しなかったらそこに移動する

    //↓参考にした動画
    //https://www.youtube.com/watch?v=0OQ7mP97vdc

    public bool MinoSuperRotation(Block block)
    {
        //初期(未回転)状態をnorthとして、
        //右回転後の向きをeast
        //左回転後の向きをwest
        //2回右回転または左回転した時の向きをsouthとする
        int north = data.north;
        int east = data.east;
        int south = data.south;
        int west = data.west;

        //回転後の角度(minoAngleAfter)の調整
        data.CalibrateMinoAngleAfter(block);

        //SRSはIミノとそれ以外のミノとで処理が違うため分けて処理する
        //Iミノ以外のSRS
        if (!block.name.Contains("I"))
        {
            //Debug.Log("Iミノ以外のSRS");

            //northからeast
            //southからeastに回転する時
            if ((data.minoAngleBefore == north && data.minoAngleAfter == east) ||
                (data.minoAngleBefore == south && data.minoAngleAfter == east))
            {
                //Debug.Log("northからeastまたは、southからeastに回転する時");

                //第一法則
                //左に1つ移動
                block.MoveLeft();

                //LastSRSには、SRSが成功した際の軌跡の段階を格納(TspinMiniの判定に必要)
                //LastSRSは1
                data.lastSRS++;

                if (!board.CheckPosition(block))
                {
                    //第二法則
                    //上に1つ移動
                    block.MoveUp();

                    //LastSRSは2
                    data.lastSRS++;

                    if (!board.CheckPosition(block))
                    {
                        //第三法則
                        //右に1つ移動、下に3つ移動
                        block.MoveRight();
                        block.MoveDown();
                        block.MoveDown();
                        block.MoveDown();

                        //LastSRSは3
                        data.lastSRS++;

                        if (!board.CheckPosition(block))
                        {
                            //第四法則
                            //左に1つ移動
                            block.MoveLeft();

                            //LastSRSは4
                            data.lastSRS++;

                            if (!board.CheckPosition(block))
                            {
                                //SRSができなかった際に回転前の状態に戻る
                                block.MoveRight();
                                block.MoveUp();
                                block.MoveUp();

                                //通常回転のリセット
                                data.RotateReset(block, data.minoAngleAfter);

                                //SRSができなかった時、falseを返す
                                return false;
                            }
                        }
                    }
                }
            }
            //westからnorth
            //westからsouthに回転する時
            else if ((data.minoAngleBefore == west && data.minoAngleAfter == north) ||
                (data.minoAngleBefore == west && data.minoAngleAfter == south))
            {
                //Debug.Log("westからnorthまたは、westからsouthに回転する時");

                //第一法則
                //左に1つ移動
                block.MoveLeft();

                //LastSRSには、SRSが成功した際の軌跡の段階を格納(TspinMiniの判定に必要)
                //LastSRSは1
                data.lastSRS++;

                if (!board.CheckPosition(block))
                {
                    //第二法則
                    //下に1つ移動
                    block.MoveDown();

                    //LastSRSは2
                    data.lastSRS++;

                    if (!board.CheckPosition(block))
                    {
                        //第三法則
                        //右に1つ移動、上に3つ移動
                        block.MoveRight();
                        block.MoveUp();
                        block.MoveUp();
                        block.MoveUp();

                        //LastSRSは3
                        data.lastSRS++;

                        if (!board.CheckPosition(block))
                        {
                            //第四法則
                            //左に1つ移動
                            block.MoveLeft();

                            //LastSRSは4
                            data.lastSRS++;

                            if (!board.CheckPosition(block))
                            {
                                //SRSができなかった際に回転前の状態に戻る
                                block.MoveRight();
                                block.MoveUp();
                                block.MoveUp();

                                //通常回転のリセット
                                data.RotateReset(block, data.minoAngleAfter);

                                //SRSができなかった時、falseを返す
                                return false;
                            }
                        }
                    }
                }
            }
            //eastからnorth
            //eastからsouthに回転する時
            else if ((data.minoAngleBefore == east && data.minoAngleAfter == north) ||
                (data.minoAngleBefore == east && data.minoAngleAfter == south))
            {
                //Debug.Log("eastからnorthまたは、eastからsouthに回転する時");

                //第一法則
                //右に1つ移動
                block.MoveRight();

                //LastSRSには、SRSが成功した際の軌跡の段階を格納(TspinMiniの判定に必要)
                //LastSRSは1
                data.lastSRS++;

                if (!board.CheckPosition(block))
                {
                    //第二法則
                    //下に1つ移動
                    block.MoveDown();

                    //LastSRSは2
                    data.lastSRS++;

                    if (!board.CheckPosition(block))
                    {
                        //第三法則
                        //左に1つ移動、上に3つ移動
                        block.MoveLeft();
                        block.MoveUp();
                        block.MoveUp();
                        block.MoveUp();

                        //LastSRSは3
                        data.lastSRS++;

                        if (!board.CheckPosition(block))
                        {
                            //第四法則
                            //右に1つ移動
                            block.MoveRight();

                            //LastSRSは4
                            data.lastSRS++;

                            if (!board.CheckPosition(block))
                            {
                                //SRSができなかった際に回転前の状態に戻る
                                block.MoveLeft();
                                block.MoveDown();
                                block.MoveDown();

                                //通常回転のリセット
                                data.RotateReset(block, data.minoAngleAfter);

                                //SRSができなかった時、falseを返す
                                return false;
                            }
                        }
                    }
                }
            }
            //northからwest
            //southからwestに回転する時
            else if ((data.minoAngleBefore == north && data.minoAngleAfter == west) ||
                (data.minoAngleBefore == south && data.minoAngleAfter == west))
            {
                //Debug.Log("northからwestまたはsouthからwestに回転する時");

                //第一法則
                //右に1つ移動
                block.MoveRight();

                //LastSRSには、SRSが成功した際の軌跡の段階を格納(TspinMiniの判定に必要)
                //LastSRSは1
                data.lastSRS++;

                if (!board.CheckPosition(block))
                {
                    //第二法則
                    //上に1つ移動
                    block.MoveUp();

                    //LastSRSは2
                    data.lastSRS++;

                    if (!board.CheckPosition(block))
                    {
                        //第三法則
                        //左に1つ移動、下に3つ移動
                        block.MoveLeft();
                        block.MoveDown();
                        block.MoveDown();
                        block.MoveDown();

                        //LastSRSは3
                        data.lastSRS++;

                        if (!board.CheckPosition(block))
                        {
                            //第四法則
                            //右に1つ移動
                            block.MoveRight();

                            //LastSRSは4
                            data.lastSRS++;

                            if (!board.CheckPosition(block))
                            {
                                //SRSができなかった時、通常回転前の状態に戻る
                                //SRSのリセット
                                block.MoveLeft();
                                block.MoveUp();
                                block.MoveUp();

                                //通常回転のリセット
                                data.RotateReset(block, data.minoAngleAfter);

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

            //northからeast
            //westからsouthに回転する時
            if ((data.minoAngleBefore == north && data.minoAngleAfter == east) ||
                (data.minoAngleBefore == north && data.minoAngleAfter == south))
            {
                //Debug.Log("northからeastまたはwestからsouthに回転する時");

                //第一法則
                //左に2つ移動
                block.MoveLeft();
                block.MoveLeft();

                if (!board.CheckPosition(block))
                {
                    //第二法則
                    //右に3つ移動
                    block.MoveRight();
                    block.MoveRight();
                    block.MoveRight();

                    if (!board.CheckPosition(block))
                    {
                        //第三法則
                        //左に3つ移動、下に1つ移動
                        block.MoveLeft();
                        block.MoveLeft();
                        block.MoveLeft();
                        block.MoveDown();

                        if (!board.CheckPosition(block))
                        {
                            //第四法則
                            //右に3つ移動、上に3つ移動
                            block.MoveRight();
                            block.MoveRight();
                            block.MoveRight();
                            block.MoveUp();
                            block.MoveUp();
                            block.MoveUp();

                            if (!board.CheckPosition(block))
                            {
                                //SRSができなかった際に回転前の状態に戻る
                                block.MoveLeft();
                                block.MoveDown();
                                block.MoveDown();

                                //通常回転のリセット
                                data.RotateReset(block, data.minoAngleAfter);

                                //SRSができなかった時、falseを返す
                                return false;
                            }
                        }
                    }
                }
            }
            //westからnorth
            //southからeastに回転する時
            else if ((data.minoAngleBefore == west && data.minoAngleAfter == north) ||
                (data.minoAngleBefore == south && block.transform.rotation.eulerAngles.z == east))
            {
                //Debug.Log("westからnorthまたはsouthからeastに回転する時");

                //第一法則
                //右に1つ移動
                block.MoveRight();

                if (!board.CheckPosition(block))
                {
                    //第二法則
                    //左に3つ移動
                    block.MoveLeft();
                    block.MoveLeft();
                    block.MoveLeft();

                    if (!board.CheckPosition(block))
                    {
                        //第三法則
                        //右に3つ移動、下に2つ移動
                        block.MoveRight();
                        block.MoveRight();
                        block.MoveRight();
                        block.MoveDown();
                        block.MoveDown();

                        if (!board.CheckPosition(block))
                        {
                            //第四法則
                            //左に3つ移動、上に3つ移動
                            block.MoveLeft();
                            block.MoveLeft();
                            block.MoveLeft();
                            block.MoveUp();
                            block.MoveUp();
                            block.MoveUp();

                            if (!board.CheckPosition(block))
                            {
                                //SRSができなかった際に回転前の状態に戻る
                                block.MoveRight();
                                block.MoveRight();
                                block.MoveDown();

                                //通常回転のリセット
                                data.RotateReset(block, data.minoAngleAfter);

                                //SRSができなかった時、falseを返す
                                return false;
                            }
                        }
                    }
                }
            }
            //eastからnorthまたはsouthからwestに回転する時
            else if ((data.minoAngleBefore == east && data.minoAngleAfter == north) ||
                (data.minoAngleBefore == south && data.minoAngleAfter == west))
            {
                //Debug.Log("eastからnorthまたはsouthからwestに回転する時");

                //第一法則
                //右に2つ移動
                block.MoveRight();
                block.MoveRight();

                if (!board.CheckPosition(block))
                {
                    //第二法則
                    //左に3つ移動
                    block.MoveLeft();
                    block.MoveLeft();
                    block.MoveLeft();

                    if (!board.CheckPosition(block))
                    {
                        //第三法則
                        //右に3つ移動、上に1つ移動
                        block.MoveRight();
                        block.MoveRight();
                        block.MoveRight();
                        block.MoveUp();

                        if (!board.CheckPosition(block))
                        {
                            //第四法則
                            //左に3つ移動、下に3つ移動
                            block.MoveLeft();
                            block.MoveLeft();
                            block.MoveLeft();
                            block.MoveDown();
                            block.MoveDown();
                            block.MoveDown();

                            if (!board.CheckPosition(block))
                            {
                                //SRSができなかった際に回転前の状態に戻る
                                block.MoveRight();
                                block.MoveUp();
                                block.MoveUp();

                                //通常回転のリセット
                                data.RotateReset(block, data.minoAngleAfter);

                                //SRSができなかった時、falseを返す
                                return false;
                            }
                        }
                    }
                }
            }
            //northからwestまたはeastからsouthに回転する時
            else if ((data.minoAngleBefore == north && data.minoAngleAfter == west) ||
                (data.minoAngleBefore == east && data.minoAngleAfter == south))
            {
                //Debug.Log("northからwestまたはeastからsouthに回転する時");

                //第一法則
                //左に1つ移動
                block.MoveLeft();

                if (!board.CheckPosition(block))
                {
                    //第二法則
                    //右に3つ移動
                    block.MoveRight();
                    block.MoveRight();
                    block.MoveRight();

                    if (!board.CheckPosition(block))
                    {
                        //第三法則
                        //左に3つ移動、上に2つ移動
                        block.MoveLeft();
                        block.MoveLeft();
                        block.MoveLeft();
                        block.MoveUp();
                        block.MoveUp();

                        if (!board.CheckPosition(block))
                        {
                            //第四法則
                            //右に3つ移動、下に3つ移動
                            block.MoveRight();
                            block.MoveRight();
                            block.MoveRight();
                            block.MoveDown();
                            block.MoveDown();
                            block.MoveDown();

                            if (!board.CheckPosition(block))
                            {
                                //SRSができなかった時、回転前の状態に戻る
                                block.MoveLeft();
                                block.MoveLeft();
                                block.MoveUp();

                                //通常回転のリセット
                                data.RotateReset(block, data.minoAngleAfter);

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

    //SRS時に重複しないか判定する関数
    bool RotationCheck(int Rx, int Ry, Block block)
    {
        foreach (Transform item in block.transform)
        {
            Vector2 pos = Rounding.Round(item.position);

            //Gridの座標が負の場合false
            if ((int)pos.x + Rx < 0 || (int)pos.y + Ry < 0)
            {
                Debug.Log("SRS中に枠外に出た。A");
                return false;
            }

            //各gameobjectの位置を調べて、そこが重複していたらfalse
            if (board.Grid[(int)pos.x + Rx, (int)pos.y + Ry] != null
                && board.Grid[(int)pos.x + Rx, (int)pos.y + Ry].parent != block.transform)
            {
                Debug.Log("SRS先が重複");
                return false;
            }

            //枠外に出てもfalse
            if (!board.BoardOutCheck((int)pos.x + Rx, (int)pos.y + Ry))
            {
                Debug.Log("SRS中に枠外に出た。");
                return false;
            }
        }
        return true;
    }

    public int TspinCheck(Block block)
    {
        Debug.Log("====this is TspinCheck====");

        List<int> AroundBlocksCheck_ForT = new List<int>();

        if (AroundBlocksCheck_ForT.Count != 0)
        {
            for (int i = 0; i < 4; i++)
            {
                AroundBlocksCheck_ForT.RemoveAt(0);
            }
        }

        //Tミノの中心から順番に[1, 1]、[1, -1]、[-1, 1]、[-1, -1]の分だけ移動した座標にブロックや壁がないか確認する関数
        //ブロックや壁があった時は1、ない時は0でBlockCountのリストに追加されていく
        //例:AroundBlocksCheck_ForT[1, 1, 0, 1]
        for (int x = 1; x >= -1; x -= 2)
        {
            for (int y = 1; y >= -1; y -= 2)
            {
                if (board.BlockCheckForTspin((int)Mathf.Round(block.transform.position.x) + x, (int)Mathf.Round(block.transform.position.y) + y, block))
                {
                    AroundBlocksCheck_ForT.Add(1);
                }
                else
                {
                    AroundBlocksCheck_ForT.Add(0);
                }
            }
        }

        for (int i = 0; i < 4; i++)
        {
            Debug.Log(AroundBlocksCheck_ForT[i]);
        }

        if (data.lastSRS != 4)
        {
            if (AroundBlocksCheck_ForT.FindAll(x => x == 1).Count == 3)
            {
                switch (block.transform.rotation.eulerAngles.z)
                {
                    case 270:
                        if (AroundBlocksCheck_ForT[0] == 0 || AroundBlocksCheck_ForT[1] == 0)
                        {
                            Debug.Log("270でMini");
                            data.spinMini = true;
                            return 4;
                        }
                        break;
                    case 180:
                        if (AroundBlocksCheck_ForT[1] == 0 || AroundBlocksCheck_ForT[3] == 0)
                        {
                            Debug.Log("180でMini");
                            data.spinMini = true;
                            return 4;
                        }
                        break;
                    case 90:
                        if (AroundBlocksCheck_ForT[2] == 0 || AroundBlocksCheck_ForT[3] == 0)
                        {
                            Debug.Log("90でMini");
                            data.spinMini = true;
                            return 4;
                        }
                        break;
                    case 0:
                        if (AroundBlocksCheck_ForT[0] == 0 || AroundBlocksCheck_ForT[2] == 0)
                        {
                            Debug.Log("0でMini");
                            data.spinMini = true;
                            return 4;
                        }
                        break;
                }
                Debug.Log("Tspin");
                return 4;
            }
        }

        if (AroundBlocksCheck_ForT.FindAll(x => x == 1).Count == 4)
        {
            return 4;
        }
        return 7;
    }

    public int SpinTerminal(Block block)
    {
        if (data.useSpin == false)
        {
            return 7;
        }
        /*if (block.name.Contains("I"))
        {
            return IspinCheck(block);
        }*/
        /*else if (block.name.Contains("J"))
        {
            return JspinCheck(block);
        }*/
        /*else if (block.name.Contains("L"))
        {
            return LspinCheck(block);
        }*/
        /*else if (block.name.Contains("S"))
        {
            return SspinCheck(block);
        }*/
        else if (block.name.Contains("T"))
        {
            return TspinCheck(block);
        }
        /*else if (block.name.Contains("Z"))
        {
            return ZspinCheck(block);
        }*/
        /*else if (block.name.Contains("O"))
        {
            return OspinCheck(block);
        }*/
        Debug.Log("T以外のミノ");
        return 7;
    }

}
