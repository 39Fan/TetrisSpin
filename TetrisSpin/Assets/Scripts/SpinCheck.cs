using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// スピン判定 列挙型
/// </summary>
public enum SpinTypeNames
{
    Ispin,
    IspinMini,
    Jspin,
    Lspin,
    Ospin,
    Sspin,
    SspinMini,
    Tspin,
    TspinMini,
    Zspin,
    ZspinMini,
    None
}

/// <summary>
/// ブロックの存在判定 列挙型
/// </summary>
public enum Existence
{
    Exist,
    NotExist
}

/// <summary>
/// Spinの判定を確認するクラス
/// </summary>
public class SpinCheck : MonoBehaviour
{
    /// <summary> Spinの種類 </summary>
    private SpinTypeNames spinTypeName;

    // ゲッタープロパティ //
    public SpinTypeNames SpinTypeName => spinTypeName;

    /// <summary> ログの詳細 </summary>
    private string logDetail;

    // 干渉するスクリプト //
    Board board;
    Spawner spawner;

    /// <summary>
    /// インスタンス化
    /// </summary>
    private void Awake()
    {
        board = FindObjectOfType<Board>();
        spawner = FindObjectOfType<Spawner>();
    }

    /// <summary> Spin判定をリセットする関数 </summary>
    public void ResetSpinTypeName()
    {
        LogHelper.DebugLog(eClasses.SpinCheck, eMethod.ResetSpinTypeName, eLogTitle.Start);

        spinTypeName = SpinTypeNames.None;

        LogHelper.DebugLog(eClasses.SpinCheck, eMethod.ResetSpinTypeName, eLogTitle.End);
    }

    /// <summary> 各ミノのスピン判定をチェックする関数 </summary>
    /// <remarks>
    /// この関数は回転成功時に呼び出される
    /// </remarks>
    public void CheckSpinType(eMinoDirection _minoAngleAfter, int _stepsSRS)
    {
        LogHelper.DebugLog(eClasses.SpinCheck, eMethod.CheckSpinType, eLogTitle.Start);

        switch (SpawnerStats.ActiveMinoName)
        {
            case eMinoType.IMino:
                IspinCheck(_minoAngleAfter);
                break;
            case eMinoType.JMino:
                JspinCheck(_minoAngleAfter, _stepsSRS);
                break;
            case eMinoType.LMino:
                LspinCheck(_minoAngleAfter, _stepsSRS);
                break;
            case eMinoType.SMino:
                SspinCheck(_minoAngleAfter, _stepsSRS);
                break;
            case eMinoType.TMino:
                TspinCheck(_minoAngleAfter, _stepsSRS);
                break;
            case eMinoType.ZMino:
                ZspinCheck(_minoAngleAfter, _stepsSRS);
                break;
            default:
                LogHelper.WarningLog(eClasses.SpinCheck, eMethod.ResetSpinTypeName, eLogTitle.MinosIdentificationFailed);
                break;
        }

        LogHelper.DebugLog(eClasses.SpinCheck, eMethod.CheckSpinType, eLogTitle.End);
    }

    /// <summary> Ispinの判定をする関数 </summary>
    /// <remarks>
    /// Mini判定も確認する
    /// </remarks>
    private void IspinCheck(eMinoDirection _minoAngleAfter)
    {
        LogHelper.DebugLog(eClasses.SpinCheck, eMethod.IspinCheck, eLogTitle.Start);

        if (_minoAngleAfter == eMinoDirection.North || _minoAngleAfter == eMinoDirection.South) // Iミノが横向きの場合
        {
            // IspinMiniの判定をチェックする

            // 条件
            // ① Iミノを構成する各ブロックの1マス上に少なくともブロックが1つ以上存在すること
            // ② Iミノを構成する各ブロックの1マス下に少なくともブロックが3つ以上存在すること
            // ▫️▫️▫️▫️ ←①
            // ▪️▪️▪️▪️
            // ▫️▫️▫️▫️ ←②

            // 指定マスのブロックの有無情報を格納するリスト
            List<Existence> checkBlocklistAboveI = new List<Existence>();
            List<Existence> checkBlocklistBelowI = new List<Existence>();

            // IspinMiniの判定に必要なオフセット
            int yOffset = 1;

            // ①を調べる
            foreach (Transform item in spawner.ActiveMino.transform) // Iミノを構成するブロックそれぞれを確認する
            {
                Vector2 pos = Rounding.Round(item.position); // floatからintに値を丸める

                // 1マス上部にブロックがあるか調べる
                if (board.CheckGrid(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y + yOffset), spawner.ActiveMino))
                {
                    checkBlocklistAboveI.Add(Existence.Exist);
                }
                else
                {
                    checkBlocklistAboveI.Add(Existence.NotExist);
                }
            }

            // ②を調べる
            foreach (Transform item in spawner.ActiveMino.transform) // Iミノを構成するブロックそれぞれを確認する
            {
                Vector2 pos = Rounding.Round(item.position); // floatからintに値を丸める

                // 1マス上部にブロックがあるか調べる
                if (board.CheckGrid(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y - yOffset), spawner.ActiveMino))
                {
                    checkBlocklistBelowI.Add(Existence.Exist);
                }
                else
                {
                    checkBlocklistBelowI.Add(Existence.NotExist);
                }
            }

