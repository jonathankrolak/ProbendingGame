using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float knockbackForce = 5f; // Add knockback force strength

    private Vector3 moveDirection;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.position += moveDirection * speed * Time.deltaTime;
    }

    public void Launch(Vector3 direction)
    {
        moveDirection = direction.normalized;
    }

    private bool hasHit = false;

private void OnTriggerEnter(Collider other)
{
    if (hasHit) return;

    if (other.CompareTag("Enemy") || other.CompareTag("Player"))
    {
        hasHit = true;
        Debug.Log("Collided with " + other.name);

        Vector3 knockbackDir = (other.transform.position - transform.position).normalized;
//knockbackDir.y = 0f; // Remove vertical launch
knockbackDir.Normalize();

Rigidbody rb = other.GetComponent<Rigidbody>();
if (rb != null)
{
    // Option 1: Directly set velocity (overwrites motion briefly)
    rb.AddForce(knockbackDir * knockbackForce, ForceMode.Impulse);


    // Optional: add a vertical pop (like a stagger)
    // rb.velocity += Vector3.up * 2f;
}


        // TODO: Apply damage here if needed

        Destroy(gameObject);
    }
    else
    {
        Destroy(gameObject);
    }
}

}




