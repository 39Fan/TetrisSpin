using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawnerの統計情報を保持する静的クラス
/// </summary>
public static class SpawnerStats
{
    /// <summary> 生成されるミノの順番リスト </summary>
    private static List<eMinoType> spawnMinoOrders = new List<eMinoType>();

    /// <summary> 操作中のミノの名前 </summary>
    private static eMinoType activeMinoName = default;
    /// <summary> ホールドミノの名前 </summary>
    private static eMinoType holdMinoName = default;

    /// <summary> 操作中のミノから底までの距離 </summary>
    /// <remarks>
    /// ゴーストミノの生成座標の計算で必要
    /// </remarks>
    private static int activeMinoToBaseDistance = 20;

    // ゲッタープロパティ //
    public static List<eMinoType> SpawnMinoOrders => spawnMinoOrders;
    public static eMinoType ActiveMinoName => activeMinoName;
    public static eMinoType HoldMinoName => holdMinoName;
    public static int ActiveMinoToBaseDistance => activeMinoToBaseDistance;

    /// <summary> スタッツログの詳細 </summary>
    private static string logStatsDetail;

    /// <summary> 指定されたフィールドの値を更新する関数 </summary>
    /// <param name="_activeMinoName"> 操作中のミノの名前 </param>
    /// <param name="_holdMinoName"> ホールドミノの名前 </param>
    /// <param name="_activeMinoToBaseDistance"> 操作中のミノから底までの距離 </param>
    /// <remarks>
    /// 指定されていない引数は現在の値を維持
    /// </remarks>
    public static void UpdateStats(eMinoType? _activeMinoName = null, eMinoType? _holdMinoName = null, int? _activeMinoToBaseDistance = null)
    {
        LogHelper.DebugLog(eClasses.SpawnerStats, eMethod.UpdateStats, eLogTitle.Start);

        activeMinoName = _activeMinoName ?? activeMinoName;
        holdMinoName = _holdMinoName ?? holdMinoName;
        activeMinoToBaseDistance = _activeMinoToBaseDistance ?? activeMinoToBaseDistance;

        logStatsDetail = $"activeMinoName : {activeMinoName}, holdMinoName : {holdMinoName}, activeMinoToBaseDistance : {activeMinoToBaseDistance}";
        LogHelper.InfoLog(eClasses.SpawnerStats, eMethod.UpdateStats, eLogTitle.StatsInfo, logStatsDetail);

        LogHelper.DebugLog(eClasses.SpawnerStats, eMethod.UpdateStats, eLogTitle.End);
    }

    /// <summary> デフォルトの <see cref="AttackCalculatorStats"/> にリセットする関数 </summary>
    public static void ResetStats()
    {
        LogHelper.DebugLog(eClasses.SpawnerStats, eMethod.ResetStats, eLogTitle.Start);

        spawnMinoOrders.Clear();
        activeMinoName = default;
        holdMinoName = default;
        activeMinoToBaseDistance = 20;

        LogHelper.DebugLog(eClasses.SpawnerStats, eMethod.ResetStats, eLogTitle.End);
    }

    /// <summary> 生成されるミノの順番リストを追加する関数 </summary>
    /// <param name="_addeMinoType"> 追加するミノの種類 </param>
    public static void AddSpawnMinoOrder(eMinoType _addeMinoType)
    {
        LogHelper.DebugLog(eClasses.SpawnerStats, eMethod.AddSpawnMinoOrder, eLogTitle.Start);

        spawnMinoOrders.Add(_addeMinoType);

        LogHelper.DebugLog(eClasses.SpawnerStats, eMethod.AddSpawnMinoOrder, eLogTitle.End);
    }
}

/// <summary>
/// Spawnerのミノ表示に関する座標情報を保持する静的クラス
/// </summary>
internal static class SpawnerSettings
{
    /// <summary> 操作中のミノの生成座標 </summary>
    public static readonly Vector3 SPAWN_MINO_POSITION = new Vector3(4, 19, 0);

    /// <summary> ホールドミノの生成座標 </summary>
    public static readonly Vector3 HOLD_MINO_POSITION = new Vector3(-2.95f, 17.7f, 0);

    /// <summary> ネクストミノの生成座標リスト </summary>
    public static readonly Vector3[] NEXT_MINO_POSITIONS = new Vector3[5] // NextMinos
    {
    new Vector3(11.85f, 17.7f, 0),
    new Vector3(11.95f, 14, 0),
    new Vector3(11.95f, 10.9f, 0),
    new Vector3(11.95f, 7.8f, 0),
    new Vector3(11.95f, 4.7f, 0)
    };