            // ①と②の状態を文字列に変換
            string checkBlocksAboveInfo = string.Join(", ", checkBlocklistAboveI);
            string checkBlocksBelowInfo = string.Join(", ", checkBlocklistBelowI);

            // LogHelper.Log(eLogLevel.Info, "SpinCheck", "IspinCheck()", checkBlocksAboveInfo);
            // LogHelper.Log(eLogLevel.Info, "SpinCheck", "IspinCheck()", checkBlocksBelowInfo); // Infoログ // TODO

            // 条件を満たすか確認
            if (checkBlocklistAboveI.FindAll(block => block == Existence.Exist).Count >= 1 ||
                checkBlocklistBelowI.FindAll(block => block == Existence.Exist).Count >= 3)
            {
                spinTypeName = SpinTypeNames.IspinMini;
            }
        }
        else // Iミノが縦向きの場合
        {
            // Ispinの判定をチェックする

            // 条件
            // ① Iミノを構成する各ブロックの1マス横側にブロックが存在するか確認し、それぞれの側面で3つ以上ブロックが存在する
            // ② Iミノの上部にブロックが存在する

            //  ↓②
            //  .
            //  .
            //  .
            //  ▫️
            //  ▫️
            // ▫️▪️▫️
            // ▫️▪️▫️
            // ▫️▪️▫️
            // ▫️▪️▫️
            // ↑ ↑①


            // 指定マスのブロックの有無情報を格納するリスト
            List<Existence> checkBlocklistRightSideI = new List<Existence>();
            List<Existence> checkBlocklistLeftSideI = new List<Existence>();
            List<Existence> checkBlocklistUpperI = new List<Existence>();

            // Iミノの上部にブロックがあるか調べるために必要な変数
            int IminoTopPositionY = board.CheckActiveMinoTopBlockPositionY(spawner.ActiveMino); // Iミノの最上部のy座標

            // Ispinの判定に必要なオフセット
            int xOffset = 1;
            int yOffset = 1;

            // ①を調べる
            foreach (Transform item in spawner.ActiveMino.transform) // Iミノを構成するブロックそれぞれを確認する
            {
                Vector2 pos = Rounding.Round(item.position); // floatからintに値を丸める

                // 1マス右側にブロックがあるか調べる
                if (board.CheckGrid(Mathf.RoundToInt(pos.x + xOffset), Mathf.RoundToInt(pos.y), spawner.ActiveMino))
                {
                    checkBlocklistRightSideI.Add(Existence.Exist);
                }
                else
                {
                    checkBlocklistRightSideI.Add(Existence.NotExist);
                }
            }
            foreach (Transform item in spawner.ActiveMino.transform) // Iミノを構成するブロックそれぞれを確認する
            {
                Vector2 pos = Rounding.Round(item.position); // floatからintに値を丸める

                // 1マス左側にブロックがあるか調べる
                if (board.CheckGrid(Mathf.RoundToInt(pos.x - xOffset), Mathf.RoundToInt(pos.y), spawner.ActiveMino))
                {
                    checkBlocklistLeftSideI.Add(Existence.Exist);
                }
                else
                {
                    checkBlocklistLeftSideI.Add(Existence.NotExist);
                }
            }

            // ②を調べる
            while (IminoTopPositionY + yOffset < 20) // ゲームボードの上部からはみ出すまで調べる
            {
                // Iミノの上部にブロックが存在するか1マスずつ調べていく
                if (board.CheckGrid(Mathf.RoundToInt(spawner.ActiveMino.transform.position.x), IminoTopPositionY + yOffset, spawner.ActiveMino))
                {
                    checkBlocklistUpperI.Add(Existence.Exist);
                }
                else
                {
                    checkBlocklistUpperI.Add(Existence.NotExist);
                }

                yOffset++;
            }


            // // ①と②の状態を文字列に変換
            // string checkBlocksRightSideInfo = string.Join(", ", checkBlocklistRightSideI);
            // string checkBlocksLeftSideInfo = string.Join(", ", checkBlocklistLeftSideI);
            // string checkBlocksUpperInfo = string.Join(", ", checkBlocklistUpperI);

            // LogHelper.Log(eLogLevel.Info, "SpinCheck", "IspinCheck()", checkBlocksRightSideInfo);
            // LogHelper.Log(eLogLevel.Info, "SpinCheck", "IspinCheck()", checkBlocksLeftSideInfo);
            // LogHelper.Log(eLogLevel.Info, "SpinCheck", "IspinCheck()", checkBlocksUpperInfo); // Infoログ // TODO

            // 条件を満たすか確認
            if (checkBlocklistRightSideI.FindAll(block => block == Existence.Exist).Count >= 3 &&
                checkBlocklistLeftSideI.FindAll(block => block == Existence.Exist).Count >= 3 &&
                checkBlocklistUpperI.FindAll(block => block == Existence.Exist).Count >= 1)
            {
                spinTypeName = SpinTypeNames.Ispin;
            }
        }

        LogHelper.DebugLog(eClasses.SpinCheck, eMethod.IspinCheck, eLogTitle.End);
    }

    /// <summary> Jspinの判定をする関数 </summary>
    private void JspinCheck(eMinoDirection _minoAngleAfter, int _stepsSRS)
    {
        LogHelper.DebugLog(eClasses.SpinCheck, eMethod.JspinCheck, eLogTitle.Start);

        // Jspinの判定をチェックする

        // 条件
        // ① 以下の図の指定されたマス(▫️)のうち3つ以上がブロックや壁である
        // ② SRSを使用している(SRSの段階が1以上)

        // North    // East    // South    // West
        //
        //  ▫️            ▫️                      ▫️
        //  ▪️           ▫️▪️▪️▫️         ▫️          ▪️
        // ▫️▪️◆▪️▫️         ◆        ▫️▪️◆▪️▫️         ◆
        //  ▫️            ▪️           ▪️        ▫️▪️▪️▫️
        //               ▫️           ▫️          ▫️       // ◆はJミノの軸


        // 指定マスのブロックの有無情報を格納するリスト
        List<Existence> checkBlocklistJ = new List<Existence>();

        // 各向きに対応した、軸から各マスまでのオフセット
        int[,] northOffsetJ = new int[,]
        {
        { 2, 0 },{ -1, -1 },{ -2, 0 },{ -1, 2 }
        };
        int[,] eastOffsetJ = new int[,]
        {
        { 2, 1 },{ 0, -2 },{ -1, 1 },{ 0, 2 }
        };
        int[,] southOffsetJ = new int[,]
        {
        { 2, 0 },{ 1, -2 },{ -2, 0 },{ 1, 1 }
        };
        int[,] westOffsetJ = new int[,]
        {
        { 1, -1 },{ 0, -2 },{ -2, -1 },{ 0, 2 }
        };

        // 操作中のミノのx,y座標
        int posX = Mathf.RoundToInt(spawner.ActiveMino.transform.position.x);
        int posY = Mathf.RoundToInt(spawner.ActiveMino.transform.position.y);

        int[,] currentOffsetsJ = null;

        // Jミノの向きで処理を分岐させる
        if (_minoAngleAfter == eMinoDirection.North)
        {
            currentOffsetsJ = northOffsetJ;
        }
        else if (_minoAngleAfter == eMinoDirection.East)
        {
            currentOffsetsJ = eastOffsetJ;
        }
        else if (_minoAngleAfter == eMinoDirection.South)
        {
            currentOffsetsJ = southOffsetJ;
        }
        else if (_minoAngleAfter == eMinoDirection.West)
        {
            currentOffsetsJ = westOffsetJ;
        }

        // 指定マスのブロックの有無を調べる
        for (int grid = 0; grid < currentOffsetsJ.GetLength(0); grid++)
        {
            if (board.CheckGrid(posX + currentOffsetsJ[grid, 0], posY + currentOffsetsJ[grid, 1], spawner.ActiveMino))
            {
                checkBlocklistJ.Add(Existence.Exist);
            }
            else
            {
                checkBlocklistJ.Add(Existence.NotExist);
            }
        }

        // 条件を満たすか確認(②の確認込み)
        if (checkBlocklistJ.FindAll(block => block == Existence.Exist).Count >= 3 && _stepsSRS >= 1)
        {
            spinTypeName = SpinTypeNames.Jspin;
        }

        LogHelper.DebugLog(eClasses.SpinCheck, eMethod.JspinCheck, eLogTitle.End);
    }

    /// <summary> Lspinの判定をする関数 </summary>
    private void LspinCheck(eMinoDirection _minoAngleAfter, int _stepsSRS)
    {
        LogHelper.DebugLog(eClasses.SpinCheck, eMethod.LspinCheck, eLogTitle.Start);

        // Lspinの判定をチェックする

        // 条件
        // ① 以下の図の指定されたマス(▫️)のうち3つ以上がブロックや壁である
        // ② SRSを使用している(SRSの段階が1以上)

        // North    // East    // South    // West
        //
        //    ▫️          ▫️                      ▫️
        //    ▪️          ▪️         ▫️          ▫️▪️▪️▫️
        // ▫️▪️◆▪️▫️         ◆        ▫️▪️◆▪️▫️         ◆
        //    ▫️         ▫️▪️▪️▫️       ▪️            ▪️
        //               ▫️         ▫️            ▫️      // ◆はLミノの軸


        // 指定マスのブロックの有無情報を格納するリスト
        List<Existence> checkBlocklistL = new List<Existence>();

        // 各向きに対応した、軸から各マスまでのオフセット
        int[,] northOffsetL = new int[,]
        {
        { 2, 0 },{ 1, -1 },{ -2, 0 },{ 1, 2 }
        };
        int[,] eastOffsetL = new int[,]
        {
        { 2, -1 },{ 0, -2 },{ -1, -1 },{ 0, 2 }
        };
        int[,] southOffsetL = new int[,]
        {
        { 2, 0 },{ -1, -2 },{ -2, 0 },{ -1, 1 }
        };
        int[,] westOffsetL = new int[,]
        {
        { 1, 1 },{ 0, -2 },{ -2, 1 },{ 0, 2 }
        };

        // 操作中のミノのx,y座標
        int posX = Mathf.RoundToInt(spawner.ActiveMino.transform.position.x);
        int posY = Mathf.RoundToInt(spawner.ActiveMino.transform.position.y);

        // 現在の向きに対応したオフセット
        int[,] currentOffsetsL = null;

        // Lミノの向きで処理を分岐させる
        if (_minoAngleAfter == eMinoDirection.North)
        {
            currentOffsetsL = northOffsetL;
        }
        else if (_minoAngleAfter == eMinoDirection.East)
        {
            currentOffsetsL = eastOffsetL;
        }
        else if (_minoAngleAfter == eMinoDirection.South)
        {
            currentOffsetsL = southOffsetL;
        }
        else if (_minoAngleAfter == eMinoDirection.West)
        {
            currentOffsetsL = westOffsetL;
        }

        // 指定マスのブロックの有無を調べる
        for (int grid = 0; grid < currentOffsetsL.GetLength(0); grid++)
        {
            if (board.CheckGrid(posX + currentOffsetsL[grid, 0], posY + currentOffsetsL[grid, 1], spawner.ActiveMino))
            {
                checkBlocklistL.Add(Existence.Exist);
            }
            else
            {
                checkBlocklistL.Add(Existence.NotExist);
            }
        }

        // Lspinの判定をチェックする
        if (checkBlocklistL.FindAll(block => block == Existence.Exist).Count >= 3 && _stepsSRS >= 1)
        {
            spinTypeName = SpinTypeNames.Lspin;
        }

        LogHelper.DebugLog(eClasses.SpinCheck, eMethod.LspinCheck, eLogTitle.End);
    }

    /// <summary> Sspinの判定をする関数 </summary>
    /// <remarks>
    /// Mini判定も確認する
    /// </remarks>
    private void SspinCheck(eMinoDirection _minoAngleAfter, int _stepsSRS)
    {
        LogHelper.DebugLog(eClasses.SpinCheck, eMethod.SspinCheck, eLogTitle.Start);

        // Sspinの判定をチェックする
        // 条件
        // ① 以下の図の指定されたマス(▫️)のうち、ブロックや壁でないマスが2つ以内である。

        // SspinMiniの判定をチェックする
        // 条件
        // ① 以下の図の指定されたマス(▫️)のうち、ブロックや壁でないマスが3つ以内である。
        // ② SRSを使用している(SRSの段階が1以上)

        // North    // East    // South    // West
        //
        //   ▫️▫️          ▫️                     ▫️
        //  ▫️▪️▪️▫️        ▫️▪️▫️         ▫️▫️        ▫️▪️▫️
        // ▫️▪️◆▫️         ▫️◆▪️▫️       ▫️◆▪️▫️       ▫️▪️◆▫️
        //  ▫️▫️           ▫️▪️▫️      ▫️▪️▪️▫️         ▫️▪️▫️
        //                ▫️        ▫️▫️           ▫️      // ◆はSミノの軸


        // 指定マスのブロックの有無情報を格納するリスト
        List<Existence> checkBlocklistS = new List<Existence>();

        // 各向きに対応した、軸から各マスまでのオフセット
        int[,] northOffsetS = new int[,]
        {
        { 1, 0 },{ 0, -1 },{ -1, -1 },{ -2, 0 },{ -1, 1 },{ 0, 2 },{ 1, 2 },{ 2, 1 }
        };
        int[,] eastOffsetS = new int[,]
        {
        { 2, 0 },{ 2, -1 },{ 1, -2 },{ 0, -1 },{ -1, 0 },{ -1, 1 },{ 0, 2 },{ 1, 1 }
        };
        int[,] southOffsetS = new int[,]
        {
        { 2, 0 },{ 1, -1 },{ 0, -2 },{ -1, -2 },{ -2, -1 },{ -1, 0 },{ 0, 1 },{ 1, 1 }
        };
        int[,] westOffsetS = new int[,]
        {
        { 1, 0 },{ 1, -1 },{ 0, -2 },{ -1, -1 },{ -2, 0 },{ -2, 1 },{ -1, 2 },{ 0, 1 }
        };

        // 操作中のミノのx,y座標
        int posX = Mathf.RoundToInt(spawner.ActiveMino.transform.position.x);
        int posY = Mathf.RoundToInt(spawner.ActiveMino.transform.position.y);

        // 現在の向きに対応したオフセット
        int[,] currentOffsetsS = null;

        // Sミノの向きで処理を分岐させる
        if (_minoAngleAfter == eMinoDirection.North)
        {
            currentOffsetsS = northOffsetS;
        }
        else if (_minoAngleAfter == eMinoDirection.East)
        {
            currentOffsetsS = eastOffsetS;
        }
        else if (_minoAngleAfter == eMinoDirection.South)
        {
            currentOffsetsS = southOffsetS;
        }
        else if (_minoAngleAfter == eMinoDirection.West)
        {
            currentOffsetsS = westOffsetS;
        }

        // 指定マスのブロックの有無を調べる
        for (int grid = 0; grid < currentOffsetsS.GetLength(0); grid++)
        {
            if (board.CheckGrid(posX + currentOffsetsS[grid, 0], posY + currentOffsetsS[grid, 1], spawner.ActiveMino))
            {
                checkBlocklistS.Add(Existence.Exist);
            }
            else
            {
                checkBlocklistS.Add(Existence.NotExist);
            }
        }

        // Sspinの判定をチェックする
        if (checkBlocklistS.FindAll(block => block == Existence.NotExist).Count <= 2)
        {
            spinTypeName = SpinTypeNames.Sspin;

            LogHelper.DebugLog(eClasses.SpinCheck, eMethod.SspinCheck, eLogTitle.End);
            return;
        }
        // SspinMiniの判定をチェックする
        if (checkBlocklistS.FindAll(block => block == Existence.NotExist).Count <= 3 && _stepsSRS >= 1)
        {
            spinTypeName = SpinTypeNames.SspinMini;

            LogHelper.DebugLog(eClasses.SpinCheck, eMethod.SspinCheck, eLogTitle.End);
            return;
        }
    }

    /// <summary> Tspinの判定をする関数 </summary> // TODO 条件のところをもう少し詳しく書く
    /// <remarks>
    /// Mini判定も確認する
    /// </remarks>
    private void TspinCheck(eMinoDirection _minoAngleAfter, int _stepsSRS)
    {
        LogHelper.DebugLog(eClasses.SpinCheck, eMethod.TspinCheck, eLogTitle.Start);

        // Tミノの中心から順番に[1, 1]、[1, -1]、[-1, 1]、[-1, -1]の分だけ移動した座標にブロックや壁がないか確認する
        // ブロックや壁があった時は Exist ない時は Not Exist で_ActiveMinoCountのリストに追加されていく
        // 例: checkBlocksT["Exist", "Exist", Existence.NotExist, "Exist"]
        // ▫️▪️▫️
        // ▪️▪️▪️
        // ▫️ ▫️  ←四隅を調べる

        // 指定マスのブロックの有無情報を格納するリスト
        List<Existence> checkBlocklistT = new List<Existence>();

        for (int x = 1; x >= -1; x -= 2)
        {
            for (int y = 1; y >= -1; y -= 2)
            {
                if (board.CheckGrid((int)Mathf.Round(spawner.ActiveMino.transform.position.x) + x, (int)Mathf.Round(spawner.ActiveMino.transform.position.y) + y, spawner.ActiveMino))
                {
                    checkBlocklistT.Add(Existence.Exist);
                }
                else
                {
                    checkBlocklistT.Add(Existence.NotExist);
                }
            }
        }

        // string checkBlocksInfo = string.Join(", ", checkBlocklistT);

        // LogHelper.Log(eLogLevel.Info, "SpinCheck", "TspinCheck()", checkBlocksInfo); // Infoログ // TODO


        // Tspinの判定をチェックする

        // 条件
        // checkBlocksTにExistが3つ以上含まれる


        // 同時にTspinMiniの判定もチェックする

        // 条件
        // Tspinの条件を満たす
        // SRSの段階が4以外
        // Tミノの突起側が Not Exist の場合
        // ▫️▪️▫️ ←この隅のどちらかが Not Exist の場合
        // ▪️▪️▪️
        // ▫️ ▫️

        if (checkBlocklistT.FindAll(block => block == Existence.Exist).Count >= 3) // 3マス以上ブロックや壁に埋まっている場合
        {
            if (_stepsSRS == 4) // SRSが4段階の時は T-Spin 判定になる
            {
                spinTypeName = SpinTypeNames.Tspin;
            }
            else // SRSが4段階でない時
            {
                int right_up = 0; // 右上のブロックを指す
                int right_down = 1; // 右下
                int left_up = 2; // 左上
                int left_down = 3; // 左下

                // Tミノの突起の左右が空白の時、T-Spin Mini 判定になる
                switch (_minoAngleAfter)
                {
                    case eMinoDirection.North: // Tミノが北向きの時、右上と左上を確認する
                        if (checkBlocklistT[right_up] == Existence.NotExist || checkBlocklistT[left_up] == Existence.NotExist)
                        {
                            spinTypeName = SpinTypeNames.TspinMini;
                        }
                        else
                        {
                            spinTypeName = SpinTypeNames.Tspin; // Tミノの底側のブロックが空白の時は T-Spin 判定になる
                        }

                        break;

                    case eMinoDirection.East: // 右上と右下
                        if (checkBlocklistT[right_up] == Existence.NotExist || checkBlocklistT[right_down] == Existence.NotExist)
                        {
                            spinTypeName = SpinTypeNames.TspinMini;
                        }
                        else
                        {
                            spinTypeName = SpinTypeNames.Tspin;
                        }

                        break;

                    case eMinoDirection.South: // 右下と左下
                        if (checkBlocklistT[right_down] == Existence.NotExist || checkBlocklistT[left_down] == Existence.NotExist)
                        {
                            spinTypeName = SpinTypeNames.TspinMini;
                        }
                        else
                        {
                            spinTypeName = SpinTypeNames.Tspin;
                        }

                        break;

                    case eMinoDirection.West: // 左上と左下
                        if (checkBlocklistT[left_up] == Existence.NotExist || checkBlocklistT[left_down] == Existence.NotExist)
                        {
                            spinTypeName = SpinTypeNames.TspinMini;
                        }
                        else
                        {
                            spinTypeName = SpinTypeNames.Tspin;
                        }

                        break;
                }
            }
        }

        LogHelper.DebugLog(eClasses.SpinCheck, eMethod.TspinCheck, eLogTitle.End);
    }

    /// <summary> Zspinの判定をする関数 </summary>
    /// <remarks>
    /// Mini判定も確認する
    /// </remarks>
    private void ZspinCheck(eMinoDirection _minoAngleAfter, int _stepsSRS)
    {
        LogHelper.DebugLog(eClasses.SpinCheck, eMethod.ZspinCheck, eLogTitle.Start);

        // Zspinの判定をチェックする
        // 条件
        // ① 以下の図の指定されたマス(▫️)のうち、ブロックや壁でないマスが2つ以内である。

        // ZspinMiniの判定をチェックする
        // 条件
        // ① 以下の図の指定されたマス(▫️)のうち、ブロックや壁でないマスが3つ以内である。
        // ② SRSを使用している(SRSの段階が1以上)

        // North    // East    // South    // West
        //
        //  ▫️▫️            ▫️                    ▫️
        // ▫️▪️▪️▫️          ▫️▪️▫️       ▫️▫️         ▫️▪️▫️
        //  ▫️◆▪️▫️        ▫️◆▪️▫️      ▫️▪️◆▫️        ▫▪️◆▫️
        //   ▫️▫️         ▫️▪️▫️        ▫️▪️▪️▫️        ▫️▪️▫️
        //               ▫️          ▫️▫️          ▫️      // ◆はZミノの軸


        // 指定マスのブロックの有無情報を格納するリスト
        List<Existence> checkBlocklistZ = new List<Existence>();

        // 各向きに対応した、軸から各マスまでのオフセット
        int[,] northOffsetZ = new int[,]
        {
        { 2, 0 },{ 1, -1 },{ 0, -1 },{ -1, 0 },{ -2, 1 },{ -1, 2 },{ 0, 2 },{ 1, 1 }
        };
        int[,] eastOffsetZ = new int[,]
        {
        { 2, 0 },{ 1, -1 },{ 0, -2 },{ -1, -1 },{ -1, 0 },{ 0, 1 },{ 1, 2 },{ 2, 1 }
        };
        int[,] southOffsetZ = new int[,]
        {
        { 1, 0 },{ 2, -1 },{ 1, -2 },{ 0, -2 },{ -1, -1 },{ -2, 0 },{ -1, 1 },{ 0, 1 }
        };
        int[,] westOffsetZ = new int[,]
        {
        { 1, 0 },{ 1, -1 },{ 0, -2 },{ -1, -1 },{ -2, 0 },{ -2, 1 },{ -1, 2 },{ 0, 1 }
        };

        // 操作中のミノのx,y座標
        int posX = Mathf.RoundToInt(spawner.ActiveMino.transform.position.x);
        int posY = Mathf.RoundToInt(spawner.ActiveMino.transform.position.y);

        // 現在の向きに対応したオフセット
        int[,] currentOffsetsZ = null;

        // Zミノの向きで処理を分岐させる
        if (_minoAngleAfter == eMinoDirection.North)
        {
            currentOffsetsZ = northOffsetZ;
        }
        else if (_minoAngleAfter == eMinoDirection.East)
        {
            currentOffsetsZ = eastOffsetZ;
        }
        else if (_minoAngleAfter == eMinoDirection.South)
        {
            currentOffsetsZ = southOffsetZ;
        }
        else if (_minoAngleAfter == eMinoDirection.West)
        {
            currentOffsetsZ = westOffsetZ;
        }

        // 指定マスのブロックの有無を調べる
        for (int grid = 0; grid < currentOffsetsZ.GetLength(0); grid++)
        {
            if (board.CheckGrid(posX + currentOffsetsZ[grid, 0], posY + currentOffsetsZ[grid, 1], spawner.ActiveMino))
            {
                checkBlocklistZ.Add(Existence.Exist);
            }
            else
            {
                checkBlocklistZ.Add(Existence.NotExist);
            }
        }

        // Zspinの判定をチェックする
        if (checkBlocklistZ.FindAll(block => block == Existence.NotExist).Count <= 2)
        {
            spinTypeName = SpinTypeNames.Zspin;

            LogHelper.DebugLog(eClasses.SpinCheck, eMethod.ZspinCheck, eLogTitle.End);
            return;
        }
        // ZspinMiniの判定をチェックする
        if (checkBlocklistZ.FindAll(block => block == Existence.NotExist).Count <= 3 && _stepsSRS >= 1)
        {
            spinTypeName = SpinTypeNames.ZspinMini;

            LogHelper.DebugLog(eClasses.SpinCheck, eMethod.ZspinCheck, eLogTitle.End);
            return;
        }
    }
}

