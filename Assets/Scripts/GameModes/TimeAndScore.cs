using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TimeAndScore : MonoBehaviour
{
    //handles time display and checking time and score limits if enabled

    private int scoreLimit;
    public int startMinutes; //(time limit)

    public bool useTimeLimit; //whether or not to end the game based on time
    public bool useScoreLimit;//whether or not to end the game based on score

    float currentTime;
    public Text currentTimeText; //goes to text box on canvas

    public bool gameOver;

    

    // Start is called before the first frame update
    void Start()
    {
        currentTime = startMinutes * 60;
    }



    // Update is called once per frame
    void Update()
    {
        if (useTimeLimit) { CheckTime(); }
    }


    //only called if useTimeLimit flag is true
    public void CheckTime() 
    {

        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        currentTime = currentTime - Time.deltaTime;
        currentTimeText.text = time.Minutes.ToString() + ":" + time.Seconds.ToString();

        if (currentTime <= 0)
        {
            Start();
            gameOver = true;
        }  
    }



    public void SetScoreLimit(int s)
    {
        if(s > 0)
        {
            scoreLimit = s;
        }
    }



    public void SetTimeLimit(int min)
    {
        if(min > 0)
        {
            startMinutes = min;
        }
    }



    public int GetScoreLimit()
    {
        return scoreLimit;
    }
}
