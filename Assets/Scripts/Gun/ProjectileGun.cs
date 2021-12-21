using UnityEngine;
using TMPro;
using Photon.Pun;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class ProjectileGun : MonoBehaviourPun
{
    //Inventory
    public bool active;
    public int ID;
    public int weaponSlot;

    // Bullet reference
    public GameObject bullet;

    // Bullet force
    public float shootForce;
    public float upwardForce;

    // Gun stats
    public float timeBetweenShooting;
    public float spread;
    public float reloadTime;
    public float timeBetweenShots;
    public float damage;
    public float headshotMultiplier;
    public float range;

    public int magazineSize;
    public int bulletsPerTap;
    public int magsLeft;
    public bool unlimitedMags;
    public bool unlimitedAmmo;

    [System.NonSerialized]
    public int bulletsLeft;
    int bulletsShot;

    bool shooting;
    [System.NonSerialized]
    public bool readyToShoot;
    [System.NonSerialized]
    public bool reloading;
    public bool allowReload;

    public bool allowButtonHold;
    [System.NonSerialized]
    public bool secondaryFire;

    // References
    public Camera fpsCam;
    public Transform attackPoint;

    // Graphics
    public ParticleSystem muzzleFlash;
    public TextMeshProUGUI ammunitionDisplay;

    // Audio
    public string shoot_audio;
    public string reload_audio;

    // Bug fixing
    public bool allowInvoke = true;

    private PhotonView PV;


    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
        PV = GetComponentInParent<PhotonView>();
    }

    private void Update()
    {
        if (PV.IsMine)
        {
            MyInput();

            if (unlimitedAmmo)
                bulletsLeft = 999;
            if (unlimitedMags)
                magsLeft = 999;
            allowReload = (magsLeft > 0);

            // Set ammo display
            if (ammunitionDisplay != null)
            {
                string bullets = bulletsLeft.ToString();
                string mags = magsLeft.ToString();
                if (bulletsLeft >= 999)
                    bullets = "\u221E";
                if (magsLeft >= 999)
                    mags = "\u221E";
                ammunitionDisplay.SetText(bullets + " / " + mags);
            }
            else
            {
                if (GameObject.Find("AmmoCountText") != null)
                    ammunitionDisplay = GameObject.Find("AmmoCountText").GetComponent<TextMeshProUGUI>();
            }
        }
    }

    private void MyInput()
    {
        // Check if allowed to hold down button and take corresponding input
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        // Reloading
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading && allowReload)
            Reload();

        // Reload automatically when trying to shoot with no ammo
        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0 && allowReload)
            Reload();


        // Shooting
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            // Set bullets shot to 0
            bulletsShot = bulletsPerTap;

            photonView.RPC("Shoot", RpcTarget.AllViaServer);
            PhotonNetwork.SendAllOutgoingCommands();
        }
    }

    [PunRPC]
    public void Shoot()
    {
        if (muzzleFlash != null)
            muzzleFlash.Play();
        FindObjectOfType<SoundManager>().PlaySound(shoot_audio);

        readyToShoot = false;

        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        // Check if ray hits something
        Vector3 targetPoint;
        if(Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(75); // Set random range for bullet to stop
        }

        // Calculate direction from attackPoint to targetPoint
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        // Calculate spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        // Calculate new direction with spread
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);

        // Instantiate bullet/projectile
        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);

        // Equation for flight time of projectile
        float time = range / shootForce;
        Destroy(currentBullet, time);

        // Rotate bullet to shoot direction
        currentBullet.transform.forward = directionWithSpread.normalized;

        // Add forces to bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(fpsCam.transform.up * upwardForce, ForceMode.Impulse);
        //Check if Bullet or Grenade
        if (currentBullet.GetComponent<Bullet>() != null)
        {
            currentBullet.GetComponent<Bullet>().SetDamage(damage);
            currentBullet.GetComponent<Bullet>().SetMultiplier(headshotMultiplier);
        }
        else if (currentBullet.GetComponent<Grenade>() != null)
        {
            currentBullet.GetComponent<Grenade>().SetDamage(damage);
            if (secondaryFire)
            {
                currentBullet.GetComponent<Grenade>().SetSecondary(secondaryFire);
                currentBullet.GetComponent<Grenade>().delay = 1f;
                currentBullet.GetComponent<Grenade>().explodeDamage = 50f;
                currentBullet.GetComponent<Grenade>().GetComponent<Rigidbody>().useGravity = false;
            }
                
        }
            
        bulletsLeft--;
        bulletsShot--;

        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }

        if(bulletsShot > 0 && bulletsLeft > 0)
        {
            StartCoroutine(DelayShot(timeBetweenShots));
        }
    }

    IEnumerator DelayShot(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        photonView.RPC("Shoot", RpcTarget.All);
        PhotonNetwork.SendAllOutgoingCommands();
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }


    public void Reload()
    {
        FindObjectOfType<SoundManager>().PlaySound(reload_audio);
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        magsLeft--;
        bulletsLeft = magazineSize;
        reloading = false;
    }
}
