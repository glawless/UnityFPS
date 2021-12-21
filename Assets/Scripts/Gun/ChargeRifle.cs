using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class ChargeRifle : MonoBehaviour
{
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

    [System.NonSerialized]
    public int bulletsLeft;
    int bulletsShot;

    bool shooting;
    [System.NonSerialized]
    public bool readyToShoot;
    [System.NonSerialized]
    public bool reloading;

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

    //Charge Values
    public float chargeRate;
    public float chargeMax;
    public float chargedAmount;
    public string chargeStartAudio;
    public string chargeOneAudio;
    public string chargeTwoAudio;
    public string chargeThreeAudio;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }


    private void Update()
    {
        MyInput();

        // Set ammo display
        if (ammunitionDisplay != null)
        {
            ammunitionDisplay.SetText(bulletsLeft / bulletsPerTap + " / " + magazineSize / bulletsPerTap);
        }
    }

    private void MyInput()
    {
        shooting = Input.GetKeyDown(KeyCode.Mouse0);
        bool charging = Input.GetKey(KeyCode.Mouse0);

        // Reloading
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) 
            Reload();

        // Reload automatically when trying to shoot with no ammo
        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0) 
            Reload();

        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            FindObjectOfType<SoundManager>().PlaySound(chargeStartAudio);
        }
        if (readyToShoot && charging && !reloading && bulletsLeft > 0)
        {
            if (chargedAmount < chargeMax)
                chargedAmount += Time.deltaTime * chargeRate;
            if (chargedAmount < chargeMax * 0.5f)
            {
                FindObjectOfType<SoundManager>().PlaySound(chargeOneAudio);
            }
            else if (chargedAmount < chargeMax)
            {
                FindObjectOfType<SoundManager>().PlaySound(chargeTwoAudio);
            }
            else if (chargedAmount >= chargeMax)
            {
                FindObjectOfType<SoundManager>().PlaySound(chargeThreeAudio);
            }
        }
        else if (!charging && chargedAmount >= chargeMax)
        {
            chargedAmount = 0;
            FindObjectOfType<SoundManager>().StopSound(chargeOneAudio);
            FindObjectOfType<SoundManager>().StopSound(chargeTwoAudio);
            FindObjectOfType<SoundManager>().StopSound(chargeThreeAudio);
            bulletsShot = bulletsPerTap;
            Shoot();
        }
        else if (chargedAmount > 0)
        {
            chargedAmount -= Time.deltaTime * chargeRate;
            FindObjectOfType<SoundManager>().StopSound(chargeOneAudio);
            FindObjectOfType<SoundManager>().StopSound(chargeTwoAudio);
            FindObjectOfType<SoundManager>().StopSound(chargeThreeAudio);
        }
    }
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
        if (Physics.Raycast(ray, out hit))
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

        bulletsLeft--;
        bulletsShot--;

        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }

        if (bulletsShot > 0 && bulletsLeft > 0)
        {
            Invoke("Shoot", timeBetweenShots);
        }
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
        bulletsLeft = magazineSize;
        reloading = false;
    }
}


