using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private float attackPower = 1f;
    [SerializeField] private float speed = 10f;
    //[SerializeField] private float baseKnockbackForce = 5f;

    private Vector3 moveDirection;
    private bool hasHit = false;

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
        if (hasHit) return;

        if (other.CompareTag("Enemy"))
        {
            hasHit = true;
            Debug.Log("Projectile hit: " + other.name);

            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                // Direction from projectile to enemy
                Vector3 hitDirection = (other.transform.position - transform.position).normalized;

                // Pass knockback and damage through the Enemy script
                enemy.TakeHit(hitDirection, attackPower); 
                // (damage / 10f) keeps scaling consistent, tweak as needed
            }
        } else if (other.CompareTag("Player"))
            {
                PlayerKnockback kb = other.GetComponentInParent<PlayerKnockback>();
                Debug.Log("hti player");
            if (kb != null)
            {
                Vector3 dir = (other.transform.position - transform.position).normalized;
                float knockbackForce = attackPower * (1 + (100 / 100f));
                kb.ApplyKnockback(dir, knockbackForce);
                Debug.Log("player knockback");

            }
            }


        Destroy(gameObject); // Always destroy projectile after a collision
    }
}
