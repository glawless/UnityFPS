using UnityEngine;
using Photon.Pun;

public class WeaponSwapV2 : MonoBehaviour
{
    public int selectedWeapon = 0;
    private int previousSelectedWeapon;
    private ProjectileGun chosenWeapon;
    private PhotonView PV;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        SelectWeapon(selectedWeapon);
    }

    // Update is called once per frame
    void Update()
    {
        if (PV.IsMine)
        {
            if (!chosenWeapon.reloading)
            {
                if (Input.GetKeyDown(KeyCode.V))
                {
                    previousSelectedWeapon = selectedWeapon;
                    selectedWeapon = 0;
                }

                if (Input.GetKeyDown(KeyCode.Alpha1) | Input.GetKeyDown(KeyCode.Q))
                {
                    previousSelectedWeapon = selectedWeapon;
                    selectedWeapon = 1;
                }

                if (Input.GetKeyDown(KeyCode.Alpha2) | Input.GetKeyDown(KeyCode.E))
                {
                    previousSelectedWeapon = selectedWeapon;
                    selectedWeapon = 2;
                }
            }
            if (previousSelectedWeapon != selectedWeapon)
                PV.RPC("SelectWeapon", RpcTarget.AllBuffered, selectedWeapon);
        }
    }

    [PunRPC]
    public void SelectWeapon(int selection)
    {
        selectedWeapon = selection;
        foreach(Transform weaponTrans in transform)
        {
            ProjectileGun weapon = weaponTrans.GetComponent<ProjectileGun>();
            if (weapon.active && weapon.weaponSlot == selectedWeapon)
            {
                previousSelectedWeapon = selectedWeapon;
                weapon.gameObject.SetActive(true);
                chosenWeapon = weapon.gameObject.GetComponent<ProjectileGun>();
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
        }
    }
}
