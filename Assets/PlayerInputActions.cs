using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputActions : MonoBehaviour
{
    public InputSystem_Actions inputActions;
    public bool isPlayerOne = true;

    public bool AttackPressed { get; private set; }

    private void Awake()
    {
        inputActions = new InputSystem_Actions();

            inputActions.Player.Attack.performed += ctx => AttackPressed = true;

    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    private void LateUpdate()
    {
        // Reset every frame so you only get one press per input
        AttackPressed = false;
    }

    public void ResetAttack()
    {
        AttackPressed = false;
    }
}
