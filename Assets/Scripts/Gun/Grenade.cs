using UnityEngine;

public class Grenade : MonoBehaviour
{

    private float damage = 0f;
    public float explodeDamage = 0f;

    public string explodeAudio;

    public float delay = 1f;
    public float blastRadius = 5f;
    public float blastForce = 700f;
    public GameObject explosionEffect;
    float countdown;
    bool hasExploded = false;
    public bool secondaryFire = false;

    private void Start()
    {
        countdown = delay;
    }

    private void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0f && !hasExploded)
        {
            Explode();
        }
    }

    void Explode()
    {
        GameObject effect = Instantiate(explosionEffect, transform.position, transform.rotation);
        FindObjectOfType<SoundManager>().PlaySound(explodeAudio);
        Collider[] colliders = Physics.OverlapSphere(transform.position, blastRadius);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(blastForce, transform.position, blastRadius);
            }
            if (nearbyObject.gameObject.CompareTag("Enemy") | nearbyObject.gameObject.CompareTag("Player"))
            {
                Target target = nearbyObject.gameObject.GetComponent<Target>();
                target.TakeDamage(explodeDamage);
            }
        }
        Destroy(effect, 0.25f);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (secondaryFire && (collision.gameObject.CompareTag("Enemy") | collision.gameObject.CompareTag("Player")))
            {
                Target target = collision.gameObject.GetComponent<Target>();
                target.TakeDamage(damage);
                Explode();
                secondaryFire = false;
            }
    }

    public void SetDamage(float d)
    {
        damage = d;
    }

    public void SetSecondary(bool s)
    {
        secondaryFire = s;
    }
}
