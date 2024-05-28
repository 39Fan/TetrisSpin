using System.Collections.Generic;
using UnityEngine;


// ↓このスクリプトで可能なこと↓ //

// 生成するミノの決定
// ゲーム画面にミノを表示する

/// <summary>
/// ミノの出現に関するスクリプト
/// ・生成するミノの決定
/// ・ミノをゲーム画面に表示
/// </summary>
public class Spawner : MonoBehaviour
{
    /// <summary>ミノのPrefabs(アルファベット順)</summary>
    [SerializeField] private Mino[] minos;

    /// <summary>ゴーストミノのPrefabs(アルファベット順)</summary>
    [SerializeField] private Mino[] ghostMinos;

    // // ミノの名前 //
    // private string[] MinoNames = new string[]
    // {
    //     "I_Mino", "J_Mino", "L_Mino", "O_Mino", "S_Mino", "T_Mino", "Z_Mino"
    // };

    /// <summary>ミノの名前と対応するミノの辞書</summary>
    private Dictionary<MinoType, Mino> minoDictionary;

    /// <summary>ゴーストミノの名前と対応するゴーストミノの辞書</summary>
    private Dictionary<MinoType, Mino> ghostMinoDictionary;

    /// <summary>生成されるミノの順番</summary>
    [SerializeField] private List<MinoType> spawnMinoOrders = new List<MinoType>();

    /// <summary>操作中のミノ</summary>
    private Mino activeMino;
    /// <summary>ゴーストミノ</summary>
    private Mino ghostMino;
    /// <summary>ネクストミノ(次に出現するミノ)</summary>
    private Mino[] nextMinos = new Mino[5];
    /// <summary>ホールドミノ(保持されたミノ)</summary>
    private Mino holdMino;

    /// <summary>操作中のミノの名前</summary>
    private MinoType activeMinoName;
    /// <summary>保持されたミノの名前</summary>
    private MinoType holdMinoName;

    /// <summary>MinoType 列挙型のすべての値を配列として取得</summary>
    // private MinoType[] minoTypes = (MinoType[])System.Enum.GetValues(typeof(MinoType));

    /// <summary>操作中のミノから底までの距離(ゴーストミノの生成座標の計算で必要)</summary>
    private int activeMinoToBaseDistance;

    /// <summary>ミノの生成座標</summary>
    private Vector3 spawnMinoPosition { get; } = new Vector3(4, 19, 0);
    /// <summary>ホールドミノの生成座標</summary>
    private Vector3 holdMinoPosition { get; } = new Vector3(-3, 17, 0);
    /// <summary>ネクストミノの生成座標</summary>
    private Vector3[] nextMinoPositions = new Vector3[5]
    {
        new Vector3(12, 17, 0),
        new Vector3(12, 14, 0),
        new Vector3(12, 11, 0),
        new Vector3(12, 8, 0),
        new Vector3(12, 5, 0)
    };

    /// <summary>ゲッタープロパティ: 操作中のミノ</summary>
    public Mino ActiveMino
    {
        get { return activeMino; }
    }
    /// <summary>ゲッタープロパティ: ゴーストミノ</summary>
    public Mino GhostMino
    {
        get { return GhostMino; }
    }
    /// <summary>ゲッタープロパティ: 操作中のミノの名前</summary>
    public MinoType ActiveMinoName
    {
        get { return activeMinoName; }
    }

    // 干渉するスクリプト //
    Board board;
    GameStatus gameStatus;

    /// <summary>
    /// インスタンス化時の処理
    /// ミノとゴーストミノの辞書を作成
    /// </summary>
    private void Awake()
    {
        board = FindObjectOfType<Board>();
        gameStatus = FindObjectOfType<GameStatus>();

        minoDictionary = new Dictionary<MinoType, Mino>();
        ghostMinoDictionary = new Dictionary<MinoType, Mino>();

        // ミノの名前と対応するミノの辞書を作成
        CreateMinoDictionary();
        // ゴーストミノの名前と対応するゴーストミノの辞書を作成
        CreateGhostMinoDictionary();
    }

    private void CreateMinoDictionary()
    {
        minoDictionary = new Dictionary<MinoType, Mino>();

        if (minos.Length != System.Enum.GetValues(typeof(MinoType)).Length)
        {
            Debug.LogError("Mino array length does not match MinoType enum length.");
            return;
        }

        for (int i = 0; i < minos.Length; i++)
        {
            minoDictionary.Add((MinoType)i, minos[i]);
        }
    }

