using System.Collections.Generic;
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

// public class Spawner : MonoBehaviour
// {
//     //各種干渉するスクリプトの設定
//     //Board board;
//     Calculate calculate;
//     TetrisSpinData tetrisSpinData;
//     GameStatus gameStatus;
//     Mino Mino;
//     // SpawnMinos spawnMinos;

//     //インスタンス化
//     private void Awake()
//     {
//         //board = FindObjectOfType<Board>();
//         calculate = FindObjectOfType<Calculate>();
//         tetrisSpinData = FindObjectOfType<TetrisSpinData>();
//         gameStatus = FindObjectOfType<GameStatus>();
//         Mino = FindObjectOfType<Mino>();
//         // SpawnMinos spawnMinos = new SpawnMinos();
//     }

//     //選ばれたミノを生成する関数
//     // 新しいActiveMinoまたは、Holdされたミノが選択肢にあるため、仮引数名は_SelectMinoにしている
//     public Mino SpawnMino(int _SelectMino)
//     {
//         //Minos[mino].transform.localScale = Vector3.one * 1;

//         //Quaternion.identityは、向きの回転に関する設定をしないことを表す
//         Mino newSpawnMino = Instantiate(tetrisSpinData.MINOS[_SelectMino],
//         tetrisSpinData.SPAWNMINOPOSITION, Quaternion.identity);


//     }

//     //ゴーストミノを生成する関数
//     public Mino_Ghost SpawnMino_Ghost()
//     {
//         //active_mino_infoの各座標を格納する変数を宣言
//         int activeMinoInfo_x = Mathf.RoundToInt(gameStatus.ActiveMino.transform.position.x);
//         int activeMinoInfo_y = Mathf.RoundToInt(gameStatus.ActiveMino.transform.position.y);
//         int activeMinoInfo_z = Mathf.RoundToInt(gameStatus.ActiveMino.transform.position.z);

//         //active_mino_infoから他のミノ、または底までの距離を計算
//         calculate.CheckDistance_Y();

//         // gameStatus.ActiveMinoの種類を判別
//         int order = calculate.CheckActiveMinoShape();

//         //orderに対応するゴーストミノを、active_mino_infoのY座標からdistanceの値だけ下に移動した位置に生成
//         Mino_Ghost ghostMino = Instantiate(tetrisSpinData.MINOS_GHOST[order],
//             new Vector3(activeMinoInfo_x, activeMinoInfo_y - gameStatus.Distance, activeMinoInfo_z), Quaternion.identity);

//         if (ghostMino)
//         {
//             return ghostMino;
//         }
//         else
//         {
//             return null;
//         }
//     }

//     //Nextミノを表示する関数
//     public Mino SpawnNextMinos()
//     {
//         //Debug.Log("====this is SpawnNextMinos====");

//         //Nextの数は5に設定
//         int nexts = 5;

//         //ゲーム画面で表示するNext1〜5の座標を格納する配列
//         Vector3[] nextMinoPositions = new Vector3[nexts];

//         //Next1〜5の座標は、Y座標を3ずつ下に下げて配置する
//         int position_y = 3;

//         //Nextの数だけ繰り返す
//         for (int count = 0; count < nexts; count++)
//         {
//             //Y座標をposition_yずつ下げて宣言
//             nextMinoPositions[count] = new Vector3(12, 17 - (position_y * count), 0);
//         }

//         //以下のように設定される
//         // nextMinoPosition[0] = new Vector3 (12, 17, 0);
//         // nextMinoPosition[1] = new Vector3 (12, 14, 0);
//         // nextMinoPosition[2] = new Vector3 (12, 11, 0);
//         // nextMinoPosition[3] = new Vector3 (12, 8, 0);
//         // nextMinoPosition[4] = new Vector3 (12, 5, 0);

//         //Nextの数だけ繰り返す
//         for (int nextMinoOrder = 0; nextMinoOrder < nexts; nextMinoOrder++)
//         {
//             //ゲームスタート時のNext表示
//             if (gameStatus.MinoPopNumber == 0)
//             {
//                 //Next1〜5の表示
//                 //nextMinosに格納
//                 gameStatus.NextMino_Array[nextMinoOrder] = Instantiate(tetrisSpinData.MINOS[gameStatus.SpawnMinoOrder_List[gameStatus.MinoPopNumber + nextMinoOrder + 1]],
//                     nextMinoPositions[nextMinoOrder], Quaternion.identity);
//             }
//             //2回目以降のNext表示
//             else
//             {
//                 //以前のNextMinoを消去
//                 Destroy(gameStatus.NextMino_Array[nextMinoOrder].gameObject);

