using UnityEngine;
using Photon.Pun;

public class BotManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] bots;

    public static BotManager instance;

    private PhotonView PV;

    private void Awake()
    {
        instance = this;
        PV = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (!PV.IsMine)
            return;

            GameObject[] spawners = GameObject.FindGameObjectsWithTag("BotSpawn");
            foreach (GameObject spawner in spawners)
            {
                EnemySpawn spawnComp = spawner.GetComponent<EnemySpawn>();
                if (spawnComp != null && spawnComp.readyToSpawn)
                    spawnComp.StartCoroutine(spawnComp.CountDown());
            }
    }

    public GameObject GetRandomBot()
    {
        int ranNum = Random.Range(0, bots.Length);
        return bots[ranNum];
    }
}
