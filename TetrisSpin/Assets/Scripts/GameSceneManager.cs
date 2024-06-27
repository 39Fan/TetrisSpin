using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// シーンの状態を管理するクラス
/// </summary>
public static class GameSceneManagerStats
{
    /// <summary> メニューシーンの状態 </summary>
    private static bool menuScene = true;
    /// <summary> スコアシーンの状態 </summary>
    private static bool scoreScene = false;
    /// <summary> オプションシーンの状態 </summary>
    private static bool optionScene = false;
    /// <summary> プレイシーンの状態 </summary>
    private static bool playScene = false;
    /// <summary> ゲームオーバーシーンの状態 </summary>
    private static bool gameOverScene = false;
    /// <summary> ゲームクリアシーンの状態 </summary>
    private static bool gameClearScene = false;
    /// <summary> プレイシーン中のポーズ画面の状態 </summary>
    private static bool poseState = false;

    // ゲッタープロパティ //
    public static bool MenuScene => menuScene;
    public static bool ScoreScene => scoreScene;
    public static bool OptionScene => optionScene;
    public static bool PlayScene => playScene;
    public static bool GameOverScene => gameOverScene;
    public static bool GameClearScene => gameClearScene;
    public static bool PoseState => poseState;

    /// <summary> スタッツログの詳細 </summary>
    private static string logStatsDetail;

    /// <summary> 指定されたフィールドの値を更新する関数 </summary>
    /// <param name="_menuScene"> メニューシーンの状態 </param>
    /// <param name="_scoreScene"> スコアシーンの状態 </param>
    /// <param name="_optionScene"> オプションシーンの状態 </param>
    /// <param name="_playScene"> プレイシーンの状態 </param>
    /// <param name="_gameOverScene"> ゲームオーバーの状態 </param>
    /// <param name="_gameClearScene"> ゲームクリアの状態 </param>
    /// <param name="_poseState"> ポーズシーンの状態 </param>
    /// <remarks>
    /// 指定されたシーン状態を true に設定し、他のすべてのシーン状態を false に設定する。
    /// </remarks>
    public static void UpdateStats(bool? _menuScene = null, bool? _scoreScene = null, bool? _optionScene = null, bool? _playScene = null, bool? _gameOverScene = null, bool? _gameClearScene = null, bool? _poseState = null)
    {
        LogHelper.DebugLog(eClasses.GameSceneManagerStats, eMethod.UpdateStats, eLogTitle.Start);

        // 最初にすべてのシーン状態を false に設定
        menuScene = false;
        scoreScene = false;
        optionScene = false;
        playScene = false;
        gameOverScene = false;
        gameClearScene = false;
        poseState = false;

        string loadScene = "NoScene";

        // 引数で true が指定された状態のみを true に設定
        if (_menuScene == true)
        {
            menuScene = true;
            loadScene = "menuScene";
        }
        else if (_scoreScene == true)
        {
            scoreScene = true;
            loadScene = "scoreScene";
        }
        else if (_optionScene == true)
        {
            optionScene = true;
            loadScene = "optionScene";
        }
        else if (_playScene == true)
        {
            playScene = true;
            loadScene = "playScene";
        }
        else if (_gameOverScene == true)
        {
            gameOverScene = true;
            loadScene = "gameOverScene";
        }
        else if (_gameClearScene == true)
        {
            gameClearScene = true;
            loadScene = "gameClearScene";
        }
        else if (_poseState == true)
        {
            poseState = true;
            loadScene = "poseState";
        }

        logStatsDetail = $"LoadScene = {loadScene}";
        LogHelper.InfoLog(eClasses.GameSceneManagerStats, eMethod.UpdateStats, eLogTitle.StatsInfo, logStatsDetail);

        LogHelper.DebugLog(eClasses.GameSceneManagerStats, eMethod.UpdateStats, eLogTitle.End);
    }

    /// <summary> poseStateを true にする関数 </summary>
    public static void LoadPoseState()
    {
        LogHelper.DebugLog(eClasses.GameSceneManagerStats, eMethod.LoadPoseState, eLogTitle.Start);

        poseState = true;

        LogHelper.DebugLog(eClasses.GameSceneManagerStats, eMethod.LoadPoseState, eLogTitle.End);
    }

    /// <summary> デフォルトの <see cref="GameStateManager"/> にリセットする関数 </summary>
    public static void ResetStats()
    {
        LogHelper.DebugLog(eClasses.GameSceneManagerStats, eMethod.ResetStats, eLogTitle.Start);

        menuScene = true;
        scoreScene = false;
        optionScene = false;
        playScene = false;
        gameOverScene = false;
        gameClearScene = false;
        poseState = false;

        LogHelper.DebugLog(eClasses.GameSceneManagerStats, eMethod.ResetStats, eLogTitle.End);
    }
}

/// <summary>
/// シーン遷移に関するクラス
/// </summary>
public class GameSceneManager : MonoBehaviour
{
    /// <summary> Playシーンに遷移する関数 </summary>
    public void LoadPlayScene()
    {
        LogHelper.DebugLog(eClasses.GameSceneManager, eMethod.LoadPlayScene, eLogTitle.Start);

        SceneManager.LoadScene("Play", LoadSceneMode.Single); // 他のシーンはアンロードする

        LogHelper.DebugLog(eClasses.GameSceneManager, eMethod.LoadPlayScene, eLogTitle.End);
    }

    /// <summary> GameOverScene の Retry ボタンが押された時の処理をする関数 </summary>
    public void SelectRetry()
    {
        LogHelper.DebugLog(eClasses.GameSceneManager, eMethod.SelectRetry, eLogTitle.Start);

        SceneManager.LoadScene("Play", LoadSceneMode.Single);

        LogHelper.DebugLog(eClasses.GameSceneManager, eMethod.SelectRetry, eLogTitle.End);
    }

    /// <summary> GameOverScene の Menu ボタン、MainScene の Menu ボタンが押された時の処理をする関数 </summary>
    public void SelectMenu()
    {
        LogHelper.DebugLog(eClasses.GameSceneManager, eMethod.SelectMenu, eLogTitle.Start);

        SceneManager.LoadScene("Menu", LoadSceneMode.Single);

        LogHelper.DebugLog(eClasses.GameSceneManager, eMethod.SelectMenu, eLogTitle.End);
    }

    /// <summary> PlayScene でゲームオーバーになった時の処理をする関数 </summary>
    public void GameOver()
    {
        LogHelper.DebugLog(eClasses.GameSceneManager, eMethod.GameOver, eLogTitle.Start);

        SceneManager.LoadScene("GameOver", LoadSceneMode.Single);

        LogHelper.DebugLog(eClasses.GameSceneManager, eMethod.GameOver, eLogTitle.End);
    }
}