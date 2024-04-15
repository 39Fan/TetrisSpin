using UnityEngine;

public class Spawner : MonoBehaviour
{
    //[SerializeField]
    //private Block[] Blocks;

    //[SerializeField]
    //private Block_Ghost[] Blocks_Ghost;

    private Block OldHoldMino;
    private Board board;
    private Data data;
    private GameManager gameManager;

    private int GhostBlockPosition;
    private int ActiveBlockOrder;


    private void Awake()
    {
        board = FindObjectOfType<Board>();
        data = FindObjectOfType<Data>();
        gameManager = FindObjectOfType<GameManager>();
    }

    //選ばれたミノを生成する関数
    public Block SpawnMino(int mino)
    {
        //Blocks[mino].transform.localScale = Vector3.one * 1;

        //Quaternion.identityは、向きの回転に関する設定をしないことを表す
        Block spawnMino = Instantiate(data.minos[mino],
        data.minoSpawnPosition, Quaternion.identity);

        if (spawnMino)
        {
            return spawnMino;
        }
        else
        {
            return null;
        }
    }

    //GhostBloskを生成する関数
    /*public Block_Ghost SpawnBlock_Ghost(Block activeBlock)
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
    }*/

    /*HoldミノをActiveBlockに戻す関数
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
    }*/

    //Nextミノを表示する関数
    public Block SpawnNextBlocks()
    {
        //Debug.Log("====this is SpawnNextBlocks====");

        //Nextの数は5に設定
        int nexts = 5;

        //ゲーム画面で表示するNext1〜5の座標を格納する配列
        Vector3[] nextMinoPositions = new Vector3[nexts];

        //Next1〜5の座標は、Y座標を3ずつ下に下げて配置する
        int position_y = 3;

        //Nextの数だけ繰り返す
        for (int count = 0; count < nexts; count++)
        {
            //Y座標をposition_yずつ下げて宣言
            nextMinoPositions[count] = new Vector3(12, 17 - (position_y * count), 0);
        }

        //以下のように設定される
        // nextMinoPosition[0] = new Vector3 (12, 17, 0);
        // nextMinoPosition[1] = new Vector3 (12, 14, 0);
        // nextMinoPosition[2] = new Vector3 (12, 11, 0);
        // nextMinoPosition[3] = new Vector3 (12, 8, 0);
        // nextMinoPosition[4] = new Vector3 (12, 5, 0);

        //Nextの数だけ繰り返す
        for (int nextMinoOrder = 0; nextMinoOrder < nexts; nextMinoOrder++)
        {
            //ゲームスタート時のNext表示
            if (data.count == 0)
            {
                //Next1〜5の表示
                //nextBlocksに格納
                data.nextBlocks[nextMinoOrder] = Instantiate(data.minos[data.spawnMinoOrder[data.count + nextMinoOrder + 1]],
                    nextMinoPositions[nextMinoOrder], Quaternion.identity);
            }
            //2回目以降のNext表示
            else
            {
                //以前のNextMinoを消去
                Destroy(data.nextBlocks[nextMinoOrder].gameObject);

                //Next1〜5の表示
                //nextBlocksに格納
                data.nextBlocks[nextMinoOrder] = Instantiate(data.minos[data.spawnMinoOrder[data.count + nextMinoOrder + 1]],
                    nextMinoPositions[nextMinoOrder], Quaternion.identity);
            }
        }
        return null;
    }

    //Holdした時の処理をする関数
    /*public Block Hold(Block activeBlock)
    {
        //ホールドが使用された
        data.UseHold = true;

        activeBlock.transform.position = data.HoldMinoPosition;

        //Holdが1回目の時
        if (data.FirstHold == true) //最初のHoldの時
        {
            gameManager.MinoSpawn(); //新たなActiveBlockの表示
            SpawnNextBlocks();
            data.FirstHold = false;
        }
        //Holdが2回目以降の時
        else
        {
            activeBlock = data.SpawnMinos[data.HoldMinoCount];
        }

        data.AngleReset();

        return activeBlock;
    }*/
}

