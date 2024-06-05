using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawnerの統計情報を保持する静的クラス
/// </summary>
public static class SpawnerStats
{
    /// <summary> 生成されるミノの順番リスト </summary>
    private static List<MinoType> spawnMinoOrders = new List<MinoType>();

    /// <summary> 操作中のミノの名前 </summary>
    private static MinoType activeMinoName;
    /// <summary> ホールドミノの名前 </summary>
    private static MinoType holdMinoName;

    /// <summary> 操作中のミノから底までの距離 </summary>
    /// <remarks>
    /// ゴーストミノの生成座標の計算で必要
    /// </remarks>
    private static int activeMinoToBaseDistance;

    // ゲッタープロパティ //
    public static List<MinoType> SpawnMinoOrders => spawnMinoOrders;
    public static MinoType ActiveMinoName => activeMinoName;
    public static MinoType HoldMinoName => holdMinoName;
    public static int ActiveMinoToBaseDistance => activeMinoToBaseDistance;

    /// <summary> 指定されたフィールドの値を更新する関数 </summary>
    /// <param name="_activeMinoName"> 操作中のミノの名前 </param>
    /// <param name="_holdMinoName"> ホールドミノの名前 </param>
    /// <param name="_activeMinoToBaseDistance"> 操作中のミノから底までの距離 </param>
    /// <remarks>
    /// 指定されていない引数は現在の値を維持
    /// </remarks>
    public static void Update(MinoType? _activeMinoName = null, MinoType? _holdMinoName = null, int? _activeMinoToBaseDistance = null)
    {
        activeMinoName = _activeMinoName ?? activeMinoName;
        holdMinoName = _holdMinoName ?? holdMinoName;
        activeMinoToBaseDistance = _activeMinoToBaseDistance ?? activeMinoToBaseDistance;
        // TODO: ログの記入
    }

    /// <summary> デフォルトの <see cref="AttackCalculatorStats"/> にリセットする関数 </summary>
    public static void Reset()
    {
        spawnMinoOrders.Clear();
        activeMinoName = default;
        holdMinoName = default;
        activeMinoToBaseDistance = 0;
    }

    /// <summary> 生成されるミノの順番リストを追加する関数 </summary>
    /// <param name="_addMinoType"> 追加するミノの種類 </param>
    public static void AddSpawnMinoOrder(MinoType _addMinoType)
    {
        spawnMinoOrders.Add(_addMinoType);
        // TODO: ログの記入
    }
}

/// <summary>
/// ミノの出現に関するスクリプト
/// </summary>
public class Spawner : MonoBehaviour
{
    /// <summary> 操作中のミノ </summary>
    [SerializeField] private MinoMovement activeMino;
    /// <summary> ゴーストミノ </summary>
    private MinoMovement ghostMino;
    /// <summary> ネクストミノリスト </summary>
    private MinoMovement[] nextMino = new MinoMovement[5];
    /// <summary> ホールドミノ </summary>
    private MinoMovement holdMino;

    /// <summary> ミノのPrefabs </summary>
    /// <remarks>
    /// 順番は(I, J, L, O, S, T, Z)
    /// </remarks>
    [SerializeField] private MinoMovement[] minos;
    /// <summary> ゴーストミノのPrefabs </summary>
    /// <remarks>
    /// 順番は(I, J, L, O, S, T, Z)
    /// </remarks>
    [SerializeField] private MinoMovement[] ghostMinos;

    // /// <summary> ミノの名前リスト </summary>
    // private string[] MinoNames = new string[]
    // {
    //     "I_Mino", "J_Mino", "L_Mino", "O_Mino", "S_Mino", "T_Mino", "Z_Mino"
    // };

    /// <summary> MinosとMinoNamesのDictionary </summary>
    Dictionary<MinoType, MinoMovement> minoDictionary = new Dictionary<MinoType, MinoMovement>();

    /// <summary> GhostMinosとMinoNamesのDictionary </summary>
    Dictionary<MinoType, MinoMovement> ghostMinoDictionary = new Dictionary<MinoType, MinoMovement>();

    // /// <summary> 生成されるミノの順番リスト </summary>
    // private List<string> spawnMinoOrders = new List<string>();

    // /// <summary> 操作中のミノ </summary>
    // private MinoMovement activeMino;
    // /// <summary> ゴーストミノ </summary>
    // private MinoMovement ghostMino;
    // /// <summary> ネクストミノリスト </summary>
    // private MinoMovement[] nextMinos = new MinoMovement[5];
    // /// <summary> ホールドミノ </summary>
    // private MinoMovement holdMino;

    // /// <summary> 操作中のミノの名前 </summary>
    // private string activeMinoName;
    // /// <summary> ホールドミノの名前 </summary>
    // private string holdMinoName;

    // /// <summary> ActiveMinoから底までの距離 </summary>
    // /// <remarks>
    // /// ゴーストミノの生成座標の計算で必要
    // /// </remarks>
    // private int activeMinoToBaseDistance;

    /// <summary> 操作中のミノの生成座標 </summary>
    private Vector3 spawnMinoPosition { get; } = new Vector3(4, 19, 0);

    /// <summary> ホールドミノの生成座標 </summary>
    private Vector3 holdMinoPosition { get; } = new Vector3(-3, 17, 0);

    /// <summary> ネクストミノの生成座標リスト </summary>
    private Vector3[] nextMinoPositions = new Vector3[5] // NextMinos
    {
        new Vector3(12, 17, 0),
        new Vector3(12, 14, 0),
        new Vector3(12, 11, 0),
        new Vector3(12, 8, 0),
        new Vector3(12, 5, 0)
    };

    // // ゲッタープロパティ //
    // public MinoMovement ActiveMino => activeMino;
    // public MinoMovement GhostMino => ghostMino;
    // public string ActiveMinoName => activeMinoName;
    public MinoMovement ActiveMino => activeMino;
    public MinoMovement GhostMino => ghostMino;
    public MinoMovement[] NextMino => nextMino;
    public MinoMovement HoldMino => holdMino;

