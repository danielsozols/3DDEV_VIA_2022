using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour
{
    public static bool playerIsDead = false;

    public GameObject DeathMenu;
    public GameObject CanvasCam;

    public Text scoreText;
    float score = Shoot.killCount;

    private void Update()
    {
        score = Shoot.killCount;
        scoreText.text = "You Died! Your Score: " + score.ToString();
    }

    public void GameOverMessage()
    {
        scoreText.text = "You Died! Your Score: " + score.ToString();
        DeathMenu.SetActive(true);
        CanvasCam.SetActive(false);
        playerIsDead = true;
        Time.timeScale = 0f;
    }
    public void RespawnPlayer()
    {
        SceneManager.LoadScene("SampleScene");
        playerIsDead = false;
        Time.timeScale = 1f;
    }
}
