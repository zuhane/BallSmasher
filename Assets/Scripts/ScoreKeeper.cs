using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour
{
    public static ScoreKeeper instance;

    private Text text;

    private int team1Score, team2Score;
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

    public void UpdateScore(int team)
    {
        if (team == 1) team2Score++;
        if (team == 2) team1Score++;
        
        text.text = string.Format(scoreBoard, team1Score, team2Score);
    }


}