//                 //Next1〜5の表示
//                 //nextMinosに格納
//                 gameStatus.NextMino_Array[nextMinoOrder] = Instantiate(tetrisSpinData.MINOS[gameStatus.SpawnMinoOrder_List[gameStatus.MinoPopNumber + nextMinoOrder + 1]],
//                     nextMinoPositions[nextMinoOrder], Quaternion.identity);
//             }
//         }
//         return null;
//     }

//     //Holdされたミノを表示する関数
//     public Mino SpawnHoldMino()
//     {
//         //minoに対応するミノを生成
//         //Quaternion.identityは、向きの回転に関する設定をしないことを表す
//         Mino spawnHoldMino = Instantiate(tetrisSpinData.MINOS[gameStatus.HoldMinoNumber],
//         tetrisSpinData.HOLDMINOPOSITION, Quaternion.identity);

//         if (spawnHoldMino)
//         {
//             return spawnHoldMino;
//         }
//         else
//         {
//             return null;
//         }
//     }
// }

public class Spawner : MonoBehaviour
{
    //各種干渉するスクリプトの設定
    //Board board;
    // Calculate calculate;
    // TetrisSpinData tetrisSpinData;
    // GameStatus gameStatus;
    // Mino Mino;
    // SpawnMinos spawnMinos;

    // ミノのPrefabs //
    [SerializeField] private Mino[] Minos; // 順番は(I, J, L, O, S, T, Z)
    [SerializeField] private Mino[] GhostMinos; // 順番は(I, J, L, O, S, T, Z)

    // ミノの名前 //
    private string[] MinoNames = new string[]
    {
        "I_Mino", "J_Mino", "L_Mino", "O_Mino", "S_Mino", "T_Mino", "Z_Mino"
    };

    // Dictionaryの作成 //
    Dictionary<string, Mino> MinoDictionary = new Dictionary<string, Mino>(); // MinosとMinoNamesのDictionary
    Dictionary<string, Mino> GhostMinoDictionary = new Dictionary<string, Mino>(); // GhostMinosとMinoNamesのDictionary

    // 生成されるミノの順番 //
    private List<string> SpawnMinoOrders = new List<string>();

    // ゲームに可視化されるミノ //
    private Mino ActiveMino; // 操作中のミノ
    private Mino GhostMino; // ゴーストミノ
    private Mino[] NextMinos = new Mino[5]; // Nextミノ
    private Mino HoldMino; // Holdミノ

    // ActiveMinoとHoldMinoの名前 //
    private string ActiveMinoName;
    private string HoldMinoName;

    // ゲッタープロパティ //
    public Mino activeMino
    {
        get { return ActiveMino; }
    }
    public Mino ghostMino
    {
        get { return GhostMino; }
    }
    public string activeMinoName
    {
        get { return ActiveMinoName; }
    }

    // ActiveMinoから底までの距離 //
    private int ActiveMinoToBaseDistance; // ゴーストミノの生成座標の計算で必要

    // ミノの生成座標
    private Vector3 SpawnMinoPosition { get; } = new Vector3(4, 19, 0); // ActiveMino
    private Vector3 HoldMinoPosition { get; } = new Vector3(-3, 17, 0); // HoldMino
    private Vector3[] NextMinoPositions = new Vector3[5] // NextMinos
    {
        new Vector3(12, 17, 0),
        new Vector3(12, 14, 0),
        new Vector3(12, 11, 0),
        new Vector3(12, 8, 0),
        new Vector3(12, 5, 0)
    };

    // 干渉するスクリプト //
    Board board;
    GameStatus gameStatus;
    //Spawner spawner;
    //TetrisSpinData tetrisSpinData;

