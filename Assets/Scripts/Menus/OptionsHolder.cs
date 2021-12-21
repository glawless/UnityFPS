using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsHolder : MonoBehaviour
{
    public bool botsActive;
    public bool orbsActive;
    public float volume;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void BotToggle()
    {
        botsActive = !botsActive;
    }

    public void OrbToggle()
    {
        orbsActive = !orbsActive;
    }

    public void VolumeChange(float newValue)
    {
        volume = newValue;
    }
}
