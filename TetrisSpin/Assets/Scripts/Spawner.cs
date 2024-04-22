using UnityEngine;

//ミノの出現に関するスクリプト

// public class SpawnMinos : MonoBehaviour
// {
//     //ミノのPrefabsをminosに格納
//     //順番は(I, J, L, O, S, T, Z)
//     public Mino[] Minos;

//     //ゴーストミノについて//

//     //ゴーストミノとは、操作中のミノをそのままドロップした時、またはハードドロップした時に
//     //設置される想定の場所を薄く表示するミノのこと
//     //これを実装することで、テトリスのプレイが格段にしやすくなる

//     //ゴーストミノのPrefabsをminos_Ghostに格納
//     //順番は(I, J, L, O, S, T, Z)
//     public Mino_Ghost[] Minos_Ghost;
// }

public class Spawner : MonoBehaviour
{
    //各種干渉するスクリプトの設定
    //Board board;
    Calculate calculate;
    TetrisSpinData tetrisSpinData;
    GameStatus gameStatus;
    // SpawnMinos spawnMinos;

    //インスタンス化
    private void Awake()
    {
        //board = FindObjectOfType<Board>();
        calculate = FindObjectOfType<Calculate>();
        tetrisSpinData = FindObjectOfType<TetrisSpinData>();
        gameStatus = FindObjectOfType<GameStatus>();
        // SpawnMinos spawnMinos = new SpawnMinos();
    }

    //選ばれたミノを生成する関数
    // 新しいActiveMinoまたは、Holdされたミノが選択肢にあるため、仮引数名は_SelectMinoにしている
    public Mino SpawnMino(int _SelectMino)
    {
        //Minos[mino].transform.localScale = Vector3.one * 1;

        //Quaternion.identityは、向きの回転に関する設定をしないことを表す
        Mino newSpawnMino = Instantiate(tetrisSpinData.MINOS[_SelectMino],
        tetrisSpinData.SPAWNMINOPOSITION, Quaternion.identity);

        if (newSpawnMino)
        {
            return newSpawnMino;
        }
        else
        {
            return null;
        }
    }

    //ゴーストミノを生成する関数
    public Mino_Ghost SpawnMino_Ghost()
    {
        //active_mino_infoの各座標を格納する変数を宣言
        int activeMinoInfo_x = Mathf.RoundToInt(gameStatus.ActiveMino.transform.position.x);
        int activeMinoInfo_y = Mathf.RoundToInt(gameStatus.ActiveMino.transform.position.y);
        int activeMinoInfo_z = Mathf.RoundToInt(gameStatus.ActiveMino.transform.position.z);

        //active_mino_infoから他のミノ、または底までの距離を計算
        calculate.CheckDistance_Y();

        // gameStatus.ActiveMinoの種類を判別
        int order = calculate.CheckActiveMinoShape();

        //orderに対応するゴーストミノを、active_mino_infoのY座標からdistanceの値だけ下に移動した位置に生成
        Mino_Ghost ghostMino = Instantiate(tetrisSpinData.MINOS_GHOST[order],
            new Vector3(activeMinoInfo_x, activeMinoInfo_y - gameStatus.Distance, activeMinoInfo_z), Quaternion.identity);

        if (ghostMino)
        {
            return ghostMino;
        }
        else
        {
            return null;
        }
    }

    //Nextミノを表示する関数
    public Mino SpawnNextMinos()
    {
        //Debug.Log("====this is SpawnNextMinos====");

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
            if (gameStatus.MinoPopNumber == 0)
            {
                //Next1〜5の表示
                //nextMinosに格納
                gameStatus.NextMino_Array[nextMinoOrder] = Instantiate(tetrisSpinData.MINOS[gameStatus.SpawnMinoOrder_List[gameStatus.MinoPopNumber + nextMinoOrder + 1]],
                    nextMinoPositions[nextMinoOrder], Quaternion.identity);
            }
            //2回目以降のNext表示
            else
            {
                //以前のNextMinoを消去
                Destroy(gameStatus.NextMino_Array[nextMinoOrder].gameObject);

                //Next1〜5の表示
                //nextMinosに格納
                gameStatus.NextMino_Array[nextMinoOrder] = Instantiate(tetrisSpinData.MINOS[gameStatus.SpawnMinoOrder_List[gameStatus.MinoPopNumber + nextMinoOrder + 1]],
                    nextMinoPositions[nextMinoOrder], Quaternion.identity);
            }
        }
        return null;
    }

    //Holdされたミノを表示する関数
    public Mino SpawnHoldMino()
    {
        //minoに対応するミノを生成
        //Quaternion.identityは、向きの回転に関する設定をしないことを表す
        Mino spawnHoldMino = Instantiate(tetrisSpinData.MINOS[gameStatus.HoldMinoNumber],
        tetrisSpinData.HOLDMINOPOSITION, Quaternion.identity);

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

