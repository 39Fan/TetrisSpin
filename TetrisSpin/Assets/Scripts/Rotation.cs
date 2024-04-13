using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    [SerializeField]
    Board board;
    Spawner spawner;

    GameManager gameManager;

    Block blockOriginPosition;

    [SerializeField]
    List<int> BlockCount = new List<int>();


    private void Start()
    {
        board = GameObject.FindObjectOfType<Board>();

        gameManager = GameObject.FindObjectOfType<GameManager>();
    }


    //SR時に重複しないか判定する関数
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

    //スーパーローテーションシステム(SRS)
    //通常回転ができなかった時に試す回転
    //4つの軌跡を辿り、ブロックや壁に衝突しなかったらそこに移動する
    public bool MinoSuperRotation(int minoAngleBefore, int lastSRS, Block block)
    {
        //blockOriginPositionに通常回転後の情報を格納
        //SRSができなかった際に回転前の状態に戻るため
        blockOriginPosition = block;

        //初期状態を0°として、右、下、左の角度をそれぞれ90°、180°、270°と表記することにする
        //Z軸で回転を行っているため、90°と270°はプログラム上 270, 90 と表記されている
        //lastSRSには、SRSが成功した際の軌跡の段階を格納(TspinMiniの判定に必要)

        //↓参考にした動画
        //https://www.youtube.com/watch?v=0OQ7mP97vdc

        //Iミノ以外のSRS
        if (!block.name.Contains("I"))
        {
            //0°から90°または180°から90°に回転する時
            if ((minoAngleBefore == 0 && block.transform.rotation.eulerAngles.z == 270) ||
                (minoAngleBefore == 180 && block.transform.rotation.eulerAngles.z == 270))
            {
                block.MoveLeft();
                if (!board.CheckPosition(block))
                {
                    block.MoveUp();
                    if (!board.CheckPosition(block))
                    {
                        block.MoveRight();
                        block.MoveDown();
                        block.MoveDown();
                        block.MoveDown();
                        if (!board.CheckPosition(block))
                        {
                            block.MoveLeft();
                            if (!board.CheckPosition(block))
                            {
                                block = blockOriginPosition;
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
                block.MoveLeft();
                if (!board.CheckPosition(block))
                {
                    block.MoveDown();
                    if (!board.CheckPosition(block))
                    {
                        block.MoveRight();
                        block.MoveUp();
                        block.MoveUp();
                        block.MoveUp();
                        if (!board.CheckPosition(block))
                        {
                            block.MoveLeft();
                            if (!board.CheckPosition(block))
                            {
                                block = blockOriginPosition;
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
                block.MoveRight();
                if (!board.CheckPosition(block))
                {
                    block.MoveDown();
                    if (!board.CheckPosition(block))
                    {
                        block.MoveLeft();
                        block.MoveUp();
                        block.MoveUp();
                        block.MoveUp();
                        if (!board.CheckPosition(block))
                        {
                            block.MoveRight();
                            if (!board.CheckPosition(block))
                            {
                                block = blockOriginPosition;
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
                block.MoveRight();
                if (!board.CheckPosition(block))
                {
                    block.MoveUp();
                    if (!board.CheckPosition(block))
                    {
                        block.MoveLeft();
                        block.MoveDown();
                        block.MoveDown();
                        block.MoveDown();
                        if (!board.CheckPosition(block))
                        {
                            block.MoveRight();
                            if (!board.CheckPosition(block))
                            {
                                block = blockOriginPosition;
                                return false;
                            }
                        }
                    }
                }
            }
        }
        //IミノのSRS(かなり複雑)
        //Iミノの軸は他のミノと違うため別の処理
        else
        {
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
                                block = blockOriginPosition;
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
                                block = blockOriginPosition;
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
                                block = blockOriginPosition;
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
                                block = blockOriginPosition;
                                return false;
                            }
                        }
                    }
                }
            }
        }
        return true;
    }


    public int TspinCheck(Block block, int lastSRS)
    {
        Debug.Log("====this is TspinCheck====");

        if (BlockCount.Count != 0)
        {
            for (int i = 0; i < 4; i++)
            {
                BlockCount.RemoveAt(0);
            }
        }

        for (int x = 1; x >= -1; x -= 2)
        {
            for (int y = -1; y <= 1; y += 2)
            {
                if (board.BlockCheckForTspin((int)Mathf.Round(block.transform.position.x) + x, (int)Mathf.Round(block.transform.position.y) + y, block))
                {
                    BlockCount.Add(1);
                }
                else
                {
                    BlockCount.Add(0);
                }
            }
        }

        if (lastSRS != 4)
        {
            if (BlockCount.FindAll(x => x == 1).Count == 3)
            {
                switch (block.transform.rotation.eulerAngles.z)
                {
                    case 270:
                        if (BlockCount[0] == 0 || BlockCount[1] == 0)
                        {
                            Debug.Log("270でMini");
                            gameManager.SpinMini = true;
                            return 4;
                        }
                        break;
                    case 180:
                        if (BlockCount[1] == 0 || BlockCount[2] == 0)
                        {
                            Debug.Log("180でMini");
                            gameManager.SpinMini = true;
                            return 4;
                        }
                        break;
                    case 90:
                        if (BlockCount[2] == 0 || BlockCount[3] == 0)
                        {
                            Debug.Log("90でMini");
                            gameManager.SpinMini = true;
                            return 4;
                        }
                        break;
                    case 0:
                        if (BlockCount[3] == 0 || BlockCount[0] == 0)
                        {
                            Debug.Log("0でMini");
                            gameManager.SpinMini = true;
                            return 4;
                        }
                        break;
                }
                return 4;
            }
        }

        if (BlockCount.FindAll(x => x == 1).Count == 4)
        {
            return 4;
        }
        return 7;
    }

    public int SpinTerminal(bool useSpin, int lastSRS, Block block)
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
            return TspinCheck(block, lastSRS);
        }
        /*else if (block.name.Contains("Z"))
        {
            return ZspinCheck(block);
        }*/
        /*else if (block.name.Contains("O"))
        {
            return OspinCheck(block);
        }*/
        Debug.Log("バグ発生");
        return 7;
    }

}
