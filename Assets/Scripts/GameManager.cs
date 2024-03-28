using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverScreen;


    public void GameOver(){
        Time.timeScale = 0f;
        gameOverScreen.SetActive(true);
        FindObjectOfType<FPSPlayerController>().UnlockCursor();
    }

    public void PlayAgain(){
        Time.timeScale = 1f;
        gameOverScreen.SetActive(false);
        SceneManager.LoadScene(0);
    }
}
