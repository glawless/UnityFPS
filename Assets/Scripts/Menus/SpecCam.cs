using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpecCam : MonoBehaviour
{
    public GameObject player;
    private AudioListener audioListener;
    private Camera cam;
    private GameObject playerCanvas;

    public int id;
    public GameObject masterSpec;

    private void Start()
    {
        audioListener = GetComponent<AudioListener>();
        cam = GetComponent<Camera>();
        playerCanvas = GameObject.Find("PlayerCanvas");
    }
    // Update is called once per frame
    void Update()
    {
        if (id == 1)
            audioListener.enabled = true;

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.GetComponent<PhotonView>().IsMine)
            {
                cam.enabled = false;
                audioListener.enabled = false;
                foreach (Transform can in playerCanvas.transform)
                {
                    can.gameObject.SetActive(true);
                }
            }
        }
        bool masterSpecEnabled = false;
        if (masterSpec.GetComponent<Camera>() != null)
            masterSpecEnabled = masterSpec.GetComponent<Camera>().enabled;
        if (audioListener.enabled || masterSpecEnabled)
        {
            cam.enabled = true;
            foreach (Transform can in playerCanvas.transform)
            {
                can.gameObject.SetActive(false);
            }
        }
    }
}
