using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UIElements;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private Block[] Blocks;

    [SerializeField]
    private Block_Ghost[] Blocks_Ghost;
    private Block[] NextBlocks = new Block[5];
    private Block OldHoldMino;
    Board board;

    private int GhostBlockPosition;
    private int ActiveBlockOrder;


    private void Awake()
    {
        board = FindObjectOfType<Board>(); //インスタンス化を真っ先に行う
    }

    //選ばれたミノを生成する関数
    public Block SpawnBlock(int mino)
    {
        Blocks[mino].transform.localScale = Vector3.one * 1;

        Block block = Instantiate(Blocks[mino],
        transform.position, Quaternion.identity);

        if (block)
        {
            return block;
        }
        else
        {
            return null;
        }
    }

    //GhostBloskを生成する関数
    public Block_Ghost SpawnBlock_Ghost(Block activeBlock)
    {
        for (int i = 0; i < 20; i++)
        {
            if (board.CheckPosition(activeBlock)) //ActiveBlockから底までの距離をGhostBlockPositionに格納
            {
                activeBlock.transform.position =
                    new Vector3(activeBlock.transform.position.x, activeBlock.transform.position.y - 1, activeBlock.transform.position.z);

                GhostBlockPosition++;
            }
        }

        activeBlock.transform.position =
            new Vector3(activeBlock.transform.position.x, activeBlock.transform.position.y + GhostBlockPosition, activeBlock.transform.position.z);

        for (int i = 0; i < Blocks.Length; i++)
        {
            if (activeBlock.name.Contains(Blocks[i].name))
            {
                ActiveBlockOrder = i;
                break;
            }
        }

        Block_Ghost block = Instantiate(Blocks_Ghost[ActiveBlockOrder],
            new Vector3(activeBlock.transform.position.x, activeBlock.transform.position.y - GhostBlockPosition, activeBlock.transform.position.z),
            activeBlock.transform.rotation);

        if (!board.CheckPosition_Ghost(block))
        {
            block.transform.position =
                new Vector3(block.transform.position.x, block.transform.position.y + 1, block.transform.position.z);
        }

        GhostBlockPosition = 0;

        if (block)
        {
            return block;
        }
        else
        {
            return null;
        }
    }

    //HoldミノをActiveBlockに戻す関数
    public Block HoldChange()
    {
        Block block = Instantiate(OldHoldMino,
        transform.position, Quaternion.identity);

        if (block)
        {
            return block;
        }
        else
        {
            return null;
        }
    }

    //Holdミノを表示する関数
    public Block SpawnHoldBlock(bool firstHold, Block newHoldMino)
    {
        if (firstHold == false) //2回目以降のHoldの時
        {
            board.DestroyBlock(OldHoldMino);
        }

        Block block = Instantiate(newHoldMino,
            new Vector3(-3, 17, 0), Quaternion.identity);

        OldHoldMino = block;

        if (block)
        {
            return block;
        }
        else
        {
            return null;
        }
    }

    //Nextミノを表示する関数
    public Block SpawnNextBlocks(int count, int[] minos) //この時もらうcountは1足されているので、ちょうどずれていてそのまま使用できる。
    {
        //Debug.Log("====this is SpawnNextBlocks====");

        for (int i = 0; i < 5; i++)
        {
            if (count == 1) //ゲームスタート時のNext表示
            {
                //Blocks[minos[count]].transform.localScale = Vector3.one * 0.9f;

                Block block = Instantiate(Blocks[minos[count + i]],
                    new Vector3(12, 17 - 3 * i, 0), Quaternion.identity); //Next1の表示

                NextBlocks[i] = block;
            }
            else
            {
                //Blocks[minos[count + i]].transform.localScale = Vector3.one * 0.8f;

                Block block = Instantiate(Blocks[minos[count + i]],
                    new Vector3(12, 17 - 3 * i, 0), Quaternion.identity); //Next2〜5の表示

                board.DestroyBlock(NextBlocks[i]);

                NextBlocks[i] = block;
            }
        }
        return null;
    }
}

