using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// シーン遷移に関するスクリプト
/// </summary>
public class SceneTransition : MonoBehaviour
{
    /// <summary>
    /// ゲームオブジェクトをシーン遷移時に破壊されないように設定する
    /// </summary>
    private void Start()
    {
        DontDestroyOnLoad(gameObject); // シーンが変わっても破壊されない
    }

    /// <summary> MenuScene の Start ボタンが押された時の処理をする関数 </summary>
    public void SelectStartButton()
    {
        LogHelper.DebugLog(eClasses.SceneTransition, eMethod.SelectStartButton, eLogTitle.Start);

        SceneManager.LoadScene("Play", LoadSceneMode.Single); // 他のシーンはアンロードする

        LogHelper.DebugLog(eClasses.SceneTransition, eMethod.SelectStartButton, eLogTitle.End);
    }

    /// <summary> GameOverScene の Retry ボタンが押された時の処理をする関数 </summary>
    public void SelectRetry()
    {
        LogHelper.DebugLog(eClasses.SceneTransition, eMethod.SelectRetry, eLogTitle.Start);

        SceneManager.LoadScene("Play", LoadSceneMode.Single);

        LogHelper.DebugLog(eClasses.SceneTransition, eMethod.SelectRetry, eLogTitle.End);
    }

    /// <summary> GameOverScene の Menu ボタン、MainScene の Menu ボタンが押された時の処理をする関数 </summary>
    public void SelectMenu()
    {
        LogHelper.DebugLog(eClasses.SceneTransition, eMethod.SelectMenu, eLogTitle.Start);

        SceneManager.LoadScene("Menu", LoadSceneMode.Single);

        LogHelper.DebugLog(eClasses.SceneTransition, eMethod.SelectMenu, eLogTitle.End);
    }

    /// <summary> PlayScene でゲームオーバーになった時の処理をする関数 </summary>
    public void GameOver()
    {
        LogHelper.DebugLog(eClasses.SceneTransition, eMethod.GameOver, eLogTitle.Start);

        SceneManager.LoadScene("GameOver", LoadSceneMode.Single);

        LogHelper.DebugLog(eClasses.SceneTransition, eMethod.GameOver, eLogTitle.End);
    }
}