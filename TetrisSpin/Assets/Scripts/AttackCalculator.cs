using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AttackCalculatorの統計情報を保持する静的クラス
/// </summary>
public static class AttackCalculatorStats
{
    /// <summary> BackToBackの判定 </summary>
    private static bool backToBack = false;

    /// <summary> PerfectClearの判定 </summary>
    /// <remarks>
    /// ゲーム開始時を除いて、ゲームプレイ中にブロックがなくなったらPerfectClear判定となる
    /// </remarks>
    private static bool perfectClear = false;

    /// <summary> Renの判定 </summary>
    /// <remarks>
    /// 3回連続で列消去すると「2REN」なので、初期値は-1に設定
    /// </remarks>
    /// <value> -1~ </value>
    private static int ren = -1;

    /// <summary> 攻撃ライン数 </summary>
    /// <remarks>
    /// ゲームスタート時に攻撃ラインは存在しないので、初期値は0
    /// </remarks>
    /// <value> 0~ </value>
    private static int sumAttackLines = 0;

    // ゲッタープロパティ //
    public static bool BackToBack => backToBack;
    public static bool PerfectClear => perfectClear;
    public static int Ren => ren;
    public static int SumAttackLines => sumAttackLines;

    /// <summary> スタッツログの詳細 </summary>
    private static string logStatsDetail;

    /// <summary> 指定されたフィールドの値を更新する関数 </summary>
    /// <param name="_backToBack"> BackToBackの判定 </param>
    /// <param name="_perfectClear"> PerfectClearの判定 </param>
    /// <param name="_ren"> Renの判定 </param>
    /// <param name="_sumAttackLines"> 攻撃ライン数 </param>
    /// <remarks>
    /// 指定されていない引数は現在の値を維持
    /// </remarks>
    public static void UpdateStats(bool? _backToBack = null, bool? _perfectClear = null, int? _ren = null, int? _sumAttackLines = null)
    {
        LogHelper.DebugLog(eClasses.AttackCalculator, eMethod.UpdateStats, eLogTitle.Start);

        backToBack = _backToBack ?? backToBack;
        perfectClear = _perfectClear ?? perfectClear;
        ren = _ren ?? ren;
        sumAttackLines = _sumAttackLines ?? sumAttackLines;

        logStatsDetail = $"backToBack : {backToBack}, perfectClear : {perfectClear}, ren : {ren}, sumAttackLines : {sumAttackLines}";
        LogHelper.InfoLog(eClasses.AttackCalculator, eMethod.UpdateStats, eLogTitle.StatsInfo, logStatsDetail);

        LogHelper.DebugLog(eClasses.AttackCalculator, eMethod.UpdateStats, eLogTitle.End);
    }

    /// <summary> デフォルトの <see cref="AttackCalculatorStats"/> にリセットする関数 </summary>
    public static void ResetStats()
    {
        LogHelper.DebugLog(eClasses.AttackCalculator, eMethod.ResetStats, eLogTitle.Start);

        backToBack = false;
        perfectClear = false;
        ren = -1;
        sumAttackLines = 0;

        LogHelper.DebugLog(eClasses.AttackCalculator, eMethod.ResetStats, eLogTitle.End);
    }
}

