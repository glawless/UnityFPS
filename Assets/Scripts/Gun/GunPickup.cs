using System.Collections;
using UnityEngine;
using Photon.Pun;

public class GunPickup : MonoBehaviour
{
    [SerializeField]
    private int idToActivate;
    public int magIncrease;
    public bool deactivate;

    private void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * 100);
        if (deactivate)
        {
            Deactivate(transform);
            foreach (Transform element in GetComponentInChildren<Transform>())
            {
                Deactivate(element);
                foreach (Transform element2 in element.GetComponentInChildren<Transform>())
                {
                    Deactivate(element2);
                }
            }
        }
    }

    private void Deactivate(Transform transform)
    {
        if (transform.GetComponent<MeshRenderer>() != null)
            transform.GetComponent<MeshRenderer>().enabled = false;
        if (transform.GetComponentInChildren<MeshRenderer>() != null)
            transform.GetComponentInChildren<MeshRenderer>().enabled = false;
        if (transform.GetComponent<Collider>() != null)
            transform.GetComponent<Collider>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerMesh"))
        {
            StartCoroutine(PickUp(other));
        }
    }

    IEnumerator PickUp(Collider player)
    {
        Transform playerT = player.transform.parent.Find("WeaponPivot/GunPosition/WeaponSwitcher");
        int currWeaponSlot = playerT.GetComponent<WeaponSwapV2>().selectedWeapon;
        bool isKnife = currWeaponSlot == 0;
        bool isActiveElsewhere = false;
        if (!isKnife)
        {
            ProjectileGun newWeapon = null;
            FindObjectOfType<SoundManager>().PlaySound("GunPickUp");

            foreach (Transform weaponTrans in playerT)
            {
                ProjectileGun weapon = weaponTrans.GetComponent<ProjectileGun>();
                if (weapon.ID == idToActivate && weapon.active && weapon.weaponSlot != currWeaponSlot)
                {
                    isActiveElsewhere = true;
                    weapon.magsLeft += magIncrease;
                }
            }

            if (!isActiveElsewhere)
            {
                //Iterate through all weapons in WeaponSwitcher,
                //    to deactivate current equipped weapon,
                //    and find new weapon to equip.
                foreach (Transform weaponTrans in playerT)
                {
                    ProjectileGun weapon = weaponTrans.GetComponent<ProjectileGun>();
                    //Deactivate any weapons in current slot.
                    if (weapon.active && weapon.weaponSlot == currWeaponSlot)
                        weapon.active = false;
                    //Make sure weapon is correct ID and not already active in another slot.
                    if (!weapon.active && weapon.ID == idToActivate)
                        newWeapon = weapon;
                }

                //Activate new weapon and give ammo to that gun.
                newWeapon.weaponSlot = currWeaponSlot;
                newWeapon.active = true;
                newWeapon.magsLeft += magIncrease;
                //player.transform.Find("MaleDummy/WeaponPivot/GunPosition/WeaponSwitcher").GetComponent<WeaponSwapV2>().selectedWeapon = currWeaponSlot;
                playerT.GetComponent<WeaponSwapV2>().SelectWeapon(currWeaponSlot);
            }
        }
        yield return new WaitForSeconds(10);

        PhotonView PV = GameObject.Find("OrbManager").GetComponent<PhotonView>();
        if (PhotonNetwork.IsConnected && PV.IsMine)
            PhotonNetwork.Destroy(gameObject);
        else
            Destroy(gameObject);
    }
}

    
