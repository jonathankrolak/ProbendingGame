using UnityEngine;

public class PlayerKnockback : MonoBehaviour
{
    private Fragsurf.Movement.SurfCharacter surfCharacter;

    [Header("Smash-Style Knockback")]
    public float damagePercent = 0f;   // starts at 0, increases on each hit
    [SerializeField] private float baseKnockbackMultiplier = 1f; 
    [SerializeField] private float damageKnockbackMultiplier = 1f; // extra scaling per % damage

    private void Awake()
    {
        surfCharacter = GetComponent<Fragsurf.Movement.SurfCharacter>();
    }

    /// <summary>
    /// Applies damage and knockback to the player
    /// </summary>
    /// <param name="direction">Direction of the knockback</param>
    /// <param name="force">Base knockback force of the attack</param>
    /// <param name="damage">Damage percentage to add</param>
    public void ApplyKnockback(Vector3 direction, float force, float damage)
    {
        if (surfCharacter == null) return;

        // Add damage first
        damagePercent += damage;

        // Normalize knockback direction
        direction.Normalize();

        // Scale knockback with accumulated damage
        float scaledForce = force * (baseKnockbackMultiplier + (damagePercent / 100f) * damageKnockbackMultiplier);

        // Apply to velocity
        surfCharacter.moveData.velocity += direction * scaledForce;
    }

    /// <summary>
    /// Optional: Reset damage percent (on respawn, KO, etc.)
    /// </summary>
    public void ResetDamage()
    {
        damagePercent = 0f;
    }
}