    // 干渉するスクリプト //
    Board board;
    MinoMovement minoMovement;

    /// <summary>
    /// インスタンス化時の処理
    /// </summary>
    /// <remarks>
    /// 辞書の作成も行う
    /// </remarks>
    private void Awake()
    {
        board = FindObjectOfType<Board>();
        minoMovement = FindObjectOfType<MinoMovement>();

        // Minos と MinoNames の辞書を作成
        for (int ii = 0; ii < minos.Length; ii++)
        {
            minoDictionary[(MinoType)ii] = minos[ii];
        }
        // GhostMinos と MinoNames の辞書を作成
        for (int ii = 0; ii < ghostMinos.Length; ii++)
        {
            ghostMinoDictionary[(MinoType)ii] = ghostMinos[ii];
        }
    }

    /// <summary> 生成されるミノの順番を決める関数 </summary>
    public void DetermineSpawnMinoOrder()
    {
        /// <summary> ミノの種類が入るリスト </summary>
        List<MinoType> minoNames = new List<MinoType>();

        for (int ii = 0; ii < MinoType.GetValues(typeof(MinoType)).Length; ii++)
        {
            minoNames.Add((MinoType)ii);
        }

        while (minoNames.Count > 0)
        {
            int index = Random.Range(0, minoNames.Count);
            MinoType randomMinoType = minoNames[index];
            SpawnerStats.AddSpawnMinoOrder(randomMinoType);
            minoNames.RemoveAt(index);
        }
    }

    /// <summary> 操作中のミノから底までの距離を計算する関数 </summary>
    /// <returns> 操作中のミノから底までの距離(newActiveMinoToBaseDistance) </returns>
    private int CheckActiveMinoToBaseDistance()
    {
        /// <summary> 操作中のミノから底までの距離 </summary>
        int newActiveMinoToBaseDistance;

        // 操作中のミノの各座標を格納する変数を宣言
        int activeMino_x = Mathf.RoundToInt(activeMino.transform.position.x);
        int activeMino_y = Mathf.RoundToInt(activeMino.transform.position.y);
        int activeMino_z = Mathf.RoundToInt(activeMino.transform.position.z);

        // ゲームボードの高さのマスの数 + 2　回繰り返す
        for (newActiveMinoToBaseDistance = 0; newActiveMinoToBaseDistance < board.Height - board.Header + 2; newActiveMinoToBaseDistance++)
        {
            // 操作中のミノのY座標を newActiveMinoToBaseDistance の値だけ下に移動する
            activeMino.transform.position = new Vector3
                (activeMino_x, activeMino_y - newActiveMinoToBaseDistance, activeMino_z);

            // 操作中のミノが他のミノにぶつかる、またはゲームボードからはみ出した場合
            if (!board.CheckPosition(activeMino))
            {
                // この段階で newActiveMinoToBaseDistance から1引いた値が、操作中のミノから底までの距離となる
                newActiveMinoToBaseDistance--;

                // 操作中のミノを元の位置に戻す
                activeMino.transform.position = new Vector3
                    (activeMino_x, activeMino_y, activeMino_z);

                // breakでこのfor文を抜けて newActiveMinoToBaseDistance の値を決定
                break;
            }

            // 操作中のミノを元の位置に戻す
            activeMino.transform.position = new Vector3
                (activeMino_x, activeMino_y, activeMino_z);
        }

        return newActiveMinoToBaseDistance;
    }

    /// <summary> ゴーストミノの位置調整を行う関数 </summary>
    public void AdjustGhostMinoPosition()
    {
        SpawnerStats.Update(_activeMinoToBaseDistance: CheckActiveMinoToBaseDistance());

        // 操作中のミノの各座標を格納する変数を宣言
        int activeMinoPos_x = Mathf.RoundToInt(activeMino.transform.position.x);
        int activeMinoPos_y = Mathf.RoundToInt(activeMino.transform.position.y);
        int activeMinoPos_z = Mathf.RoundToInt(activeMino.transform.position.z);

        ghostMino.transform.rotation = activeMino.transform.rotation; // 向きの調整
        ghostMino.transform.position = new Vector3(activeMinoPos_x, activeMinoPos_y - SpawnerStats.ActiveMinoToBaseDistance, activeMinoPos_z); // 位置の調整
    }

    /// <summary> 新しい activeMino を生成する際の処理をする関数 </summary>
    /// <param name="_minoPopNumber"> ミノの生成数 </param>
    public void CreateNewActiveMino(int _minoPopNumber)
    {
        if (_minoPopNumber % 7 == 0) // 生成数が7の倍数の時
        {
            DetermineSpawnMinoOrder(); // 生成されるミノを補充
        }

        activeMino = SpawnActiveMino(minoDictionary[SpawnerStats.SpawnMinoOrders[_minoPopNumber]]);

        SpawnerStats.Update(_activeMinoName: SpawnerStats.SpawnMinoOrders[_minoPopNumber]);

        if (ghostMino) // すでにゴーストミノが存在する時
        {
            Destroy(ghostMino.gameObject); // 古いゴーストミノを削除
        }

        SpawnerStats.Update(_activeMinoToBaseDistance: CheckActiveMinoToBaseDistance());

        ghostMino = SpawnGhostMino(ghostMinoDictionary[SpawnerStats.SpawnMinoOrders[_minoPopNumber]], activeMino, SpawnerStats.ActiveMinoToBaseDistance); // ゴーストミノの生成も同時に行う
        // SpawnerStats.Update(_activeMino: SpawnGhostMino(ghostMinoDictionary[SpawnerStats.SpawnMinoOrders[_MinoPopNumber]], activeMino, activeMinoToBaseDistance));
    }

