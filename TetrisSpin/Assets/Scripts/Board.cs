using System.Reflection.Emit;
using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ボードの統計情報を保持する静的クラス
/// </summary>
public static class BoardStats
{
    /// <summary> ライン消去の履歴を記録するリスト </summary>
    private static List<int> lineClearCountHistory = new List<int>();

    // ゲッタープロパティ //
    public static List<int> LineClearCountHistory => lineClearCountHistory;

    /// <summary> 指定されたフィールドの値を更新する関数 </summary>
    /// <param name="_lineClearCountHistory"> ライン消去の履歴 </param>
    /// <remarks>
    /// 指定されていない引数は現在の値を維持
    /// </remarks>
    public static void Update(List<int> _lineClearCountHistory = null)
    {
        lineClearCountHistory = _lineClearCountHistory ?? new List<int>(lineClearCountHistory);
        // TODO: ログの記入
    }

    /// <summary> デフォルトの <see cref="BoardStats"/> にリセットする関数 </summary>
    public static void Reset()
    {
        lineClearCountHistory.Clear();
    }

    /// <summary> ライン消去数の履歴を追加する関数 </summary>
    /// <param name="_lineClearCount"> 消去したライン数 </param>
    public static void AddLineClearCountHistory(int _lineClearCount)
    {
        lineClearCountHistory.Add(_lineClearCount);
        // TODO: ログの記入
    }
}

/// <summary>
/// テトリスのボードを管理するクラス
/// </summary>
/// <remarks>
/// ミノの位置判定、ブロックのセーブ、ライン消去などを処理する
/// </remarks>
public class Board : MonoBehaviour
{
    /// <summary> ゲームボードのヘッダー(20) </summary>
    private int header = 20;
    /// <summary> ゲームボードの高さ(40) </summary>
    /// <remarks>
    /// Height - Header (20)がゲームボードの縦幅となる
    /// </remarks>
    private int height = 40;
    /// <summary> ゲームボードの横幅(10) </summary>
    private int width = 10;

    // ゲッタープロパティ //
    public int Header => header;
    public int Height => height;

    /// <summary>ゲームボード基盤の四角形 </summary>
    [SerializeField] private Transform emptySprite;

    /// <summary> ゲーム画面内のグリッド </summary>
    /// <remarks>
    /// 2次元配列型[width(10),height(40)]
    /// </remarks>
    /// <value> [0~ ,0~] </value>
    public Transform[,] grid;

    /// <summary>
    /// ゲームボードグリッドの定義
    /// </summary>
    private void Awake()
    {
        grid = new Transform[width, height]; // ゲームボードを作成
    }

    /// <summary>
    /// ゲームボードを作成する関数の呼び出し
    /// </summary>
    private void Start()
    {
        CreateBoard();
    }

