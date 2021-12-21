using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeForAll : Game, GameMode
{

    public Team players;
    private int topScore;

    public FreeForAll(Team a, TimeAndScore t)
    {
        game = this;
        ts = t;

        players = a;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckScore();
    }



    public void CheckScore()
    {
        FindTopScore();
        if((topScore >= base.GetScoreLimit()) && base.GetScoreFlag())
        {
            EndGame();
        }

    }

    private void FindTopScore() //walks through players for highest score to compare (helper method)
    {
        int top = 0;

        foreach(GamePlayer p in players._gamePlayers)
        {
            if(p.GetKillCount() > top)
            {
                top = p.GetKillCount();
            }
        }
    }

    public new void EndGame()
    {
        string playerName = "";

        foreach (GamePlayer p in players._gamePlayers)
        {
            if (p.GetKillCount() == topScore)
            {
                playerName = p.username;
            }
        }
        Debug.Log("CONGRATS ON THE WIN " + playerName + "!");
        base.EndGame();
    }
}