    /// <summary>  ネクストミノを生成する関数 </summary>
    /// <param name="_minoPopNumber"> ミノの生成数 </param>
    public void CreateNextMinos(int _minoPopNumber)
    {
        for (int nextMinoOrder = 0; nextMinoOrder < nextMino.Length; nextMinoOrder++) // NextMinosの数だけ繰り返す
        {
            if (_minoPopNumber == 0) // ゲームスタート時
            {
                // Nextミノを一個ずつ生成していく
                nextMino[nextMinoOrder] = SpawnNextMino(minoDictionary[SpawnerStats.SpawnMinoOrders[_minoPopNumber + nextMinoOrder + 1]], nextMinoOrder);
                // SpawnerStats.AddNextMinos(_addNextMino: SpawnNextMino(minoDictionary[SpawnerStats.SpawnMinoOrders[_MinoPopNumber + nextMinoOrder + 1]], nextMinoOrder), _number: nextMinoOrder);
            }
            else // 2回目以降
            {
                //以前のNextMinoを消去
                Destroy(nextMino[nextMinoOrder].gameObject);

                nextMino[nextMinoOrder] = SpawnNextMino(minoDictionary[SpawnerStats.SpawnMinoOrders[_minoPopNumber + nextMinoOrder + 1]], nextMinoOrder);
                // SpawnerStats.AddNextMinos(_addNextMino: SpawnNextMino(minoDictionary[SpawnerStats.SpawnMinoOrders[_MinoPopNumber + nextMinoOrder + 1]], nextMinoOrder), _number: nextMinoOrder);
            }
        }
    }

    /// <summary> ホールド機能の処理をする関 </summary>
    /// <param name="_firstHold"> ゲーム中で最初のホールドの使用判定 </param>
    /// <param name="_minoPopNumber"> ミノの生成数 </param>
    public void CreateHoldMino(bool _firstHold, int _minoPopNumber)
    {
        // 1回目のHold
        if (_firstHold == true)
        {
            Destroy(activeMino.gameObject);
            Destroy(ghostMino.gameObject);

            // holdMinoName = activeMinoName; // activeMinoの名前を保存
            SpawnerStats.Update(_holdMinoName: SpawnerStats.ActiveMinoName);

            holdMino = SpawnHoldMino(minoDictionary[SpawnerStats.HoldMinoName]); // Holdされたミノを画面左上に表示
            // SpawnerStats.Update(_holdMino: SpawnHoldMino(minoDictionary[SpawnerStats.HoldMinoName]));

            CreateNewActiveMino(_minoPopNumber);

            CreateNextMinos(_minoPopNumber);
        }

        // 2回目以降のHold
        else
        {
            Destroy(activeMino.gameObject);
            Destroy(ghostMino.gameObject);

            // activeMinoName と holdMinoName の名前を交換する
            MinoType temp;
            temp = SpawnerStats.ActiveMinoName;
            // activeMinoName = SpawnerStats.HoldMinoName;
            SpawnerStats.Update(_activeMinoName: SpawnerStats.HoldMinoName);
            // holdMinoName = temp;
            SpawnerStats.Update(_holdMinoName: temp);

            activeMino = SpawnActiveMino(holdMino); // HoldミノをactiveMinoに戻す
            // SpawnerStats.Update(_activeMino: SpawnActiveMino(SpawnerStats.HoldMino));

            SpawnerStats.Update(_activeMinoToBaseDistance: CheckActiveMinoToBaseDistance());

            ghostMino = SpawnGhostMino(ghostMinoDictionary[SpawnerStats.ActiveMinoName], activeMino, SpawnerStats.ActiveMinoToBaseDistance);
            // SpawnerStats.Update(_ghostMino: SpawnGhostMino(ghostMinoDictionary[activeMinoName], activeMino, activeMinoToBaseDistance));

            Destroy(holdMino.gameObject); // 以前のホールドミノを削除

            holdMino = SpawnHoldMino(minoDictionary[SpawnerStats.HoldMinoName]); // Holdされたミノを画面左上に表示
            // SpawnerStats.Update(_holdMino: SpawnHoldMino(minoDictionary[SpawnerStats.HoldMinoName]));

            minoMovement.ResetAngle();
            minoMovement.ResetStepsSRS();
        }
    }

    /// <summary> 新しい activeMino を生成する関数 </summary>
    /// <param name="_selectMino"> 生成するミノの種類 </param>
    /// <returns> 新しい activeMino (newActiveMino) </returns>
    public MinoMovement SpawnActiveMino(MinoMovement _selectMino)
    {
        MinoMovement newActiveMino = Instantiate(_selectMino,
        spawnMinoPosition, Quaternion.identity); // Quaternion.identityは、向きの回転に関する設定をしないことを表す

        if (newActiveMino)
        {
            return newActiveMino;
        }
        else
        {
            return null;
        }
    }

    /// <summary> 新しい ghostMino を生成する関数 </summary>
    /// <param name="_selectMino"> 生成するミノの種類 </param>
    /// <param name="_activeMino"> 操作中のミノ </param>
    /// <param name="_activeMinoToBaseDistance"> 操作中のミノから底までの距離 </param>
    /// <returns> 新しい ghostMino (newGhostMino) </returns>
    public MinoMovement SpawnGhostMino(MinoMovement _selectMino, MinoMovement _activeMino, int _activeMinoToBaseDistance)
    {
        // 操作中のミノの各座標を格納する変数を宣言
        int activeMinoInfo_x = Mathf.RoundToInt(_activeMino.transform.position.x);
        int activeMinoInfo_y = Mathf.RoundToInt(_activeMino.transform.position.y);
        int activeMinoInfo_z = Mathf.RoundToInt(_activeMino.transform.position.z);

        MinoMovement newGhostMino = Instantiate(_selectMino,
            new Vector3(activeMinoInfo_x, activeMinoInfo_y - _activeMinoToBaseDistance, activeMinoInfo_z), Quaternion.identity);

        if (newGhostMino)
        {
            return newGhostMino;
        }
        else
        {
            return null;
        }
    }

    /// <summary> 新しい nextMino を生成する関数 </summary>
    /// <param name="_selectMino"> 生成するミノの種類 </param>
    /// <param name="_nextMinoOrder"> 何番目のネクストか </param>
    /// <returns> 新しい nextMino (newNextMino) </returns>
    public MinoMovement SpawnNextMino(MinoMovement _selectMino, int _nextMinoOrder)
    {
        MinoMovement newNextMino = Instantiate(_selectMino,
            nextMinoPositions[_nextMinoOrder], Quaternion.identity);

        return newNextMino;
    }