/////////////////// 旧コード ///////////////////

// // ミノの向き //
// private Existence North = "North";
// private Existence East = "East";
// private Existence South = "South";
// private Existence West = "West";


// public void CheckSpinType() // この関数は、回転時に呼び出される
// {
//     if (spawner.ActiveMinoName == "I_Mino")
//     {
//         IspinCheck();
//     }
//     if (spawner.ActiveMinoName == "J_Mino")
//     {
//         JspinCheck();
//     }
//     if (spawner.ActiveMinoName == "L_Mino")
//     {
//         LspinCheck();
//     }
//     /*else if (mino == O_mino)
//     {
//         return OspinCheck(_ActiveMino);
//     }*/
//     /*else if (mino == S_mino)
//     {
//         return SspinCheck(_ActiveMino);
//     }*/
//     if (spawner.ActiveMinoName == "T_Mino")
//     {
//         TspinCheck();
//     }
//     /*else if (mino == Z_mino)
//     {
//         return ZspinCheck(_ActiveMino);
//     }*/
//     else
//     {
//         // Debug.LogError("[SpinCheck CheckSpinType()] activeMinoNameが特定できません。");
//     }
// }

// // Lspinの判定をする関数
// public void LspinCheck()
// {
//     // Lspinの判定をチェックする

