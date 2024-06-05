using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// スクリプト一覧
/// </summary>
public enum eScripts
{
    AudioManager,
    Board,
    GameManager,
    GameStatus,
    LogHelper,
    Mino,
    Rounding,
    SceneTransition,
    Spawner,
    SpinCheck,
    TextEffect,
    Timer
}

/// <summary>
/// 関数一覧
/// </summary>
public enum eMethod
{
    //
}

/// <summary>
/// ログ出力の補助を行う静的クラス
/// </summary>
public static class LogHelper
{
    /// <summary>
    /// ログのレベルを定義する列挙型
    /// </summary>
    public enum eLogLevel
    {
        Debug,
        Info,
        Warning,
        Error
    }

    /// <summary> デバッグメッセージを格納する辞書 </summary>
    private static readonly Dictionary<string, string> DebugMessages;

    /// <summary>
    /// 静的コンストラクタでデバッグメッセージを初期化
    /// </summary>
    static LogHelper()
    {
        DebugMessages = new Dictionary<string, string>();
        DebugList();
    }

    /// <summary> デバッグメッセージを辞書に設定する </summary>
    private static void DebugList()
    {
        DebugMessages["Start"] = "Starting Method";
        DebugMessages["End"] = "Ending Method";

        // DebugMessages["KeyNotFound"] = "Error: The specified key was not found in the dictionary.";

        DebugMessages["EndRightMoveInput"] = "Ending RightMoveInput";
        DebugMessages["MoveRightSuccess"] = "Move right succeeded: Mino successfully moved to the right";
        DebugMessages["MoveRightFailure"] = "Move right failed: Cannot move to the right - Reverting to original position";

        DebugMessages["StartLeftMoveInput"] = "Starting LeftMoveInput";
        DebugMessages["EndLeftMoveInput"] = "Ending LeftMoveInput";
        DebugMessages["MoveLeftSuccess"] = "Move left succeeded: Mino successfully moved to the left";
        DebugMessages["MoveLeftFailure"] = "Move left failed: Cannot move to the left - Reverting to original position";

        // 他のメッセージも同様に追加
    }

    /// <summary> 指定されたログレベルと情報でログを出力する関数を呼ぶ関数 </summary>
    /// <param name="_level"> ログレベル </param>
    /// <param name="_script"> スクリプト名 </param>
    /// <param name="_method"> メソッド名 </param>
    /// <param name="_title_or_Info"> タイトルまたは情報 </param>
    public static void Log(eLogLevel _level, string _script, string _method, string _title_or_Info)
    {
        if (_level == eLogLevel.Debug)
        {
            DebugLog(_level, _script, _method, _title_or_Info);
        }
        else if (_level == eLogLevel.Info)
        {
            InfoLog(_level, _script, _method, _title_or_Info);
        }
    }

    /// <summary> デバッグログを出力する関数 </summary>
    /// <param name="level"> ログレベル </param>
    /// <param name="script"> スクリプト名 </param>
    /// <param name="method"> メソッド名 </param>
    /// <param name="title"> タイトル </param>
    private static void DebugLog(eLogLevel level, string script, string method, string title)
    {
        if (DebugMessages.TryGetValue(title, out string message)) // Keyの照合を行う
        {
            string logMessage = FormatDebugLogMessage(level, script, method, title, message);
            Debug.Log(logMessage);
        }
        else // Keyが照合しなかった場合
        {
            string logMessage = FormatDebugLogMessage(level, script, method, "KeyNotFound", "The specified key was not found in the dictionary");
            Debug.Log(logMessage);
        }
    }

    /// <summary> 情報ログを出力する関数 </summary>
    /// <param name="level"> ログレベル </param>
    /// <param name="script"> スクリプト名 </param>
    /// <param name="method"> メソッド名 </param>
    /// <param name="info"> 情報 </param>
    private static void InfoLog(eLogLevel level, string script, string method, string info)
    {
        string logMessage = FormatInfoLogMessage(level, script, method, info);
        Debug.Log(logMessage);
    }

    /// <summary> デバッグログメッセージをフォーマットする関数 </summary>
    /// <param name="level"> ログレベル</param>
    /// <param name="script"> スクリプト名</param>
    /// <param name="method">メソッド名</param>
    /// <param name="title">タイトル</param>
    /// <param name="message">メッセージ</param>
    /// <returns> フォーマットされたログメッセージ </returns>
    private static string FormatDebugLogMessage(eLogLevel level, string script, string method, string title, string message)
    {
        string color = GetColorByLevel(level);
        return $"<size=10><color={color}>[{level}] [{script}] [{method}]</color> {title} / {message}</size>";
    }

    /// <summary> 情報ログメッセージをフォーマットする関数 </summary>
    /// <param name="level"> ログレベル </param>
    /// <param name="script"> スクリプト名 </param>
    /// <param name="method"> メソッド名 </param>
    /// <param name="info"> 情報 </param>
    /// <returns> フォーマットされたログメッセージ </returns>
    private static string FormatInfoLogMessage(eLogLevel level, string script, string method, string info)
    {
        string color = GetColorByLevel(level);
        return $"<size=10><color={color}>[{level}] [{script}] [{method}]</color> {info}</size>";
    }

    /// <summary> ログレベルに応じて色を選択する関数 </summary>
    /// <param name="level"> ログレベル </param>
    /// <returns> 色 </returns>
    private static string GetColorByLevel(eLogLevel level)
    {
        switch (level)
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
