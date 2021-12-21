using System;
using UnityEngine;

public abstract class Game : MonoBehaviour
{
    public TimeAndScore ts;
    public GameMode game;

    //called once every frame.
    private void Update()
    {
        if(ts.gameOver == true)
        {
            game.EndGame();
        }
    }



    public void EndGame()
    {
        Debug.Log("GAME OVER.");
        //Application.Quit();
    }

    public int GetScoreLimit()
    {
        return ts.GetScoreLimit();
    }

    public bool GetScoreFlag()
    {
        return ts.useScoreLimit;
    }

    public bool GetTimerFlag()
    {
        return ts.useTimeLimit;
    }
}