    // インスタンス化 //
    private void Awake()
    {
        board = FindObjectOfType<Board>();
        gameStatus = FindObjectOfType<GameStatus>();
        //spawner = new Spawner();
        //tetrisSpinData = FindObjectOfType<TetrisSpinData>();

        // Minos と MinoNames に対応する要素を MinoDictionary に追加
        for (int i = 0; i < MinoNames.Length; i++)
        {
            MinoDictionary[MinoNames[i]] = Minos[i];
        }
        // GhostMinos と MinoNames に対応する要素を MinoDictionary に追加
        for (int i = 0; i < MinoNames.Length; i++)
        {
            GhostMinoDictionary[MinoNames[i]] = GhostMinos[i];
        }
    }

    // 生成されるミノの順番を決める関数 //
    public void DetermineSpawnMinoOrder()
    {
        List<string> minoNames = new List<string>(); // ミノの名前が入るリスト

        // minoNamesにミノの名前を全て入れる
        for (int numbers = 0; numbers < MinoNames.Length; numbers++)
        {
            minoNames.Add(MinoNames[numbers]);
        }

        while (minoNames.Count > 0) // minoNames の配列がなくなるまで繰り返す
        {
            // 0から minoNames の配列数までの範囲でランダムな数値を取得し index に格納
            int index = Random.Range(0, minoNames.Count);

            // minoNames[index] の名前を randomName に格納
            string randomName = minoNames[index];

            // randomNameを SpawnMinoOrders に追加
            SpawnMinoOrders.Add(randomName);

            // インデックス位置の要素を削除
            minoNames.RemoveAt(index);
        }
    }

    // ActiveMino から底までの距離を計算する関数 //
    private void CheckActiveMinoToBaseDistance()
    {
        // ActiveMino の各座標を格納する変数を宣言
        int activeMino_x = Mathf.RoundToInt(ActiveMino.transform.position.x);
        int activeMino_y = Mathf.RoundToInt(ActiveMino.transform.position.y);
        int activeMino_z = Mathf.RoundToInt(ActiveMino.transform.position.z);

        // ゲームボードの高さのマスの数 + 2　回繰り返す
        for (ActiveMinoToBaseDistance = 0; ActiveMinoToBaseDistance < board.height - board.header + 2; ActiveMinoToBaseDistance++)
        {
            // ActiveMino のY座標を ActiveMinoToBaseDistance の値だけ下に移動する
            ActiveMino.transform.position = new Vector3
                (activeMino_x, activeMino_y - ActiveMinoToBaseDistance, activeMino_z);

            // ActiveMino が他のミノにぶつかる、またはゲームボードからはみ出した時
            if (!board.CheckPosition(ActiveMino))
            {
                // この段階で ActiveMinoToBaseDistance から1引いた値が ActiveMino から底までの距離となる
                ActiveMinoToBaseDistance--;

                // ActiveMinoの位置を元に戻す
                ActiveMino.transform.position = new Vector3
                    (activeMino_x, activeMino_y, activeMino_z);

                // breakでこのfor文を抜けて ActiveMinoToBaseDistance の値を決定
                break;
            }

            // ActiveMinoを元の位置に戻す
            ActiveMino.transform.position = new Vector3
                (activeMino_x, activeMino_y, activeMino_z);
        }
    }

    // 新しいActiveMinoを生成する関数
    public void CreateNewActiveMino(int _MinoPopNumber)
    {
        if (_MinoPopNumber % 7 == 0) // 生成数が7の倍数の時
        {
            DetermineSpawnMinoOrder(); // 生成されるミノを補充
        }

        ActiveMino = SpawnActiveMino(MinoDictionary[SpawnMinoOrders[_MinoPopNumber]]); // 実際に生成する

        ActiveMinoName = SpawnMinoOrders[_MinoPopNumber]; // 名前を保存

        if (GhostMino) // すでにゴーストミノが存在する時
        {
            Destroy(GhostMino.gameObject); // 古いゴーストミノを削除
        }

        CheckActiveMinoToBaseDistance(); // ActiveMinoToBaseDistance の計算

        GhostMino = SpawnGhostMino(GhostMinoDictionary[SpawnMinoOrders[_MinoPopNumber]], ActiveMino, ActiveMinoToBaseDistance); // ゴーストミノの生成も同時に行う

        // gameStatus.AllReset();
    }

