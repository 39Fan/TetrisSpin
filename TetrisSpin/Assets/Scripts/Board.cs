using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Board : MonoBehaviour
{
    public Transform[,] Grid; //ゲーム画面内のグリッド

    //ボード基盤用の四角形格納用
    [SerializeField]
    private Transform emptySprite;

    [SerializeField]
    private int height = 25, width = 10, header = 5;

    private void Awake()
    {
        Grid = new Transform[width, height];
    }

    private void Start()
    {
        CreateBoard(); //ボードを作成する関数
    }

    void CreateBoard()
    {
        if (emptySprite) //ボード作成
        {
            for (int y = 0; y < height - header + 1; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Transform clone = Instantiate(emptySprite,
                        new Vector3(x, y, 0), Quaternion.identity);
                    //InstantiateはPrefabsからオブジェクトを生成できる。
                    //()内は生成するオブジェクトの条件を記入
                    //Quaternion.identityは回転しないよの意

                    clone.transform.parent = transform;
                }
            }
        }
    }

    //ブロックが枠内にあるのか判定する関数を呼ぶ関数
    public bool CheckPosition(Block block)
    {
        foreach (Transform item in block.transform)
        {
            Vector2 pos = Rounding.Round(item.position);

            if (!BoardOutCheck((int)pos.x, (int)pos.y))
            {
                return false; //ブロックが枠外に出たときfalse
            }

            if (BlockCheck((int)pos.x, (int)pos.y, block))
            {
                return false; //移動先に何かブロックがあるときfalse
            }
        }

        return true;
    }

    //ブロックが枠内にあるのか判定する関数を呼ぶ関数
    public bool CheckPosition_Ghost(Block_Ghost block)
    {
        foreach (Transform item in block.transform)
        {
            Vector2 pos = Rounding.Round(item.position);

            if (!BoardOutCheck((int)pos.x, (int)pos.y))
            {
                return false; //ブロックが枠外に出たときfalse
            }

            if (BlockCheck_Ghost((int)pos.x, (int)pos.y, block))
            {
                return false; //移動先に何かブロックがあるときfalse
            }
        }

        return true;
    }

    //枠内にあるのか判定する関数
    public bool BoardOutCheck(int x, int y)
    {
        //x軸は0以上width未満 y軸は0以上
        return (x >= 0 && x < width && y >= 0);
    }

    //移動先にブロックがないか判定する関数
    bool BlockCheck(int x, int y, Block block)
    {
        //二次元配列が空ではない(他のブロックがある時)
        //親が違う
        return (Grid[x, y] != null && Grid[x, y].parent != block.transform);
    }

    bool BlockCheck_Ghost(int x, int y, Block_Ghost block)
    {
        //二次元配列が空ではない(他のブロックがある時)
        //親が違う
        return (Grid[x, y] != null && Grid[x, y].parent != block.transform);
    }

    //ブロックが落ちたポジションを記録する関数
    public void SaveBlockInGrid(Block block)
    {
        foreach (Transform item in block.transform)
        {
            Vector2 pos = Rounding.Round(item.position);

            Grid[(int)pos.x, (int)pos.y] = item;
        }
    }

    //指定されたマスがミノで埋まっているか、壁ならtrueを返す関数(Tspin判定に必要)
    public bool BlockCheckForTspin(int x, int y, Block block)
    {
        Debug.Log(x);
        Debug.Log(y);

        //Gridの座標が負の場合(壁判定)true
        if (x < 0 || y < 0)
        {
            return true;
        }
        else if (!BoardOutCheck(x, y))
        {
            return true;
        }

        if (BlockCheck(x, y, block))
        {
            return true;
        }

        return false;
    }

    //全ての行をチェックして、埋まっていれば削除する関数
    public void ClearAllRows()
    {
        for (int y = 0; y < height; y++)
        {
            if (IsComplete(y))
            {
                ClearRow(y);

                ShiftRowsDown(y + 1);

                y--;
            }
        }
    }

    //全ての行をチェックする関数
    bool IsComplete(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (Grid[x, y] == null)
            {
                return false;
            }
        }

        return true; //何かしら埋まっているとき
    }

    //削除する関数
    void ClearRow(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (Grid[x, y] != null)
            {
                Destroy(Grid[x, y].gameObject);
            }
            Grid[x, y] = null;
        }
    }

    //上にあるブロックを1段下げる関数
    void ShiftRowsDown(int startY)
    {
        for (int y = startY; y < height; y++)
        {
            for (int x = 0; x < width; x++)
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

    //Blockを消す関数
    public void DestroyBlock(Block Dblock)
    {
        //Debug.Log("====this is DestroyBlock in Board====");

        Destroy(Dblock.gameObject);
    }

    //GhostBlockを消す関数
    public void DestroyBlock_Ghost(Block_Ghost Dblock)
    {
        //Debug.Log("====this is DestroyBlock_Ghost in Board====");

        Destroy(Dblock.gameObject);
    }

    public bool OverLimit(Block block)
    {
        foreach (Transform item in block.transform)
        {
            if (item.transform.position.y > height - header - 2)
            {
                return true;
            }
        }

        return false;
    }
}
