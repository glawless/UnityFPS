using System.Collections;
using UnityEngine;
using Photon.Pun;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField]
    public PlayerManager pManager;

    private Vector3 padLocation;
    public int spawnerID;

    public float respawnTime = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        //Spawn orb at the start
        padLocation = transform.position;
        pManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
    }

    public IEnumerator CountDown()
    {
        pManager.spawning = true;
        yield return new WaitForSeconds(respawnTime);
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        if (PhotonNetwork.IsConnected)
            PhotonNetwork.Instantiate(pManager.playerPrefab.name, padLocation, Quaternion.identity);
        else
            Instantiate(pManager.playerPrefab, padLocation, Quaternion.identity);
        pManager.spawning = false;
    }
}
