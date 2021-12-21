using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{
    public int selectedWeapon = 0;
    private ProjectileGun chosenWeapon;
    // Start is called before the first frame update
    void Start()
    {
        SelectWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        if (!chosenWeapon.reloading)
        {
            int previousSelectedWeapon = selectedWeapon;

            if (Input.GetKeyDown(KeyCode.V))
                selectedWeapon = 0;

            if (Input.GetKeyDown(KeyCode.Alpha1) && transform.childCount >= 2)
                selectedWeapon = 1;

            if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 3)
                selectedWeapon = 2;

            if (Input.GetKeyDown(KeyCode.Alpha3) && transform.childCount >= 4)
                selectedWeapon = 3;

            if (Input.GetKeyDown(KeyCode.Alpha4) && transform.childCount >= 5)
                selectedWeapon = 4;

            if (Input.GetKeyDown(KeyCode.Alpha5) && transform.childCount >= 6)
                selectedWeapon = 5;

            if (Input.GetKeyDown(KeyCode.Alpha6) && transform.childCount >= 7)
                selectedWeapon = 6;

            if (Input.GetKeyDown(KeyCode.Alpha7) && transform.childCount >= 8)
                selectedWeapon = 7;

            if (Input.GetKeyDown(KeyCode.Alpha8) && transform.childCount >= 9)
                selectedWeapon = 8;

            if ((Input.GetKeyDown(KeyCode.Q) | Input.GetAxis("Mouse ScrollWheel") < 0f) && previousSelectedWeapon != 0)
                selectedWeapon = previousSelectedWeapon - 1;

            if ((Input.GetKeyDown(KeyCode.E) | Input.GetAxis("Mouse ScrollWheel") > 0f) && transform.childCount > (previousSelectedWeapon + 1))
                selectedWeapon = previousSelectedWeapon + 1;

            if (previousSelectedWeapon != selectedWeapon)
                SelectWeapon();
        }
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach(Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
                chosenWeapon = weapon.gameObject.GetComponent<ProjectileGun>();
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }
}
