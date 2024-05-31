using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ログ出力の補助を行う静的クラス
/// </summary>
public static class LogHelper
{
    /// <summary>
    /// ログのレベルを定義する列挙型
    /// </summary>
    public enum LogLevel
    {
        Debug,
        Info,
        Warning,
        Error
    }

    /// <summary>
    /// デバッグメッセージを格納する辞書
    /// </summary>
    private static readonly Dictionary<string, string> DebugMessages;

    /// <summary>
    /// 静的コンストラクタでデバッグメッセージを初期化
    /// </summary>
    static LogHelper()
    {
        DebugMessages = new Dictionary<string, string>();
        DebugList();
    }

    /// <summary>
    /// デバッグメッセージを辞書に設定する
    /// </summary>
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

    /// <summary>
    /// 指定されたログレベルと情報でログを出力する
    /// </summary>
    /// <param name="level">ログレベル</param>
    /// <param name="script">スクリプト名</param>
    /// <param name="method">メソッド名</param>
    /// <param name="title_or_Info">タイトルまたは情報</param>
    public static void Log(LogLevel level, string script, string method, string title_or_Info)
    {
        if (level == LogLevel.Debug)
        {
            DebugLog(level, script, method, title_or_Info);
        }
        else if (level == LogLevel.Info)
        {
            InfoLog(level, script, method, title_or_Info);
        }

    }

    /// <summary>
    /// デバッグログを出力する
    /// </summary>
    /// <param name="level">ログレベル</param>
    /// <param name="script">スクリプト名</param>
    /// <param name="method">メソッド名</param>
    /// <param name="title">タイトル</param>
    private static void DebugLog(LogLevel level, string script, string method, string title)
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

    /// <summary>
    /// 情報ログを出力する
    /// </summary>
    /// <param name="level">ログレベル</param>
    /// <param name="script">スクリプト名</param>
    /// <param name="method">メソッド名</param>
    /// <param name="info">情報</param>
    private static void InfoLog(LogLevel level, string script, string method, string info)
    {
        string logMessage = FormatInfoLogMessage(level, script, method, info);
        Debug.Log(logMessage);
    }

    /// <summary>
    /// デバッグログメッセージをフォーマットする
    /// </summary>
    /// <param name="level">ログレベル</param>
    /// <param name="script">スクリプト名</param>
    /// <param name="method">メソッド名</param>
    /// <param name="title">タイトル</param>
    /// <param name="message">メッセージ</param>
    /// <returns>フォーマットされたログメッセージ</returns>
    private static string FormatDebugLogMessage(LogLevel level, string script, string method, string title, string message)
    {
        string color = GetColorByLevel(level);
        return $"<size=10><color={color}>[{level}] [{script}] [{method}]</color> {title} / {message}</size>";
    }

    /// <summary>
    /// 情報ログメッセージをフォーマットする
    /// </summary>
    /// <param name="level">ログレベル</param>
    /// <param name="script">スクリプト名</param>
    /// <param name="method">メソッド名</param>
    /// <param name="info">情報</param>
    /// <returns>フォーマットされたログメッセージ</returns>
    private static string FormatInfoLogMessage(LogLevel level, string script, string method, string info)
    {
        string color = GetColorByLevel(level);
        return $"<size=10><color={color}>[{level}] [{script}] [{method}]</color> {info}</size>";
    }

    /// <summary>
    /// ログレベルに応じて色を選択する
    /// </summary>
    /// <param name="level">ログレベル</param>
    /// <returns>色</returns>
    private static string GetColorByLevel(LogLevel level)
    {
        switch (level)
        {
            case LogLevel.Debug:
                return "#98FB98"; // PaleGreen (優しい緑)
            case LogLevel.Info:
                return "#ADD8E6"; // LightBlue (優しい青)
            case LogLevel.Warning:
                return "yellow";
            case LogLevel.Error:
                return "red";
            default:
                return "white";
        }
    }
}
