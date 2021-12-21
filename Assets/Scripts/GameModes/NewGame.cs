using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon;

public class NewGame : MonoBehaviour
{
    public static NewGame instance;
    bool TeamDeathMatchIsTrue; //controlled by boolean in room menu
    public Game TheGame; //this is access and saved in room manager 

    // Use this for initialization
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    //for master side, create game is called inside launcher under start game
    public void CreateGame(Photon.Realtime.Room r)
    {
        //retrieving players from photon room

        ArrayList _playerList = new ArrayList();

        foreach (KeyValuePair<int, Photon.Realtime.Player> player in r.Players)
        {
            _playerList.Add(player.Value);
        }

        //deciding how to split based on game mode (toggle in menu)

        if (TeamDeathMatchIsTrue)
        {
            Team a = new Team(1, "blue");
            Team b = new Team(2, "red");

            //adds game players to team a
            for(int x = 0; x < _playerList.Count/2; x++)
            {
                Photon.Realtime.Player p = (Photon.Realtime.Player) _playerList[x];
                a.AddPlayer(new GamePlayer(p.UserId, p.NickName, null));
                Debug.Log("player " + p.UserId + " added to team a.");
            }

            Debug.Log("players added to team a.");

            //adds game players to team b
            for (int x = _playerList.Count / 2; x < _playerList.Count; x++)
            {
                Photon.Realtime.Player p = (Photon.Realtime.Player)_playerList[x];
                GamePlayer g = new GamePlayer(p.UserId, p.NickName, null);
                a.AddPlayer(g);
                Debug.Log("player " + g.id + " added to team b.");
            }

            Debug.Log("players added to team b.");

            TheGame = new TeamDeathMatch(a, b, new TimeAndScore());

            Debug.Log("team death match - create game ()");

        }

        //logic for free for all
        else
        {
            Team a = new Team(1, "free team");

            foreach(Photon.Realtime.Player player in _playerList)
            {
                GamePlayer g = new GamePlayer(player.UserId, player.NickName, null);
                a.AddPlayer(g);
                Debug.Log("player " + g.id + " added to team in FFA.");
            }

            TheGame = new FreeForAll(a, new TimeAndScore());

            Debug.Log("free for all - create game ()");

        }

        Debug.Log("Game created");
    }

}
