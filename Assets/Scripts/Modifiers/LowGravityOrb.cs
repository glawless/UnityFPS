using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LowGravityOrb : MonoBehaviour
{
    //public GameObject pickupEffect;
    [SerializeField]
    private float gravity = 3.0f;
    [SerializeField]
    private float duration = 5.0f;

    private Rigidbody rb;
    private bool isOn;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerMesh"))
        {
            rb = other.GetComponentInParent<Rigidbody>();
            isOn = true;
            StartCoroutine(PickUp(other));
        }
    }

    IEnumerator PickUp(Collider player)
    {
        //Instantiate(pickupEffect, transform.position, transform.rotation);

        FindObjectOfType<SoundManager>().PlaySound("GravPower");

        yield return new WaitForSeconds(duration);

        rb.useGravity = true;

        PhotonView PV = GameObject.Find("OrbManager").GetComponent<PhotonView>();
        if (PhotonNetwork.IsConnected && PV.IsMine)
            PhotonNetwork.Destroy(gameObject);
        else
            Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        if (isOn)
        {
            rb.useGravity = false;
            rb.AddForce(new Vector3(0, -1.0f, 0) * rb.mass * gravity);
        }

    }
}
