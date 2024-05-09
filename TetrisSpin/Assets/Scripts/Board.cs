using UnityEngine;

///// ゲームボードに関するスクリプト /////


// ↓このスクリプトで可能なこと↓ //

// ゲームボードの作成
// ミノの位置判定
// ミノの設置場所を管理

public class Board : MonoBehaviour
{
    // ゲームボード //
    private int Header = 20; // ヘッダー20マス
    private int Height = 40; // 高さ40マス     //Height - Header がゲームボードの縦幅
    private int Width = 10; // 横幅10マス

    // ゲッタープロパティ //
    public int header
    {
        get { return Header; }
    }
    public int height
    {
        get { return Height; }
    }

    // ゲーム画面内のグリッド //
    public Transform[,] Grid;

    // ボード基盤の四角形 //
    [SerializeField] private Transform EmptySprite;

    private void Awake()
    {
        Grid = new Transform[Width, Height]; // ゲームボードを作成
    }

    private void Start()
    {
        CreateBoard();
    }

    // ボードを作成する関数 //
    void CreateBoard()
    {
        if (EmptySprite)
        {
            for (int y = 0; y < Height - Header; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Transform clone = Instantiate(EmptySprite,
                        new Vector3(x, y, 0), Quaternion.identity); // EmptySpriteを並べていく

                    clone.transform.parent = transform;
                }
            }
        }
    }

    // ActiveMinoが枠内にあるのか判定する関数を呼ぶ関数 //
    public bool CheckPosition(Mino _ActiveMino)
    {
        foreach (Transform item in _ActiveMino.transform)
        {
            Vector2 pos = Rounding.Round(item.position); // floatからintに値を丸める

            if (!BoardOutCheck((int)pos.x, (int)pos.y)) // ブロックが枠外に出たとき
            {
                return false;
            }

            if (BlockCheck((int)pos.x, (int)pos.y, _ActiveMino)) // 移動先に何かブロックがあるとき
            {
                return false;
            }
        }

        return true;
    }

    // ブロックが枠内にあるか判定する関数 //
    public bool BoardOutCheck(int x, int y)
    {
        // x軸は 0 以上 Width 未満、 y軸は 0 以上
        return (x >= 0 && x < Width && y >= 0);
    }

    // 移動先にブロックがないか判定する関数 //
    private bool BlockCheck(int x, int y, Mino _ActiveMino)
    {
        // 二次元配列が空ではない(他のブロックがある時)
        // 親が違う
        return (Grid[x, y] != null && Grid[x, y].parent != _ActiveMino.transform);
    }

    // ブロックが落ちたポジションを記録する関数 //
    public void SaveBlockInGrid(Mino _ActiveMino)
    {
        foreach (Transform item in _ActiveMino.transform)
        {
            Vector2 pos = Rounding.Round(item.position); // floatからintに値を丸める

            Grid[(int)pos.x, (int)pos.y] = item;
        }
    }

    // 全ての行をチェックして、埋まっていれば削除する関数 //
    public int ClearAllRows()
    {
        int ClearRowCount = 0; // 合計列消去数を ClearRowCount に格納

        for (int y = 0; y < Height; y++)
        {
            if (IsComplete(y))
            {
                ClearRow(y);

                ShiftRowsDown(y + 1);

                y--;

                ClearRowCount++;
            }
        }

        return ClearRowCount; // 列消去数を返す
    }

    // 指定された行がブロックで埋まっているか確認する関数 //
    bool IsComplete(int y)
    {
        for (int x = 0; x < Width; x++)
        {
            if (Grid[x, y] == null) // ブロックがない場合
            {
                return false;
            }
        }

        return true; // 行が埋まっている場合
    }

    // 指定された行のブロックを削除する関数 //
    private void ClearRow(int y)
    {
        for (int x = 0; x < Width; x++)
        {
            if (Grid[x, y] != null)
            {
                Destroy(Grid[x, y].gameObject);
            }

            Grid[x, y] = null;
        }
    }

    // 上にあるブロックを1段下げる関数 //
    private void ShiftRowsDown(int startY)
    {
        for (int y = startY; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                if (Grid[x, y] != null)
                {
                    Grid[x, y - 1] = Grid[x, y];
                    Grid[x, y] = null;
                    Grid[x, y - 1].position += new Vector3(0, -1, 0);
                }
            }
        }
    }

    // 指定されたマスがブロック、もしくは壁かどうか確認する関数 //
    public bool ActiveMinoCheckForTspin(int x, int y, Mino _ActiveMino)
    {
        if (x < 0 || y < 0) // Gridの座標が負の場合(壁判定)
        {
            return true;
        }
        else if (!BoardOutCheck(x, y)) // ゲームボード外の場合
        {
            return true;
        }

        if (BlockCheck(x, y, _ActiveMino)) // ブロックで埋まっている場合
        {
            return true;
        }

        return false;
    }

    // ミノを構成するブロックについて、1番底に近いブロックのy座標データを返す関数 //
    public int CheckActiveMinoBottomBlockPosition_y(Mino _activeMino)
    {
        int bottomBlockPosition_y = 21; // 1番下のブロックのY座標(初期値はゲームオーバーになる数値)
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

    // ゲームオーバーか判定する関数 //
    public bool CheckGameOver(Mino _activeMino)
    {
        int bottomBlockPosition_y = CheckActiveMinoBottomBlockPosition_y(_activeMino); // ActiveMino の1番下のブロックのy座標

        if (bottomBlockPosition_y >= 21) // ミノの1番下のブロックのy座標が21を超えている場合
        {
            return true;
        }

        return false;
    }
}