    /// <summary> 新しい holdMino を生成する関数 </summary>
    /// <param name="_selectMino"> 生成するミノの種類 </param>
    /// <returns> 新しい holdMino (newHoldMino) </returns>
    public MinoMovement SpawnHoldMino(MinoMovement _selectMino)
    {
        MinoMovement newHoldMino = Instantiate(_selectMino,
        holdMinoPosition, Quaternion.identity);

        if (newHoldMino)
        {
            return newHoldMino;
        }
        else
        {
            return null;
        }
    }
}

/////////////////// 旧コード ///////////////////

// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Pool;

// /// <summary>
// /// Spawnerの統計情報を保持する静的クラス
// /// </summary>
// public static class SpawnerStats
// {
//     /// <summary> 生成されるミノの順番リスト </summary>
//     private static List<MinoType> spawnMinoOrders = new List<MinoType>();

//     /// <summary> 操作中のミノの名前 </summary>
//     private static MinoType activeMinoName;
//     /// <summary> ホールドミノの名前 </summary>
//     private static MinoType holdMinoName;

//     /// <summary> 操作中のミノから底までの距離 </summary>
//     /// <remarks>
//     /// ゴーストミノの生成座標の計算で必要
//     /// </remarks>
//     private static int activeMinoToBaseDistance;

//     // ゲッタープロパティ //
//     public static List<MinoType> SpawnMinoOrders => spawnMinoOrders;
//     public static MinoType ActiveMinoName => activeMinoName;
//     public static MinoType HoldMinoName => holdMinoName;
//     public static int ActiveMinoToBaseDistance => activeMinoToBaseDistance;

//     /// <summary> 指定されたフィールドの値を更新する関数 </summary>
//     /// <param name="_activeMinoName"> 操作中のミノの名前 </param>
//     /// <param name="_holdMinoName"> ホールドミノの名前 </param>
//     /// <param name="_activeMinoToBaseDistance"> 操作中のミノから底までの距離 </param>
//     /// <remarks>
//     /// 指定されていない引数は現在の値を維持
//     /// </remarks>
//     public static void Update(MinoType? _activeMinoName = null, MinoType? _holdMinoName = null, int? _activeMinoToBaseDistance = null)
//     {
//         activeMinoName = _activeMinoName ?? activeMinoName;
//         holdMinoName = _holdMinoName ?? holdMinoName;
//         activeMinoToBaseDistance = _activeMinoToBaseDistance ?? activeMinoToBaseDistance;
//         // TODO: ログの記入
//     }

//     /// <summary> デフォルトの <see cref="AttackCalculatorStats"/> にリセットする関数 </summary>
//     public static void Reset()
//     {
//         spawnMinoOrders.Clear();
//         activeMinoName = default;
//         holdMinoName = default;
//         activeMinoToBaseDistance = 0;
//     }

//     /// <summary> 生成されるミノの順番リストを追加する関数 </summary>
//     /// <param name="_addMinoType"> 追加するミノの種類 </param>
//     public static void AddSpawnMinoOrder(MinoType _addMinoType)
//     {
//         spawnMinoOrders.Add(_addMinoType);
//         // TODO: ログの記入
//     }

//     // /// <summary> ネクストリストを追加する関数 </summary>
//     // /// <param name="_addNextMino"> 追加するミノの種類 </param>
//     // ///  <param name="_number"> 何番目のネクストか </param>
//     // public static void AddNextMinos(MinoMovement _addNextMino, int _number)
//     // {
//     //     nextMinos[_number] = _addNextMino;
//     //     // TODO: ログの記入
//     // }
// }

// public class ObjectPool<T> where T : MonoBehaviour
// {
//     private readonly T prefab;
//     private readonly Queue<T> objects = new Queue<T>();

//     public ObjectPool(T prefab, int initialCount)
//     {
//         this.prefab = prefab;
//         for (int i = 0; i < initialCount; i++)
//         {
//             T newObject = Object.Instantiate(prefab);
//             newObject.gameObject.SetActive(false);
//             objects.Enqueue(newObject);
//         }
//     }

//     public T Get()
//     {
//         if (objects.Count > 0)
//         {
//             T pooledObject = objects.Dequeue();
//             pooledObject.gameObject.SetActive(true);
//             return pooledObject;
//         }
//         else
//         {
//             return Object.Instantiate(prefab);
//         }
//     }

//     public void Return(T obj)
//     {
//         obj.gameObject.SetActive(false);
//         objects.Enqueue(obj);
//     }
// }

// /// <summary>
// /// ミノの出現に関するスクリプト
// /// </summary>
// public class Spawner : MonoBehaviour
// {
//     /// <summary> 操作中のミノ </summary>
//     [SerializeField] private MinoMovement activeMino;
//     /// <summary> ゴーストミノ </summary>
//     private MinoMovement ghostMino;
//     /// <summary> ネクストミノリスト </summary>
//     private MinoMovement[] nextMino = new MinoMovement[5];
//     /// <summary> ホールドミノ </summary>
//     private MinoMovement holdMino;

//     /// <summary> ミノのPrefabs </summary>
//     /// <remarks>
//     /// 順番は(I, J, L, O, S, T, Z)
//     /// </remarks>
//     [SerializeField] private MinoMovement[] minos;
//     /// <summary> ゴーストミノのPrefabs </summary>
//     /// <remarks>
//     /// 順番は(I, J, L, O, S, T, Z)
//     /// </remarks>
//     [SerializeField] private MinoMovement[] ghostMinos;

//     // /// <summary> ミノの名前リスト </summary>
//     // private string[] MinoNames = new string[]
//     // {
//     //     "I_Mino", "J_Mino", "L_Mino", "O_Mino", "S_Mino", "T_Mino", "Z_Mino"
//     // };

