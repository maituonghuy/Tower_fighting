using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;

    public void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayRunAnimation()
    {
        animator.SetBool("isRunning", true);
    }

    public void StopRunAnimation()
    {
        animator.SetBool("isRunning", false);
    }

    public void PlayJumpAnimation()
    {
        animator.SetTrigger("jump");
    }

    public void PlayAttackAnimation()
    {
        animator.SetTrigger("attack");
    }

    public void PlayHitAnimation()
    {
        animator.SetTrigger("hit");
    }

    public void PlayDeathAnimation()
    {
        animator.SetTrigger("death");
    }
}
