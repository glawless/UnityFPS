using System.Collections;
using UnityEngine;
using Photon.Pun;

public class EnemySpawn : MonoBehaviour
{

    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private GameObject spawnBeamPrefab;

    public float respawnTime = 30f;
    public bool readyToSpawn = true;

    private BotManager botManager;
    private Vector3 padLocation;

    // Start is called before the first frame update
    void Start()
    {
        botManager = BotManager.instance;
        padLocation = transform.position;
    }

    public IEnumerator CountDown()
    {
        readyToSpawn = false;
        SpawnBot();
        yield return new WaitForSeconds(respawnTime);
        readyToSpawn = true;
    }


    private void SpawnBot()
    {
        if (PhotonNetwork.IsConnected)
            PhotonNetwork.InstantiateRoomObject(botManager.GetRandomBot().name, padLocation, Quaternion.identity);
        else
            Instantiate(botManager.GetRandomBot(), padLocation, Quaternion.identity);
        Instantiate(spawnBeamPrefab, padLocation, Quaternion.identity);
    }
}
