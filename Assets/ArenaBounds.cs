using UnityEngine;
using TMPro;

public class ArenaBounds : MonoBehaviour
{
    [Header("Bounds")]
    public float minY = -10f;

    [Header("References")]
    public Transform player;
    public Transform playerSpawn;
    public Enemy[] enemies; // Assign all enemies in Inspector
    public TextMeshProUGUI resultText; // UI text for Win/Lose messages

    private bool gameEnded = false;

    private void Update()
    {
        if (gameEnded) return;

        // Check player
        HandleOutOfBounds(player.gameObject, isPlayer: true);

        // Check enemies
        foreach (Enemy enemy in enemies)
        {
            if (enemy != null)
            {
                HandleOutOfBounds(enemy.gameObject, isPlayer: false);
            }
        }

        // Check win condition
        if (AllEnemiesDown() && !gameEnded)
        {
            WinGame();
        }
    }

    private void HandleOutOfBounds(GameObject obj, bool isPlayer)
    {
        if (obj.transform.position.y < minY)
        {
            if (isPlayer)
            {
                RespawnPlayer();
            }
            else
            {
                Debug.Log(obj.name + " fell out of bounds!");
                Destroy(obj);
            }
        }
    }

    private void RespawnPlayer()
    {
        Debug.Log("Player fell out of bounds — respawning...");

        // Move player back to spawn
        player.position = playerSpawn.position;

        // Reset velocity if the player uses Rigidbody
        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // Optional: reset SurfCharacter velocity too
        Fragsurf.Movement.SurfCharacter surf = player.GetComponent<Fragsurf.Movement.SurfCharacter>();
        if (surf != null)
        {
            surf.moveData.velocity = Vector3.zero;
        }
    }

    private bool AllEnemiesDown()
    {
        foreach (Enemy enemy in enemies)
        {
            if (enemy != null && enemy.transform.position.y > minY)
            {
                return false;
            }
        }
        return true;
    }

    private void WinGame()
    {
        gameEnded = true;
        Debug.Log("Player won!");
        if (resultText != null)
            resultText.text = "You Win!";
    }
}
