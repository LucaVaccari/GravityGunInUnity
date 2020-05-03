using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class GasTank : MonoBehaviour
{
    [SerializeField] private float explosionForce = 10, explosionRadius = 5, velocityTreshold = 10;
    [SerializeField] private LayerMask layerMask;

    [SerializeField] private AudioClip explosionClip;
    [SerializeField] private GameObject particles;

    private Rigidbody rb;
    private AudioSource audioSource;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //magnitude is squared to compare faster
        if (rb.velocity.sqrMagnitude > velocityTreshold * velocityTreshold)
        {
            Explode();
        }
    }

    private void Explode()
    {
        //add explosion force to surrounding game objects
        Collider[] objs = Physics.OverlapSphere(transform.position, explosionRadius, layerMask);
        foreach (Collider obj in objs)
        {
            if (obj.GetComponent<Rigidbody>() != null)
                obj.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRadius, 1, ForceMode.VelocityChange);

            if (obj.GetComponent<Sniper>() != null)
            {
                Destroy(obj.gameObject);
                Debug.Log("SNIPER HIT");
            }
        }

        //audio
        if (explosionClip != null && audioSource != null)
            audioSource.PlayOneShot(explosionClip);

        //particles
        if (particles != null)
            Instantiate(particles, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
