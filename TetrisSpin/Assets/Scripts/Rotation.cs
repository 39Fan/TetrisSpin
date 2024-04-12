using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    [SerializeField]
    Board board;
    Spawner spawner;

    /* Tスピン関連 */
    bool UseTSpin = false;  // Tスピン使用フラグ
    bool UseTSpinMini = false;  // Tスピンミニ使用フラグ
    int LastSR = 0; // 最後に行ったスーパーローテーション(SR)パターン(0-4)
    int BlockCount = 0;


    private void Start()
    {
        board = GameObject.FindObjectOfType<Board>();
    }


    //SR時に重複しないか判定する関数
    bool RotationCheck(int Rx, int Ry, Block block)
    {
        foreach (Transform item in block.transform)
        {
            Vector2 pos = Rounding.Round(item.position);

            //Gridの座標が負の場合false
            if ((int)pos.x + Rx < 0 || (int)pos.y + Ry < 0)
            {
                Debug.Log("SR中に枠外に出た。");
                return false;
            }

            //各gameobjectの位置を調べて、そこが重複していたらfalse
            if (board.Grid[(int)pos.x + Rx, (int)pos.y + Ry] != null
                && board.Grid[(int)pos.x + Rx, (int)pos.y + Ry].parent != block.transform)
            {
                Debug.Log("SR先が重複");
                return false;
            }

            //枠外に出てもfalse
            if (!board.BoardOutCheck((int)pos.x + Rx, (int)pos.y + Ry))
            {
                Debug.Log("SR中に枠外に出た。");
                return false;
            }
        }
        Debug.Log(Rx);
        Debug.Log(Ry);
        return true;
    }

    //スーパーローテーション(SR)
    public bool MinoSuperRotation(int MinoAngleBefore, int MinoAngleAfter, Block block)
    {
        int movex = 0;  // X座標移動量
        int movey = 0;  // Y座標移動量
        LastSR = 0;
        // Iミノ以外
        if (!block.name.Contains("I"))
        {
            // 1. 軸を左右に動かす
            // 0が90度（B）の場合は左，-90度（D）の場合は右へ移動
            // 0が0度（A），180度（C）の場合は回転した方向の逆へ移動
            Debug.Log("Iミノ以外のSR判定開始");
            switch (MinoAngleAfter)
            {
                case 1: // 右向き
                    movex = -1;
                    break;
                case 3: // 左向き
                    movex = 1;
                    break;
                case 0: // 上向き
                case 2: // 下向き
                    switch (MinoAngleBefore)
                    {
                        case 1: // 回転前が右向き
                            movex = 1;
                            break;
                        case 3: // 回転前が左向き
                            movex = -1;
                            break;
                    }
                    break;
            }
            LastSR++; //1
            if (!RotationCheck(movex, movey, block))
            {
                // 2.その状態から軸を上下に動かす
                // 0が90度（B），-90度（D）の場合は上へ移動
                // 0が0度（A），180度（C）の場合は下へ移動
                Debug.Log("1.失敗");
                switch (MinoAngleAfter)
                {
                    case 1:
                    case 3:
                        movey = 1;
                        break;
                    case 0:
                    case 2:
                        movey = -1;
                        break;
                }
                LastSR++; //2
                if (!RotationCheck(movex, movey, block))
                {
                    // 3.元に戻し、軸を上下に2マス動かす
                    // 0が90度（B），-90度（D）の場合は下へ移動
                    // 0が0度（A），180度（C）の場合は上へ移動
                    Debug.Log("2.失敗");
                    movex = 0;
                    movey = 0;
                    switch (MinoAngleAfter)
                    {
                        case 1:
                        case 3:
                            movey = -2;
                            break;
                        case 0:
                        case 2:
                            movey = 2;
                            break;
                    }
                    LastSR++; //3
                    if (!RotationCheck(movex, movey, block))
                    {
                        // 4.その状態から軸を左右に動かす
                        // 0が90度（B）の場合は左，-90度（D）の場合は右へ移動
                        // 0が0度（A），180度（C）の場合は回転した方向の逆へ移動
                        Debug.Log("3.失敗");
                        switch (MinoAngleAfter)
                        {
                            case 1:
                                movex = -1;
                                break;
                            case 3:
                                movex = 1;
                                break;
                            case 0:
                            case 2:
                                switch (MinoAngleBefore)
                                {
                                    case 1: // 回転前が右向き
                                        movex = 1;
                                        break;
                                    case 3: // 回転前が左向き
                                        movex = -1;
                                        break;
                                }
                                break;
                        }
                        LastSR++; //4
                        if (!RotationCheck(movex, movey, block))
                        {
                            Debug.Log("SR失敗");
                            return false;
                        }
                    }
                }
            }
        }
        // Iミノの場合
        else
        {
            int pt1x;   // 1のX移動量
            int pt2x;   // 2のX移動量

            // 1. 軸を左右に動かす
            // 0が90度（B）の場合は右，-90度（D）の場合は左へ移動（枠にくっつく）
            // 0が0度（A），180度（C）の場合は回転した方向の逆へ移動 0度は２マス移動
            switch (MinoAngleAfter)
            {
                case 1:
                    movex = 1;
                    break;
                case 3:
                    movex = -1;
                    break;
                case 0:
                case 2:
                    switch (MinoAngleBefore)
                    {
                        case 1:
                            movex = -1;
                            break;
                        case 3:
                            movex = 1;
                            break;
                    }
                    if (MinoAngleAfter == 0) movex *= 2;   // 0度は2マス移動
                    break;
            }
            pt1x = movex;
            if (!RotationCheck(movex, movey, block))
            {
                // 2. 軸を左右に動かす
                // 0が90度（B）の場合は左，-90度（D）の場合は右へ移動（枠にくっつく）
                // 0が0度（A），180度（C）の場合は回転した方向へ移動 180度は２マス移動
                switch (MinoAngleAfter)
                {
                    case 1:
                        movex = -1;
                        break;
                    case 3:
                        movex = 1;
                        break;
                    case 0:
                    case 2:
                        switch (MinoAngleBefore)
                        {
                            case 1:
                                movex = 1;
                                break;
                            case 3:
                                movex = -1;
                                break;
                        }
                        if (MinoAngleAfter == 2) movex *= 2;   // 180度は2マス移動
                        break;
                }
                pt2x = movex;
                if (!RotationCheck(movex, movey, block))
                {
                    // 3. 軸を上下に動かす
                    // 0が90度（B）の場合は1を下，-90度（D）の場合は1を上へ移動
                    // 0が0度（A），180度（C）の場合は
                    // 回転前のミノが右半分にある（B）なら1を上へ
                    // 回転前のミノが左半分にある（D）なら2を下へ移動
                    // 左回転なら２マス動かす
                    switch (MinoAngleAfter)
                    {
                        case 1:
                            movex = pt1x;
                            movey = -1;
                            break;
                        case 3:
                            movex = pt1x;
                            movey = 1;
                            break;
                        case 0:
                        case 2:
                            switch (MinoAngleBefore)
                            {
                                case 1:
                                    movex = pt1x;
                                    movey = 1;
                                    break;
                                case 3:
                                    movex = pt2x;
                                    movey = -1;
                                    break;
                            }
                            break;
                    }
                    // 左回転
                    if (MinoAngleBefore == 0 && MinoAngleAfter == 3 || MinoAngleBefore == 3 && MinoAngleAfter == 2
                        || MinoAngleBefore == 2 && MinoAngleAfter == 1 || MinoAngleBefore == 1 && MinoAngleAfter == 0)
                    {
                        movey *= -2;
                    }
                    if (!RotationCheck(movex, movey, block))
                    {
                        // 4. 軸を上下に動かす
                        // 0が90度（B）の場合は2を上，-90度（D）の場合は2を下へ移動
                        // 0が0度（A），180度（C）の場合は
                        // 回転前のミノが右半分にある（B）なら2を下へ
                        // 回転前のミノが左半分にある（D）なら1を上へ移動
                        // 右回転なら２マス動かす
                        switch (MinoAngleAfter)
                        {
                            case 1:
                                movex = pt2x;
                                movey = 1;
                                break;
                            case 3:
                                movex = pt2x;
                                movey = -1;
                                break;
                            case 0:
                            case 2:
                                switch (MinoAngleBefore)
                                {
                                    case 1:
                                        movex = pt2x;
                                        movey = -1;
                                        break;
                                    case 3:
                                        movex = pt1x;
                                        movey = 1;
                                        break;
                                }
                                break;
                        }
                        // 右回転
                        if (MinoAngleBefore == 3 && MinoAngleAfter == 0 || MinoAngleBefore == 0 && MinoAngleAfter == 1
                            || MinoAngleBefore == 1 && MinoAngleAfter == 2 || MinoAngleBefore == 2 && MinoAngleAfter == 3)
                        {
                            movey *= -2;
                        }
                        if (!RotationCheck(movex, movey, block))
                        {
                            return false;
                        }
                    }
                }
            }
        }

        block.Move(new Vector3(movex, 0, 0));
        block.Move(new Vector3(0, movey, 0));

        return true;
    }

    public bool TspinCheck(bool useSpin, Block block)
    {
        Debug.Log("====this is TspinCheck====");

        if (board.BlockCheckForTspin((int)Mathf.Round(block.transform.position.x) + 1, (int)Mathf.Round(block.transform.position.y) + 1, block))
        {
            BlockCount++;
        }
        if (board.BlockCheckForTspin((int)Mathf.Round(block.transform.position.x) + 1, (int)Mathf.Round(block.transform.position.y) - 1, block))
        {
            BlockCount++;
        }
        if (board.BlockCheckForTspin((int)Mathf.Round(block.transform.position.x) - 1, (int)Mathf.Round(block.transform.position.y) + 1, block))
        {
            BlockCount++;
        }
        if (board.BlockCheckForTspin((int)Mathf.Round(block.transform.position.x) - 1, (int)Mathf.Round(block.transform.position.y) - 1, block))
        {
            BlockCount++;
        }

        Debug.Log(BlockCount);

        if (BlockCount >= 3 && useSpin == true)
        {
            Debug.Log("Tspin!");
            return true;
        }

        BlockCount = 0;

        return false;
    }
}
