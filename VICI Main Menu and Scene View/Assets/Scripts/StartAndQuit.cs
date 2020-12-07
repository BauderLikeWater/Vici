using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartAndQuit : MonoBehaviour
{
    
    public void PlayGame()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.LoadScene("InGame Example", LoadSceneMode.Single);
    }

    public void MainMenu()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void QuitGame ()
    {
        Application.Quit();
    }

}