//     /// <summary> MinosとMinoNamesのDictionary </summary>
//     Dictionary<MinoType, MinoMovement> minoDictionary = new Dictionary<MinoType, MinoMovement>();

//     /// <summary> GhostMinosとMinoNamesのDictionary </summary>
//     Dictionary<MinoType, MinoMovement> ghostMinoDictionary = new Dictionary<MinoType, MinoMovement>();

//     private Dictionary<MinoType, ObjectPool<MinoMovement>> minoPools = new Dictionary<MinoType, ObjectPool<MinoMovement>>();
//     private Dictionary<MinoType, ObjectPool<MinoMovement>> ghostMinoPools = new Dictionary<MinoType, ObjectPool<MinoMovement>>();

//     // /// <summary> 生成されるミノの順番リスト </summary>
//     // private List<string> spawnMinoOrders = new List<string>();

//     // /// <summary> 操作中のミノ </summary>
//     // private MinoMovement activeMino;
//     // /// <summary> ゴーストミノ </summary>
//     // private MinoMovement ghostMino;
//     // /// <summary> ネクストミノリスト </summary>
//     // private MinoMovement[] nextMinos = new MinoMovement[5];
//     // /// <summary> ホールドミノ </summary>
//     // private MinoMovement holdMino;

//     // /// <summary> 操作中のミノの名前 </summary>
//     // private string activeMinoName;
//     // /// <summary> ホールドミノの名前 </summary>
//     // private string holdMinoName;

//     // /// <summary> ActiveMinoから底までの距離 </summary>
//     // /// <remarks>
//     // /// ゴーストミノの生成座標の計算で必要
//     // /// </remarks>
//     // private int activeMinoToBaseDistance;

//     /// <summary> 操作中のミノの生成座標 </summary>
//     private Vector3 spawnActiveMinoPosition { get; } = new Vector3(4, 19, 0);

//     /// <summary> ホールドミノの生成座標 </summary>
//     private Vector3 spawnHoldMinoPosition { get; } = new Vector3(-3, 17, 0);

//     /// <summary> ネクストミノの生成座標リスト </summary>
//     private Vector3[] spawnNextMinoPositions = new Vector3[5] // NextMinos
//     {
//         new Vector3(12, 17, 0),
//         new Vector3(12, 14, 0),
//         new Vector3(12, 11, 0),
//         new Vector3(12, 8, 0),
//         new Vector3(12, 5, 0)
//     };

//     // // ゲッタープロパティ //
//     // public MinoMovement ActiveMino => activeMino;
//     // public MinoMovement GhostMino => ghostMino;
//     // public string ActiveMinoName => activeMinoName;
//     public MinoMovement ActiveMino => activeMino;
//     public MinoMovement GhostMino => ghostMino;
//     public MinoMovement[] NextMino => nextMino;
//     public MinoMovement HoldMino => holdMino;

//     // 干渉するスクリプト //
//     Board board;
//     MinoMovement minoMovement;

//     /// <summary>
//     /// インスタンス化時の処理
//     /// </summary>
//     /// <remarks>
//     /// 辞書の作成も行う
//     /// </remarks>
//     private void Awake()
//     {
//         board = FindObjectOfType<Board>();
//         minoMovement = FindObjectOfType<MinoMovement>();

//         // // Minos と MinoNames の辞書を作成
//         // for (int ii = 0; ii < minos.Length; ii++)
//         // {
//         //     minoDictionary[(MinoType)ii] = minos[ii];
//         // }
//         // // GhostMinos と MinoNames の辞書を作成
//         // for (int ii = 0; ii < ghostMinos.Length; ii++)
//         // {
//         //     ghostMinoDictionary[(MinoType)ii] = ghostMinos[ii];
//         // }

//         for (int ii = 0; ii < minos.Length; ii++)
//         {
//             minoDictionary[(MinoType)ii] = minos[ii];
//             minoPools[(MinoType)ii] = new ObjectPool<MinoMovement>(
//                 createFunc: () => Instantiate(minos[ii]),
//                 actionOnGet: (mino) => { mino.gameObject.SetActive(true); },
//                 actionOnRelease: (mino) => { mino.gameObject.SetActive(false); },
//                 actionOnDestroy: (mino) => { Destroy(mino.gameObject); },
//                 collectionCheck: false,
//                 defaultCapacity: 10,
//                 maxSize: 20
//             );
//         }

//         for (int ii = 0; ii < ghostMinos.Length; ii++)
//         {
//             ghostMinoDictionary[(MinoType)ii] = ghostMinos[ii];
//             ghostMinoPools[(MinoType)ii] = new ObjectPool<MinoMovement>(
//                 createFunc: () => Instantiate(ghostMinos[ii]),
//                 actionOnGet: (mino) => { mino.gameObject.SetActive(true); },
//                 actionOnRelease: (mino) => { mino.gameObject.SetActive(false); },
//                 actionOnDestroy: (mino) => { Destroy(mino.gameObject); },
//                 collectionCheck: false,
//                 defaultCapacity: 10,
//                 maxSize: 20
//             );
//         }
//     }

//     /// <summary> 生成されるミノの順番を決める関数 </summary>
//     public void DetermineSpawnMinoOrder()
//     {
//         /// <summary> ミノの種類が入るリスト </summary>
//         List<MinoType> minoNames = new List<MinoType>();

//         for (int ii = 0; ii < MinoType.GetValues(typeof(MinoType)).Length; ii++)
//         {
//             minoNames.Add((MinoType)ii);
//         }

//         while (minoNames.Count > 0)
//         {
//             int index = Random.Range(0, minoNames.Count);
//             MinoType randomMinoType = minoNames[index];
//             SpawnerStats.AddSpawnMinoOrder(randomMinoType);
//             minoNames.RemoveAt(index);
//         }
//     }

//     /// <summary> 操作中のミノから底までの距離を計算する関数 </summary>
//     /// <returns> 操作中のミノから底までの距離(newActiveMinoToBaseDistance) </returns>
//     private int CheckActiveMinoToBaseDistance()
//     {
//         /// <summary> 操作中のミノから底までの距離 </summary>
//         int newActiveMinoToBaseDistance;

