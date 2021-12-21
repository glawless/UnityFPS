using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOptionsApplicator : MonoBehaviour
{
    public OptionsHolder oh;
    GameObject pManager;
    GameObject sManager;
    GameObject oManager;
    GameObject bManager;

    private void Awake()
    {
        oh = GameObject.Find("OptionsHolder").GetComponent<OptionsHolder>();
        pManager = GameObject.Find("PlayerManager");
        sManager = GameObject.Find("SoundManager");
        oManager = GameObject.Find("OrbManager");
        bManager = GameObject.Find("BotManager");

        sManager.GetComponent<SoundManager>().globalVolume = oh.volume;
        oManager.GetComponent<OrbManager>().enabled = oh.orbsActive;
        bManager.GetComponent<BotManager>().enabled = oh.botsActive;
    }
}