    /// <summary> ネクスト0ミノのスケール </summary>
    public static readonly Vector3 NEXT0_MINO_SCALE = new Vector3(0.8f, 0.8f, 0.8f);
    /// <summary> ネクストミノのスケール </summary>
    public static readonly Vector3 NEXT_MINO_SCALE = new Vector3(0.6f, 0.6f, 0.6f);
    /// <summary> ホールドミノのスケール </summary>
    public static readonly Vector3 HOLD_MINO_SCALE = new Vector3(0.8f, 0.8f, 0.8f);
    /// <summary> ミノのデフォルトスケール </summary>
    public static readonly Vector3 MINO_DEFAULT_SCALE = new Vector3(1, 1, 1);

    /// <summary> 各ミノの中心座標を保持する辞書 </summary>
    public static readonly Dictionary<eMinoType, Vector3> MINO_CENTER_POSITIONS_DICTIONARY = new Dictionary<eMinoType, Vector3>
    {
        { eMinoType.IMino, new Vector3(0.3125f, 0, 0) },
        { eMinoType.JMino, new Vector3(0, 0.375f, 0) },
        { eMinoType.LMino, new Vector3(0, 0.375f, 0) },
        { eMinoType.OMino, new Vector3(0.3125f, 0.3125f, 0) },
        { eMinoType.SMino, new Vector3(0, 0.25f, 0) },
        { eMinoType.TMino, new Vector3(0, 0.25f, 0) },
        { eMinoType.ZMino, new Vector3(0, 0.25f, 0) }
    };
}

/// <summary>
/// ミノの出現に関するスクリプト
/// </summary>
public class Spawner : MonoBehaviour
{
    /// <summary> 操作中のミノ </summary>
    private MinoMovement activeMino;
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

    /// <summary> MinosとMinoNamesのDictionary </summary>
    Dictionary<eMinoType, MinoMovement> minoDictionary = new Dictionary<eMinoType, MinoMovement>();

    /// <summary> GhostMinosとMinoNamesのDictionary </summary>
    Dictionary<eMinoType, MinoMovement> ghostMinoDictionary = new Dictionary<eMinoType, MinoMovement>();

    // /// <summary> 操作中のミノの生成座標 </summary>
    // private Vector3 spawnMinoPosition { get; } = new Vector3(4, 20, 0);

    // /// <summary> ホールドミノの生成座標 </summary>
    // private Vector3 holdMinoPosition { get; } = new Vector3(-2.9f, 17.7f, 0);

    // /// <summary> ネクストミノの生成座標リスト </summary>
    // private Vector3[] nextMinoPositions = new Vector3[5] // NextMinos
    // {
    //     new Vector3(12, 17.7f, 0),
    //     new Vector3(11.95f, 14.2f, 0),
    //     new Vector3(11.95f, 11, 0),
    //     new Vector3(11.95f, 7.8f, 0),
    //     new Vector3(11.95f, 4.6f, 0)
    // };

    // // ゲッタープロパティ //
    public MinoMovement ActiveMino => activeMino;
    public MinoMovement GhostMino => ghostMino;
    public MinoMovement[] NextMino => nextMino;
    public MinoMovement HoldMino => holdMino;

    /// <summary> ログの詳細 </summary>
    private string logDetail;

    // 干渉するスクリプト //
    Board board;
    MinoMovement minoMovement;

