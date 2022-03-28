using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public static bool GamePaused = false;
    public GameObject pauseUI;

    //Makes sure that when you are moving to the next level the game doesnt stay paused.
    private void Start()
    {
        Resume();
    }

    //Will always be checking for if the player is trying to open the pause menu,
    //If they are then it will activate the corrisponding functions.
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    //Resume will remove the HUD overlay, continue time and remove the players cursor.
    private void Resume()
    {
        pauseUI.SetActive(false);
        Time.timeScale = 1f;
        GamePaused = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    //Pause will do the opposit of the above function.
    private void Pause()
    {
        pauseUI.SetActive(true);
        Time.timeScale = 0f;
        GamePaused = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void NextLevel(string nextLevel)
    {
        SceneManager.LoadScene(nextLevel);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