/// <summary>
/// 攻撃値の計算を行うクラス
/// </summary>
public class AttackCalculator : MonoBehaviour
{
    /// <summary>
    /// スピンタイプと消去ライン数に対応する攻撃力をマッピングするディクショナリ
    /// </summary>
    private Dictionary<SpinTypeNames, Dictionary<int, int>> spinTypeAttackMapping = new Dictionary<SpinTypeNames, Dictionary<int, int>>
    {
        { SpinTypeNames.Ispin, new Dictionary<int, int>
            {
                { 0, 0 },
                { 1, 2 }, // IspinSingleAttack
                { 2, 4 }, // IspinDoubleAttack
                { 3, 6 }, // IspinTripleAttack
                { 4, 8 }  // IspinQuattroAttack
            }
        },
        { SpinTypeNames.IspinMini, new Dictionary<int, int>
            {
                { 0, 0 },
                { 1, 1 } // IspinMiniAttack
            }
        },
        { SpinTypeNames.Jspin, new Dictionary<int, int>
            {
                { 0, 0 },
                { 1, 1 }, // JspinSingleAttack
                { 2, 3 }, // JspinDoubleAttack
                { 3, 6 }  // JspinTripleAttack
            }
        },
        { SpinTypeNames.Lspin, new Dictionary<int, int>
            {
                { 0, 0 },
                { 1, 1 }, // LspinSingleAttack
                { 2, 3 }, // LspinDoubleAttack
                { 3, 6 }  // LspinTripleAttack
            }
        },
        { SpinTypeNames.Sspin, new Dictionary<int, int>
            {
                { 0, 0 },
                { 1, 2 }, // SspinSingleAttack
                { 2, 4 }, // SspinDoubleAttack
                { 3, 8 }  // SspinTripleAttack
            }
        },
        { SpinTypeNames.SspinMini, new Dictionary<int, int>
            {
                { 0, 0 },
                { 1, 0 }, // SspinMiniAttack
                { 2, 1 }, // SspinDoubleMiniAttack
                { 3, 3 }  // SspinTripleMiniAttack
            }
        },
        { SpinTypeNames.Tspin, new Dictionary<int, int>
            {
                { 0, 0 },
                { 1, 2 }, // TspinSingleAttack
                { 2, 4 }, // TspinDoubleAttack
                { 3, 6 }  // TspinTripleAttack
            }
        },
        { SpinTypeNames.TspinMini, new Dictionary<int, int>
            {
                { 0, 0 },
                { 1, 0 }, // TspinMiniAttack
                { 2, 1 }  // TspinDoubleMiniAttack
            }
        },
        { SpinTypeNames.Zspin, new Dictionary<int, int>
            {
                { 0, 0 },
                { 1, 2 }, // ZspinSingleAttack
                { 2, 4 }, // ZspinDoubleAttack
                { 3, 8 }  // ZspinTripleAttack
            }
        },
        { SpinTypeNames.ZspinMini, new Dictionary<int, int>
            {
                { 0, 0 },
                { 1, 0 }, // ZspinMiniAttack
                { 2, 1 }, // ZspinDoubleMiniAttack
                { 3, 3 }  // ZspinTripleMiniAttack
            }
        },
        { SpinTypeNames.None, new Dictionary<int, int>
            {
                { 0, 0 },
                { 1, 0 }, // OneLineClearAttack
                { 2, 1 }, // TwoLineClearAttack
                { 3, 2 }, // ThreeLineClearAttack
                { 4, 4 }  // TetrisAttack
            }
        }
    };

    /// <summary> ログの詳細 </summary>
    private string logDetail;

    // 干渉するクラス //
    Board board;
    DisplayManager displayManager;

    /// <summary>
    /// インスタンス化
    /// </summary>
    private void Awake()
    {
        board = FindObjectOfType<Board>();
        displayManager = FindObjectOfType<DisplayManager>();
    }

