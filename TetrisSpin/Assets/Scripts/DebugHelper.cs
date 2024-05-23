using UnityEngine;

public static class DebugHelper
{
    public enum LogLevel
    {
        Debug,
        Info,
        Warning,
        Error
    }

    // ログを出力する関数 //
    public static void Log(string message, LogLevel level, string script, string method)
    {
        string logMessage = FormatLogMessage(message, level, script, method);
        Debug.Log(logMessage);
    }

    // レベルとカテゴリに基づいてログメッセージを作成する関数 //
    private static string FormatLogMessage(string message, LogLevel level, string script, string method)
    {
        string color = GetColorByLevel(level);
        return $"<b><color={color}>[{level}][{script}]</color></b> <color=grey>{method}</color> {message}";
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