//         // 操作中のミノの各座標を格納する変数を宣言
//         int activeMino_x = Mathf.RoundToInt(activeMino.transform.position.x);
//         int activeMino_y = Mathf.RoundToInt(activeMino.transform.position.y);
//         int activeMino_z = Mathf.RoundToInt(activeMino.transform.position.z);

//         // ゲームボードの高さのマスの数 + 2　回繰り返す
//         for (newActiveMinoToBaseDistance = 0; newActiveMinoToBaseDistance < board.Height - board.Header + 2; newActiveMinoToBaseDistance++)
//         {
//             // 操作中のミノのY座標を newActiveMinoToBaseDistance の値だけ下に移動する
//             activeMino.transform.position = new Vector3
//                 (activeMino_x, activeMino_y - newActiveMinoToBaseDistance, activeMino_z);

//             // 操作中のミノが他のミノにぶつかる、またはゲームボードからはみ出した場合
//             if (!board.CheckPosition(activeMino))
//             {
//                 // この段階で newActiveMinoToBaseDistance から1引いた値が、操作中のミノから底までの距離となる
//                 newActiveMinoToBaseDistance--;

//                 // 操作中のミノを元の位置に戻す
//                 activeMino.transform.position = new Vector3
//                     (activeMino_x, activeMino_y, activeMino_z);

//                 // breakでこのfor文を抜けて newActiveMinoToBaseDistance の値を決定
//                 break;
//             }

//             // 操作中のミノを元の位置に戻す
//             activeMino.transform.position = new Vector3
//                 (activeMino_x, activeMino_y, activeMino_z);
//         }

//         return newActiveMinoToBaseDistance;
//     }

//     /// <summary> 新しい activeMino を生成する際の処理をする関数 </summary>
//     /// <param name="_minoPopNumber"> ミノの生成数 </param>
//     public void CreateNewActiveMino(int _minoPopNumber)
//     {
//         if (_minoPopNumber % 7 == 0) // 生成数が7の倍数の時
//         {
//             DetermineSpawnMinoOrder(); // 生成されるミノを補充
//         }

//         if (activeMino != null)
//         {
//             ReturnMino(activeMino);
//         }

//         activeMino = SpawnActiveMino(minoDictionary[SpawnerStats.SpawnMinoOrders[_minoPopNumber]]);

//         SpawnerStats.Update(_activeMinoName: SpawnerStats.SpawnMinoOrders[_minoPopNumber]);

//         // if (ghostMino) // すでにゴーストミノが存在する時
//         // {
//         //     Destroy(ghostMino.gameObject); // 古いゴーストミノを削除
//         // }
//         if (ghostMino != null) // すでにゴーストミノが存在する時
//         {
//             ReturnGhostMino(ghostMino); // 古いゴーストミノをプールに返す
//         }

//         SpawnerStats.Update(_activeMinoToBaseDistance: CheckActiveMinoToBaseDistance());

//         ghostMino = SpawnGhostMino(ghostMinoDictionary[SpawnerStats.SpawnMinoOrders[_minoPopNumber]], activeMino, SpawnerStats.ActiveMinoToBaseDistance); // ゴーストミノの生成も同時に行う
//         // SpawnerStats.Update(_activeMino: SpawnGhostMino(ghostMinoDictionary[SpawnerStats.SpawnMinoOrders[_MinoPopNumber]], activeMino, activeMinoToBaseDistance));
//     }

//     /// <summary> ゴーストミノの位置調整を行う関数 </summary>
//     public void AdjustGhostMinoPosition()
//     {
//         SpawnerStats.Update(_activeMinoToBaseDistance: CheckActiveMinoToBaseDistance());

//         // 操作中のミノの各座標を格納する変数を宣言
//         int activeMinoPos_x = Mathf.RoundToInt(activeMino.transform.position.x);
//         int activeMinoPos_y = Mathf.RoundToInt(activeMino.transform.position.y);
//         int activeMinoPos_z = Mathf.RoundToInt(activeMino.transform.position.z);

//         ghostMino.transform.rotation = activeMino.transform.rotation; // 向きの調整
//         ghostMino.transform.position = new Vector3(activeMinoPos_x, activeMinoPos_y - SpawnerStats.ActiveMinoToBaseDistance, activeMinoPos_z); // 位置の調整
//     }

//     /// <summary>  ネクストミノを生成する関数 </summary>
//     /// <param name="_minoPopNumber"> ミノの生成数 </param>
//     public void CreateNextMinos(int _minoPopNumber)
//     {
//         for (int nextMinoOrder = 0; nextMinoOrder < nextMino.Length; nextMinoOrder++) // NextMinosの数だけ繰り返す
//         {
//             if (_minoPopNumber == 0) // ゲームスタート時
//             {
//                 // Nextミノを一個ずつ生成していく
//                 nextMino[nextMinoOrder] = SpawnNextMino(minoDictionary[SpawnerStats.SpawnMinoOrders[_minoPopNumber + nextMinoOrder + 1]], nextMinoOrder);
//                 // SpawnerStats.AddNextMinos(_addNextMino: SpawnNextMino(minoDictionary[SpawnerStats.SpawnMinoOrders[_MinoPopNumber + nextMinoOrder + 1]], nextMinoOrder), _number: nextMinoOrder);
//             }
//             else // 2回目以降
//             {
//                 //以前のNextMinoを消去
//                 Destroy(nextMino[nextMinoOrder].gameObject);

//                 nextMino[nextMinoOrder] = SpawnNextMino(minoDictionary[SpawnerStats.SpawnMinoOrders[_minoPopNumber + nextMinoOrder + 1]], nextMinoOrder);
//                 // SpawnerStats.AddNextMinos(_addNextMino: SpawnNextMino(minoDictionary[SpawnerStats.SpawnMinoOrders[_MinoPopNumber + nextMinoOrder + 1]], nextMinoOrder), _number: nextMinoOrder);
//             }
//         }
//     }

