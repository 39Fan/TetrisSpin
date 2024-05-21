using UnityEngine;
using UnityEngine.SceneManagement;

///// シーン遷移に関するスクリプト /////


// ↓このスクリプトで可能なこと↓ //

// シーン遷移

public class SceneTransition : MonoBehaviour
{
    // 扱うシーン //
    // [SerializeField] private Scene MenuScene;
    // [SerializeField] private Scene PlayScene;
    // [SerializeField] private Scene GameOverScene;

    private void Start()
    {
        DontDestroyOnLoad(gameObject); // シーンが変わっても破壊されない
    }

    // MenuScene の Start ボタンが押された時の処理をする関数 //
    public void SelectStartBottun()
    {
        // PlayScene に遷移
        SceneManager.LoadScene("Play", LoadSceneMode.Single); // 他のシーンはアンロードする
    }

    // GameOverScene の Retry ボタンが押された時の処理をする関数 //
    public void SelectRetry()
    {
        // PlaySceneに遷移
        SceneManager.LoadScene("Play", LoadSceneMode.Single); // 他のシーンはアンロードする
    }

    // GameOverScene の Menu ボタン、MainScene の Menu ボタンが押された時の処理をする関数(未実装) //
    public void SelectMenu()
    {
        // MenuSceneに遷移
        SceneManager.LoadScene("Menu", LoadSceneMode.Single); // 他のシーンはアンロードする
    }

    // PlayScene でゲームオーバーになった時の処理をする関数 //
    public void GameOver()
    {
        // GameOverSceneに遷移
        SceneManager.LoadScene("GameOver", LoadSceneMode.Single); // 他のシーンはアンロードする
    }
}
