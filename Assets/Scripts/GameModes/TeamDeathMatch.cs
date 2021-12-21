using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamDeathMatch : Game,  GameMode
{
    public Team t1;
    public Team t2;

    public TeamDeathMatch(Team a, Team b, TimeAndScore t) 
    {
        t1 = a;
        t2 = b;
        game = this;
        ts = t;
    }

    private void Update()
    {
        CheckScore();
    }

    public void CheckScore() 
    {
        if((t1.GetScore() >= base.GetScoreLimit() || t2.GetScore() >= base.GetScoreLimit()) && base.GetScoreFlag())
        {
            EndGame();
        }
    }

    public new void EndGame()
    {
        Team winner;

        if(t1.GetScore() > t2.GetScore())
        {
            winner = t1;
        }

        else
        {
            winner = t2;
        }

        Debug.Log("CONGRATS ON THE WIN " + winner.teamName + "!");

        base.EndGame();
    }
}
