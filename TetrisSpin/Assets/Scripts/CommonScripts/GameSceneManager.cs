using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

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
    /// <summary> モードセレクトシーンの状態 </summary>
    private static bool modeSelectScene = false;
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
    public static bool ModeSelectScene => modeSelectScene;
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
    /// <param name="_modeSelectScene"> モードセレクトシーンの状態 </param>
    /// <param name="_playScene"> プレイシーンの状態 </param>
    /// <param name="_gameOverScene"> ゲームオーバーの状態 </param>
    /// <param name="_gameClearScene"> ゲームクリアの状態 </param>
    /// <remarks>
    /// 指定されたシーン状態を true に設定し、他のすべてのシーン状態を false に設定する。
    /// </remarks>
    public static void UpdateStats(bool? _menuScene = null, bool? _scoreScene = null, bool? _optionScene = null,
        bool? _modeSelectScene = null, bool? _playScene = null, bool? _gameOverScene = null, bool? _gameClearScene = null)
    {
        LogHelper.DebugLog(eClasses.GameSceneManagerStats, eMethod.UpdateStats, eLogTitle.Start);

        // 最初にすべてのシーン状態を false に設定
        menuScene = false;
        scoreScene = false;
        optionScene = false;
        modeSelectScene = false;
        playScene = false;
        gameOverScene = false;
        gameClearScene = false;

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
        else if (_modeSelectScene == true)
        {
            modeSelectScene = true;
            loadScene = "modeSelectScene";
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

    /// <summary> poseStateを false にする関数 </summary>
    public static void UnLoadPoseState()
    {
        LogHelper.DebugLog(eClasses.GameSceneManagerStats, eMethod.UnLoadPoseState, eLogTitle.Start);

        poseState = false;

        LogHelper.DebugLog(eClasses.GameSceneManagerStats, eMethod.UnLoadPoseState, eLogTitle.End);
    }

    /// <summary> デフォルトの <see cref="GameStateManager"/> にリセットする関数 </summary>
    public static void ResetStats()
    {
        LogHelper.DebugLog(eClasses.GameSceneManagerStats, eMethod.ResetStats, eLogTitle.Start);

        menuScene = true;
        scoreScene = false;
        optionScene = false;
        modeSelectScene = false;
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
    [Header("フェード用画像")] public FadeImage fadeImage;

    private bool firstPush = false;
    private bool goNextScene = false;
    private string nextSceneName = "";

    private void Start()
    {
        // フェードインを開始
        fadeImage.StartFadeIn();
    }

    private void Update()
    {
        if (!goNextScene && fadeImage.IsFadeOutComplete())
        {
            SceneManager.LoadScene(nextSceneName);
            goNextScene = true;
        }
    }

    // フェードアウトが完了してからシーン遷移を行う関数
    private void StartSceneTransition(string sceneName)
    {
        if (!firstPush)
        {
            fadeImage.StartFadeOut();
            firstPush = true;
            nextSceneName = sceneName;
        }
    }

    /// <summary> Menuシーンに遷移する関数 </summary>
    public void LoadMenuScene()
    {
        LogHelper.DebugLog(eClasses.GameSceneManager, eMethod.LoadMenuScene, eLogTitle.Start);
        GameSceneManagerStats.UpdateStats(_menuScene: true);
        StartSceneTransition("Menu");
        LogHelper.DebugLog(eClasses.GameSceneManager, eMethod.LoadMenuScene, eLogTitle.End);
    }

    /// <summary> Scoreシーンに遷移する関数 </summary>
    public void LoadScoreScene()
    {
        LogHelper.DebugLog(eClasses.GameSceneManager, eMethod.LoadScoreScene, eLogTitle.Start);
        GameSceneManagerStats.UpdateStats(_scoreScene: true);
        StartSceneTransition("Score");
        LogHelper.DebugLog(eClasses.GameSceneManager, eMethod.LoadScoreScene, eLogTitle.End);
    }

    /// <summary> Optionシーンに遷移する関数 </summary>
    public void LoadOptionScene()
    {
        LogHelper.DebugLog(eClasses.GameSceneManager, eMethod.LoadOptionScene, eLogTitle.Start);
        GameSceneManagerStats.UpdateStats(_optionScene: true);
        StartSceneTransition("Option");
        LogHelper.DebugLog(eClasses.GameSceneManager, eMethod.LoadOptionScene, eLogTitle.End);
    }

    /// <summary> ModeSelectシーンに遷移する関数 </summary>
    public void LoadModeSelectScene()
    {
        LogHelper.DebugLog(eClasses.GameSceneManager, eMethod.LoadModeSelectScene, eLogTitle.Start);
        GameSceneManagerStats.UpdateStats(_modeSelectScene: true);
        StartSceneTransition("ModeSelect");
        LogHelper.DebugLog(eClasses.GameSceneManager, eMethod.LoadModeSelectScene, eLogTitle.End);
    }

    /// <summary> Playシーンに遷移する関数 </summary>
    public void LoadPlayScene()
    {
        LogHelper.DebugLog(eClasses.GameSceneManager, eMethod.LoadPlayScene, eLogTitle.Start);
        GameSceneManagerStats.UpdateStats(_playScene: true);
        StartSceneTransition("Play");
        LogHelper.DebugLog(eClasses.GameSceneManager, eMethod.LoadPlayScene, eLogTitle.End);
    }

    /// <summary> GameClearシーンに遷移する関数 </summary>
    public void LoadGameClearScene()
    {
        LogHelper.DebugLog(eClasses.GameSceneManager, eMethod.LoadGameClearScene, eLogTitle.Start);
        GameSceneManagerStats.UpdateStats(_gameClearScene: true);
        StartSceneTransition("GameClear");
        LogHelper.DebugLog(eClasses.GameSceneManager, eMethod.LoadGameClearScene, eLogTitle.End);
    }

    /// <summary> GameOverシーンに遷移する関数 </summary>
    public void LoadGameOverScene()
    {
        LogHelper.DebugLog(eClasses.GameSceneManager, eMethod.LoadGameOverScene, eLogTitle.Start);
        GameSceneManagerStats.UpdateStats(_gameOverScene: true);
        StartSceneTransition("GameOver");
        LogHelper.DebugLog(eClasses.GameSceneManager, eMethod.LoadGameOverScene, eLogTitle.End);
    }
}