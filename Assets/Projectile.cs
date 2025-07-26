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

    private void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Enemy") || other.CompareTag("Player"))
    {
        Debug.Log("Collided");
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Debug.Log("Has rb");
            Vector3 knockbackDir = (other.transform.position - transform.position).normalized;
            rb.AddForce(knockbackDir * knockbackForce, ForceMode.Impulse);
        }

        // TODO: Apply damage if needed

        Destroy(gameObject);
    }
    else
    {
        // Optionally destroy projectile on other collisions
         //Destroy(gameObject);
    }
}

}


