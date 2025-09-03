using UnityEngine;

public class FireballAttack : BasicAttack
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Camera playerCamera;

    public override void PerformAttack()
    {
        Vector3 targetDirection = GetAttackDirectionFromCamera();
        SpawnProjectile(targetDirection);
    }

    private Vector3 GetAttackDirectionFromCamera()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)); // Center of camera
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            Vector3 direction = (hit.point - spawnPoint.position).normalized;
            return direction;
        }

        // Fallback if nothing is hit
        return ray.direction;
    }

    private void SpawnProjectile(Vector3 direction)
    {
        GameObject projectile = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.LookRotation(direction));
        projectile.GetComponent<Projectile>().Launch(direction);
    }
}
