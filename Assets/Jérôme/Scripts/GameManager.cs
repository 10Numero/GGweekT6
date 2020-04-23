using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    bool IsPaused;
    public GameObject PauseMenu;

    public void Pause()
    {
        IsPaused = !IsPaused;

        if (IsPaused)
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Locked;
    }

    public void Update()
    {
        PauseMenu.SetActive(IsPaused);

        if (IsPaused) Time.timeScale = 0f;
        else Time.timeScale = 1f;

        if (Input.GetKeyDown(KeyCode.Escape))
            Pause();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {

        Application.Quit();

    }
}