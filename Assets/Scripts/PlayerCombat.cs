using UnityEngine;
using System.Collections;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private BasicAttack attackAbility;
    [SerializeField] private float attackCooldown = 0.5f;

    private PlayerInputActions inputActions;
    private bool isRightPunchNext = true;
    private bool isAttackOnCooldown = false;

    private static readonly int attackRightHash = Animator.StringToHash("attackRight");
    private static readonly int attackLeftHash = Animator.StringToHash("attackLeft");

    private void Awake()
    {
        inputActions = GetComponent<PlayerInputActions>();
    }

    private void Update()
    {
        HandleAttack();
    }

    private void HandleAttack()
    {
        if (inputActions.AttackPressed && !isAttackOnCooldown)
        {
            if (isRightPunchNext)
                _animator.SetTrigger(attackRightHash);
            else
                _animator.SetTrigger(attackLeftHash);

            isRightPunchNext = !isRightPunchNext;
            StartCoroutine(AttackCooldownRoutine());
            inputActions.ResetAttack();
        }
    }

    private IEnumerator AttackCooldownRoutine()
    {
        isAttackOnCooldown = true;
        yield return new WaitForSeconds(attackCooldown);
        isAttackOnCooldown = false;
    }

    // 👇 Call this from the animation event
    public void FireballAnimationEvent()
    {
        attackAbility.PerformAttack();
    }
}