    /// <summary> ゲームボードの作成 </summary>
    void CreateBoard()
    {
        for (int y = 0; y < height - header; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Transform clone = Instantiate(emptySprite,
                    new Vector3(x, y, 0), Quaternion.identity); // EmptySpriteを並べていく

                clone.transform.parent = transform;
            }
        }

    }

    /// <summary> activeMinoが存在できる位置か判定する関数を呼ぶ関数 </summary>
    /// <param name="_activeMino"> 操作中のミノ </param>
    /// <returns> ブロックが存在できる場合 true、それ以外の場合は false </returns>
    public bool CheckPosition(MinoMovement _activeMino)
    {
        // 操作中のミノを構成するブロック個々に調べる
        foreach (Transform item in _activeMino.transform)
        {
            Vector2 pos = Rounding.Round(item.position);

            if (!IsWithinBoard((int)pos.x, (int)pos.y)) // ブロックが枠外に出たとき
            {
                return false;
            }

            if (CheckMinoCollision((int)pos.x, (int)pos.y, _activeMino)) // 移動先に何かブロックがあるとき
            {
                return false;
            }
        }

        return true;
    }

    /// <summary> ブロックが枠内にあるか判定する関数 </summary>
    /// <param name="_x"> activeMinoを構成するブロックの x 座標 </param>
    /// <param name="_y"> activeMinoを構成するブロックの y 座標 </param>
    /// <returns> ブロックが枠内にある場合 true、それ以外の場合は false </returns>
    public bool IsWithinBoard(int _x, int _y)
    {
        return (_x >= 0 && _x < width && _y >= 0);
    }

    /// <summary> activeMinoとブロックが重なっているか判定する関数 </summary>
    /// <param name="_x"> activeMinoを構成するブロックの x 座標 </param>
    /// <param name="_y"> activeMinoを構成するブロックの y 座標 </param>
    /// <param name="_activeMino"> 操作中のミノ </param>
    /// <returns> ブロックが重なっていない場合 true、それ以外の場合は false </returns>
    private bool CheckMinoCollision(int _x, int _y, MinoMovement _activeMino)
    {
        // 二次元配列が空ではない(他のブロックがある時)
        // 親が違う
        return (grid[_x, _y] != null && grid[_x, _y].parent != _activeMino.transform);
    }

    /// <summary> ブロックが落ちたポジションを記録する関数 </summary>
    /// <param name="_activeMino"> 操作中のミノ </param>
    public void SaveBlockInGrid(MinoMovement _activeMino)
    {
        foreach (Transform item in _activeMino.transform)
        {
            Vector2 pos = Rounding.Round(item.position); // floatからintに値を丸める

            grid[(int)pos.x, (int)pos.y] = item;
        }
    }

    /// <summary> 全ての行をチェックして、埋まっていれば削除する関数を呼ぶ関数 </summary>
    /// <returns> 合計の消去ライン数(lineClearCount) </returns>
    public int ClearAllRows()
    {
        /// <summary> 合計の消去ライン数 </summary>
        int lineClearCount = 0;

        for (int y = 0; y < Height; y++)
        {
            if (IsComplete(y))
            {
                ClearRow(y);

                ShiftRowsDown(y + 1);

                y--;

                lineClearCount++;
            }
        }

        return lineClearCount;
    }

    /// <summary> 指定された行がブロックで埋まっているか確認する関数 </summary>
    /// <param name="_y"> 調べる y 座標 </param>
    /// <returns> 指定された行がブロックで埋まっている場合 true、それ以外の場合は false </returns>
    bool IsComplete(int _y)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, _y] == null) // ブロックがない場合
            {
                return false;
            }
        }

        return true; // 行が埋まっている場合
    }

    /// <summary> 指定された行のブロックを削除する関数 </summary>
    /// <param name="_y"> 調べる y 座標 </param>
    private void ClearRow(int _y)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, _y] != null)
            {
                Destroy(grid[x, _y].gameObject);
            }

            grid[x, _y] = null;
        }
    }

    /// <summary> 指定されたy座標より高い位置にある全てのブロックを1段下げる関数 </summary>
    /// <param name="_y"> 下げる y 座標(この y 以上のブロックが1段下がる) </param>
    private void ShiftRowsDown(int _y)
    {
        for (int y = _y; y < Height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[x, y] != null)
                {
                    grid[x, y - 1] = grid[x, y];
                    grid[x, y] = null;
                    grid[x, y - 1].position += new Vector3(0, -1, 0);
                }
            }
        }
    }

    /// <summary> PerfectClearか判別する関数 </summary>
    /// <returns> PerfectClear 判定の場合 true、それ以外の場合は false </returns>
    public bool CheckPerfectClear()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[x, y] != null) // ブロックがある場合
                {
                    return false;
                }
            }
        }
        return true;
    }

    /// <summary> 指定されたマスがブロック、もしくは壁かどうか確認する関数 </summary>
    /// <param name="_x"> 調べる x 座標 </param>
    /// <param name="_y"> 調べる y 座標 </param>
    /// <param name="_activeMino"> 操作中のミノ </param>
    /// <returns> 指定されたマスがブロック、もしくは壁の場合 true、それ以外の場合は false </returns>
    public bool CheckGrid(int _x, int _y, MinoMovement _activeMino)
    {
        if (_x < 0 || _y < 0) // gridの座標が負の場合(壁判定)
        {
            return true;
        }
        else if (!IsWithinBoard(_x, _y)) // ゲームボード外の場合
        {
            return true;
        }

        if (CheckMinoCollision(_x, _y, _activeMino)) // ブロックで埋まっている場合
        {
            return true;
        }

        return false;
    }

    /// <summary> ミノを構成するブロックについて、最上部のブロックのy座標を返す関数 </summary>
    /// <param name="_activeMino"> 操作中のミノ </param>
    /// <returns> 最上部のブロックのy座標 topBlockPosition_y </returns>
    public int CheckActiveMinoTopBlockPosition_y(MinoMovement _activeMino)
    {
        int topBlockPosition_y = 0; // 最上部のブロックのY座標(初期値は底の数値)
        int temp; // 一時的な変数

        foreach (Transform item in _activeMino.transform) // ミノの各ブロックを調べる
        {
            temp = Mathf.RoundToInt(item.transform.position.y); // ブロックのy座標の値

            // 1番ブロックのy座標が高い値を探す
            if (temp >= topBlockPosition_y)
            {
                topBlockPosition_y = temp;
            }
        }

        return topBlockPosition_y;
    }

    /// <summary> ミノを構成するブロックについて、最下部ブロックのy座標データを返す関数 </summary>
    /// <param name="_activeMino"> 操作中のミノ </param>
    /// <returns> 最下部のブロックのy座標 bottomBlockPosition_y </returns>
    public int CheckActiveMinoBottomBlockPosition_y(MinoMovement _activeMino)
    {
        int bottomBlockPosition_y = 21; // 最下部のブロックのY座標(初期値はゲームオーバーになる数値)
        int temp; // 一時的な変数

        foreach (Transform item in _activeMino.transform) // ミノの各ブロックを調べる
        {
            temp = Mathf.RoundToInt(item.transform.position.y); // ブロックのy座標の値

            // 1番ブロックのy座標が低い値を探す
            if (temp < bottomBlockPosition_y)
            {
                bottomBlockPosition_y = temp;
            }
        }

        return bottomBlockPosition_y;
    }

    /// <summary> ライン消去数の履歴を追加する </summary>
    /// <param name="_lineClearCount"> 消去ライン数 </param>
    public void AddLineClearCountHistory(int _lineClearCount)
    {
        BoardStats.AddLineClearCountHistory(_lineClearCount);
    }

    /// <summary> ゲームオーバーか判定する関数 </summary>
    /// <param name="_activeMino"> 操作中のミノ </param>
    /// <returns> ゲームオーバー判定の場合 true、それ以外の場合は false </returns>
    public bool CheckGameOver(MinoMovement _activeMino)
    {
        int bottomBlockPosition_y = CheckActiveMinoBottomBlockPosition_y(_activeMino); // ActiveMino の1番下のブロックのy座標

        if (bottomBlockPosition_y >= 21) // ミノの1番下のブロックのy座標が21を超えている場合
        {
            return true;
        }

        return false;
    }
}

