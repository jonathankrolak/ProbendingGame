using UnityEngine;
using TMPro;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public float damagePercent = 0f; // starts at 0%

    [Header("References")]
    public Rigidbody rb;
    public TextMeshProUGUI damageText;
    [SerializeField] private Transform playerCam; // reference to the player's camera
    [SerializeField] private Transform attackPoint; // where fireballs spawn
    [SerializeField] private GameObject projectilePrefab; // the projectile prefab
    [SerializeField] private Transform playerTarget; // the player object to aim at

    [Header("Attack Settings")]
    public float projectileSpeed = 10f;
    public float minAttackDelay = 1f;
    public float maxAttackDelay = 10f;

    private void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        UpdateDamageText();

        // start attack loop
        if (attackPoint != null && projectilePrefab != null && playerTarget != null)
        {
            StartCoroutine(AttackLoop());
        }
    }

    private void Update()
    {
        // Make damage text always face the camera
        if (damageText != null && playerCam != null)
        {
            damageText.transform.rotation = Quaternion.LookRotation(damageText.transform.position - playerCam.position);
        }
    }

    public void TakeHit(Vector3 direction, float damage)
    {
        // Increase damage percentage
        damagePercent += damage;
        UpdateDamageText();

        // Calculate knockback based on formula
        float knockbackForce = 7.5f * (1 + (damagePercent / 100f));
        rb.AddForce(direction.normalized * knockbackForce, ForceMode.Impulse);
    }

    private void UpdateDamageText()
    {
        if (damageText != null)
        {
            damageText.text = Mathf.RoundToInt(damagePercent) + "%";
        }
    }

    private IEnumerator AttackLoop()
    {
        while (true)
        {
            float delay = Random.Range(minAttackDelay, maxAttackDelay);
            yield return new WaitForSeconds(delay);

            ShootAtPlayer();
        }
    }

    private void ShootAtPlayer()
    {
        if (attackPoint == null || playerTarget == null || projectilePrefab == null) return;

        // spawn projectile
        GameObject proj = Instantiate(projectilePrefab, attackPoint.position, Quaternion.identity);

        // find direction to player
        Vector3 dir = (playerTarget.position - attackPoint.position).normalized;

        // if projectile uses Rigidbody
        Rigidbody projRb = proj.GetComponent<Rigidbody>();
        if (projRb != null)
        {
            projRb.linearVelocity = dir * projectileSpeed;
        }
        else
        {
            // if using your Projectile script
            Projectile p = proj.GetComponent<Projectile>();
            if (p != null)
            {
                p.Launch(dir);
            }
        }
    }
}
