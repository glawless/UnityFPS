using UnityEngine;

public class DamageText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 1f);
        transform.localPosition += new Vector3(0, 2, 0);
    }

    void Update()
    {
        if (Camera.main != null)
            transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
    }
}