    /// <summary>
    /// インスタンス化
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
            minoDictionary[(eMinoType)ii] = minos[ii];
        }
        // GhostMinos と MinoNames の辞書を作成
        for (int ii = 0; ii < ghostMinos.Length; ii++)
        {
            ghostMinoDictionary[(eMinoType)ii] = ghostMinos[ii];
        }
    }

    /// <summary> 生成されるミノの順番を決める関数 </summary>
    public void DetermineSpawnMinoOrder()
    {
        LogHelper.DebugLog(eClasses.Spawner, eMethod.DetermineSpawnMinoOrder, eLogTitle.Start);

        /// <summary> ミノの種類が入るリスト </summary>
        List<eMinoType> minoNames = new List<eMinoType>();

        for (int ii = 0; ii < eMinoType.GetValues(typeof(eMinoType)).Length; ii++)
        {
            minoNames.Add((eMinoType)ii);
        }

        while (minoNames.Count > 0)
        {
            int index = Random.Range(0, minoNames.Count);
            eMinoType randomeMinoType = minoNames[index];
            SpawnerStats.AddSpawnMinoOrder(randomeMinoType);
            minoNames.RemoveAt(index);
        }

        LogHelper.DebugLog(eClasses.Spawner, eMethod.DetermineSpawnMinoOrder, eLogTitle.End);
    }

    /// <summary> 操作中のミノから底までの距離を計算する関数 </summary>
    /// <returns> 操作中のミノから底までの距離(newActiveMinoToBaseDistance) </returns>
    private int CheckActiveMinoToBaseDistance()
    {
        LogHelper.DebugLog(eClasses.Spawner, eMethod.CheckActiveMinoToBaseDistance, eLogTitle.Start);

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

        LogHelper.DebugLog(eClasses.Spawner, eMethod.CheckActiveMinoToBaseDistance, eLogTitle.End);
        return newActiveMinoToBaseDistance;
    }

    /// <summary> ゴーストミノの位置調整を行う関数 </summary>
    public void AdjustGhostMinoPosition()
    {
        LogHelper.DebugLog(eClasses.Spawner, eMethod.AdjustGhostMinoPosition, eLogTitle.Start);

        SpawnerStats.UpdateStats(_activeMinoToBaseDistance: CheckActiveMinoToBaseDistance());

        // 操作中のミノの各座標を格納する変数を宣言
        int activeMinoPos_x = Mathf.RoundToInt(activeMino.transform.position.x);
        int activeMinoPos_y = Mathf.RoundToInt(activeMino.transform.position.y);
        int activeMinoPos_z = Mathf.RoundToInt(activeMino.transform.position.z);

        ghostMino.transform.rotation = activeMino.transform.rotation; // 向きの調整
        ghostMino.transform.position = new Vector3(activeMinoPos_x, activeMinoPos_y - SpawnerStats.ActiveMinoToBaseDistance, activeMinoPos_z); // 位置の調整

        LogHelper.DebugLog(eClasses.Spawner, eMethod.AdjustGhostMinoPosition, eLogTitle.End);
    }

    /// <summary> 新しい activeMino を生成する際の処理をする関数 </summary>
    /// <param name="_minoPopNumber"> ミノの生成数 </param>
    public void CreateNewActiveMino(int _minoPopNumber)
    {
        LogHelper.DebugLog(eClasses.Spawner, eMethod.CreateNewActiveMino, eLogTitle.Start);

        if (_minoPopNumber % 7 == 0) // 生成数が7の倍数の時
        {
            DetermineSpawnMinoOrder(); // 生成されるミノを補充
        }

        activeMino = SpawnActiveMino(minoDictionary[SpawnerStats.SpawnMinoOrders[_minoPopNumber]]);

        SpawnerStats.UpdateStats(_activeMinoName: SpawnerStats.SpawnMinoOrders[_minoPopNumber]);

        if (ghostMino) // すでにゴーストミノが存在する時
        {
            Destroy(ghostMino.gameObject); // 古いゴーストミノを削除
        }

        SpawnerStats.UpdateStats(_activeMinoToBaseDistance: CheckActiveMinoToBaseDistance());

        ghostMino = SpawnGhostMino(ghostMinoDictionary[SpawnerStats.SpawnMinoOrders[_minoPopNumber]], activeMino, SpawnerStats.ActiveMinoToBaseDistance); // ゴーストミノの生成も同時に行う

        LogHelper.DebugLog(eClasses.Spawner, eMethod.CreateNewActiveMino, eLogTitle.End);
    }

    /// <summary>  ネクストミノを生成する関数 </summary>
    /// <param name="_minoPopNumber"> ミノの生成数 </param>
    public void CreateNextMinos(int _minoPopNumber)
    {
        LogHelper.DebugLog(eClasses.Spawner, eMethod.CreateNextMinos, eLogTitle.Start);

        for (int nextMinoOrder = 0; nextMinoOrder < nextMino.Length; nextMinoOrder++) // NextMinosの数だけ繰り返す
        {
            if (_minoPopNumber == 0) // ゲームスタート時
            {
                // Nextミノを一個ずつ生成していく
                nextMino[nextMinoOrder] = SpawnNextMino(minoDictionary[SpawnerStats.SpawnMinoOrders[_minoPopNumber + nextMinoOrder + 1]], SpawnerStats.SpawnMinoOrders[_minoPopNumber + nextMinoOrder + 1], nextMinoOrder);
                // SpawnerStats.AddNextMinos(_addNextMino: SpawnNextMino(minoDictionary[SpawnerStats.SpawnMinoOrders[_MinoPopNumber + nextMinoOrder + 1]], nextMinoOrder), _number: nextMinoOrder);
            }
            else // 2回目以降
            {
                //以前のNextMinoを消去
                Destroy(nextMino[nextMinoOrder].gameObject);

                nextMino[nextMinoOrder] = SpawnNextMino(minoDictionary[SpawnerStats.SpawnMinoOrders[_minoPopNumber + nextMinoOrder + 1]], SpawnerStats.SpawnMinoOrders[_minoPopNumber + nextMinoOrder + 1], nextMinoOrder);
                // SpawnerStats.AddNextMinos(_addNextMino: SpawnNextMino(minoDictionary[SpawnerStats.SpawnMinoOrders[_MinoPopNumber + nextMinoOrder + 1]], nextMinoOrder), _number: nextMinoOrder);
            }
        }

        LogHelper.DebugLog(eClasses.Spawner, eMethod.CreateNextMinos, eLogTitle.End);
    }

    /// <summary> ホールド機能の処理をする関 </summary>
    /// <param name="_firstHold"> ゲーム中で最初のホールドの使用判定 </param>
    /// <param name="_minoPopNumber"> ミノの生成数 </param>
    public void CreateHoldMino(bool _firstHold, int _minoPopNumber)
    {
        LogHelper.DebugLog(eClasses.Spawner, eMethod.CreateHoldMino, eLogTitle.Start);

        // 1回目のHold
        if (_firstHold == true)
        {
            Destroy(activeMino.gameObject);
            Destroy(ghostMino.gameObject);

            // holdMinoName = activeMinoName; // activeMinoの名前を保存
            SpawnerStats.UpdateStats(_holdMinoName: SpawnerStats.ActiveMinoName);

            holdMino = SpawnHoldMino(minoDictionary[SpawnerStats.HoldMinoName], SpawnerStats.HoldMinoName); // Holdされたミノを画面左上に表示
            // SpawnerStats.UpdateStats(_holdMino: SpawnHoldMino(minoDictionary[SpawnerStats.HoldMinoName]));

            CreateNewActiveMino(_minoPopNumber);

            CreateNextMinos(_minoPopNumber);
        }

        // 2回目以降のHold
        else
        {
            Destroy(activeMino.gameObject);
            Destroy(ghostMino.gameObject);

            // activeMinoName と holdMinoName の名前を交換する
            eMinoType temp;
            temp = SpawnerStats.ActiveMinoName;
            SpawnerStats.UpdateStats(_activeMinoName: SpawnerStats.HoldMinoName);
            SpawnerStats.UpdateStats(_holdMinoName: temp);

            activeMino = SpawnActiveMino(holdMino); // HoldミノをactiveMinoに戻す
            activeMino.transform.localScale = SpawnerSettings.MINO_DEFAULT_SCALE;

            SpawnerStats.UpdateStats(_activeMinoToBaseDistance: CheckActiveMinoToBaseDistance());

            ghostMino = SpawnGhostMino(ghostMinoDictionary[SpawnerStats.ActiveMinoName], activeMino, SpawnerStats.ActiveMinoToBaseDistance);

            Destroy(holdMino.gameObject); // 以前のホールドミノを削除

            holdMino = SpawnHoldMino(minoDictionary[SpawnerStats.HoldMinoName], SpawnerStats.HoldMinoName); // Holdされたミノを画面左上に表示
            holdMino.transform.localScale = SpawnerSettings.HOLD_MINO_SCALE;

            minoMovement.ResetAngle();
            minoMovement.ResetStepsSRS();
        }

        LogHelper.DebugLog(eClasses.Spawner, eMethod.CreateHoldMino, eLogTitle.End);
    }

    /// <summary> 新しい activeMino を生成する関数 </summary>
    /// <param name="_selectMino"> 生成するミノの種類 </param>
    /// <returns> 新しい activeMino (newActiveMino) </returns>
    public MinoMovement SpawnActiveMino(MinoMovement _selectMino)
    {
        LogHelper.DebugLog(eClasses.Spawner, eMethod.SpawnActiveMino, eLogTitle.Start);

        MinoMovement newActiveMino = Instantiate(_selectMino,
        SpawnerSettings.SPAWN_MINO_POSITION, Quaternion.identity); // Quaternion.identityは、向きの回転に関する設定をしないことを表す
        newActiveMino.transform.localScale = SpawnerSettings.MINO_DEFAULT_SCALE;

        if (newActiveMino)
        {
            LogHelper.DebugLog(eClasses.Spawner, eMethod.SpawnActiveMino, eLogTitle.End);
            return newActiveMino;
        }
        else
        {
            LogHelper.DebugLog(eClasses.Spawner, eMethod.SpawnActiveMino, eLogTitle.End);
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
        LogHelper.DebugLog(eClasses.Spawner, eMethod.SpawnGhostMino, eLogTitle.Start);

        // 操作中のミノの各座標を格納する変数を宣言
        int activeMinoInfo_x = Mathf.RoundToInt(_activeMino.transform.position.x);
        int activeMinoInfo_y = Mathf.RoundToInt(_activeMino.transform.position.y);
        int activeMinoInfo_z = Mathf.RoundToInt(_activeMino.transform.position.z);

        MinoMovement newGhostMino = Instantiate(_selectMino,
            new Vector3(activeMinoInfo_x, activeMinoInfo_y - _activeMinoToBaseDistance, activeMinoInfo_z), Quaternion.identity);

        if (newGhostMino)
        {
            LogHelper.DebugLog(eClasses.Spawner, eMethod.SpawnGhostMino, eLogTitle.End);
            return newGhostMino;
        }
        else
        {
            LogHelper.DebugLog(eClasses.Spawner, eMethod.SpawnGhostMino, eLogTitle.End);
            return null;
        }
    }

    /// <summary> 新しい nextMino を生成する関数 </summary>
    /// <param name="_selectMino"> 生成するミノの種類 </param>
    /// <param name="_nextMinoOrder"> 何番目のネクストか </param>
    /// <returns> 新しい nextMino (newNextMino) </returns>
    public MinoMovement SpawnNextMino(MinoMovement _selectMino, eMinoType _selecyMinoType, int _nextMinoOrder)
    {
        LogHelper.DebugLog(eClasses.Spawner, eMethod.SpawnNextMino, eLogTitle.Start);

        MinoMovement newNextMino = Instantiate(_selectMino,
            SpawnerSettings.NEXT_MINO_POSITIONS[_nextMinoOrder] - SpawnerSettings.MINO_CENTER_POSITIONS_DICTIONARY[_selecyMinoType], Quaternion.identity);

        if (_nextMinoOrder == 0)
        {
            newNextMino.transform.localScale = SpawnerSettings.NEXT0_MINO_SCALE;
        }
        else
        {
            newNextMino.transform.localScale = SpawnerSettings.NEXT_MINO_SCALE;
        }

        LogHelper.DebugLog(eClasses.Spawner, eMethod.SpawnNextMino, eLogTitle.End);
        return newNextMino;
    }

    /// <summary> 新しい holdMino を生成する関数 </summary>
    /// <param name="_selectMino"> 生成するミノの種類 </param>
    /// <returns> 新しい holdMino (newHoldMino) </returns>
    public MinoMovement SpawnHoldMino(MinoMovement _selectMino, eMinoType _selecyMinoType)
    {
        LogHelper.DebugLog(eClasses.Spawner, eMethod.SpawnHoldMino, eLogTitle.Start);

        MinoMovement newHoldMino = Instantiate(_selectMino,
            SpawnerSettings.HOLD_MINO_POSITION - SpawnerSettings.MINO_CENTER_POSITIONS_DICTIONARY[_selecyMinoType], Quaternion.identity);
        newHoldMino.transform.localScale = SpawnerSettings.HOLD_MINO_SCALE;

        if (newHoldMino)
        {
            LogHelper.DebugLog(eClasses.Spawner, eMethod.SpawnHoldMino, eLogTitle.End);
            return newHoldMino;
        }
        else
        {
            LogHelper.DebugLog(eClasses.Spawner, eMethod.SpawnHoldMino, eLogTitle.End);
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
//     private static List<eMinoType> spawnMinoOrders = new List<eMinoType>();

//     /// <summary> 操作中のミノの名前 </summary>
//     private static eMinoType activeMinoName;
//     /// <summary> ホールドミノの名前 </summary>
//     private static eMinoType holdMinoName;

//     /// <summary> 操作中のミノから底までの距離 </summary>
//     /// <remarks>
//     /// ゴーストミノの生成座標の計算で必要
//     /// </remarks>
//     private static int activeMinoToBaseDistance;

//     // ゲッタープロパティ //
//     public static List<eMinoType> SpawnMinoOrders => spawnMinoOrders;
//     public static eMinoType ActiveMinoName => activeMinoName;
//     public static eMinoType HoldMinoName => holdMinoName;
//     public static int ActiveMinoToBaseDistance => activeMinoToBaseDistance;

//     /// <summary> 指定されたフィールドの値を更新する関数 </summary>
//     /// <param name="_activeMinoName"> 操作中のミノの名前 </param>
//     /// <param name="_holdMinoName"> ホールドミノの名前 </param>
//     /// <param name="_activeMinoToBaseDistance"> 操作中のミノから底までの距離 </param>
//     /// <remarks>
//     /// 指定されていない引数は現在の値を維持
//     /// </remarks>
//     public static void Update(eMinoType? _activeMinoName = null, eMinoType? _holdMinoName = null, int? _activeMinoToBaseDistance = null)
//     {
//         activeMinoName = _activeMinoName ?? activeMinoName;
//         holdMinoName = _holdMinoName ?? holdMinoName;
//         activeMinoToBaseDistance = _activeMinoToBaseDistance ?? activeMinoToBaseDistance;
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
//     /// <param name="_addeMinoType"> 追加するミノの種類 </param>
//     public static void AddSpawnMinoOrder(eMinoType _addeMinoType)
//     {
//         spawnMinoOrders.Add(_addeMinoType);
//     }

//     // /// <summary> ネクストリストを追加する関数 </summary>
//     // /// <param name="_addNextMino"> 追加するミノの種類 </param>
//     // ///  <param name="_number"> 何番目のネクストか </param>
//     // public static void AddNextMinos(MinoMovement _addNextMino, int _number)
//     // {
//     //     nextMinos[_number] = _addNextMino;
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
//     Dictionary<eMinoType, MinoMovement> minoDictionary = new Dictionary<eMinoType, MinoMovement>();

//     /// <summary> GhostMinosとMinoNamesのDictionary </summary>
//     Dictionary<eMinoType, MinoMovement> ghostMinoDictionary = new Dictionary<eMinoType, MinoMovement>();

//     private Dictionary<eMinoType, ObjectPool<MinoMovement>> minoPools = new Dictionary<eMinoType, ObjectPool<MinoMovement>>();
//     private Dictionary<eMinoType, ObjectPool<MinoMovement>> ghostMinoPools = new Dictionary<eMinoType, ObjectPool<MinoMovement>>();

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
//         //     minoDictionary[(eMinoType)ii] = minos[ii];
//         // }
//         // // GhostMinos と MinoNames の辞書を作成
//         // for (int ii = 0; ii < ghostMinos.Length; ii++)
//         // {
//         //     ghostMinoDictionary[(eMinoType)ii] = ghostMinos[ii];
//         // }

//         for (int ii = 0; ii < minos.Length; ii++)
//         {
//             minoDictionary[(eMinoType)ii] = minos[ii];
//             minoPools[(eMinoType)ii] = new ObjectPool<MinoMovement>(
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
//             ghostMinoDictionary[(eMinoType)ii] = ghostMinos[ii];
//             ghostMinoPools[(eMinoType)ii] = new ObjectPool<MinoMovement>(
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
//         List<eMinoType> minoNames = new List<eMinoType>();

//         for (int ii = 0; ii < eMinoType.GetValues(typeof(eMinoType)).Length; ii++)
//         {
//             minoNames.Add((eMinoType)ii);
//         }

//         while (minoNames.Count > 0)
//         {
//             int index = Random.Range(0, minoNames.Count);
//             eMinoType randomeMinoType = minoNames[index];
//             SpawnerStats.AddSpawnMinoOrder(randomeMinoType);
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

//         SpawnerStats.UpdateStats(_activeMinoName: SpawnerStats.SpawnMinoOrders[_minoPopNumber]);

//         // if (ghostMino) // すでにゴーストミノが存在する時
//         // {
//         //     Destroy(ghostMino.gameObject); // 古いゴーストミノを削除
//         // }
//         if (ghostMino != null) // すでにゴーストミノが存在する時
//         {
//             ReturnGhostMino(ghostMino); // 古いゴーストミノをプールに返す
//         }

//         SpawnerStats.UpdateStats(_activeMinoToBaseDistance: CheckActiveMinoToBaseDistance());

//         ghostMino = SpawnGhostMino(ghostMinoDictionary[SpawnerStats.SpawnMinoOrders[_minoPopNumber]], activeMino, SpawnerStats.ActiveMinoToBaseDistance); // ゴーストミノの生成も同時に行う
//         // SpawnerStats.UpdateStats(_activeMino: SpawnGhostMino(ghostMinoDictionary[SpawnerStats.SpawnMinoOrders[_MinoPopNumber]], activeMino, activeMinoToBaseDistance));
//     }

//     /// <summary> ゴーストミノの位置調整を行う関数 </summary>
//     public void AdjustGhostMinoPosition()
//     {
//         SpawnerStats.UpdateStats(_activeMinoToBaseDistance: CheckActiveMinoToBaseDistance());

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
//             SpawnerStats.UpdateStats(_holdMinoName: SpawnerStats.ActiveMinoName);

//             holdMino = SpawnHoldMino(minoDictionary[SpawnerStats.HoldMinoName]); // Holdされたミノを画面左上に表示
//             // SpawnerStats.UpdateStats(_holdMino: SpawnHoldMino(minoDictionary[SpawnerStats.HoldMinoName]));

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
//             eMinoType temp;
//             temp = SpawnerStats.ActiveMinoName;
//             // activeMinoName = SpawnerStats.HoldMinoName;
//             SpawnerStats.UpdateStats(_activeMinoName: SpawnerStats.HoldMinoName);
//             // holdMinoName = temp;
//             SpawnerStats.UpdateStats(_holdMinoName: temp);

//             activeMino = SpawnActiveMino(holdMino); // HoldミノをactiveMinoに戻す
//             // SpawnerStats.UpdateStats(_activeMino: SpawnActiveMino(SpawnerStats.HoldMino));

//             SpawnerStats.UpdateStats(_activeMinoToBaseDistance: CheckActiveMinoToBaseDistance());

//             ghostMino = SpawnGhostMino(ghostMinoDictionary[SpawnerStats.ActiveMinoName], activeMino, SpawnerStats.ActiveMinoToBaseDistance);
//             // SpawnerStats.UpdateStats(_ghostMino: SpawnGhostMino(ghostMinoDictionary[activeMinoName], activeMino, activeMinoToBaseDistance));

//             // Destroy(holdMino.gameObject); // 以前のホールドミノを削除
//             if (holdMino != null) ReturnMino(holdMino);

//             holdMino = SpawnHoldMino(minoDictionary[SpawnerStats.HoldMinoName]); // Holdされたミノを画面左上に表示
//             // SpawnerStats.UpdateStats(_holdMino: SpawnHoldMino(minoDictionary[SpawnerStats.HoldMinoName]));

//             minoMovement.ResetAngle();
//             minoMovement.ResetStepsSRS();
//         }
//     }

//     private eMinoType GeteMinoTypeFromDictionary(MinoMovement _selectMino)
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

//         eMinoType type = GeteMinoTypeFromDictionary(_selectMino);
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

//         eMinoType type = GeteMinoTypeFromDictionary(_selectMino);
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

//         eMinoType type = GeteMinoTypeFromDictionary(_selectMino);
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

//         eMinoType type = GeteMinoTypeFromDictionary(_selectMino);
//         MinoMovement newHoldMino = minoPools[type].Get();
//         newHoldMino.transform.position = spawnHoldMinoPosition;
//         newHoldMino.transform.rotation = Quaternion.identity;
//         return newHoldMino;
//     }

//     public void ReturnMino(MinoMovement mino)
//     {
//         eMinoType type = GeteMinoTypeFromDictionary(mino);
//         if (minoPools.ContainsKey(type))
//         {
//             minoPools[type].Release(mino);
//         }
//     }

//     public void ReturnGhostMino(MinoMovement ghostMino)
//     {
//         eMinoType type = GeteMinoTypeFromDictionary(ghostMino);
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

// /// <summary> ミノの名前リスト </summary>
// private string[] MinoNames = new string[]
// {
//     "I_Mino", "J_Mino", "L_Mino", "O_Mino", "S_Mino", "T_Mino", "Z_Mino"
// };

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

/////////////////////////////////////////////////////////