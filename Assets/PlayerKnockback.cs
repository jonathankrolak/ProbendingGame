using UnityEngine;

public class PlayerKnockback : MonoBehaviour
{
    private Fragsurf.Movement.SurfCharacter surfCharacter;

    private void Awake()
    {
        surfCharacter = GetComponent<Fragsurf.Movement.SurfCharacter>();
    }

    public void ApplyKnockback(Vector3 direction, float force)
    {
        if (surfCharacter == null) return;

        // normalize direction to ensure consistent knockback
        direction.Normalize();

        // add force to the surfCharacter's velocity
        surfCharacter.moveData.velocity += direction * force;
    }
}
