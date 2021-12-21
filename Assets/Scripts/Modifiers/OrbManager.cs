using UnityEngine;
using Photon.Pun;

public class OrbManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] orbs;
    private PhotonView PV;

    #region Singleton

    public static OrbManager instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    private void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (!PV.IsMine)
            return;

        GameObject[] spawners = GameObject.FindGameObjectsWithTag("OrbSpawn");
        foreach (GameObject spawner in spawners)
        {
            ModifierSpawn spawnComp = spawner.GetComponent<ModifierSpawn>();
            if (spawnComp != null && spawnComp.isEmpty)
                spawnComp.StartCoroutine(spawnComp.CountDown());
        }
    }

    public GameObject GetRandomOrb()
    {
        int ranNum = Random.Range(0, orbs.Length);
        return orbs[ranNum];
    }
}
