
using System.Collections.Generic;

public enum GameMode
{
    Goals = 0,
    Time = 1
}


public static class LevelData
{
    public static int[] Times = new int[] { 10, 60, 300, 600 };


    public static int levelIndex = 1;
    public static int playerCount = 1;
    public static int player1Team = 1, player2Team = 1, player3Team = 1, player4Team = 1;
    public static GameMode gameMode = GameMode.Goals;
    public static int GoalsToWin = 3, MatchTime = 300;
}