//     //////////////////////
//     // 条件
//     // ①以下の図の指定されたマス(▫️)のうち3つ以上がブロックや壁である
//     // ②SRSを使用している(SRSの段階が1以上)

//     // North    // East    // South    // West
//     //
//     //  ▫️            ▫️                      ▫️
//     //  ▪️           ▫️▪️▪️▫️         ▫️          ▪️
//     // ▫️▪️◆▪️▫️         ◆        ▫️▪️◆▪️▫️         ◆
//     //  ▫️            ▪️           ▪️        ▫️▪️▪️▫️
//     //               ▫️           ▫️          ▫️       // ◆はJミノの軸


//     List<Existence> checkBlocksJ = new List<Existence>(); // IspinMiniの判定に必要なリスト

//     // 各向きに対応したオフセット //
//     int[,] northOffsetJ = new int[,]
//     {
//         { 2, 0 },{ -1, -1 },{ -2, 0 },{ -1, 2 }
//     };
//     int[,] eastOffsetJ = new int[,]
//     {
//         { 2, 1 },{ 0, -2 },{ -1, 1 },{ 0, 2 }
//     };
//     int[,] southOffsetJ = new int[,]
//     {
//         { 2, 0 },{ 1, -2 },{ -2, 0 },{ 1, 1 }
//     };
//     int[,] westOffsetJ = new int[,]
//     {
//         { 1, -1 },{ 0, -2 },{ -2, -1 },{ 0, 2 }
//     };

