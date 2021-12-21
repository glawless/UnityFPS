using UnityEngine;

public class DShotty : MonoBehaviour
{
    ProjectileGun gunScript;
    public int holdValue;
    // Start is called before the first frame update
    void Start()
    {
        gunScript = GetComponent<ProjectileGun>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            holdValue = gunScript.bulletsPerTap;
            while (gunScript.bulletsLeft > 0)
            {
                gunScript.bulletsPerTap = gunScript.magazineSize;
                gunScript.Shoot();
            }
            gunScript.bulletsPerTap = holdValue;
        }
    }
}
