using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public GameObject self;
    Target target;
    public float fillAmount;
    Image image;

    // Start is called before the first frame update
    void Start()
    {
        target = self.GetComponent<Target>();
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null && image != null)
        {
            fillAmount = target.curHealth / target.maxHealth;
            image.fillAmount = fillAmount;
        }
    }
}