    private void CreateGhostMinoDictionary()
    {
        ghostMinoDictionary = new Dictionary<MinoType, Mino>();

        if (ghostMinos.Length != System.Enum.GetValues(typeof(MinoType)).Length)
        {
            // Debug.LogError("Mino array length does not match MinoType enum length.");
            return;
        }

        for (int i = 0; i < ghostMinos.Length; i++)
        {
            ghostMinoDictionary.Add((MinoType)i, ghostMinos[i]);
        }
    }

    /// <summary>
    /// 生成されるミノの順番を決める関数
    /// </summary>
    public void DetermineSpawnMinoOrder()
    {
        // ミノの名前が入るリスト
        // List<MinoType> tempMinoNames = new List<MinoType>();

        // MinoType 列挙型のすべての値を取得
        MinoType[] minoTypes = (MinoType[])System.Enum.GetValues(typeof(MinoType));
        List<MinoType> tempMinoNames = new List<MinoType>(minoTypes);

        // // minoNamesにミノの名前を全て入れる
        // for (int i = 0; i < minos.Length; i++)
        // {
        //     MinoType minoNames = (MinoType)i;
        //     tempMinoNames.Add(minoNames);
        // }

        while (tempMinoNames.Count > 0) // minoNames の配列がなくなるまで繰り返す
        {
            // 0から minoNames の配列数までの範囲でランダムな数値を取得し index に格納
            int index = Random.Range(0, tempMinoNames.Count);

            // minoNames[index] の名前を randomName に格納
            MinoType randomName = tempMinoNames[index];

            // randomNameを spawnMinoOrders に追加
            spawnMinoOrders.Add(randomName);

            // インデックス位置の要素を削除
            tempMinoNames.RemoveAt(index);
        }
    }

    // activeMino から底までの距離を計算する関数 //
    private void CheckActiveMinoToBaseDistance()
    {
        // activeMino の各座標を格納する変数を宣言
        int activeMino_x = Mathf.RoundToInt(activeMino.transform.position.x);
        int activeMino_y = Mathf.RoundToInt(activeMino.transform.position.y);
        int activeMino_z = Mathf.RoundToInt(activeMino.transform.position.z);

        // ゲームボードの高さのマスの数 + 2　回繰り返す
        for (activeMinoToBaseDistance = 0; activeMinoToBaseDistance < board.Height - board.Header + 2; activeMinoToBaseDistance++)
        {
            // activeMino のY座標を activeMinoToBaseDistance の値だけ下に移動する
            activeMino.transform.position = new Vector3
                (activeMino_x, activeMino_y - activeMinoToBaseDistance, activeMino_z);

            // activeMino が他のミノにぶつかる、またはゲームボードからはみ出した時
            if (!board.CheckPosition(activeMino))
            {
                // この段階で activeMinoToBaseDistance から1引いた値が activeMino から底までの距離となる
                activeMinoToBaseDistance--;

                // activeMinoの位置を元に戻す
                activeMino.transform.position = new Vector3
                    (activeMino_x, activeMino_y, activeMino_z);

                // breakでこのfor文を抜けて activeMinoToBaseDistance の値を決定
                break;
            }

            // activeMinoを元の位置に戻す
            activeMino.transform.position = new Vector3
                (activeMino_x, activeMino_y, activeMino_z);
        }
    }

    // 新しいactiveMinoを生成する関数
    public void CreateNewActiveMino(int _MinoPopNumber)
    {
        if (_MinoPopNumber % 7 == 0) // 生成数が7の倍数の時
        {
            DetermineSpawnMinoOrder(); // 生成されるミノを補充
        }

        activeMino = SpawnactiveMino(minoDictionary[spawnMinoOrders[_MinoPopNumber]]); // 実際に生成する

        activeMinoName = spawnMinoOrders[_MinoPopNumber]; // 名前を保存

        if (GhostMino) // すでにゴーストミノが存在する時
        {
            Destroy(GhostMino.gameObject); // 古いゴーストミノを削除
        }

        CheckActiveMinoToBaseDistance(); // activeMinoToBaseDistance の計算

        ghostMino = SpawnGhostMino(ghostMinoDictionary[spawnMinoOrders[_MinoPopNumber]], activeMino, activeMinoToBaseDistance); // ゴーストミノの生成も同時に行う
    }

    // ゴーストミノの位置調整を行う関数
    public void AdjustGhostMinoPosition()
    {
        CheckActiveMinoToBaseDistance(); // activeMinoToBaseDistance の計算

        // activeMino の情報を取得
        int activeMinoPos_x = Mathf.RoundToInt(activeMino.transform.position.x);
        int activeMinoPos_y = Mathf.RoundToInt(activeMino.transform.position.y);
        int activeMinoPos_z = Mathf.RoundToInt(activeMino.transform.position.z);

        GhostMino.transform.rotation = activeMino.transform.rotation; // 向きの調整
        GhostMino.transform.position = new Vector3(activeMinoPos_x, activeMinoPos_y - activeMinoToBaseDistance, activeMinoPos_z); // 位置の調整
    }