    // ゴーストミノの位置調整を行う関数
    public void AdjustGhostMinoPosition()
    {
        CheckActiveMinoToBaseDistance(); // ActiveMinoToBaseDistance の計算

        // ActiveMino の情報を取得
        int activeMinoPos_x = Mathf.RoundToInt(ActiveMino.transform.position.x);
        int activeMinoPos_y = Mathf.RoundToInt(ActiveMino.transform.position.y);
        int activeMinoPos_z = Mathf.RoundToInt(ActiveMino.transform.position.z);

        GhostMino.transform.rotation = ActiveMino.transform.rotation; // 向きの調整
        GhostMino.transform.position = new Vector3(activeMinoPos_x, activeMinoPos_y - ActiveMinoToBaseDistance, activeMinoPos_z); // 位置の調整
    }

    // Nextミノを生成する関数 //
    public void CreateNewNextMinos(int _MinoPopNumber)
    {
        for (int nextMinoOrder = 0; nextMinoOrder < NextMinos.Length; nextMinoOrder++) // NextMinosの数だけ繰り返す
        {
            if (_MinoPopNumber == 0) // ゲームスタート時
            {
                // Nextミノを一個ずつ生成していく
                NextMinos[nextMinoOrder] = SpawnNextMino(MinoDictionary[SpawnMinoOrders[_MinoPopNumber + nextMinoOrder + 1]], nextMinoOrder);
            }
            else // 2回目以降
            {
                //以前のNextMinoを消去
                Destroy(NextMinos[nextMinoOrder].gameObject);

                NextMinos[nextMinoOrder] = SpawnNextMino(MinoDictionary[SpawnMinoOrders[_MinoPopNumber + nextMinoOrder + 1]], nextMinoOrder);
            }
        }
    }

    // Hold機能の処理をする関数 //
    public void CreateNewHoldMino(bool _FirstHold, int _MinoPopNumber)
    {
        // 1回目のHold
        if (_FirstHold == true)
        {
            Destroy(ActiveMino.gameObject); // ActiveMinoを削除

            HoldMinoName = ActiveMinoName; // ActiveMinoの名前を保存

            HoldMino = SpawnHoldMino(MinoDictionary[HoldMinoName]); // Holdされたミノを画面左上に表示

            //1回目のHoldでは、新しく生成されるミノはNext1のミノになるので、
            //countを1つ進める
            //count++;

            //count進行
            //gameStatus.MinoPopNumber++;

            //countが7の倍数の時
            // if (gameStatus.MinoPopNumber % 7 == 0)
            // {
            //     //ミノの配列の補充
            //     DetermineSpawnMinoOrder();

            //     //次のActiveMinoの生成
            //     gameStatus.ActiveMino = spawner.SpawnMino(gameStatus.SpawnMinoOrder_List[gameStatus.MinoPopNumber]);
            // }
            // else
            // {
            //     //次のActiveMinoの生成
            //     gameStatus.ActiveMino = spawner.SpawnMino(gameStatus.SpawnMinoOrder_List[gameStatus.MinoPopNumber]);
            // }

            // //activeMinoの種類を判別
            // //CheckActiveMinoShape();

            // //変数の初期化
            // gameStatus.AngleReset();
            // gameStatus.SpinResetFlag();

            // //Nextの表示
            // spawner.SpawnNextMinos();

            // gameStatus.FirstHold = false;

            CreateNewActiveMino(_MinoPopNumber); // 新しいActiveMinoを生成

            CreateNewNextMinos(_MinoPopNumber); // 新しいNextミノを生成
        }

        // 2回目以降のHold
        else
        {
            Destroy(ActiveMino.gameObject); // ActiveMinoを削除
            Destroy(GhostMino.gameObject); // GhostMinoを削除

            // ActiveMinoName と HoldMinoName の名前を交換する
            string temp;
            temp = ActiveMinoName;
            ActiveMinoName = HoldMinoName;
            HoldMinoName = temp;


            ActiveMino = SpawnActiveMino(HoldMino); // HoldミノをActiveMinoに戻す

            AdjustGhostMinoPosition();  // ゴーストミノの位置調整

            GhostMino = SpawnGhostMino(GhostMinoDictionary[ActiveMinoName], ActiveMino, ActiveMinoToBaseDistance);

            //activeMinoの種類を判別
            //CheckActiveMinoShape();

            //1つ上のコードでactiveMinoが変化しているため、
            //holdMinoCountに、 "以前" のactiveMinoの種類の数値情報を格納
            //gameStatus.HoldMinoNumber = gameStatus.SpawnMinoOrder_List[gameStatus.MinoPopNumber];

            //以前のホールドミノを削除
            Destroy(HoldMino.gameObject);

            HoldMino = SpawnHoldMino(MinoDictionary[HoldMinoName]); // Holdされたミノを画面左上に表示

            //変数の初期化
            gameStatus.Reset_Angle();
            gameStatus.Reset_LastSRS();
        }
    }

