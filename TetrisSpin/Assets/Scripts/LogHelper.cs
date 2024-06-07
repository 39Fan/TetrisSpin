using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ログレベル 列挙型
/// </summary>
public enum eLogLevel
{
    Debug,
    Info,
    Warning,
    Error
}

/// <summary>
/// ログタイトル 列挙型
/// </summary>
public enum eLogTitle
{
    // 共通タイトル //
    Start, End, KeyNotFound, UpdateFunctionRunning,
    // 各種 Stats クラス //
    StatsInfo,
    // AttackCalculator クラス //
    AttackLinesValue,
    // AudioManager クラス //
    MismatchBetweenAudioAndAudioNameCount, AudioNameAlreadyExists,
    // BoardStats クラス //

    // Board クラス //
    LineClearCountValue,
    // GameAutoRunner クラス //

    // LogHelper クラス //

    // MinoMovement クラス //
    SRSInfo, InvalidMinosPositionDetected,
    // PlayerInput クラス //

    // SceneTransition クラス //

    // SpawnerStats クラス //

    // Spawner クラス //

    // SpinCheck クラス //
    MinosIdentificationFailed,
    // TextEffect クラス //

    // Timer クラス //
}

// /// <summary>
// /// スクリプト 列挙型
// /// </summary>
// public enum eScripts
// {
//     AttackCalculator,
//     AudioManager,
//     Board,
//     GameAutoRunner,
//     GameManager,
//     LogHelper,
//     MinoMovement,
//     PlayerInput,
//     Rounding,
//     SceneTransition,
//     Spawner,
//     SpinCheck,
//     TextEffect,
//     Timer
// }

/// <summary>
/// クラス 列挙型
/// </summary>
public enum eClasses
{
    AttackCalculator,
    AttackCalculatorStats,
    AudioManager,
    Board,
    BoardStats,
    GameAutoRunner,
    GameAutoRunnerStats,
    GameManager,
    GameManagerStats,
    LogHelper,
    MinoMovement,
    MinoMovementStats,
    PlayerInput,
    PlayerInputStats,
    Rounding,
    SceneTransition,
    Spawner,
    SpawnerStats,
    SpinCheck,
    TextEffect,
    Timer
}

/// <summary>
/// 関数 列挙型
/// </summary>
public enum eMethod
{
    // 共通関数 //
    Awake, Start, Update,
    // 各種 Stats クラス //
    UpdateStats, ResetStats,
    // AttackCalculator クラス //
    CalculateAttackLines, CalculateBackToBack, CalculatePerfectClear, CalculateRen,
    // AudioManager クラス //
    PlaySound, SetVolume, BuildAudioClipDictionary,
    // BoardStats クラス //
    AddLineClearCountHistory,
    // Board クラス //
    CreateBoard, CheckPosition, IsWithinBoard, CheckMinoCollision, SaveBlockInGrid, CheckAllRows, IsComplete, ClearRow, ShiftRowsDown,
    CheckPerfectClear, CheckGrid, CheckActiveMinoTopBlockPositionY, CheckActiveMinoBottomBlockPositionY, CheckGameOver,
    // GameAutoRunner クラス //
    RockDown, ResetRockDown, AutoDown, SetMinoFixed,
    // LogHelper クラス //
    Log, DebugLog, InfoLog, WarningLog, ErrorLog, FormatLogMessage, FormatInfoLogMessage, GetColorByLevel,
    // MinoMovement クラス //
    Move, MoveLeft, MoveRight, MoveUp, MoveDown, RotateRight, RotateLeft, AxisCheckForI,
    UpdateMinoAngleAfter, UpdateMinoAngleAfterToMinoAngleBefore, UpdateMinoAngleBeforeToMinoAngleAfter,
    ResetRotate, ResetAngle, ResetStepsSRS, SuperRotationSystem, TrySuperRotation, GetMinoAngleAfter, GetMinoAngleBefore, GetStepsSRS,
    // PlayerInput クラス //
    InputInGame, MoveRightInput, ContinuousMoveRightInput, MoveLeftInput, ContinuousMoveLeftInput,
    ReleaseContinuousMoveRightLeftInput, MoveDownInput, RotateRightInput, RotateLeftInput,
    HardDropInput, HoldInput, SuccessRotateAction, IncreaseBottomMoveCount, ConfirmMinoMovement,
    // SceneTransition クラス //
    SelectStartButton, SelectRetry, SelectMenu, GameOver,
    // SpawnerStats クラス //
    AddSpawnMinoOrder,
    // Spawner クラス //
    DetermineSpawnMinoOrder, CheckActiveMinoToBaseDistance, AdjustGhostMinoPosition,
    CreateNewActiveMino, CreateNextMinos, CreateHoldMino, SpawnActiveMino, SpawnGhostMino, SpawnNextMino, SpawnHoldMino,
    // SpinCheck クラス //
    CheckSpinType, ResetSpinTypeName, IspinCheck, JspinCheck, LspinCheck, SspinCheck, TspinCheck, ZspinCheck,
    // TextEffect クラス //
    TextDisplay, DetermineTextToDisplay, TextAnimation, TextFadeInAndOut, TextMove,
    BackToBackAnimation, PerfectClearAnimation, ReadyGoAnimation, StopAnimation,
    // Timer クラス //
    ResetTimer, UpdateMoveLeftRightTimer, UpdateMoveDownTimer, UpdateRotateTimer

}

