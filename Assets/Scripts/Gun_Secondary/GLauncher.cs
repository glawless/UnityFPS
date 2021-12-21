using UnityEngine;

public class GLauncher : MonoBehaviour
{
    ProjectileGun playerGun;

    // Start is called before the first frame update
    void Start()
    {
        playerGun = GetComponent<ProjectileGun>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            //Set Secondary Values
            playerGun.shootForce /= 2;
            playerGun.secondaryFire = true;

            if (playerGun.bulletsLeft > 0 && playerGun.readyToShoot && !playerGun.reloading)
                playerGun.Shoot();
            else if (!playerGun.reloading && playerGun.bulletsLeft <= 0)
                playerGun.Reload();

            //Reset Gun Values
            playerGun.shootForce *= 2;
            playerGun.secondaryFire = false;
        }
    }
}
