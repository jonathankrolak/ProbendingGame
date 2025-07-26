using UnityEngine;

public class PlayerInputActions : MonoBehaviour
{
    public bool AttackPressed { get; private set; }

    void Update()
    {
        AttackPressed = Input.GetMouseButtonDown(0); // Left mouse button
    }

    public void ResetAttack()
    {
        AttackPressed = false;
    }
} 