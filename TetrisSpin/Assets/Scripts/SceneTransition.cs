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
        SceneManager.LoadScene("Play", LoadSceneMode.Single); // 他のシーンはアンロードする
    }

    /// <summary> GameOverScene の Retry ボタンが押された時の処理をする関数 </summary>
    public void SelectRetry()
    {
        SceneManager.LoadScene("Play", LoadSceneMode.Single); // 他のシーンはアンロードする
    }

    /// <summary> GameOverScene の Menu ボタン、MainScene の Menu ボタンが押された時の処理をする関数 </summary>
    public void SelectMenu()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single); // 他のシーンはアンロードする
    }

    /// <summary> PlayScene でゲームオーバーになった時の処理をする関数 </summary>
    public void GameOver()
    {
        SceneManager.LoadScene("GameOver", LoadSceneMode.Single); // 他のシーンはアンロードする
    }
}