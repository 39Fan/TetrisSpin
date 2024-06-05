using UnityEngine;

/// <summary>
/// タイマーを管理する静的クラス
/// </summary>
public static class Timer
{
    /// <summary> 次の左右キー入力までの時間 </summary>
    /// <remarks> 左右キーが押されたときに更新される </remarks>
    private static float nextKeyLeftRightTimer;
    /// <summary> 次の回転キー入力までの時間 </summary>
    /// <remarks> 回転キーが押されたときに更新される </remarks>
    private static float nextKeyRotateTimer;
    /// <summary> 次の下キー入力までの時間 </summary>
    /// <remarks> 下キーが押されたときに更新される </remarks>
    private static float nextKeyDownTimer;
    /// <summary> 次の自動落下までの時間 </summary>
    private static float autoDropTimer;
    /// <summary> 次のロックダウンまでの時間 </summary>
    private static float bottomTimer;

    /// <summary> 通常の左右キー入力のインターバル </summary>
    private static float nextKeyLeftRightInterval_Normal = 0.20f;
    /// <summary> 連続左右キー入力のインターバル </summary>
    /// <remarks> 連続左右入力用 </remarks>
    private static float nextKeyLeftRightInterval_Short = 0.05f;
    /// <summary> 回転キー入力のインターバル </summary>
    private static float nextKeyRotateInterval = 0.05f;
    /// <summary> 下キー入力のインターバル </summary>
    private static float nextKeyDownInterval = 0.05f;
    /// <summary> 自動落下のインターバル </summary>
    private static float autoDropInterval = 1f;
    /// <summary> ロックダウンのインターバル </summary>
    private static float bottomTimerInterval = 1f;

    /// <summary> 連続左右入力が行われているかどうか </summary>
    private static bool continuousLRKey = false;

    // ゲッタープロパティ
    public static float NextKeyLeftRightTimer => nextKeyLeftRightTimer;
    public static float NextKeyRotateTimer => nextKeyRotateTimer;
    public static float NextKeyDownTimer => nextKeyDownTimer;
    public static float AutoDropTimer => autoDropTimer;
    public static float BottomTimer => bottomTimer;
    public static bool ContinuousLRKey
    {
        get => continuousLRKey;
        set => continuousLRKey = value;
    }

    /// <summary>
    /// タイマーを初期化する関数
    /// </summary>
    /// <remarks>
    /// ゲームスタート時とミノが設置された時に使用される
    /// </remarks>
    public static void Reset()
    {
        nextKeyDownTimer = Time.time;
        nextKeyLeftRightTimer = Time.time;
        nextKeyRotateTimer = Time.time;
        autoDropTimer = Time.time + autoDropInterval;
        bottomTimer = Time.time + bottomTimerInterval;
    }

    /// <summary>
    /// 右入力または左入力のタイマーを更新する関数
    /// </summary>
    public static void UpdateLeftRightTimer()
    {
        if (continuousLRKey)
        {
            nextKeyLeftRightTimer = Time.time + nextKeyLeftRightInterval_Short;
        }
        else
        {
            nextKeyLeftRightTimer = Time.time + nextKeyLeftRightInterval_Normal;
        }

        bottomTimer = Time.time + bottomTimerInterval;
    }

    /// <summary>
    /// 回転入力のタイマーを更新する関数
    /// </summary>
    public static void UpdateRotateTimer()
    {
        nextKeyRotateTimer = Time.time + nextKeyRotateInterval;
        bottomTimer = Time.time + bottomTimerInterval;
    }

    /// <summary>
    /// 下入力および自動落下のタイマーを更新する関数
    /// </summary>
    public static void UpdateDownTimer()
    {
        nextKeyDownTimer = Time.time + nextKeyDownInterval;
        autoDropTimer = Time.time + autoDropInterval;
        bottomTimer = Time.time + bottomTimerInterval;
    }
}


/////////////////// 旧コード ///////////////////

// /// <summary>
// /// インスタンス化
// /// </summary>
// private void Awake()
// {
//     nextKeyLeftRightInterval_Normal = 0.20f;
//     nextKeyLeftRightInterval_Short = 0.05f;
//     nextKeyRotateInterval = 0.05f;
//     nextKeyDownInterval = 0.05f;
//     autoDropInterval = 1f;
//     bottomTimerInterval = 1f;
//     continuousLRKey = false;
// }

// using UnityEngine;

// ///// タイマーに関するスクリプト /////


// // ↓このスクリプトで可能なこと↓ //

// // キー入力の受付時間の管理

// public class Timer : MonoBehaviour
// {
//     // 入力受付タイマー(3種類)
//     public float NextKeyLeftRightTimer { get; private set; }
//     public float NextKeyRotateTimer { get; private set; }
//     public float NextKeyDownTimer { get; private set; }

//     // 入力インターバル(4種類)
//     public float NextKeyLeftRightInterval_Normal { get; private set; } = 0.20f;
//     public float NextKeyLeftRightInterval_Short { get; private set; } = 0.05f; // 連続左右入力用
//     public float NextKeyRotateInterval { get; private set; } = 0.05f;
//     public float NextKeyDownInterval { get; private set; } = 0.05f;

//     // 自動でミノが落ちるまでの時間とそのインターバル
//     public float AutoDropTimer { get; private set; }
//     public float AutoDropInteaval { get; private set; } = 1f;

//     // 連続右入力、または連続左入力の判定
//     public bool ContinuousLRKey { get; set; } = false;

//     // ロックダウン //
//     public float BottomTimer { get; private set; }
//     public float BottomTimerInterval { get; private set; } = 1f;


