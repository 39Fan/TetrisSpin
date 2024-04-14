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
    GameManager gameManager;

    private void Start()
    {
        //BoardとGameManagerオブジェクトをそれぞれboardとgamemanager変数に格納する
        board = GameObject.FindObjectOfType<Board>();
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    //スーパーローテーションシステム(SRS)
    //通常回転ができなかった時に試す回転
    //4つの軌跡を辿り、ブロックや壁に衝突しなかったらそこに移動する
    public bool MinoSuperRotation(int minoAngleBefore, Block block)
    {
        //初期状態を0°として、右、下、左の角度をそれぞれ90°、180°、270°と表記することにする
        //Z軸で回転を行っているため、90°と270°はプログラム上 270, 90 と表記されている
        //LastSRSには、SRSが成功した際の軌跡の段階を格納(TspinMiniの判定に必要)

        //↓参考にした動画
        //https://www.youtube.com/watch?v=0OQ7mP97vdc

        //Iミノ以外のSRS
        if (!block.name.Contains("I"))
        {
            Debug.Log("Iミノ以外のSRS");

            //0°から90°または180°から90°に回転する時
            if ((minoAngleBefore == 0 && block.transform.rotation.eulerAngles.z == 270) ||
                (minoAngleBefore == 180 && block.transform.rotation.eulerAngles.z == 270))
            {
                Debug.Log("0°から90°または180°から90°に回転する時");

                gameManager.LastSRS++;

                block.MoveLeft();

                if (!board.CheckPosition(block))
                {
                    gameManager.LastSRS++;

                    block.MoveUp();

                    if (!board.CheckPosition(block))
                    {
                        gameManager.LastSRS++;

                        block.MoveRight();
                        block.MoveDown();
                        block.MoveDown();
                        block.MoveDown();

                        if (!board.CheckPosition(block))
                        {
                            gameManager.LastSRS++;

                            block.MoveLeft();

                            if (!board.CheckPosition(block))
                            {
                                //SRSができなかった際に回転前の状態に戻る
                                block.MoveRight();
                                block.MoveUp();
                                block.MoveUp();
                                return false;
                            }
                        }
                    }
                }
            }
            //270°から0°または270°から180°に回転する時
            else if ((minoAngleBefore == 90 && block.transform.rotation.eulerAngles.z == 0) ||
                (minoAngleBefore == 90 && block.transform.rotation.eulerAngles.z == 180))
            {
                Debug.Log("270°から0°または270°から180°に回転する時");

                gameManager.LastSRS++;

                block.MoveLeft();

                if (!board.CheckPosition(block))
                {
                    gameManager.LastSRS++;

                    block.MoveDown();

                    if (!board.CheckPosition(block))
                    {
                        gameManager.LastSRS++;

                        block.MoveRight();
                        block.MoveUp();
                        block.MoveUp();
                        block.MoveUp();

                        if (!board.CheckPosition(block))
                        {
                            gameManager.LastSRS++;

                            block.MoveLeft();

                            if (!board.CheckPosition(block))
                            {
                                //SRSができなかった際に回転前の状態に戻る
                                block.MoveRight();
                                block.MoveUp();
                                block.MoveUp();

                                return false;
                            }
                        }
                    }
                }
            }
            //90°から0°または90°から180°に回転する時
            else if ((minoAngleBefore == 270 && block.transform.rotation.eulerAngles.z == 0) ||
                (minoAngleBefore == 270 && block.transform.rotation.eulerAngles.z == 180))
            {
                Debug.Log("90°から0°または90°から180°に回転する時");

                gameManager.LastSRS++;

                block.MoveRight();

                if (!board.CheckPosition(block))
                {
                    gameManager.LastSRS++;

                    block.MoveDown();

                    if (!board.CheckPosition(block))
                    {
                        gameManager.LastSRS++;

                        block.MoveLeft();
                        block.MoveUp();
                        block.MoveUp();
                        block.MoveUp();

                        if (!board.CheckPosition(block))
                        {
                            gameManager.LastSRS++;

                            block.MoveRight();

                            if (!board.CheckPosition(block))
                            {
                                //SRSができなかった際に回転前の状態に戻る
                                block.MoveLeft();
                                block.MoveDown();
                                block.MoveDown();

                                return false;
                            }
                        }
                    }
                }
            }
            //0°から270°または180°から270°に回転する時
            else if ((minoAngleBefore == 0 && block.transform.rotation.eulerAngles.z == 90) ||
                (minoAngleBefore == 180 && block.transform.rotation.eulerAngles.z == 90))
            {
                Debug.Log("0°から270°または180°から270°に回転する時");

                gameManager.LastSRS++;

                block.MoveRight();

                if (!board.CheckPosition(block))
                {
                    gameManager.LastSRS++;

                    block.MoveUp();

                    if (!board.CheckPosition(block))
                    {
                        gameManager.LastSRS++;

                        block.MoveLeft();
                        block.MoveDown();
                        block.MoveDown();
                        block.MoveDown();

                        if (!board.CheckPosition(block))
                        {
                            gameManager.LastSRS++;

                            block.MoveRight();

                            if (!board.CheckPosition(block))
                            {
                                //SRSができなかった際に回転前の状態に戻る
                                block.MoveLeft();
                                block.MoveUp();
                                block.MoveUp();

                                return false;
                            }
                        }
                    }
                }
            }
        }
        //IミノのSRS(かなり複雑)
        //Iミノの軸は他のミノと違うため別の処理
        //IミノはLastSRSを扱わない
        else
        {
            Debug.Log("IミノのSRS");

            //0°から90°または270°から180°に回転する時
            if ((minoAngleBefore == 0 && block.transform.rotation.eulerAngles.z == 270) ||
                (minoAngleBefore == 90 && block.transform.rotation.eulerAngles.z == 180))
            {
                block.MoveLeft();
                block.MoveLeft();

                if (!board.CheckPosition(block))
                {
                    block.MoveRight();
                    block.MoveRight();
                    block.MoveRight();

                    if (!board.CheckPosition(block))
                    {
                        block.MoveLeft();
                        block.MoveLeft();
                        block.MoveLeft();
                        block.MoveDown();

                        if (!board.CheckPosition(block))
                        {
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

                                return false;
                            }
                        }
                    }
                }
            }
            //270°から0°または180°から90°に回転する時
            else if ((minoAngleBefore == 90 && block.transform.rotation.eulerAngles.z == 0) ||
                (minoAngleBefore == 180 && block.transform.rotation.eulerAngles.z == 270))
            {
                block.MoveRight();

                if (!board.CheckPosition(block))
                {
                    block.MoveLeft();
                    block.MoveLeft();
                    block.MoveLeft();

                    if (!board.CheckPosition(block))
                    {
                        block.MoveRight();
                        block.MoveRight();
                        block.MoveRight();
                        block.MoveDown();
                        block.MoveDown();

                        if (!board.CheckPosition(block))
                        {
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

                                return false;
                            }
                        }
                    }
                }
            }
            //90°から0°または180°から270°に回転する時
            else if ((minoAngleBefore == 270 && block.transform.rotation.eulerAngles.z == 0) ||
                (minoAngleBefore == 180 && block.transform.rotation.eulerAngles.z == 90))
            {
                block.MoveRight();
                block.MoveRight();

                if (!board.CheckPosition(block))
                {
                    block.MoveLeft();
                    block.MoveLeft();
                    block.MoveLeft();

                    if (!board.CheckPosition(block))
                    {
                        block.MoveRight();
                        block.MoveRight();
                        block.MoveRight();
                        block.MoveUp();

                        if (!board.CheckPosition(block))
                        {
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
                                return false;
                            }
                        }
                    }
                }
            }
            //0°から270°または90°から180°に回転する時
            else if ((minoAngleBefore == 0 && block.transform.rotation.eulerAngles.z == 90) ||
                (minoAngleBefore == 270 && block.transform.rotation.eulerAngles.z == 180))
            {
                block.MoveLeft();

                if (!board.CheckPosition(block))
                {
                    block.MoveRight();
                    block.MoveRight();
                    block.MoveRight();

                    if (!board.CheckPosition(block))
                    {
                        block.MoveLeft();
                        block.MoveLeft();
                        block.MoveLeft();
                        block.MoveUp();
                        block.MoveUp();

                        if (!board.CheckPosition(block))
                        {
                            block.MoveRight();
                            block.MoveRight();
                            block.MoveRight();
                            block.MoveDown();
                            block.MoveDown();
                            block.MoveDown();

                            if (!board.CheckPosition(block))
                            {
                                //SRSができなかった際に回転前の状態に戻る
                                block.MoveLeft();
                                block.MoveLeft();
                                block.MoveUp();
                                return false;
                            }
                        }
                    }
                }
            }
        }
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

        if (gameManager.LastSRS != 4)
        {
            if (AroundBlocksCheck_ForT.FindAll(x => x == 1).Count == 3)
            {
                switch (block.transform.rotation.eulerAngles.z)
                {
                    case 270:
                        if (AroundBlocksCheck_ForT[0] == 0 || AroundBlocksCheck_ForT[1] == 0)
                        {
                            Debug.Log("270でMini");
                            gameManager.SpinMini = true;
                            return 4;
                        }
                        break;
                    case 180:
                        if (AroundBlocksCheck_ForT[1] == 0 || AroundBlocksCheck_ForT[3] == 0)
                        {
                            Debug.Log("180でMini");
                            gameManager.SpinMini = true;
                            return 4;
                        }
                        break;
                    case 90:
                        if (AroundBlocksCheck_ForT[2] == 0 || AroundBlocksCheck_ForT[3] == 0)
                        {
                            Debug.Log("90でMini");
                            gameManager.SpinMini = true;
                            return 4;
                        }
                        break;
                    case 0:
                        if (AroundBlocksCheck_ForT[0] == 0 || AroundBlocksCheck_ForT[2] == 0)
                        {
                            Debug.Log("0でMini");
                            gameManager.SpinMini = true;
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

    public int SpinTerminal(bool useSpin, Block block)
    {
        if (useSpin == false)
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