    // 選ばれたミノを生成する関数 //
    public Mino SpawnActiveMino(Mino _SelecyMino)
    {
        //Minos[mino].transform.localScale = Vector3.one * 1;

        Mino activeMino = Instantiate(_SelecyMino,
        SpawnMinoPosition, Quaternion.identity); // Quaternion.identityは、向きの回転に関する設定をしないことを表す

        if (activeMino)
        {
            return activeMino;
        }
        else
        {
            return null;
        }
    }

    // ゴーストミノを生成する関数 //
    public Mino SpawnGhostMino(Mino _SelectMino, Mino _ActiveMino, int _ActiveMinoToBaseDistance)
    {
        // ActiveMinoの各座標を格納する変数を宣言
        int activeMinoInfo_x = Mathf.RoundToInt(_ActiveMino.transform.position.x);
        int activeMinoInfo_y = Mathf.RoundToInt(_ActiveMino.transform.position.y);
        int activeMinoInfo_z = Mathf.RoundToInt(_ActiveMino.transform.position.z);

        //active_mino_infoから他のミノ、または底までの距離を計算
        //calculate.CheckDistance_Y();

        // gameStatus.ActiveMinoの種類を判別
        // int order = calculate.CheckActiveMinoShape();

        //orderに対応するゴーストミノを、active_mino_infoのY座標からActiveMinoToBaseDistanceの値だけ下に移動した位置に生成
        Mino ghostMino = Instantiate(_SelectMino,
            new Vector3(activeMinoInfo_x, activeMinoInfo_y - _ActiveMinoToBaseDistance, activeMinoInfo_z), Quaternion.identity);

        if (ghostMino)
        {
            return ghostMino;
        }
        else
        {
            return null;
        }
    }

    // Nextミノを表示する関数 //
    public Mino SpawnNextMino(Mino _SelectMino, int _nextMinoOrder)
    {
        //Debug.Log("====this is SpawnNextMinos====");

        // Nextの数は5に設定
        // int nexts = 5;

        // // Nextの数だけ繰り返す
        // for (int nextMinoOrder = 0; nextMinoOrder < nexts; nextMinoOrder++)
        // {
        //     //ゲームスタート時のNext表示
        //     if (_MinoPopNumber == 0)
        //     {
        //         //Next1〜5の表示
        //         //nextMinosに格納
        //         Nextminos[] = Instantiate(tetrisSpinData.MINOS[gameStatus.SpawnMinoOrder_List[gameStatus.MinoPopNumber + nextMinoOrder + 1]],
        //             NextMinoPositions[nextMinoOrder], Quaternion.identity);
        //     }
        //     //2回目以降のNext表示
        //     else
        //     {
        //         //以前のNextMinoを消去
        //         Destroy(gameStatus.NextMino_Array[nextMinoOrder].gameObject);

        //         //Next1〜5の表示
        //         //nextMinosに格納
        //         gameStatus.NextMino_Array[nextMinoOrder] = Instantiate(tetrisSpinData.MINOS[gameStatus.SpawnMinoOrder_List[gameStatus.MinoPopNumber + nextMinoOrder + 1]],
        //             NextMinoPositions[nextMinoOrder], Quaternion.identity);
        //     }
        // }
        // return null;

        Mino nextMino = Instantiate(_SelectMino,
            NextMinoPositions[_nextMinoOrder], Quaternion.identity);

        return nextMino;
    }

    //Holdされたミノを表示する関数
    public Mino SpawnHoldMino(Mino _SelectMino)
    {
        //minoに対応するミノを生成
        //Quaternion.identityは、向きの回転に関する設定をしないことを表す
        Mino spawnHoldMino = Instantiate(_SelectMino,
        HoldMinoPosition, Quaternion.identity);

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