//     // ゲームスタート時と、ミノが設置された時のタイマーを初期化する関数 //
//     public void ResetTimer()
//     {
//         NextKeyDownTimer = Time.time;
//         NextKeyLeftRightTimer = Time.time;
//         NextKeyRotateTimer = Time.time;
//         AutoDropTimer = Time.time + AutoDropInteaval;
//         BottomTimer = Time.time + BottomTimerInterval;
//     }

//     // 右入力、または左入力のタイマーを更新する関数 //
//     public void UpdateLeftRightTimer()
//     {
//         if (ContinuousLRKey == true) // 連続入力なら
//         {
//             NextKeyLeftRightTimer = Time.time + NextKeyLeftRightInterval_Short;
//         }
//         else
//         {
//             NextKeyLeftRightTimer = Time.time + NextKeyLeftRightInterval_Normal;
//         }

//         BottomTimer = Time.time + BottomTimerInterval;
//     }

//     // 回転入力のタイマーを更新する関数 //
//     public void UpdateRotateTimer()
//     {
//         NextKeyRotateTimer = Time.time + NextKeyRotateInterval;

//         BottomTimer = Time.time + BottomTimerInterval;
//     }

//     // 下入力、自動落下のタイマーの更新をする関数 //
//     public void UpdateDownTimer()
//     {
//         NextKeyDownTimer = Time.time + NextKeyDownInterval;

//         AutoDropTimer = Time.time + AutoDropInteaval;

//         BottomTimer = Time.time + BottomTimerInterval;
//     }
// }

// /// <summary>
// /// タイマーを管理するクラス
// /// </summary>
// public class Timer : MonoBehaviour
// {
//     /// <summary> 次の左右キー入力までの時間 </summary>
//     /// <remarks> 左右キーが押されたときに更新される </remarks>
//     private float nextKeyLeftRightTimer;
//     /// <summary> 次の回転キー入力までの時間 </summary>
//     /// <remarks> 回転キーが押されたときに更新される </remarks>
//     private float nextKeyRotateTimer;
//     /// <summary> 次の下キー入力までの時間 </summary>
//     /// <remarks> 下キーが押されたときに更新される </remarks>
//     private float nextKeyDownTimer;
//     /// <summary> 次の自動落下までの時間 </summary>
//     private float autoDropTimer;
//     /// <summary> 次のロックダウンまでの時間 </summary>
//     private float bottomTimer;

//     /// <summary> 通常の左右キー入力のインターバル </summary>
//     private float nextKeyLeftRightInterval_Normal = 0.20f;
//     /// <summary> 連続左右キー入力のインターバル </summary>
//     /// <remarks> 連続左右入力用 </remarks>
//     private float nextKeyLeftRightInterval_Short = 0.05f;
//     /// <summary> 回転キー入力のインターバル </summary>
//     private float nextKeyRotateInterval = 0.05f;
//     /// <summary> 下キー入力のインターバル </summary>
//     private float nextKeyDownInterval = 0.05f;
//     /// <summary> 自動落下のインターバル </summary>
//     private float autoDropInterval = 1f;
//     /// <summary> ロックダウンのインターバル </summary>
//     private float bottomTimerInterval = 1f;

//     /// <summary> 連続左右入力が行われているかどうか </summary>
//     private bool continuousLRKey = false;

//     // ゲッタープロパティ
//     public float NextKeyLeftRightTimer => nextKeyLeftRightTimer;
//     public float NextKeyRotateTimer => nextKeyRotateTimer;
//     public float NextKeyDownTimer => nextKeyDownTimer;
//     public float AutoDropTimer => autoDropTimer;
//     public float BottomTimer => bottomTimer;
//     public bool ContinuousLRKey
//     {
//         get => continuousLRKey;
//         set => continuousLRKey = value;
//     }

//     /// <summary>
//     /// タイマーを初期化する関数
//     /// </summary>
//     /// <remarks>
//     /// ゲームスタート時とミノが設置された時に使用される
//     /// </remarks>
//     public void ResetTimer()
//     {
//         nextKeyDownTimer = Time.time;
//         nextKeyLeftRightTimer = Time.time;
//         nextKeyRotateTimer = Time.time;
//         autoDropTimer = Time.time + autoDropInterval;
//         bottomTimer = Time.time + bottomTimerInterval;
//     }

//     /// <summary>
//     /// 右入力または左入力のタイマーを更新する関数
//     /// </summary>
//     public void UpdateLeftRightTimer()
//     {
//         if (continuousLRKey)
//         {
//             nextKeyLeftRightTimer = Time.time + nextKeyLeftRightInterval_Short;
//         }
//         else
//         {
//             nextKeyLeftRightTimer = Time.time + nextKeyLeftRightInterval_Normal;
//         }

//         bottomTimer = Time.time + bottomTimerInterval;
//     }

//     /// <summary>
//     /// 回転入力のタイマーを更新する関数
//     /// </summary>
//     public void UpdateRotateTimer()
//     {
//         nextKeyRotateTimer = Time.time + nextKeyRotateInterval;
//         bottomTimer = Time.time + bottomTimerInterval;
//     }

//     /// <summary>
//     /// 下入力および自動落下のタイマーを更新する関数
//     /// </summary>
//     public void UpdateDownTimer()
//     {
//         nextKeyDownTimer = Time.time + nextKeyDownInterval;
//         autoDropTimer = Time.time + autoDropInterval;
//         bottomTimer = Time.time + bottomTimerInterval;
//     }
// }

/////////////////////////////////////////////////////////