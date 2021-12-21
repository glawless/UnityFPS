using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaDamage : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("PlayerMesh"))
        {
            Target playerHealth = collision.collider.GetComponentInParent<Target>();
            playerHealth.Die();
        }
    }
}