    /// <summary> 合計攻撃力を計算する関数 </summary>
    /// <param name="_spinType"> スピンタイプ </param>
    /// <param name="_lineClearCount"> 消去ライン数 </param>
    public void CalculateSumAttackLines(SpinTypeNames _spinType, int _lineClearCount)
    {
        LogHelper.DebugLog(eClasses.AttackCalculator, eMethod.CalculateSumAttackLines, eLogTitle.Start);

        // 今回の攻撃値
        int attackLines = 0;

        attackLines += spinTypeAttackMapping[_spinType][_lineClearCount];
        attackLines += CalculateBackToBack(_spinType, _lineClearCount);
        attackLines += CalculatePerfectClear();
        attackLines += CalculateRen(_lineClearCount);

        AttackCalculatorStats.UpdateStats(_sumAttackLines: AttackCalculatorStats.SumAttackLines + attackLines);

        // 100を超えたら100に戻す
        if (AttackCalculatorStats.SumAttackLines > 100)
        {
            AttackCalculatorStats.UpdateStats(_sumAttackLines: 100);
        }

        if (attackLines > 0)
        {
            displayManager.DisplayAttackLines(attackLines);
            displayManager.DisplaySumAttackLines();
        }

        logDetail = $"{AttackCalculatorStats.SumAttackLines}";
        LogHelper.InfoLog(eClasses.AttackCalculator, eMethod.CalculateSumAttackLines, eLogTitle.SumAttackLinesValue, logDetail);

        LogHelper.DebugLog(eClasses.AttackCalculator, eMethod.CalculateSumAttackLines, eLogTitle.End);
    }

    /// <summary> BackToBackの判定と攻撃力ボーナスを計算する関数 </summary>
    /// <param name="_spinType"> スピンタイプ </param>
    /// <param name="_lineClearCount"> 消去ライン数 </param>
    /// <returns> 攻撃力ボーナスの値(bonus) </returns>
    private int CalculateBackToBack(SpinTypeNames _spinType, int _lineClearCount)
    {
        LogHelper.DebugLog(eClasses.AttackCalculator, eMethod.CalculateBackToBack, eLogTitle.Start);

        /// <summary> 攻撃力ボーナス </summary>
        int bonus = 0;

        /// <summary> BackToBackの攻撃力ボーナス </summary>
        int backToBackBonus = 1;

        if (AttackCalculatorStats.BackToBack == true &&
            (_lineClearCount == 4 || (_spinType != SpinTypeNames.None && _lineClearCount >= 1)))
        {
            bonus = backToBackBonus;
            displayManager.BackToBackAnimation();
        }

        if (_lineClearCount == 4 || _spinType != SpinTypeNames.None)
        {
            AttackCalculatorStats.UpdateStats(_backToBack: true);
        }
        else
        {
            if (_lineClearCount >= 1)
            {
                AttackCalculatorStats.UpdateStats(_backToBack: false);
            }
        }

        LogHelper.DebugLog(eClasses.AttackCalculator, eMethod.CalculateBackToBack, eLogTitle.End);
        return bonus;
    }

    /// <summary> PerfectClearの判定と攻撃力ボーナスを計算する関数 </summary>
    /// <returns> 攻撃力ボーナスの値(bonus) </returns>
    private int CalculatePerfectClear()
    {
        LogHelper.DebugLog(eClasses.AttackCalculator, eMethod.CalculatePerfectClear, eLogTitle.Start);

        /// <summary> 攻撃力ボーナス </summary>
        int bonus = 0;

        /// <summary> PerfectClearの攻撃力ボーナス </summary>
        int perfectClearBonus = 10;

        if (board.CheckPerfectClear())
        {
            AttackCalculatorStats.UpdateStats(_perfectClear: true);
            bonus = perfectClearBonus;
            displayManager.PerfectClearAnimation();
        }
        else
        {
            AttackCalculatorStats.UpdateStats(_perfectClear: false);
        }

        LogHelper.DebugLog(eClasses.AttackCalculator, eMethod.CalculatePerfectClear, eLogTitle.End);
        return bonus;
    }

    /// <summary> Renの判定と攻撃力ボーナスを計算する関数 </summary>
    /// <param name="_lineClearCount"> 消去ライン数 </param>
    /// <returns> 攻撃力ボーナスの値(bonus) </returns>
    private int CalculateRen(int _lineClearCount)
    {
        LogHelper.DebugLog(eClasses.AttackCalculator, eMethod.CalculateRen, eLogTitle.Start);

        /// <summary> 攻撃力ボーナス </summary>
        int bonus = 0;

        /// <summary> RENの攻撃力ボーナス </summary>
        int[] renBonus = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 4, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5 };

