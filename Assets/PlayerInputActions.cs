using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputActions : MonoBehaviour
{
    //public InputSystem_Actions inputActions;

    public InputActionAsset inputAsset;
    public InputActionMap player;
    private InputAction attack;

    private void OnEnable(){
        attack = player.FindAction("Attack");
        attack.performed += ctx => AttackPressed = true;
        player.Enable();
    }
    private void OnDisable(){
       // jump = player.FindAction("Jump");
        attack.performed -= ctx => AttackPressed = true;
        player.Disable();
    }

    //public bool isPlayerOne = true;

    public bool AttackPressed { get; private set; }

    private void Awake()
    {
        //inputActions = new InputSystem_Actions();

        inputAsset = this.GetComponent<PlayerInput>().actions;
        player = inputAsset.FindActionMap("Player");

         //   inputActions.Player.Attack.performed += ctx => AttackPressed = true;

    }

    //private void OnEnable() => inputActions.Enable();
    //private void OnDisable() => inputActions.Disable();

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
