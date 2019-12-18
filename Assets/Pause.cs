using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{

    public static bool GamePaused = false;

    private float currTimeScale = 1f;

    public GameObject PausePanelUi;

    void Start()
    {
        PausePanelUi.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GamePaused)
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

        PausePanelUi.SetActive(false);
        Time.timeScale = currTimeScale;
        GamePaused = false;
    }

    private void PauseGame() 
    {
        PausePanelUi.SetActive(true);
        currTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        GamePaused = true;
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        GamePaused = false;
        SceneManager.LoadScene("MainMenu");
    }
}