/// <summary>
/// ログ出力の補助を行う静的クラス
/// </summary>
public static class LogHelper
{
    /// <summary> デバッグメッセージを格納する辞書 </summary>
    private static readonly Dictionary<eLogTitle, string> DebugMessages;

    /// <summary>
    /// 静的コンストラクタでデバッグメッセージを初期化
    /// </summary>
    static LogHelper()
    {
        DebugMessages = new Dictionary<eLogTitle, string>();
        DebugList();
    }

    /// <summary> デバッグメッセージを辞書に設定する </summary>
    private static void DebugList()
    {
        DebugMessages[eLogTitle.Start] = "関数の開始";
        DebugMessages[eLogTitle.End] = "関数の終了";
        DebugMessages[eLogTitle.KeyNotFound] = "辞書のキーが見つかりませんでした。";
        DebugMessages[eLogTitle.UpdateFunctionRunning] = "Update関数は正常に作動しています。";
        DebugMessages[eLogTitle.MismatchBetweenAudioAndAudioNameCount] = "Audios の数と eAudioName の数が一致していません。";
        DebugMessages[eLogTitle.AudioNameAlreadyExists] = "すでに登録されている AudioName が存在します。";

        // DebugMessages["MoveRightFailure"] = "Move right failed: Cannot move to the right - Reverting to original position";

        // DebugMessages["StartLeftMoveInput"] = "Starting LeftMoveInput";
        // DebugMessages["EndLeftMoveInput"] = "Ending LeftMoveInput";
        // DebugMessages["MoveLeftSuccess"] = "Move left succeeded: Mino successfully moved to the left";
        // DebugMessages["MoveLeftFailure"] = "Move left failed: Cannot move to the left - Reverting to original position";

        // 他のメッセージも同様に追加
    }

    /// <summary> デバッグログを出力する関数 </summary>
    /// <param name="_class"> クラス名 </param>
    /// <param name="_method"> メソッド名 </param>
    /// <param name="_title"> タイトル </param>
    public static void DebugLog(eClasses _class, eMethod _method, eLogTitle _title)
    {
        if (Application.isEditor)
        {
            if (DebugMessages.TryGetValue(_title, out string message)) // Keyの照合を行う
            {
                string logMessage = FormatLogMessage(eLogLevel.Debug, _class, _method, _title, message);
                Debug.Log(logMessage);
            }
            else // Keyが照合しなかった場合
            {
                string logMessage = FormatLogMessage(eLogLevel.Error, eClasses.LogHelper, eMethod.DebugLog, eLogTitle.KeyNotFound, DebugMessages[eLogTitle.KeyNotFound]);
                Debug.Log(logMessage);
            }
        }
    }

