using UnityEngine;
using TMPro;
using Photon.Pun;

public class Target : MonoBehaviour
{
    public float maxHealth = 50f;
    public float curHealth;
    public TextMeshProUGUI healthDisplay;
    public GameObject hitMarker;
    public GameObject floatText;
    private HitMarkerScript hitScript;
    public bool isPlayer;
    private PhotonView PV;

    private void Start()
    {
        PV = GetComponent<PhotonView>();
        curHealth = maxHealth;
        if (!isPlayer)
        {
            hitMarker = GameObject.Find("HitMarker");
            if (hitMarker != null)
                hitScript = hitMarker.GetComponent<HitMarkerScript>();
        }
    }

    private void Update()
    {
        if (PV != null && PV.IsMine)
        {
            if (healthDisplay != null)
            {
                healthDisplay.SetText(curHealth + " / " + maxHealth);
            }
            else if (isPlayer)
            {
                if (GameObject.Find("HealthCountText") != null)
                    healthDisplay = GameObject.Find("HealthCountText").GetComponent<TextMeshProUGUI>();
            }
        }
    }

    public void TakeDamage(float amount)
    {
        if (gameObject.CompareTag("Enemy"))
        {
            if(hitScript != null)
                hitScript.run = true;
            if (floatText != null)
            {
                GameObject points = Instantiate(floatText, transform.position, Quaternion.identity);
                points.transform.GetChild(0).GetComponent<TextMesh>().text = amount.ToString();
            }
        }

        if (!PV.IsMine)
            return;

        curHealth -= amount;
        if (curHealth <= 0f)
        {
            Die();
        }
    }

    public void IncMaxHealth(float amount)
    {
        if (maxHealth < 200)
        {
            maxHealth *= amount;
        }
    }

    public void Heal(float amount)
    {
        if(curHealth < maxHealth)
        {
            if(curHealth + amount > maxHealth)
            {
                curHealth = maxHealth;
            }
            else
            {
                curHealth += amount;
            }  
        }
    }

    public void Die()
    {
        if (PhotonNetwork.IsConnected && PV.IsMine)
            PhotonNetwork.Destroy(gameObject);
        //else
            //Destroy(gameObject);
    }
}
