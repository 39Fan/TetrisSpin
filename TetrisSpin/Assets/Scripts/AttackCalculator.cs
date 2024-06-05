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
    private static int attackLines = 0;

    // ゲッタープロパティ //
    public static bool BackToBack => backToBack;
    public static bool PerfectClear => perfectClear;
    public static int Ren => ren;
    public static int AttackLines => attackLines;

    /// <summary> 指定されたフィールドの値を更新する関数 </summary>
    /// <param name="_backToBack"> BackToBackの判定 </param>
    /// <param name="_perfectClear"> PerfectClearの判定 </param>
    /// <param name="_ren"> Renの判定 </param>
    /// <param name="_attackLines"> 攻撃ライン数 </param>
    /// <remarks>
    /// 指定されていない引数は現在の値を維持
    /// </remarks>
    public static void Update(bool? _backToBack = null, bool? _perfectClear = null, int? _ren = null, int? _attackLines = null)
    {
        backToBack = _backToBack ?? backToBack;
        perfectClear = _perfectClear ?? perfectClear;
        ren = _ren ?? ren;
        attackLines = _attackLines ?? attackLines;
        // TODO: ログの記入
    }

    /// <summary> デフォルトの <see cref="AttackCalculatorStats"/> にリセットする関数 </summary>
    public static void Reset()
    {
        backToBack = false;
        perfectClear = false;
        ren = -1;
        attackLines = 0;
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
        { SpinTypeNames.J_Spin, new Dictionary<int, int>
            {
                { 0, 0 },
                { 1, 1 }, // JspinSingleAttack
                { 2, 3 }, // JspinDoubleAttack
                { 3, 6 }  // JspinTripleAttack
            }
        },
        { SpinTypeNames.L_Spin, new Dictionary<int, int>
            {
                { 0, 0 },
                { 1, 1 }, // LspinSingleAttack
                { 2, 3 }, // LspinDoubleAttack
                { 3, 6 }  // LspinTripleAttack
            }
        },
        { SpinTypeNames.I_SpinMini, new Dictionary<int, int>
            {
                { 0, 0 },
                { 1, 1 } // IspinMiniAttack
            }
        },
        { SpinTypeNames.I_Spin, new Dictionary<int, int>
            {
                { 0, 0 },
                { 1, 2 }, // IspinSingleAttack
                { 2, 4 }, // IspinDoubleAttack
                { 3, 6 }, // IspinTripleAttack
                { 4, 8 }  // IspinQuattroAttack
            }
        },
        { SpinTypeNames.T_Spin, new Dictionary<int, int>
            {
                { 0, 0 },
                { 1, 2 }, // TspinSingleAttack
                { 2, 4 }, // TspinDoubleAttack
                { 3, 6 }  // TspinTripleAttack
            }
        },
        { SpinTypeNames.T_SpinMini, new Dictionary<int, int>
            {
                { 0, 0 },
                { 1, 0 }, // TspinMiniAttack
                { 2, 1 }  // TspinDoubleMiniAttack
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

    // 干渉するスクリプト //
    Board board;
    TextEffect textEffect;

    /// <summary>
    /// AttackCalculatorStatsの初期化を行う
    /// </summary>
    private void Awake()
    {
        board = FindObjectOfType<Board>();
        textEffect = FindObjectOfType<TextEffect>();
    }

    /// <summary> 合計攻撃力を計算する関数 </summary>
    /// <param name="_spinType"> スピンタイプ </param>
    /// <param name="_lineClearCount"> 消去ライン数 </param>
    public void CalculateAttackLines(SpinTypeNames _spinType, int _lineClearCount)
    {
        // Spin判定と消去ライン数に対応した攻撃力を加算する
        AttackCalculatorStats.Update(_attackLines: AttackCalculatorStats.AttackLines + spinTypeAttackMapping[_spinType][_lineClearCount]);

        CalculateBackToBack(_spinType, _lineClearCount);

        CalculatePerfectClear();

        CalculateRen(_lineClearCount);

        Debug.Log(AttackCalculatorStats.AttackLines);
    }

    /// <summary> BackToBackの判定と攻撃力ボーナスを計算する関数 </summary>
    /// <param name="_spinType"> スピンタイプ </param>
    /// <param name="_lineClearCount"> 消去ライン数 </param>
    private void CalculateBackToBack(SpinTypeNames _spinType, int _lineClearCount)
    {
        /// <summary> BackToBackの攻撃力ボーナス </summary>
        int backToBackBonus = 1;

        if (AttackCalculatorStats.BackToBack == true &&
            (_lineClearCount == 4 || (_spinType != SpinTypeNames.None && _lineClearCount >= 1)))
        {
            AttackCalculatorStats.Update(_attackLines: AttackCalculatorStats.AttackLines + backToBackBonus);

            textEffect.BackToBackAnimation();
        }

        if (_lineClearCount == 4 || _spinType != SpinTypeNames.None)
        {
            AttackCalculatorStats.Update(_backToBack: true);
        }
        else
        {
            if (_lineClearCount >= 1)
            {
                AttackCalculatorStats.Update(_backToBack: false);
            }
        }
    }

    /// <summary> PerfectClearの判定と攻撃力ボーナスを計算する関数 </summary>
    private void CalculatePerfectClear()
    {
        /// <summary> PerfectClearの攻撃力ボーナス </summary>
        int perfectClearBonus = 10;

        if (board.CheckPerfectClear())
        {
            AttackCalculatorStats.Update(_perfectClear: true);
            AttackCalculatorStats.Update(_attackLines: AttackCalculatorStats.AttackLines + perfectClearBonus);

            textEffect.PerfectClearAnimation();
        }
        else
        {
            AttackCalculatorStats.Update(_perfectClear: false);
        }
    }

    /// <summary> Renの判定と攻撃力ボーナスを計算する関数 </summary>
    /// <param name="_lineClearCount"> 消去ライン数 </param>
    private void CalculateRen(int _lineClearCount)
    {
        /// <summary> RENの攻撃力ボーナス </summary>
        int[] renBonus = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 4, 5 };

        if (_lineClearCount >= 1)
        {
            AttackCalculatorStats.Update(_ren: AttackCalculatorStats.Ren + 1);
            AttackCalculatorStats.Update(_attackLines:
                AttackCalculatorStats.AttackLines + renBonus[AttackCalculatorStats.Ren]);

            // TODO TextEffectでRENのテキスト表示をする
        }
        else
        {
            AttackCalculatorStats.Update(_ren: -1);
        }
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
//     [SerializeField] private int attackLines;

//     // ゲッタープロパティ //
//     public bool BackToBack => backToBack;
//     public bool PerfectClear => perfectClear;
//     public int Ren => ren;
//     public int AttackLines => attackLines;

//     /// <summary> デフォルトコンストラクタ </summary>
//     public AttackCalculatorStats(bool _backToBack, bool _perfectClear, int _ren, int _attackLines)
//     {
//         backToBack = _backToBack;
//         perfectClear = _perfectClear;
//         ren = _ren;
//         attackLines = _attackLines;
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
//             attackLines = 0
//         };
//     }

//     /// <summary> 指定されたフィールドの値を更新する関数 </summary>
//     /// <param name="_backToBack"> BackToBackの判定 </param>
//     /// <param name="_perfectClear"> PerfectClearの判定 </param>
//     /// <param name="_ren"> Renの判定 </param>
//     /// <param name="_attackLines"> 攻撃ライン数 </param>
//     /// <returns> 更新された <see cref="AttackCalculatorStats"/> の新しいインスタンス </returns>
//     /// <remarks>
//     /// 指定されていない引数は現在の値を維持
//     /// </remarks>
//     public AttackCalculatorStats Update(bool? _backToBack = null, bool? _perfectClear = null, int? _ren = null, int? _attackLines = null)
//     {
//         var updatedStats = new AttackCalculatorStats(
//             _backToBack ?? backToBack,
//             _perfectClear ?? perfectClear,
//             _ren ?? ren,
//             _attackLines ?? attackLines
//         );
//         // TODO: ログの記入
//         return updatedStats;
//     }
// }

/////////////////////////////////////////////////////////