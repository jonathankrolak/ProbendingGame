// Animation/PlayerAnimation.cs
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float locomotionBlendSpeed = 6f;

    private PlayerMovementAdvanced _movement;
    private Vector3 _currentBlendInput = Vector3.zero;

    private static readonly int inputXHash = Animator.StringToHash("inputX");
    private static readonly int inputYHash = Animator.StringToHash("inputY");
    private static readonly int inputMagnitudeHash = Animator.StringToHash("inputMagnitude");
    private static readonly int isGroundedHash = Animator.StringToHash("isGrounded");

    private void Awake()
    {
        _movement = GetComponent<PlayerMovementAdvanced>();
    }

    private void Update()
    {
        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        bool isGrounded = _movement.grounded;
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _currentBlendInput = Vector3.Lerp(_currentBlendInput, input, locomotionBlendSpeed * Time.deltaTime);

        _animator.SetBool(isGroundedHash, isGrounded);
        _animator.SetFloat(inputXHash, _currentBlendInput.x);
        _animator.SetFloat(inputYHash, _currentBlendInput.y);
        _animator.SetFloat(inputMagnitudeHash, _currentBlendInput.magnitude);
    }
}
