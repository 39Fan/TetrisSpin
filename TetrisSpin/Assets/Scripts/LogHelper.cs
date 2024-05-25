using UnityEngine;
using System.Collections.Generic;

public static class LogHelper
{
    // ログレベルの定義 //
    public enum LogLevel
    {
        Debug,
        Info,
        Warning,
        Error
    }

    // ログタイトルと詳細文の辞書
    private static readonly Dictionary<string, string> DebugMessages;

    // 静的コンストラクタでデバッグメッセージを初期化
    static LogHelper()
    {
        DebugMessages = new Dictionary<string, string>();
        DebugList();
    }

    // デバッグメッセージを連想配列に設定する関数
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

    // ログを出力する関数 //
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

    private static void InfoLog(LogLevel level, string script, string method, string info)
    {
        string logMessage = FormatInfoLogMessage(level, script, method, info);
        Debug.Log(logMessage);
    }

    // レベルとカテゴリに基づいてデバッグログメッセージを作成する関数 //
    private static string FormatDebugLogMessage(LogLevel level, string script, string method, string title, string message)
    {
        string color = GetColorByLevel(level);
        return $"<size=10><color={color}>[{level}] [{script}] [{method}]</color> {title} / {message}</size>";
    }

    // レベルとカテゴリに基づいてインフォログメッセージを作成する関数 //
    private static string FormatInfoLogMessage(LogLevel level, string script, string method, string info)
    {
        string color = GetColorByLevel(level);
        return $"<size=10><color={color}>[{level}] [{script}] [{method}]</color> {info}</size>";
    }

    // ログレベルに応じて色を選択する関数 //
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