/////////////////// 旧コード ///////////////////

// /// <summary>
// /// ボードの統計情報を保持する構造体
// /// </summary>
// public struct BoardStats
// {
//     /// <summary> ライン消去の履歴を記録するリスト </summary>
//     private List<int> lineClearCountHistory;

//     // ゲッタープロパティ //
//     public List<int> LineClearCountHistory => lineClearCountHistory;

//     /// <summary> デフォルトコンストラクタ </summary>
//     public BoardStats(List<int> _lineClearCountHistory)
//     {
//         lineClearCountHistory = _lineClearCountHistory ?? new List<int>();
//     }

//     /// <summary> デフォルトの <see cref="BoardStats"/> を作成する関数 </summary>
//     /// <returns>
//     /// デフォルト値で初期化された <see cref="BoardStats"/> のインスタンス
//     /// </returns>
//     public static BoardStats CreateDefault()
//     {
//         return new BoardStats
//         {
//             lineClearCountHistory = new List<int>()
//         };
//     }

//     /// <summary> 指定されたフィールドの値を更新する関数 </summary>
//     /// <param name="_lineClearCountHistory"> ライン消去の履歴 </param>
//     /// <returns> 更新された <see cref="BoardStats"/> の新しいインスタンス </returns>
//     /// <remarks>
//     /// 指定されていない引数は現在の値を維持
//     /// </remarks>
//     public BoardStats Update(List<int> _lineClearCountHistory = null)
//     {
//         var updatedStats = new BoardStats(
//             _lineClearCountHistory ?? new List<int>(lineClearCountHistory)
//         );
//         // TODO: ログの記入
//         return updatedStats;
//     }

//     /// <summary> ライン消去数の履歴を追加する </summary>
//     /// <param name="_lineClearCount"> 消去したライン数 </param>
//     public BoardStats AddLineClearCountHistory(int _lineClearCount)
//     {
//         var newHistory = new List<int>(lineClearCountHistory)
//         {
//             _lineClearCount
//         };
//         return Update(newHistory);
//     }
// }

/////////////////////////////////////////////////////////