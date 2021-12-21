using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SuperSpeedOrb : MonoBehaviour
{
    //public GameObject pickupEffect;
    [SerializeField]
    private float speedMultiplyer = 2.0f;
    [SerializeField]
    private float duration = 4.0f;
    [SerializeField]
    private float maxWalkSpeed = 10.0f;

    private PlayerController pc;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerMesh"))
        {
            pc = other.GetComponentInParent<PlayerController>();

            if(pc.baseSpeed < maxWalkSpeed)
            {
                StartCoroutine(PickUp(other));
            }
        }
    }

    IEnumerator PickUp(Collider player)
    {
        //Instantiate(pickupEffect, transform.position, transform.rotation);
    
        pc.baseSpeed *= speedMultiplyer;
        pc.sprintSpeed *= speedMultiplyer;
        FindObjectOfType<SoundManager>().PlaySound("SpeedPower");

        yield return new WaitForSeconds(duration);

        pc.baseSpeed /= speedMultiplyer;
        pc.sprintSpeed /= speedMultiplyer;

        PhotonView PV = GameObject.Find("OrbManager").GetComponent<PhotonView>();
        if (PhotonNetwork.IsConnected && PV.IsMine)
            PhotonNetwork.Destroy(gameObject);
        else
            Destroy(gameObject);
    }
}
