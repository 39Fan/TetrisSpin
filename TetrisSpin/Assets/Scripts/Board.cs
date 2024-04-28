using UnityEngine;

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

    //干渉するスクリプトの設定
    //Calculate calculate;
    //TetrisSpinData tetrisSpinData;
    // GameStatus gameStatus;
    // Spawner spawner;

    //ゲーム画面内のグリッド
    public Transform[,] Grid;

    //ボード基盤用の四角形格納用
    [SerializeField]
    private Transform emptySprite;

    //インスタンス化と
    //テトリス画面のフィールドを作成
    private void Awake()
    {
        //calculate = FindObjectOfType<Calculate>();
        //tetrisSpinData = FindObjectOfType<TetrisSpinData>();
        //gameStatus = FindObjectOfType<GameStatus>();
        //spawner = FindObjectOfType<Spawner>();

        Grid = new Transform[Width, Height];
    }

    private void Start()
    {
        CreateBoard(); //ボードを作成する関数
    }

    void CreateBoard()
    {
        if (emptySprite) //ボード作成
        {
            for (int y = 0; y < Height - Header; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Transform clone = Instantiate(emptySprite,
                        new Vector3(x, y, 0), Quaternion.identity);

                    clone.transform.parent = transform;
                }
            }
        }
    }

    //ブロックが枠内にあるのか判定する関数を呼ぶ関数
    public bool CheckPosition(Mino _ActiveMino)
    {
        foreach (Transform item in _ActiveMino.transform)
        {
            Vector2 pos = Rounding.Round(item.position);

            if (!BoardOutCheck((int)pos.x, (int)pos.y))
            {
                return false; //ブロックが枠外に出たときfalse
            }

            if (BlockCheck((int)pos.x, (int)pos.y, _ActiveMino))
            {
                return false; //移動先に何かブロックがあるときfalse
            }
        }

        return true;
    }

    // //ブロックが枠内にあるのか判定する関数を呼ぶ関数
    // public bool CheckPosition_Ghost(Mino _GhostMino)
    // {
    //     foreach (Transform item in _GhostMino.transform)
    //     {
    //         Vector2 pos = Rounding.Round(item.position);

    //         if (!BoardOutCheck((int)pos.x, (int)pos.y))
    //         {
    //             return false; //ブロックが枠外に出たときfalse
    //         }

    //         if (BlockCheck_Ghost((int)pos.x, (int)pos.y, _GhostMino))
    //         {
    //             return false; //移動先に何かブロックがあるときfalse
    //         }
    //     }

    //     return true;
    // }

    //枠内にあるのか判定する関数
    public bool BoardOutCheck(int x, int y)
    {
        //x軸は0以上data.width満 y軸は0以上
        return (x >= 0 && x < Width && y >= 0);
    }

    //移動先にブロックがないか判定する関数
    private bool BlockCheck(int x, int y, Mino _ActiveMino)
    {
        //二次元配列が空ではない(他のブロックがある時)
        //親が違う
        return (Grid[x, y] != null && Grid[x, y].parent != _ActiveMino.transform);
    }

    // bool BlockCheck_Ghost(int x, int y, Mino _GhostMino)
    // {
    //     //二次元配列が空ではない(他のブロックがある時)
    //     //親が違う
    //     return (Grid[x, y] != null && Grid[x, y].parent != _GhostMino.transform);
    // }

    //ブロックが落ちたポジションを記録する関数
    public void SaveBlockInGrid(Mino _ActiveMino)
    {
        foreach (Transform item in _ActiveMino.transform)
        {
            Vector2 pos = Rounding.Round(item.position);

            //Debug.Log((int)pos.x);
            //Debug.Log((int)pos.y);

            Grid[(int)pos.x, (int)pos.y] = item;
        }
    }

    //指定されたマスがミノで埋まっているか、壁ならtrueを返す関数(Tspin判定に必要)
    public bool ActiveMinoCheckForTspin(int x, int y, Mino _ActiveMino)
    {
        //Gridの座標が負の場合(壁判定)true
        if (x < 0 || y < 0)
        {
            return true;
        }
        else if (!BoardOutCheck(x, y))
        {
            return true;
        }

        if (BlockCheck(x, y, _ActiveMino))
        {
            return true;
        }

        return false;
    }

    //全ての行をチェックして、埋まっていれば削除する関数
    //帰値は列消去数
    public int ClearAllRows()
    {
        //合計列消去数をClearRowCount格納
        int ClearRowCount = 0;

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

        return ClearRowCount;
    }

    //全ての行をチェックする関数
    bool IsComplete(int y)
    {
        for (int x = 0; x < Width; x++)
        {
            if (Grid[x, y] == null)
            {
                return false;
            }
        }

        return true; //何かしら埋まっているとき
    }

    //削除する関数
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

    //上にあるブロックを1段下げる関数
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

    // ミノを構成するブロックについて、1番底に近いブロックのy座標データを返す関数 //
    public int CheckActiveMinoBottomBlockPosition_y(Mino _activeMino, int _StartingBottomBlockPosition_y)
    {
        int bottomBlockPosition_y = _StartingBottomBlockPosition_y; // 1番下のブロックのY座標(初期値はゲームボードの高さの数値)
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

    // //Blockを消す関数
    // public void DestroyBlock(Mino Dblock)
    // {
    //     //Debug.Log("====this is DestroyBlock in Board====");

    //     Destroy(Dblock.gameObject);
    // }

    // //GhostBlockを消す関数
    // public void DestroyBlock_Ghost(Block_Ghost Dblock)
    // {
    //     //Debug.Log("====this is DestroyBlock_Ghost in Board====");

    //     Destroy(Dblock.gameObject);
    // }

    public bool OverLimit(Mino _ActiveMino)
    {
        foreach (Transform item in _ActiveMino.transform)
        {
            if (item.transform.position.y > Height - Header - 1)
            {
                return true;
            }
        }

        return false;
    }
}