//     /// <summary> ホールド機能の処理をする関 </summary>
//     /// <param name="_firstHold"> ゲーム中で最初のホールドの使用判定 </param>
//     /// <param name="_minoPopNumber"> ミノの生成数 </param>
//     public void CreateHoldMino(bool _firstHold, int _minoPopNumber)
//     {
//         // 1回目のHold
//         if (_firstHold == true)
//         {
//             // Destroy(activeMino.gameObject);
//             // Destroy(ghostMino.gameObject);
//             if (activeMino != null) ReturnMino(activeMino);
//             if (ghostMino != null) ReturnGhostMino(ghostMino);

//             // holdMinoName = activeMinoName; // activeMinoの名前を保存
//             SpawnerStats.Update(_holdMinoName: SpawnerStats.ActiveMinoName);

//             holdMino = SpawnHoldMino(minoDictionary[SpawnerStats.HoldMinoName]); // Holdされたミノを画面左上に表示
//             // SpawnerStats.Update(_holdMino: SpawnHoldMino(minoDictionary[SpawnerStats.HoldMinoName]));

//             CreateNewActiveMino(_minoPopNumber);

//             CreateNextMinos(_minoPopNumber);
//         }

//         // 2回目以降のHold
//         else
//         {
//             // Destroy(activeMino.gameObject);
//             // Destroy(ghostMino.gameObject);
//             if (activeMino != null) ReturnMino(activeMino);
//             if (ghostMino != null) ReturnGhostMino(ghostMino);

//             // activeMinoName と holdMinoName の名前を交換する
//             MinoType temp;
//             temp = SpawnerStats.ActiveMinoName;
//             // activeMinoName = SpawnerStats.HoldMinoName;
//             SpawnerStats.Update(_activeMinoName: SpawnerStats.HoldMinoName);
//             // holdMinoName = temp;
//             SpawnerStats.Update(_holdMinoName: temp);

//             activeMino = SpawnActiveMino(holdMino); // HoldミノをactiveMinoに戻す
//             // SpawnerStats.Update(_activeMino: SpawnActiveMino(SpawnerStats.HoldMino));

//             SpawnerStats.Update(_activeMinoToBaseDistance: CheckActiveMinoToBaseDistance());

//             ghostMino = SpawnGhostMino(ghostMinoDictionary[SpawnerStats.ActiveMinoName], activeMino, SpawnerStats.ActiveMinoToBaseDistance);
//             // SpawnerStats.Update(_ghostMino: SpawnGhostMino(ghostMinoDictionary[activeMinoName], activeMino, activeMinoToBaseDistance));

//             // Destroy(holdMino.gameObject); // 以前のホールドミノを削除
//             if (holdMino != null) ReturnMino(holdMino);

//             holdMino = SpawnHoldMino(minoDictionary[SpawnerStats.HoldMinoName]); // Holdされたミノを画面左上に表示
//             // SpawnerStats.Update(_holdMino: SpawnHoldMino(minoDictionary[SpawnerStats.HoldMinoName]));

//             minoMovement.ResetAngle();
//             minoMovement.ResetStepsSRS();
//         }
//     }

//     private MinoType GetMinoTypeFromDictionary(MinoMovement _selectMino)
//     {
//         foreach (var kvp in minoDictionary)
//         {
//             if (kvp.Value == _selectMino)
//             {
//                 return kvp.Key;
//             }
//         }
//         throw new KeyNotFoundException("Mino type not found in dictionary");
//     }

//     /// <summary> 新しい activeMino を生成する関数 </summary>
//     /// <param name="_selectMino"> 生成するミノの種類 </param>
//     /// <returns> 新しい activeMino (newActiveMino) </returns>
//     public MinoMovement SpawnActiveMino(MinoMovement _selectMino)
//     {
//         // MinoMovement newActiveMino = Instantiate(_selectMino,
//         // spawnActiveMinoPosition, Quaternion.identity); // Quaternion.identityは、向きの回転に関する設定をしないことを表す

//         MinoType type = GetMinoTypeFromDictionary(_selectMino);
//         MinoMovement newActiveMino = minoPools[type].Get();
//         newActiveMino.transform.position = spawnActiveMinoPosition;
//         newActiveMino.transform.rotation = Quaternion.identity;
//         return newActiveMino;
//     }

//     /// <summary> 新しい ghostMino を生成する関数 </summary>
//     /// <param name="_selectMino"> 生成するミノの種類 </param>
//     /// <param name="_activeMino"> 操作中のミノ </param>
//     /// <param name="_activeMinoToBaseDistance"> 操作中のミノから底までの距離 </param>
//     /// <returns> 新しい ghostMino (newGhostMino) </returns>
//     public MinoMovement SpawnGhostMino(MinoMovement _selectMino, MinoMovement _activeMino, int _activeMinoToBaseDistance)
//     {
//         // // 操作中のミノの各座標を格納する変数を宣言
//         // int activeMinoInfo_x = Mathf.RoundToInt(_activeMino.transform.position.x);
//         // int activeMinoInfo_y = Mathf.RoundToInt(_activeMino.transform.position.y);
//         // int activeMinoInfo_z = Mathf.RoundToInt(_activeMino.transform.position.z);

//         // MinoMovement newGhostMino = Instantiate(_selectMino,
//         //     new Vector3(activeMinoInfo_x, activeMinoInfo_y - _activeMinoToBaseDistance, activeMinoInfo_z), Quaternion.identity);

//         // return newGhostMino;

//         MinoType type = GetMinoTypeFromDictionary(_selectMino);
//         MinoMovement newGhostMino = ghostMinoPools[type].Get();
//         newGhostMino.transform.position = new Vector3(_activeMino.transform.position.x, _activeMino.transform.position.y - _activeMinoToBaseDistance, _activeMino.transform.position.z);
//         newGhostMino.transform.rotation = Quaternion.identity;
//         return newGhostMino;
//     }

