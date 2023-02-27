using UnityEngine;

namespace RPG_Project.Player.States
{
    public class Rotate : StateMachineBehaviour
    {
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!animator.GetBool("isGrounded")) // jump
            {
                animator.SetLayerWeight(layerIndex, 0.0f);
            }
            else if (!animator.GetBool("isRunning"))
            {
                if (!animator.GetBool("isWalking") && !animator.GetBool("isWalkingBack")) // idle
                {
                    animator.SetLayerWeight(layerIndex, 1.0f);
                }
                else if (animator.GetBool("isWalking") || animator.GetBool("isWalkingBack")) // walk or walk back
                {
                    animator.SetLayerWeight(layerIndex, 0.0f);
                }
            }
            else if (animator.GetBool("isRunning"))
            {
                if (animator.GetBool("isWalking") || animator.GetBool("isWalkingBack")) // run or run back
                {
                    animator.SetLayerWeight(layerIndex, 0.0f);
                }
                else
                {
                    animator.SetLayerWeight(layerIndex, 1.0f);
                }
            }
        }
    }
}
