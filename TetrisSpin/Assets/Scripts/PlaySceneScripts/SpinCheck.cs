using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// スピン判定 列挙型
/// </summary>
public enum SpinTypes
{
    Ispin,
    IspinMini,
    Jspin,
    JspinMini,
    Lspin,
    LspinMini,
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
/// 詳細なスピンタイプ 列挙型
/// </summary>
public enum DetailedSpinTypes
{
    I_Spin, I_SpinSingle, I_SpinDouble, I_SpinTriple, I_SpinQuattro, I_SpinMini,
    J_Spin, J_SpinSingle, J_SpinDouble, J_SpinTriple, J_SpinMini, J_SpinDoubleMini,
    L_Spin, L_SpinSingle, L_SpinDouble, L_SpinTriple, L_SpinMini, L_SpinDoubleMini,
    S_Spin, S_SpinSingle, S_SpinDouble, S_SpinTriple, S_SpinMini, S_SpinDoubleMini,
    T_Spin, T_SpinSingle, T_SpinDouble, T_SpinTriple, T_SpinMini, T_SpinDoubleMini,
    Z_Spin, Z_SpinSingle, Z_SpinDouble, Z_SpinTriple, Z_SpinMini, Z_SpinDoubleMini,
    Tetris, None
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
    private SpinTypes spinType;

    // ゲッタープロパティ //
    public SpinTypes SpinType => spinType;

    /// <summary> ログの詳細 </summary>
    private string logDetail;

    // 干渉するクラス //
    Board board;
    Spawner spawner;

    /// <summary>
    /// インスタンス化
    /// </summary>
    private void Awake()
    {
        spinType = SpinTypes.None;

        board = FindObjectOfType<Board>();
        spawner = FindObjectOfType<Spawner>();
    }

