using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    /*[SerializeField]
    private GameObject gameOverPanel;
    [SerializeField]
    private GameObject Menu;
    [SerializeField]
    private GameObject Main;*/

    public void SelectStartBottun()
    {
        Debug.Log("SelectStartBottun");
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

    public void SelectRetry()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

    public void SelectMenu()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    public void GameOver()
    {
        SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
    }

}
