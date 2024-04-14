using UnityEngine;
using UnityEngine.SceneManagement;

//シーン遷移を扱うスクリプト
public class SceneTransition : MonoBehaviour
{
    /*[SerializeField]
    private GameObject gameOverPanel;
    [SerializeField]
    private GameObject Menu;
    [SerializeField]
    private GameObject Main;*/

    //MenuシーンのStartボタンが押された時
    public void SelectStartBottun()
    {
        //Mainシーンに遷移
        //他のシーンはアンロード
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

    //GameOverシーンのRetryボタンが押された時
    public void SelectRetry()
    {
        //Mainシーンに遷移
        //他のシーンはアンロード
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

    //GameOverシーンのMenuボタン、MainシーンのMenuボタンが押された時(未実装)
    public void SelectMenu()
    {
        //Menuシーンに遷移
        //他のシーンはアンロード
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    //Mainシーンでゲームオーバーになった時
    public void GameOver()
    {
        //GameOverシーンに遷移
        //他のシーンはアンロード
        SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
    }

}