    /// <summary> 情報ログを出力する関数 </summary>
    /// <param name="_class"> クラス名 </param>
    /// <param name="_method"> メソッド名 </param>
    /// <param name="_title"> タイトル </param>
    /// <param name="_info"> 情報 </param>
    public static void InfoLog(eClasses _class, eMethod _method, eLogTitle _title, string _info)
    {
        string logMessage = FormatLogMessage(eLogLevel.Info, _class, _method, _title, _info);
        Debug.Log(logMessage);
    }

    /// <summary> 警告ログを出力する関数 </summary>
    /// <param name="_class"> クラス名 </param>
    /// <param name="_method"> メソッド名 </param>
    /// <param name="_title"> タイトル </param>
    public static void WarningLog(eClasses _class, eMethod _method, eLogTitle _title)
    {
        if (DebugMessages.TryGetValue(_title, out string message)) // Keyの照合を行う
        {
            string logMessage = FormatLogMessage(eLogLevel.Warning, _class, _method, _title, message);
            Debug.Log(logMessage);
        }
        else // Keyが照合しなかった場合
        {
            string logMessage = FormatLogMessage(eLogLevel.Error, eClasses.LogHelper, eMethod.WarningLog, eLogTitle.KeyNotFound, DebugMessages[eLogTitle.KeyNotFound]);
            Debug.Log(logMessage);
        }
    }

    /// <summary> エラーログを出力する関数 </summary>
    /// <param name="_class"> クラス名 </param>
    /// <param name="_method"> メソッド名 </param>
    /// <param name="_title"> タイトル </param>
    public static void ErrorLog(eClasses _class, eMethod _method, eLogTitle _title)
    {
        if (DebugMessages.TryGetValue(_title, out string message)) // Keyの照合を行う
        {
            string logMessage = FormatLogMessage(eLogLevel.Error, _class, _method, _title, message);
            Debug.Log(logMessage);
        }
        else // Keyが照合しなかった場合
        {
            string logMessage = FormatLogMessage(eLogLevel.Error, eClasses.LogHelper, eMethod.ErrorLog, eLogTitle.KeyNotFound, DebugMessages[eLogTitle.KeyNotFound]);
            Debug.Log(logMessage);
        }
    }

    /// <summary> ログメッセージをフォーマットする関数 </summary>
    /// <param name="_logLevel"> ログレベル </param>
    /// <param name="_class"> クラス名 </param>
    /// <param name="_method"> メソッド名 </param>
    /// <param name="_title"> タイトル </param>
    /// <param name="_detail"> 詳細 </param>
    /// <returns> フォーマットされたログメッセージ </returns>
    private static string FormatLogMessage(eLogLevel _logLevel, eClasses _class, eMethod _method, eLogTitle _title, string _detail)
    {
        string color = GetColorByLevel(_logLevel);
        return $"<size=10><color={color}>[{_logLevel}] [{_class}] [{_method}]</color> {_title} / {_detail}</size>";
    }

    /// <summary> ログレベルに応じて色を選択する関数 </summary>
    /// <param name="_logLevel"> ログレベル </param>
    /// <returns> 色 </returns>
    private static string GetColorByLevel(eLogLevel _logLevel)
    {
        switch (_logLevel)
        {
            case eLogLevel.Debug:
                return "#98FB98"; // PaleGreen (優しい緑)
            case eLogLevel.Info:
                return "#ADD8E6"; // LightBlue (優しい青)
            case eLogLevel.Warning:
                return "yellow";
            case eLogLevel.Error:
                return "red";
            default:
                return "white";
        }
    }
}