//     int pos_x = Mathf.RoundToInt(spawner.ActiveMino.transform.position.x);
//     int pos_y = Mathf.RoundToInt(spawner.ActiveMino.transform.position.y);

//     if (gameStatus.MinoAngleAfter == North) // Jミノが北向きの場合
//     {
//         // 指定のマスにブロックまたは壁があるか調べる
//         for (int grid = 0; grid < northOffsetJ.Length; grid++)
//         {
//             if (board.CheckGrid(pos_x + northOffsetJ[grid, 0], pos_y + northOffsetJ[grid, 1], spawner.ActiveMino))
//             {
//                 checkBlocksJ.Add("Exist");
//             }
//             else
//             {
//                 checkBlocksJ.Add(Existence.NotExist);
//             }
//         }
//     }
//     if (gameStatus.MinoAngleAfter == East) // Jミノが北向きの場合
//     {
//         // 指定のマスにブロックまたは壁があるか調べる
//         for (int grid = 0; grid < eastOffsetJ.Length; grid++)
//         {
//             if (board.CheckGrid(pos_x + eastOffsetJ[grid, 0], pos_y + eastOffsetJ[grid, 1], spawner.ActiveMino))
//             {
//                 checkBlocksJ.Add("Exist");
//             }
//             else
//             {
//                 checkBlocksJ.Add(Existence.NotExist);
//             }
//         }
//     }
//     if (gameStatus.MinoAngleAfter == South) // Jミノが北向きの場合
//     {
//         // 指定のマスにブロックまたは壁があるか調べる
//         for (int grid = 0; grid < southOffsetJ.Length; grid++)
//         {
//             if (board.CheckGrid(pos_x + southOffsetJ[grid, 0], pos_y + southOffsetJ[grid, 1], spawner.ActiveMino))
//             {
//                 checkBlocksJ.Add("Exist");
//             }
//             else
//             {
//                 checkBlocksJ.Add(Existence.NotExist);
//             }
//         }
//     }
//     if (gameStatus.MinoAngleAfter == West) // Jミノが北向きの場合
//     {
//         // 指定のマスにブロックまたは壁があるか調べる
//         for (int grid = 0; grid < westOffsetJ.Length; grid++)
//         {
//             if (board.CheckGrid(pos_x + westOffsetJ[grid, 0], pos_y + westOffsetJ[grid, 1], spawner.ActiveMino))
//             {
//                 checkBlocksJ.Add("Exist");
//             }
//             else
//             {
//                 checkBlocksJ.Add(Existence.NotExist);
//             }
//         }
//     }
// }

/////////////////////////////////////////////////////////