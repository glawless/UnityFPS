using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBeam : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CountDown());
        
    }

    IEnumerator CountDown()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
