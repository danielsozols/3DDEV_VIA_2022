using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
    public static bool isGamePaused;
    
    public GameObject PauseMenu;
    public GameObject CanvasCam;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !DeathScreen.playerIsDead)
        {
            if(isGamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        PauseMenu.SetActive(false);
        CanvasCam.SetActive(true);
        Time.timeScale = 1f;
        isGamePaused = false;
    }

    void PauseGame()
    {
        PauseMenu.SetActive(true);
        CanvasCam.SetActive(false);
        Time.timeScale = 0f;
        isGamePaused = true;
    }

    public void RespawnPlayer()
    {
        SceneManager.LoadScene("MainScene");
        Time.timeScale = 1f;
        isGamePaused = false;
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