    /// <summary> Spin判定をリセットする関数 </summary>
    public void ResetSpinType()
    {
        LogHelper.DebugLog(eClasses.SpinCheck, eMethod.ResetSpinType, eLogTitle.Start);

        spinType = SpinTypes.None;

        LogHelper.DebugLog(eClasses.SpinCheck, eMethod.ResetSpinType, eLogTitle.End);
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
                LogHelper.WarningLog(eClasses.SpinCheck, eMethod.ResetSpinType, eLogTitle.MinosIdentificationFailed);
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
            // ① Iミノを構成する各ブロックの1マス上に少なくともブロックが1つ以上存在すること。
            // ② Iミノを構成する各ブロックの1マス下に少なくともブロックが3つ以上存在すること。
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
                Vector2 pos = new Vector2(Mathf.Round(item.position.x), Mathf.Round(item.position.y));

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
                Vector2 pos = new Vector2(Mathf.Round(item.position.x), Mathf.Round(item.position.y));

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

            logDetail = $"checkBlocklistAboveI : {string.Join(", ", checkBlocklistAboveI)}, checkBlocklistBelowI : {string.Join(", ", checkBlocklistBelowI)}";
            LogHelper.InfoLog(eClasses.SpinCheck, eMethod.IspinCheck, eLogTitle.CheckBlockList, logDetail);

            // 条件を満たすか確認
            if (checkBlocklistAboveI.FindAll(block => block == Existence.Exist).Count >= 1 ||
                checkBlocklistBelowI.FindAll(block => block == Existence.Exist).Count >= 3)
            {
                spinType = SpinTypes.IspinMini;
            }
        }
        else // Iミノが縦向きの場合
        {
            // Ispinの判定をチェックする
            // 条件
            // ① Iミノを構成する各ブロックの1マス横面で、それぞれ3つ以上ブロックまたは壁が存在する。
            // ② Iミノの上部3マス以内にブロックが存在する。

            //  ↓②
            //  ▫️
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

            /// <summary> Iミノの最もY座標が高いブロックのY座標 </summary>
            int IminoTopPositionY = board.CheckActiveMinoTopBlockPositionY(spawner.ActiveMino); // Iミノの最上部のy座標

            // Ispinの判定に必要なオフセット
            int xOffset = 1;

            // ①を調べる
            foreach (Transform item in spawner.ActiveMino.transform) // Iミノを構成するブロックそれぞれを確認する
            {
                Vector2 pos = new Vector2(Mathf.Round(item.position.x), Mathf.Round(item.position.y));

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
                Vector2 pos = new Vector2(Mathf.Round(item.position.x), Mathf.Round(item.position.y));

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
            for (int ii = IminoTopPositionY; ii < IminoTopPositionY + 3; ii++)
            {
                // Iミノの上部にブロックが存在するか1マスずつ調べていく
                if (board.CheckGrid(Mathf.RoundToInt(spawner.ActiveMino.transform.position.x), ii, spawner.ActiveMino))
                {
                    checkBlocklistUpperI.Add(Existence.Exist);
                }
                else
                {
                    checkBlocklistUpperI.Add(Existence.NotExist);
                }
            }

            logDetail = $"checkBlocklistRightSideI : {string.Join(", ", checkBlocklistRightSideI)}, checkBlocklistLeftSideI : {string.Join(", ", checkBlocklistLeftSideI)}, checkBlocklistUpperI : {string.Join(", ", checkBlocklistUpperI)}";
            LogHelper.InfoLog(eClasses.SpinCheck, eMethod.IspinCheck, eLogTitle.CheckBlockList, logDetail);

            // 条件を満たすか確認
            if (checkBlocklistRightSideI.FindAll(block => block == Existence.Exist).Count >= 3 &&
                checkBlocklistLeftSideI.FindAll(block => block == Existence.Exist).Count >= 3 &&
                checkBlocklistUpperI.FindAll(block => block == Existence.Exist).Count >= 1)
            {
                spinType = SpinTypes.Ispin;
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
        // ① 以下の図の指定されたマス(▫️)のうち3つ以上がブロックまたは壁である。
        // ② SRSを使用している。(SRSの段階が1以上)

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
            spinType = SpinTypes.Jspin;
        }

        LogHelper.DebugLog(eClasses.SpinCheck, eMethod.JspinCheck, eLogTitle.End);
    }

    /// <summary> Lspinの判定をする関数 </summary>
    private void LspinCheck(eMinoDirection _minoAngleAfter, int _stepsSRS)
    {
        LogHelper.DebugLog(eClasses.SpinCheck, eMethod.LspinCheck, eLogTitle.Start);

        // Lspinの判定をチェックする
        // 条件
        // ① 以下の図の指定されたマス(▫️)のうち3つ以上がブロックまたは壁である。
        // ② SRSを使用している。(SRSの段階が1以上)

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
            spinType = SpinTypes.Lspin;
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
        // ① 以下の図の指定されたマス(▫️)のうち、ブロックまたは壁でないマスが2つ以内である。

        // SspinMiniの判定をチェックする
        // 条件
        // ① 以下の図の指定されたマス(▫️)のうち、ブロックまたは壁でないマスが3つ以内である。
        // ② SRSを使用している。(SRSの段階が1以上)

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
            spinType = SpinTypes.Sspin;
        }
        // SspinMiniの判定をチェックする
        if (checkBlocklistS.FindAll(block => block == Existence.NotExist).Count <= 3 && _stepsSRS >= 1)
        {
            spinType = SpinTypes.SspinMini;
        }

        LogHelper.DebugLog(eClasses.SpinCheck, eMethod.SspinCheck, eLogTitle.End);
    }

