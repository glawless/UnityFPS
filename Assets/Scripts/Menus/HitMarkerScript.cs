using UnityEngine;
using UnityEngine.UI;

public class HitMarkerScript : MonoBehaviour
{
    [SerializeField]
    public float delayTime;
    float timer;
    public bool run = false;
    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    private void Update()
    {
        if (run)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                image.enabled = true;
            }
            if (timer <= 0)
            {
                run = false;
                timer = delayTime;
                image.enabled = false;
            }
        }
        
    }
}
