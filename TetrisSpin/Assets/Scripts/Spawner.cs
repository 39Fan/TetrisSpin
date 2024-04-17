using UnityEngine;

public class Spawner : MonoBehaviour
{
    //各種干渉するスクリプトの設定
    Board board;
    Data data;

    //インスタンス化
    private void Awake()
    {
        board = FindObjectOfType<Board>();
        data = FindObjectOfType<Data>();
    }

    //選ばれたミノを生成する関数
    public Block SpawnMino(int mino)
    {
        //Blocks[mino].transform.localScale = Vector3.one * 1;

        //Quaternion.identityは、向きの回転に関する設定をしないことを表す
        Block spawnMino = Instantiate(data.minos[mino],
        data.spawnMinoPosition, Quaternion.identity);

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
    public Block_Ghost SpawnMino_Ghost(Block activeMino)
    {
        //activeMinoの各座標を格納する変数を宣言
        int activeMino_x = Mathf.RoundToInt(activeMino.transform.position.x);
        int activeMino_y = Mathf.RoundToInt(activeMino.transform.position.y);
        int activeMino_z = Mathf.RoundToInt(activeMino.transform.position.z);

        //activeMinoから他のミノ、または底までの距離を計算
        data.CheckDistance_Y(activeMino);

        //orderに対応するゴーストミノを、activeMinoのY座標からdistanceの値だけ下に移動した位置に生成
        Block_Ghost mino_Ghost = Instantiate(data.minos_Ghost[data.order],
            new Vector3(activeMino_x, activeMino_y - data.distance, activeMino_z), Quaternion.identity);

        if (mino_Ghost)
        {
            return mino_Ghost;
        }
        else
        {
            return null;
        }
    }



    /*HoldミノをactiveMinoに戻す関数
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
    /*public Block Hold(Block activeMino)
    {
        //ホールドが使用された
        data.UseHold = true;

        activeMino.transform.position = data.HoldMinoPosition;

        //Holdが1回目の時
        if (data.FirstHold == true) //最初のHoldの時
        {
            gameManager.MinoSpawn(); //新たなactiveMinoの表示
            SpawnNextBlocks();
            data.FirstHold = false;
        }
        //Holdが2回目以降の時
        else
        {
            activeMino = data.SpawnMinos[data.HoldMinoCount];
        }

        data.AngleReset();

        return activeMino;
    }*/

    //Holdされたミノを表示する関数
    public Block SpawnHoldMino(int mino)
    {
        //minoに対応するミノを生成
        //Quaternion.identityは、向きの回転に関する設定をしないことを表す
        Block spawnHoldMino = Instantiate(data.minos[mino],
        data.holdMinoPosition, Quaternion.identity);

        if (spawnHoldMino)
        {
            return spawnHoldMino;
        }
        else
        {
            return null;
        }
    }
}