    /// <summary> Tspinの判定をする関数 </summary>
    /// <remarks>
    /// Mini判定も確認する
    /// </remarks>
    private void TspinCheck(eMinoDirection _minoAngleAfter, int _stepsSRS)
    {
        LogHelper.DebugLog(eClasses.SpinCheck, eMethod.TspinCheck, eLogTitle.Start);

        // Tspinの判定をチェックする
        // 条件
        // ① 以下の図の指定されたマス(▫️ または ○)のうち、3つ以上がブロックまたは壁である。

        // TspinMiniの判定をチェックする
        // 条件
        // ① Tspinの条件を満たす。
        // ② SRSの段階が4以外である。
        // ③ 以下の図の ○ のどちらかがブロックまたは壁でないこと。

        // North    // East    // South    // West
        //
        //  ○▪️○         ▫️▪️○        ▫️ ▫️        ○▪️▫️
        //  ▪️◆▪️          ◆▪️        ▪️◆▪️        ▪️◆
        //  ▫️ ▫️         ▫️▪️○        ○▪️○        ○▪️▫️      // ◆はTミノの軸


        /// <summary> 指定マスのブロックの有無情報を格納するリスト </summary>
        /// <remarks>
        /// Tミノの中心から順番に[1, 1]、[1, -1]、[-1, 1]、[-1, -1]の分だけ移動した座標にブロックまたは壁がないか確認する <br/>
        /// ブロックまたは壁があった時は Exist ない時は Not Exist となる
        /// </remarks>
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

        logDetail = $"checkBlocklistT : {string.Join(", ", checkBlocklistT)}";
        LogHelper.InfoLog(eClasses.SpinCheck, eMethod.TspinCheck, eLogTitle.CheckBlockList, logDetail);

        if (checkBlocklistT.FindAll(block => block == Existence.Exist).Count >= 3) // 3マス以上ブロックまたは壁に埋まっている場合
        {
            if (_stepsSRS == 4) // SRSが4段階の時は T-Spin 判定になる
            {
                spinType = SpinTypes.Tspin;
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
                            spinType = SpinTypes.TspinMini;
                        }
                        else
                        {
                            spinType = SpinTypes.Tspin; // Tミノの底側のブロックが空白の時は T-Spin 判定になる
                        }

                        break;

                    case eMinoDirection.East: // 右上と右下
                        if (checkBlocklistT[right_up] == Existence.NotExist || checkBlocklistT[right_down] == Existence.NotExist)
                        {
                            spinType = SpinTypes.TspinMini;
                        }
                        else
                        {
                            spinType = SpinTypes.Tspin;
                        }

                        break;

                    case eMinoDirection.South: // 右下と左下
                        if (checkBlocklistT[right_down] == Existence.NotExist || checkBlocklistT[left_down] == Existence.NotExist)
                        {
                            spinType = SpinTypes.TspinMini;
                        }
                        else
                        {
                            spinType = SpinTypes.Tspin;
                        }

                        break;

                    case eMinoDirection.West: // 左上と左下
                        if (checkBlocklistT[left_up] == Existence.NotExist || checkBlocklistT[left_down] == Existence.NotExist)
                        {
                            spinType = SpinTypes.TspinMini;
                        }
                        else
                        {
                            spinType = SpinTypes.Tspin;
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
        // ① 以下の図の指定されたマス(▫️)のうち、ブロックまたは壁でないマスが2つ以内である。

        // ZspinMiniの判定をチェックする
        // 条件
        // ① 以下の図の指定されたマス(▫️)のうち、ブロックまたは壁でないマスが3つ以内である。
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
            spinType = SpinTypes.Zspin;
        }
        // ZspinMiniの判定をチェックする
        else if (checkBlocklistZ.FindAll(block => block == Existence.NotExist).Count <= 3 && _stepsSRS >= 1)
        {
            spinType = SpinTypes.ZspinMini;
        }

        LogHelper.DebugLog(eClasses.SpinCheck, eMethod.ZspinCheck, eLogTitle.End);
    }

    /// <summary> スピンタイプと列消去数から詳細なスピンタイプを決定する関数 </summary>
    /// <param name="_lineClearCount"> 消去ライン数 </param>
    /// <remarks>
    /// 再生するSEの決定も行う
    /// </remarks>
    /// <returns> 詳細なスピンタイプ(DetailedSpinTypes) </returns>
    public DetailedSpinTypes DetermineDetailedSpinType(int _lineClearCount)
    {
        LogHelper.DebugLog(eClasses.SpinCheck, eMethod.DetermineDetailedSpinType, eLogTitle.Start);

        /// <summary> 詳細なスピンタイプ </summary>
        DetailedSpinTypes detailedSpinType = DetailedSpinTypes.None;

        /// <summary> スピンタイプと消去ライン数に対応する詳細なスピンタイプをマッピングするディクショナリ </summary>
        Dictionary<SpinTypes, Dictionary<int, DetailedSpinTypes>> spinTypeAndLineClearCountToDetailedSpinTypesDictionary = new Dictionary<SpinTypes, Dictionary<int, DetailedSpinTypes>>
        {
            { SpinTypes.None, new Dictionary<int, DetailedSpinTypes>
                {
                    { 0, DetailedSpinTypes.None },
                    { 1, DetailedSpinTypes.None },
                    { 2, DetailedSpinTypes.None },
                    { 3, DetailedSpinTypes.None },
                    { 4, DetailedSpinTypes.Tetris }
                }
            },
            { SpinTypes.Ispin, new Dictionary<int, DetailedSpinTypes>
                {
                    { 0, DetailedSpinTypes.I_Spin },
                    { 1, DetailedSpinTypes.I_SpinSingle },
                    { 2, DetailedSpinTypes.I_SpinDouble },
                    { 3, DetailedSpinTypes.I_SpinTriple },
                    { 4, DetailedSpinTypes.I_SpinQuattro }
                }
            },
            { SpinTypes.IspinMini, new Dictionary<int, DetailedSpinTypes>
                {
                    { 0, DetailedSpinTypes.I_SpinMini },
                    { 1, DetailedSpinTypes.I_SpinMini }
                }
            },
            { SpinTypes.Jspin, new Dictionary<int, DetailedSpinTypes>
                {
                    { 0, DetailedSpinTypes.J_Spin },
                    { 1, DetailedSpinTypes.J_SpinSingle },
                    { 2, DetailedSpinTypes.J_SpinDouble },
                    { 3, DetailedSpinTypes.J_SpinTriple }
                }
            },
            { SpinTypes.JspinMini, new Dictionary<int, DetailedSpinTypes>
                {
                    { 0, DetailedSpinTypes.J_SpinMini },
                    { 1, DetailedSpinTypes.J_SpinMini },
                    { 2, DetailedSpinTypes.J_SpinDoubleMini }
                }
            },
            { SpinTypes.Lspin, new Dictionary<int, DetailedSpinTypes>
                {
                    { 0, DetailedSpinTypes.L_Spin },
                    { 1, DetailedSpinTypes.L_SpinSingle },
                    { 2, DetailedSpinTypes.L_SpinDouble },
                    { 3, DetailedSpinTypes.L_SpinTriple }
                }
            },
            { SpinTypes.LspinMini, new Dictionary<int, DetailedSpinTypes>
                {
                    { 0, DetailedSpinTypes.L_SpinMini },
                    { 1, DetailedSpinTypes.L_SpinMini },
                    { 2, DetailedSpinTypes.L_SpinDoubleMini }
                }
            },
            { SpinTypes.Sspin, new Dictionary<int, DetailedSpinTypes>
                {
                    { 0, DetailedSpinTypes.S_Spin },
                    { 1, DetailedSpinTypes.S_SpinMini },
                    { 2, DetailedSpinTypes.S_SpinDouble },
                    { 3, DetailedSpinTypes.S_SpinTriple }
                }
            },
            { SpinTypes.SspinMini, new Dictionary<int, DetailedSpinTypes>
                {
                    { 0, DetailedSpinTypes.S_SpinMini },
                    { 1, DetailedSpinTypes.S_SpinMini },
                    { 2, DetailedSpinTypes.S_SpinDoubleMini },
                }
            },
            { SpinTypes.Tspin, new Dictionary<int, DetailedSpinTypes>
                {
                    { 0, DetailedSpinTypes.T_Spin },
                    { 1, DetailedSpinTypes.T_SpinSingle },
                    { 2, DetailedSpinTypes.T_SpinDouble },
                    { 3, DetailedSpinTypes.T_SpinTriple }
                }
            },
            { SpinTypes.TspinMini, new Dictionary<int, DetailedSpinTypes>
                {
                    { 0, DetailedSpinTypes.T_SpinMini },
                    { 1, DetailedSpinTypes.T_SpinMini },
                    { 2, DetailedSpinTypes.T_SpinDoubleMini }
                }
            },
            { SpinTypes.Zspin, new Dictionary<int, DetailedSpinTypes>
                {
                    { 0, DetailedSpinTypes.Z_Spin },
                    { 1, DetailedSpinTypes.Z_SpinSingle },
                    { 2, DetailedSpinTypes.Z_SpinDouble },
                    { 3, DetailedSpinTypes.Z_SpinTriple }
                }
            },
            { SpinTypes.ZspinMini, new Dictionary<int, DetailedSpinTypes>
                {
                    { 0, DetailedSpinTypes.Z_SpinMini },
                    { 1, DetailedSpinTypes.Z_SpinMini },
                    { 2, DetailedSpinTypes.Z_SpinDoubleMini },
                }
            }
        };

        if (spinTypeAndLineClearCountToDetailedSpinTypesDictionary.ContainsKey(spinType) &&
            spinTypeAndLineClearCountToDetailedSpinTypesDictionary[spinType].ContainsKey(_lineClearCount))
        {
            detailedSpinType = spinTypeAndLineClearCountToDetailedSpinTypesDictionary[spinType][_lineClearCount];
        }
        else
        {
            LogHelper.ErrorLog(eClasses.SpinCheck, eMethod.DetermineDetailedSpinType, eLogTitle.KeyNotFound);
        }

        if (detailedSpinType != DetailedSpinTypes.None)
        {
            if (_lineClearCount >= 1)
            {
                AudioManager.Instance.PlaySound(eAudioName.SpinDestroy);
            }
            else
            {
                AudioManager.Instance.PlaySound(eAudioName.NormalDestroy);
            }
        }
        else
        {
            switch (_lineClearCount)
            {
                case 0:
                    AudioManager.Instance.PlaySound(eAudioName.NormalDrop);
                    break;
                case 1:
                case 2:
                case 3:
                    AudioManager.Instance.PlaySound(eAudioName.NormalDestroy);
                    break;
                case 4:
                    AudioManager.Instance.PlaySound(eAudioName.Tetris);
                    break;
            }
        }

        LogHelper.DebugLog(eClasses.SpinCheck, eMethod.DetermineDetailedSpinType, eLogTitle.End);
        return detailedSpinType;
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
//     // ①以下の図の指定されたマス(▫️)のうち3つ以上がブロックまたは壁である
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

// // ①と②の状態を文字列に変換
// string checkBlocksAboveInfo = string.Join(", ", checkBlocklistAboveI);
// string checkBlocksBelowInfo = string.Join(", ", checkBlocklistBelowI);

// LogHelper.Log(eLogLevel.Info, "SpinCheck", "IspinCheck()", checkBlocksAboveInfo);
// LogHelper.Log(eLogLevel.Info, "SpinCheck", "IspinCheck()", checkBlocksBelowInfo); // Infoログ

// // ①と②の状態を文字列に変換
// string checkBlocksRightSideInfo = string.Join(", ", checkBlocklistRightSideI);
// string checkBlocksLeftSideInfo = string.Join(", ", checkBlocklistLeftSideI);
// string checkBlocksUpperInfo = string.Join(", ", checkBlocklistUpperI);

// LogHelper.Log(eLogLevel.Info, "SpinCheck", "IspinCheck()", checkBlocksRightSideInfo);
// LogHelper.Log(eLogLevel.Info, "SpinCheck", "IspinCheck()", checkBlocksLeftSideInfo);
// LogHelper.Log(eLogLevel.Info, "SpinCheck", "IspinCheck()", checkBlocksUpperInfo); // Infoログ

// string checkBlocksInfo = string.Join(", ", checkBlocklistT);
// LogHelper.Log(eLogLevel.Info, "SpinCheck", "TspinCheck()", checkBlocksInfo); // Infoログ

/////////////////////////////////////////////////////////