    // Nextミノを生成する関数 //
    public void CreateNewNextMinos(int _MinoPopNumber)
    {
        for (int nextMinoOrder = 0; nextMinoOrder < nextMinos.Length; nextMinoOrder++) // NextMinosの数だけ繰り返す
        {
            if (_MinoPopNumber == 0) // ゲームスタート時
            {
                // Nextミノを一個ずつ生成していく
                nextMinos[nextMinoOrder] = SpawnNextMino(minoDictionary[spawnMinoOrders[_MinoPopNumber + nextMinoOrder + 1]], nextMinoOrder);
            }
            else // 2回目以降
            {
                //以前のNextMinoを消去
                Destroy(nextMinos[nextMinoOrder].gameObject);

                nextMinos[nextMinoOrder] = SpawnNextMino(minoDictionary[spawnMinoOrders[_MinoPopNumber + nextMinoOrder + 1]], nextMinoOrder);
            }
        }
    }

    // Hold機能の処理をする関数 //
    public void CreateNewHoldMino(bool _FirstHold, int _MinoPopNumber)
    {
        // 1回目のHold
        if (_FirstHold == true)
        {
            Destroy(activeMino.gameObject); // activeMinoを削除

            holdMinoName = activeMinoName; // activeMinoの名前を保存

            holdMino = SpawnHoldMino(minoDictionary[holdMinoName]); // Holdされたミノを画面左上に表示

            CreateNewActiveMino(_MinoPopNumber); // 新しいactiveMinoを生成

            CreateNewNextMinos(_MinoPopNumber); // 新しいNextミノを生成
        }

        // 2回目以降のHold
        else
        {
            Destroy(activeMino.gameObject); // activeMinoを削除
            Destroy(GhostMino.gameObject); // GhostMinoを削除

            // activeMinoName と HoldMinoName の名前を交換する
            MinoType temp;
            temp = activeMinoName;
            activeMinoName = holdMinoName;
            holdMinoName = temp;

            activeMino = SpawnactiveMino(holdMino); // HoldミノをactiveMinoに戻す

            CheckActiveMinoToBaseDistance(); // activeMinoToBaseDistance の計算

            ghostMino = SpawnGhostMino(ghostMinoDictionary[activeMinoName], activeMino, activeMinoToBaseDistance);

            Destroy(holdMino.gameObject); // 以前のホールドミノを削除

            holdMino = SpawnHoldMino(minoDictionary[holdMinoName]); // Holdされたミノを画面左上に表示

            // 変数の初期化
            gameStatus.Reset_Angle();
            gameStatus.Reset_StepsSRS();
        }
    }

    // 選ばれたミノを生成する関数 //
    public Mino SpawnactiveMino(Mino _SelecyMino)
    {
        Mino activeMino = Instantiate(_SelecyMino,
        spawnMinoPosition, Quaternion.identity); // Quaternion.identityは、向きの回転に関する設定をしないことを表す

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
    public Mino SpawnGhostMino(Mino _SelectMino, Mino _activeMino, int _activeMinoToBaseDistance)
    {
        // activeMinoの各座標を格納する変数を宣言
        int activeMinoInfo_x = Mathf.RoundToInt(_activeMino.transform.position.x);
        int activeMinoInfo_y = Mathf.RoundToInt(_activeMino.transform.position.y);
        int activeMinoInfo_z = Mathf.RoundToInt(_activeMino.transform.position.z);

        //orderに対応するゴーストミノを、active_mino_infoのY座標からactiveMinoToBaseDistanceの値だけ下に移動した位置に生成
        Mino ghostMino = Instantiate(_SelectMino,
            new Vector3(activeMinoInfo_x, activeMinoInfo_y - _activeMinoToBaseDistance, activeMinoInfo_z), Quaternion.identity);

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
        Mino nextMino = Instantiate(_SelectMino,
            nextMinoPositions[_nextMinoOrder], Quaternion.identity);

        return nextMino;
    }

    //Holdされたミノを表示する関数
    public Mino SpawnHoldMino(Mino _SelectMino)
    {
        //minoに対応するミノを生成
        //Quaternion.identityは、向きの回転に関する設定をしないことを表す
        Mino spawnHoldMino = Instantiate(_SelectMino,
        holdMinoPosition, Quaternion.identity);

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
