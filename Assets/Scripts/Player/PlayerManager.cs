using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    //public PhotonView PV;
    
    public GamePlayer gamePlayer;
    #region Singleton

    public static PlayerManager instance;
    

    private void Awake()
    {
        instance = this;
        //PV = GetComponent<PhotonView>();

    }

    #endregion
    
    public GameObject playerPrefab;
    public bool canRespawn;
    private bool needToRespawn;
    public bool spawning;

    private void Update()
    {
        needToRespawn = true;
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.GetComponent<PhotonView>().IsMine && PhotonNetwork.IsConnected)
            {
                needToRespawn = false;
            }
            else if (!PhotonNetwork.IsConnected && GameObject.FindGameObjectsWithTag("Player") != null)
                needToRespawn = false;
        }
        if (canRespawn && needToRespawn && !spawning)
        {
            GameObject[] spawners = GameObject.FindGameObjectsWithTag("Respawn");
            int ranNum = Random.Range(0, spawners.Length);
            foreach (GameObject spawner in spawners)
            {
                PlayerSpawn spawnComp = spawner.GetComponent<PlayerSpawn>();
                if (spawnComp != null && spawnComp.spawnerID == ranNum)
                    spawnComp.StartCoroutine(spawnComp.CountDown());
            }
        }
    }
}