//     /// <summary> 新しい nextMino を生成する関数 </summary>
//     /// <param name="_selectMino"> 生成するミノの種類 </param>
//     /// <param name="_nextMinoOrder"> 何番目のネクストか </param>
//     /// <returns> 新しい nextMino (newNextMino) </returns>
//     public MinoMovement SpawnNextMino(MinoMovement _selectMino, int _nextMinoOrder)
//     {
//         // MinoMovement newNextMino = Instantiate(_selectMino,
//         //     spawnNextMinoPositions[_nextMinoOrder], Quaternion.identity);

//         // return newNextMino;

//         MinoType type = GetMinoTypeFromDictionary(_selectMino);
//         MinoMovement newNextMino = minoPools[type].Get();
//         newNextMino.transform.position = spawnNextMinoPositions[_nextMinoOrder];
//         newNextMino.transform.rotation = Quaternion.identity;
//         return newNextMino;
//     }

//     /// <summary> 新しい holdMino を生成する関数 </summary>
//     /// <param name="_selectMino"> 生成するミノの種類 </param>
//     /// <returns> 新しい holdMino (newHoldMino) </returns>
//     public MinoMovement SpawnHoldMino(MinoMovement _selectMino)
//     {
//         // MinoMovement newHoldMino = Instantiate(_selectMino,
//         // spawnHoldMinoPosition, Quaternion.identity);

//         // if (newHoldMino)
//         // {
//         //     return newHoldMino;
//         // }
//         // else
//         // {
//         //     return null;
//         // }

//         MinoType type = GetMinoTypeFromDictionary(_selectMino);
//         MinoMovement newHoldMino = minoPools[type].Get();
//         newHoldMino.transform.position = spawnHoldMinoPosition;
//         newHoldMino.transform.rotation = Quaternion.identity;
//         return newHoldMino;
//     }

//     public void ReturnMino(MinoMovement mino)
//     {
//         MinoType type = GetMinoTypeFromDictionary(mino);
//         if (minoPools.ContainsKey(type))
//         {
//             minoPools[type].Release(mino);
//         }
//     }

//     public void ReturnGhostMino(MinoMovement ghostMino)
//     {
//         MinoType type = GetMinoTypeFromDictionary(ghostMino);
//         if (ghostMinoPools.ContainsKey(type))
//         {
//             ghostMinoPools[type].Release(ghostMino);
//         }
//     }
// }


// /// <summary> ミノのPrefabs </summary>
// /// <remarks>
// /// 順番は(I, J, L, O, S, T, Z)
// /// </remarks>
// [SerializeField] private MinoMovement[] minos;
// /// <summary> ゴーストミノのPrefabs </summary>
// /// <remarks>
// /// 順番は(I, J, L, O, S, T, Z)
// /// </remarks>
// [SerializeField] private MinoMovement[] ghostMinos;

// /// <summary> 操作中のミノの生成座標 </summary>
// private Vector3 spawnActiveMinoPosition { get; } = new Vector3(4, 19, 0);

// /// <summary> ホールドミノの生成座標 </summary>
// private Vector3 spawnHoldMinoPosition { get; } = new Vector3(-3, 17, 0);

// /// <summary> ネクストミノの生成座標リスト </summary>
// private Vector3[] spawnNextMinoPositions = new Vector3[5] // NextMinos
// {
//         new Vector3(12, 17, 0),
//         new Vector3(12, 14, 0),
//         new Vector3(12, 11, 0),
//         new Vector3(12, 8, 0),
//         new Vector3(12, 5, 0)
// };

// private ObjectPool<MinoMovement> minosPool;
// private ObjectPool<MinoMovement> ghostMinosPool;

// private void Start()
// {
//     minosPool = new ObjectPool<MinoMovement>(OnCreatePooledMinos);
// }

// /// <summary> minos のゲームオブジェクトを生成する関数 </summary>
// /// <param name="_selectMino"> 生成するミノの種類 </param>
// /// <returns> 新しい activeMino (newActiveMino) </returns>
// public MinoMovement OnCreatePooledMinos(MinoMovement _selectMino)
// {
//     MinoMovement newActiveMino = Instantiate(_selectMino, spawnActiveMinoPosition, Quaternion.identity);

//     return null;
// }

// /// <summary> ghostMinos のゲームオブジェクトを生成する関数 </summary>
// /// <param name="_selectMino"> 生成するゴーストミノの種類 </param>
// /// <returns> 新しい ghostMino (newGhostMino) </returns>
// public MinoMovement OnCreatePooledGhostMinos(MinoMovement _selectMino)
// {
//     MinoMovement newGhostMino = Instantiate(_selectMino, spawnActiveMinoPosition, Quaternion.identity);
//     return newGhostMino;
// }

// /// <summary> プールからミノを取得する関数 </summary>
// /// <param name="_selectMino"> 取得するミノの種類 </param>
// /// <returns> 取得されたミノ (activeMino) </returns>
// public void GetMino(MinoMovement _selectMino)
// {
//     _selectMino.gameObject.SetActive(true);
// }

// /// <summary> ミノをプールに戻す関数 </summary>
// /// <param name="mino"> プールに戻すミノ </param>
// public void ReleaseMino(MinoMovement _selectMino)
// {
//     _selectMino.gameObject.SetActive(false);
// }

// /// <summary> プールからゴーストミノを取得する関数 </summary>
// /// <param name="_selectMino"> 取得するゴーストミノの種類 </param>
// /// <returns> 取得されたゴーストミノ (ghostMino) </returns>
// public void GetGhostMino(MinoMovement _selectMino)
// {
//     _selectMino.gameObject.SetActive(true);
// }

// /// <summary> ゴーストミノをプールに戻す関数 </summary>
// /// <param name="ghostMino"> プールに戻すゴーストミノ </param>
// public void ReleaseGhostMino(MinoMovement _selectMino)
// {
//     _selectMino.gameObject.SetActive(false);
// }

// // public void OnDestroyPooledObjet()
// // {

// // }

/////////////////////////////////////////////////////////