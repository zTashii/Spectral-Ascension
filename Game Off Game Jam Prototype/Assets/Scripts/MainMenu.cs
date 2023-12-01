using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public int sceneIndex;

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        PlayerPrefs.SetInt("SavedScene", 1);
    }


    public void TryAgain()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("SavedScene"));
        
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);

    }
}
