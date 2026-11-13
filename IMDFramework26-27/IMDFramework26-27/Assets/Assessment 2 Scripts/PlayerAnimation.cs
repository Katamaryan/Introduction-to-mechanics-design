using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] Animator animator;

    public void AnimationParamChanged(float direction, bool jumpInput, bool grounded, float yVelocity)
    {
        if (direction == 0)
            animator.SetFloat("Direction", 0);
        else if (direction < 0)
            animator.SetFloat("Direction", 0.5f);
        else if (direction > 0)
            animator.SetFloat("Direction", 1);

        animator.SetBool("JumpInput", jumpInput);
        animator.SetBool("Grounded", grounded);

        if (yVelocity < 0)
            animator.SetBool("Falling", true);
        else
            animator.SetBool("Falling", false);
    }

}
