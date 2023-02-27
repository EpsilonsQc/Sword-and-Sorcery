using UnityEngine;

namespace RPG_Project.Player.States
{
    public class Jump : StateMachineBehaviour
    {
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool("isGrounded", false);
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (animator.GetBool("isRunning"))
            {
                if (Move.interpolationRatio < 1.0f)
                {
                    if (Move.interpolationRatio > 0.99f)
                    {
                        return;
                    }

                Move.interpolationRatio += Time.deltaTime;
                animator.SetFloat("moveSpeed", Move.interpolationRatio);
                }
            }
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool("isGrounded", true);
        }
    }
}
