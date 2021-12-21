using UnityEngine;


public class Bullet : MonoBehaviour
{

    private float damage = 0f;
    private float headshotMultiplier = 1f;
    private GameObject body;

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.CompareTag("Head"))
        {
            damage *= headshotMultiplier;
        }

        if (!collision.collider.CompareTag("Bullet"))
        {
            body = collision.gameObject;
            Destroy(gameObject);
        }
        if (body != null)
        {
            if (body.CompareTag("Enemy"))
            {
                Target target = body.GetComponent<Target>();
                FindObjectOfType<SoundManager>().PlaySound("HitSound");
                target.TakeDamage(damage);
            }

            if (body.CompareTag("Player"))
            {
                Target target = body.GetComponent<Target>();
                target.TakeDamage(damage);
            }
        }
    }

    public void SetDamage(float d)
    {
        damage = d;
    }

    public void SetMultiplier(float d)
    {
        headshotMultiplier = d;
    }
}