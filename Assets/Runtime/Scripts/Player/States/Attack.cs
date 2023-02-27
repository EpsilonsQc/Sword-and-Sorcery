using UnityEngine;

namespace RPG_Project.Player.States
{
    public class Attack : StateMachineBehaviour
    {
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!animator.GetBool("isGrounded")) // jump
            {
                animator.SetLayerWeight(layerIndex, 0.0f);
            }
            else if (animator.GetBool("isTurningLeft") || animator.GetBool("isTurningRight")) // turn left or right
            {
                if (animator.GetLayerWeight(layerIndex) > 0.5f)
                {
                    if (animator.GetLayerWeight(layerIndex) < 0.51f)
                    {
                        return;
                    }

                    animator.SetLayerWeight(layerIndex, animator.GetLayerWeight(layerIndex) - Time.deltaTime);
                }
                else if (animator.GetLayerWeight(layerIndex) < 0.5f)
                {
                    if (animator.GetLayerWeight(layerIndex) > 0.49f)
                    {
                        return;
                    }

                    animator.SetLayerWeight(layerIndex, animator.GetLayerWeight(layerIndex) + Time.deltaTime);
                }
            }
            else if (animator.GetBool("isWalking") || animator.GetBool("isWalkingBack")) // walk or walk back
            {
                if (animator.GetBool("isAttacking")) // attack
                {
                    if (animator.GetLayerWeight(layerIndex) < 1.0f)
                    {
                        animator.SetLayerWeight(layerIndex, animator.GetLayerWeight(layerIndex) + Time.deltaTime);
                    }
                }
                else if (animator.GetBool("isBlocking")) // block
                {
                    if (animator.GetLayerWeight(layerIndex) > 0.25f)
                    {
                        animator.SetLayerWeight(layerIndex, animator.GetLayerWeight(layerIndex) - Time.deltaTime);
                    }
                }
            }
            else
            {
                if (animator.GetLayerWeight(layerIndex) < 1.0f)
                {
                    animator.SetLayerWeight(layerIndex, animator.GetLayerWeight(layerIndex) + Time.deltaTime * 2); // times 2 to make the transition faster
                }
            }
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetLayerWeight(layerIndex, 1.0f);
        }
    }
}