        if (_lineClearCount >= 1)
        {
            AttackCalculatorStats.UpdateStats(_ren: AttackCalculatorStats.Ren + 1);
            bonus = renBonus[AttackCalculatorStats.Ren];

            // 2REN以上で表示する
            if (AttackCalculatorStats.Ren >= 2)
            {
                displayManager.DisplayRen();
            }
        }
        else
        {
            AttackCalculatorStats.UpdateStats(_ren: -1);
            displayManager.ResetDisplayRen();
        }

        LogHelper.DebugLog(eClasses.AttackCalculator, eMethod.CalculateRen, eLogTitle.End);
        return bonus;
    }
}

/////////////////// 旧コード ///////////////////

// /// <summary>
// /// AttackCalculatorの統計情報を保持する構造体
// /// </summary>
// public struct AttackCalculatorStats
// {
//     /// <summary> BackToBackの判定 </summary>
//     private bool backToBack;

//     /// <summary> PerfectClearの判定 </summary>
//     /// <remarks>
//     /// ゲーム開始時を除いて、ゲームプレイ中にブロックがなくなったらPerfectClear判定となる
//     /// </remarks>
//     private bool perfectClear;

//     /// <summary> Renの判定 </summary>
//     /// <remarks>
//     /// 3回連続で列消去すると「2REN」なので、初期値は-1に設定
//     /// </remarks>
//     /// <value> -1~ </value>
//     private int ren;

//     /// <summary> 攻撃ライン数 </summary>
//     /// <remarks>
//     /// ゲームスタート時に攻撃ラインは存在しないので、初期値は0
//     /// </remarks>
//     /// <value> 0~ </value>
//     [SerializeField] private int sumAttackLines;

//     // ゲッタープロパティ //
//     public bool BackToBack => backToBack;
//     public bool PerfectClear => perfectClear;
//     public int Ren => ren;
//     public int sumAttackLines => sumAttackLines;

//     /// <summary> デフォルトコンストラクタ </summary>
//     public AttackCalculatorStats(bool _backToBack, bool _perfectClear, int _ren, int _sumAttackLines)
//     {
//         backToBack = _backToBack;
//         perfectClear = _perfectClear;
//         ren = _ren;
//         sumAttackLines = _sumAttackLines;
//     }

//     /// <summary> デフォルトの <see cref="AttackCalculatorStats"/> を作成する関数 </summary>
//     /// <returns>
//     /// デフォルト値で初期化された <see cref="AttackCalculatorStats"/> のインスタンス
//     /// </returns>
//     public static AttackCalculatorStats CreateDefault()
//     {
//         return new AttackCalculatorStats
//         {
//             backToBack = false,
//             perfectClear = false,
//             ren = -1,
//             sumAttackLines = 0
//         };
//     }

//     /// <summary> 指定されたフィールドの値を更新する関数 </summary>
//     /// <param name="_backToBack"> BackToBackの判定 </param>
//     /// <param name="_perfectClear"> PerfectClearの判定 </param>
//     /// <param name="_ren"> Renの判定 </param>
//     /// <param name="_sumAttackLines"> 攻撃ライン数 </param>
//     /// <returns> 更新された <see cref="AttackCalculatorStats"/> の新しいインスタンス </returns>
//     /// <remarks>
//     /// 指定されていない引数は現在の値を維持
//     /// </remarks>
//     public AttackCalculatorStats Update(bool? _backToBack = null, bool? _perfectClear = null, int? _ren = null, int? _sumAttackLines = null)
//     {
//         var updatedStats = new AttackCalculatorStats(
//             _backToBack ?? backToBack,
//             _perfectClear ?? perfectClear,
//             _ren ?? ren,
//             _sumAttackLines ?? sumAttackLines
//         );
//         return updatedStats;
//     }
// }

/////////////////////////////////////////////////////////