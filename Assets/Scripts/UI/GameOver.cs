using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{


    public static bool gameOver = false;

    private float currTimeScale = 1f;

    public GameObject GameOverPanelUi;
    private Timer matchLimit;

    private TextMeshProUGUI TeamWinText;

    // Start is called before the first frame update
    void Start()
    {
        GameOverPanelUi.SetActive(false);
        if (LevelData.gameMode == GameMode.Time)
        {
            matchLimit = Timer.CreateComponent(gameObject, LevelData.MatchTime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelData.gameMode == GameMode.Time)
        {
            if (matchLimit.LimitReached())
            {
                DisplayWinner();
                currTimeScale = Time.timeScale;
                Time.timeScale = 0f;
                gameOver = true;
            }
        }


    }

    public bool CheckScoreCount()
    {
        if (LevelData.gameMode == GameMode.Goals)
        {
            if (ScoreKeeper.instance.team1Score >= LevelData.GoalsToWin || ScoreKeeper.instance.team2Score >= LevelData.GoalsToWin)
            {
                DisplayWinner();
                currTimeScale = Time.timeScale;
                Time.timeScale = 0f;
                gameOver = true;

                return true;
            }
        }
        return false;
    }

    private void DisplayWinner()
    {
        GameOverPanelUi.SetActive(true);

        if (ScoreKeeper.instance.team1Score == ScoreKeeper.instance.team2Score)
        {
            GameObject.Find("WinningTeam").GetComponent<TextMeshProUGUI>().text = "Draw";
        } else
        {
            GameObject.Find("WinningTeam").GetComponent<TextMeshProUGUI>().text = string.Format("Team {0} Wins", (ScoreKeeper.instance.team1Score > ScoreKeeper.instance.team2Score ? "1" : "2"));

        }
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
        gameOver = false;
        SceneManager.LoadScene("MainMenu");
    }
}

