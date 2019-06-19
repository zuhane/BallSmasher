using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour
{
    public static ScoreKeeper instance;

    private Text text;

    private int p1Score, p2Score;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    public void UpdateScore(int goalNumber)
    {
        if (goalNumber == 1) p1Score++;
        if (goalNumber == 2) p2Score++;

        text.text = p1Score + " - " + p2Score;
    }


}
