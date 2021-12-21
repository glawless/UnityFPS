using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayer : MonoBehaviour
{

    public string id;
    public string username;
    private int killCount;
    private int deathCount;

    //holding the body of the player
    private PlayerManager p; 

    public GamePlayer(string userId, string name, PlayerManager q)
    {
        id = userId;
        username = name;
        killCount = 0;
        deathCount = 0;
        p = q;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetKillCount()
    {
        return killCount;
    }

    public int GetDeathCount()
    {
        return deathCount;
    }

    public void AddKill()
    {
        killCount++;
    }

    public void AddDeath()
    {
        deathCount++;
    }
}
