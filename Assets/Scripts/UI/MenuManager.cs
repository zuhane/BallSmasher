using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class MenuManager : MonoBehaviour
{
    private int levelCount;
    private int selectedLevelIndex = 1;
    private int playerCount = 2;

    private BetterToggleGroup toggleGroup;
    private BetterToggleGroup playerPanelGroup;
    private BetterToggleGroup teamToggleGroup;
    private BetterToggleGroup gameModeToggleGroup;

    private BetterToggleGroup player1Team, player2Team, player3Team, player4Team;

    private GameObject GoalDropDown, TimeDropDown;

    // Start is called before the first frame update
    void Start()
    {
        levelCount = GameObject.Find("LevelsScrollbar").transform.GetChild(0).transform.GetChild(0).transform.childCount;
        toggleGroup = GameObject.Find("LevelsScrollbar").transform.GetChild(0).transform.GetChild(0).GetComponent<BetterToggleGroup>();

        playerPanelGroup = GameObject.Find("PlayerPanel").transform.GetChild(1).GetComponent<BetterToggleGroup>();

        Debug.Log("Level count: " + levelCount);

        toggleGroup.OnChange += ChangeLevelIndex;
        playerPanelGroup.OnChange += ChangePlayerCount;
        player1Team = GameObject.Find("Player1Team").GetComponent<BetterToggleGroup>();
        player2Team = GameObject.Find("Player2Team").GetComponent<BetterToggleGroup>();
        player3Team = GameObject.Find("Player3Team").GetComponent<BetterToggleGroup>();
        player4Team = GameObject.Find("Player4Team").GetComponent<BetterToggleGroup>();

        gameModeToggleGroup = GameObject.Find("GameMode").GetComponent<BetterToggleGroup>();

        GoalDropDown = GameObject.Find("GoalDropDown");
        TimeDropDown = GameObject.Find("TimeDropDown");

        GoalDropDown.SetActive(true);
        TimeDropDown.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangePlayerCount(Toggle activeToggle)
    {
        playerCount = System.Array.IndexOf(playerPanelGroup.gameObject.transform.GetComponentsInChildren<Toggle>(), activeToggle) + 1;

        Debug.Log("Players: " + playerCount);
    }

    public void ChangeLevelIndex(Toggle activeToggle)
    {
        selectedLevelIndex = System.Array.IndexOf(toggleGroup.gameObject.transform.GetComponentsInChildren<Toggle>(), activeToggle) + 1;
    }

    public void ChangeGameMode(Toggle activeToggle)
    {
        switch (activeToggle.name)
        {
            case "GameModeGoal":
                GoalDropDown.SetActive(true);
                TimeDropDown.SetActive(false);
                break;
            case "GameModeTime":
                GoalDropDown.SetActive(false);
                TimeDropDown.SetActive(true);
                break;
            default:
                GoalDropDown.SetActive(true);
                TimeDropDown.SetActive(false);
                break;
        }
    }

    public void LoadLevelAutomated()
    {
        Debug.Log("player1Team: " + (player1Team.GetActiveIndex() + 1));
        Debug.Log("player2Team: " + (player2Team.GetActiveIndex() + 1));
        Debug.Log("player3Team: " + (player3Team.GetActiveIndex() + 1));
        Debug.Log("player4Team: " + (player4Team.GetActiveIndex() + 1));


        LevelData.player1Team = player1Team.GetActiveIndex() + 1;
        LevelData.player2Team = player2Team.GetActiveIndex() + 1;
        LevelData.player3Team = player3Team.GetActiveIndex() + 1;
        LevelData.player4Team = player4Team.GetActiveIndex() + 1;
        LevelData.playerCount = playerCount;
        LevelData.levelIndex = levelCount;

        LevelData.gameMode = (GameMode)gameModeToggleGroup.GetActiveIndex();
        switch (LevelData.gameMode)
        {
            case GameMode.Goals:
                LevelData.GoalsToWin = GoalDropDown.GetComponent<Dropdown>().value;
                break;
            case GameMode.Time:
                LevelData.MatchTime = LevelData.Times[TimeDropDown.GetComponent<Dropdown>().value];
                break;
        }


        SceneManager.LoadScene("Level" + selectedLevelIndex);


    }

    public void LoadLevelManual(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
}
