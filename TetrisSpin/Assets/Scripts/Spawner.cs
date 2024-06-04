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

    /// <summary> ActiveMinoから底までの距離 </summary>
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
    /// <param name="_activeMinoToBaseDistance"> ActiveMinoから底までの距離 </param>
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

    // /// <summary> ネクストリストを追加する関数 </summary>
    // /// <param name="_addNextMino"> 追加するミノの種類 </param>
    // ///  <param name="_number"> 何番目のネクストか </param>
    // public static void AddNextMinos(MinoMovement _addNextMino, int _number)
    // {
    //     nextMinos[_number] = _addNextMino;
    //     // TODO: ログの記入
    // }
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
    private MinoMovement[] nextMinos = new MinoMovement[5];
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
    public MinoMovement[] NextMinos => nextMinos;
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

    // 生成されるミノの順番を決める関数 //
    public void DetermineSpawnMinoOrder()
    {
        List<MinoType> minoNames = new List<MinoType>(); // ミノの名前が入るリスト

        // minoNamesにミノの名前を全て入れる
        for (int ii = 0; ii < MinoType.GetValues(typeof(MinoType)).Length; ii++)
        {
            minoNames.Add((MinoType)ii);
        }

        while (minoNames.Count > 0) // minoNames の配列がなくなるまで繰り返す
        {
            // 0から minoNames の配列数までの範囲でランダムな数値を取得し index に格納
            int index = Random.Range(0, minoNames.Count);

            // minoNames[index] の名前を randomName に格納
            MinoType randomName = minoNames[index];

            // randomNameを SpawnMinoOrders に追加
            SpawnerStats.AddSpawnMinoOrder(randomName);

            // インデックス位置の要素を削除
            minoNames.RemoveAt(index);
        }
    }

    // ActiveMino から底までの距離を計算する関数 //
    private void CheckActiveMinoToBaseDistance()
    {
        /// <summary> ActiveMinoから底までの距離 </summary>
        int activeMinoToBaseDistance;

        // ActiveMino の各座標を格納する変数を宣言
        int activeMino_x = Mathf.RoundToInt(activeMino.transform.position.x);
        int activeMino_y = Mathf.RoundToInt(activeMino.transform.position.y);
        int activeMino_z = Mathf.RoundToInt(activeMino.transform.position.z);

        // ゲームボードの高さのマスの数 + 2　回繰り返す
        for (activeMinoToBaseDistance = 0; activeMinoToBaseDistance < board.Height - board.Header + 2; activeMinoToBaseDistance++)
        {
            // ActiveMino のY座標を ActiveMinoToBaseDistance の値だけ下に移動する
            activeMino.transform.position = new Vector3
                (activeMino_x, activeMino_y - activeMinoToBaseDistance, activeMino_z);

            // ActiveMino が他のミノにぶつかる、またはゲームボードからはみ出した時
            if (!board.CheckPosition(activeMino))
            {
                // この段階で ActiveMinoToBaseDistance から1引いた値が ActiveMino から底までの距離となる
                activeMinoToBaseDistance--;

                // ActiveMinoの位置を元に戻す
                activeMino.transform.position = new Vector3
                    (activeMino_x, activeMino_y, activeMino_z);

                // breakでこのfor文を抜けて ActiveMinoToBaseDistance の値を決定
                break;
            }

            SpawnerStats.Update(_activeMinoToBaseDistance: activeMinoToBaseDistance);

            // ActiveMinoを元の位置に戻す
            activeMino.transform.position = new Vector3
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

        activeMino = SpawnActiveMino(minoDictionary[SpawnerStats.SpawnMinoOrders[_MinoPopNumber]]);

        SpawnerStats.Update(_activeMinoName: SpawnerStats.SpawnMinoOrders[_MinoPopNumber]);

        if (ghostMino) // すでにゴーストミノが存在する時
        {
            Destroy(ghostMino.gameObject); // 古いゴーストミノを削除
        }

        CheckActiveMinoToBaseDistance(); // ActiveMinoToBaseDistance の計算

        ghostMino = SpawnGhostMino(ghostMinoDictionary[SpawnerStats.SpawnMinoOrders[_MinoPopNumber]], activeMino, SpawnerStats.ActiveMinoToBaseDistance); // ゴーストミノの生成も同時に行う
        // SpawnerStats.Update(_activeMino: SpawnGhostMino(ghostMinoDictionary[SpawnerStats.SpawnMinoOrders[_MinoPopNumber]], activeMino, activeMinoToBaseDistance));
    }

    // ゴーストミノの位置調整を行う関数
    public void AdjustGhostMinoPosition()
    {
        CheckActiveMinoToBaseDistance(); // ActiveMinoToBaseDistance の計算

        // ActiveMino の情報を取得
        int activeMinoPos_x = Mathf.RoundToInt(activeMino.transform.position.x);
        int activeMinoPos_y = Mathf.RoundToInt(activeMino.transform.position.y);
        int activeMinoPos_z = Mathf.RoundToInt(activeMino.transform.position.z);

        ghostMino.transform.rotation = activeMino.transform.rotation; // 向きの調整
        ghostMino.transform.position = new Vector3(activeMinoPos_x, activeMinoPos_y - SpawnerStats.ActiveMinoToBaseDistance, activeMinoPos_z); // 位置の調整
    }

    // Nextミノを生成する関数 //
    public void CreateNextMinos(int _MinoPopNumber)
    {
        for (int nextMinoOrder = 0; nextMinoOrder < nextMinos.Length; nextMinoOrder++) // NextMinosの数だけ繰り返す
        {
            if (_MinoPopNumber == 0) // ゲームスタート時
            {
                // Nextミノを一個ずつ生成していく
                nextMinos[nextMinoOrder] = SpawnNextMino(minoDictionary[SpawnerStats.SpawnMinoOrders[_MinoPopNumber + nextMinoOrder + 1]], nextMinoOrder);
                // SpawnerStats.AddNextMinos(_addNextMino: SpawnNextMino(minoDictionary[SpawnerStats.SpawnMinoOrders[_MinoPopNumber + nextMinoOrder + 1]], nextMinoOrder), _number: nextMinoOrder);
            }
            else // 2回目以降
            {
                //以前のNextMinoを消去
                Destroy(nextMinos[nextMinoOrder].gameObject);

                nextMinos[nextMinoOrder] = SpawnNextMino(minoDictionary[SpawnerStats.SpawnMinoOrders[_MinoPopNumber + nextMinoOrder + 1]], nextMinoOrder);
                // SpawnerStats.AddNextMinos(_addNextMino: SpawnNextMino(minoDictionary[SpawnerStats.SpawnMinoOrders[_MinoPopNumber + nextMinoOrder + 1]], nextMinoOrder), _number: nextMinoOrder);
            }
        }
    }

    // Hold機能の処理をする関数 //
    public void CreateHoldMino(bool _FirstHold, int _MinoPopNumber)
    {
        // 1回目のHold
        if (_FirstHold == true)
        {
            Destroy(activeMino.gameObject); // ActiveMinoを削除
            Destroy(ghostMino.gameObject); // GhostMinoを削除

            // holdMinoName = activeMinoName; // ActiveMinoの名前を保存
            SpawnerStats.Update(_holdMinoName: SpawnerStats.ActiveMinoName);

            holdMino = SpawnHoldMino(minoDictionary[SpawnerStats.HoldMinoName]); // Holdされたミノを画面左上に表示
            // SpawnerStats.Update(_holdMino: SpawnHoldMino(minoDictionary[SpawnerStats.HoldMinoName]));

            CreateNewActiveMino(_MinoPopNumber); // 新しいActiveMinoを生成

            CreateNextMinos(_MinoPopNumber); // 新しいNextミノを生成
        }

        // 2回目以降のHold
        else
        {
            Destroy(activeMino.gameObject); // ActiveMinoを削除
            Destroy(ghostMino.gameObject); // GhostMinoを削除

            // ActiveMinoName と HoldMinoName の名前を交換する
            MinoType temp;
            temp = SpawnerStats.ActiveMinoName;
            // activeMinoName = SpawnerStats.HoldMinoName;
            SpawnerStats.Update(_activeMinoName: SpawnerStats.HoldMinoName);
            // holdMinoName = temp;
            SpawnerStats.Update(_holdMinoName: temp);

            activeMino = SpawnActiveMino(holdMino); // HoldミノをActiveMinoに戻す
            // SpawnerStats.Update(_activeMino: SpawnActiveMino(SpawnerStats.HoldMino));

            CheckActiveMinoToBaseDistance(); // ActiveMinoToBaseDistance の計算

            ghostMino = SpawnGhostMino(ghostMinoDictionary[SpawnerStats.ActiveMinoName], activeMino, SpawnerStats.ActiveMinoToBaseDistance);
            // SpawnerStats.Update(_ghostMino: SpawnGhostMino(ghostMinoDictionary[activeMinoName], activeMino, activeMinoToBaseDistance));

            Destroy(holdMino.gameObject); // 以前のホールドミノを削除

            holdMino = SpawnHoldMino(minoDictionary[SpawnerStats.HoldMinoName]); // Holdされたミノを画面左上に表示
            // SpawnerStats.Update(_holdMino: SpawnHoldMino(minoDictionary[SpawnerStats.HoldMinoName]));

            // 変数の初期化
            minoMovement.ResetAngle();
            minoMovement.ResetStepsSRS();
        }
    }

    // 選ばれたミノを生成する関数 //
    public MinoMovement SpawnActiveMino(MinoMovement _SelecyMino)
    {
        MinoMovement activeMino = Instantiate(_SelecyMino,
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
    public MinoMovement SpawnGhostMino(MinoMovement _SelectMino, MinoMovement _ActiveMino, int _ActiveMinoToBaseDistance)
    {
        // ActiveMinoの各座標を格納する変数を宣言
        int activeMinoInfo_x = Mathf.RoundToInt(_ActiveMino.transform.position.x);
        int activeMinoInfo_y = Mathf.RoundToInt(_ActiveMino.transform.position.y);
        int activeMinoInfo_z = Mathf.RoundToInt(_ActiveMino.transform.position.z);

        //orderに対応するゴーストミノを、active_mino_infoのY座標からActiveMinoToBaseDistanceの値だけ下に移動した位置に生成
        MinoMovement ghostMino = Instantiate(_SelectMino,
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
    public MinoMovement SpawnNextMino(MinoMovement _SelectMino, int _nextMinoOrder)
    {
        MinoMovement nextMino = Instantiate(_SelectMino,
            nextMinoPositions[_nextMinoOrder], Quaternion.identity);

        return nextMino;
    }

    //Holdされたミノを表示する関数
    public MinoMovement SpawnHoldMino(MinoMovement _SelectMino)
    {
        //minoに対応するミノを生成
        //Quaternion.identityは、向きの回転に関する設定をしないことを表す
        MinoMovement spawnHoldMino = Instantiate(_SelectMino,
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
