using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HealthOrb : MonoBehaviour
{

    //public GameObject pickupEffect;
    [SerializeField]
    private float healAmount = 25.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerMesh"))
        {
           StartCoroutine(PickUp(other));  
        }
    }

    IEnumerator PickUp(Collider player)
    {
        //Instantiate(pickupEffect, transform.position, transform.rotation);

        Target playerHealth = player.GetComponentInParent<Target>();
        playerHealth.Heal(healAmount);
        FindObjectOfType<SoundManager>().PlaySound("HealPower");
        yield return new WaitForSeconds(10);

        PhotonView PV = GameObject.Find("OrbManager").GetComponent<PhotonView>();
        if (PhotonNetwork.IsConnected && PV.IsMine)
            PhotonNetwork.Destroy(gameObject);
        else
            Destroy(gameObject);
    }
}
