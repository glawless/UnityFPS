using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour
{
    public int id;
    public string teamName;

    public ArrayList _gamePlayers;

    private int score;

    public Team(int teamId, string name)
    {
        id = teamId;
        teamName = name;
        _gamePlayers = new ArrayList();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateScore();
    }

    // adds the kills from all players on team (called in update)
    public void UpdateScore()
    {
        score = 0;
        foreach(GamePlayer p in _gamePlayers)
        {
            score += p.GetKillCount();
        }
    }

    public int GetScore()
    {
        return score;
    }

    public void AddPlayer(GamePlayer p)
    {
        _gamePlayers.Add(p);
    }

    public void RemovePlayer(GamePlayer p)
    {
        _gamePlayers.Remove(p);
    }
}
