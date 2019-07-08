using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour
{
    public static ScoreKeeper instance;

    private Text text;

    private int teamRedScore, teamGreenScore, teamBlueScore, teamYellowScore;
    string scoreBoard;
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        scoreBoard = "{0} : {1}";
    }

    public void UpdateScore(team team)
    {
        if (team == team.red) teamRedScore++;
        if (team == team.green) teamGreenScore++;
        
        text.text = string.Format(scoreBoard, teamRedScore.ToString(), teamGreenScore.ToString(), teamBlueScore.ToString(), teamYellowScore.ToString());
    }


}
