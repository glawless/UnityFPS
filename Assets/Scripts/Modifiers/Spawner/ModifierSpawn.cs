using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class ModifierSpawn : MonoBehaviour
{
    public bool isEmpty;

    private GameObject spawnedModifier;

    private Vector3 padLocation;
    private Vector3 spawnLocation;

    public float respawnTime = 2.0f;

    private OrbManager orbManager;


    // Start is called before the first frame update
    void Start()
    {
        //Spawn orb at the start
        orbManager = OrbManager.instance;
        padLocation = transform.position;

        spawnLocation = padLocation;
        spawnLocation.y = padLocation.y + 1.5f;

        isEmpty = true;
    }


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("PlayerMesh") && spawnedModifier != null && !isEmpty)
        {
            isEmpty = true;

            if (spawnedModifier.CompareTag("Orb"))
            {
                foreach (Transform element in spawnedModifier.GetComponentInChildren<Transform>())
                {
                    if (element.GetComponent<ParticleSystem>() != null)
                    {
                        ParticleSystem ps = element.GetComponent<ParticleSystem>();
                        ps.Stop();
                    }
                    if (element.GetComponent<MeshRenderer>() != null)
                    {
                        element.GetComponent<MeshRenderer>().enabled = false;
                    }
                }
            }
            else
                spawnedModifier.GetComponent<GunPickup>().deactivate = true;
        }
    }


    public IEnumerator CountDown()
    {
        isEmpty = false;
        yield return new WaitForSeconds(respawnTime);
        SpawnModifier();
    }


    private void SpawnModifier()
    {
        
        if (PhotonNetwork.IsConnected)
            spawnedModifier = PhotonNetwork.InstantiateRoomObject("Modifiers/" + orbManager.GetRandomOrb().name, spawnLocation, Quaternion.identity);
        else
            spawnedModifier = Instantiate(orbManager.GetRandomOrb());
    }
}
