using UnityEngine;

namespace RPG_Project.Player.States
{
    public class Move : StateMachineBehaviour
    {
        static public float interpolationRatio;

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if(animator.GetBool("isWalking") && !animator.GetBool("isRunning")) // walk
            {
                if (interpolationRatio > 0.0f)
                {
                    if (interpolationRatio < 0.01f)
                    {
                        return;
                    }

                    interpolationRatio -= Time.deltaTime;
                    animator.SetFloat("moveSpeed", interpolationRatio);
                }
                else if (interpolationRatio < 0.0f)
                {
                    if (interpolationRatio > -0.01f)
                    {
                        return;
                    }

                    interpolationRatio += Time.deltaTime;
                    animator.SetFloat("moveSpeed", interpolationRatio);
                }
            }
            else if(animator.GetBool("isWalkingBack") && !animator.GetBool("isRunning")) // walk back
            {
                if (interpolationRatio > -1.0f)
                {
                    if (interpolationRatio < -0.99f)
                    {
                        return;
                    }

                    interpolationRatio -= Time.deltaTime * 2; // times 2 to make the transition faster
                    animator.SetFloat("moveSpeed", interpolationRatio);
                }
                else if (interpolationRatio < -1.0f)
                {
                    if (interpolationRatio > -1.01f)
                    {
                        return;
                    }

                    interpolationRatio += Time.deltaTime * 2; // times 2 to make the transition faster
                    animator.SetFloat("moveSpeed", interpolationRatio);
                }
            }
            else if(animator.GetBool("isWalking") && animator.GetBool("isRunning")) // run
            {
                if (interpolationRatio < 1.0f)
                {
                    if (interpolationRatio > 0.99f)
                    {
                        return;
                    }

                    interpolationRatio += Time.deltaTime;
                    animator.SetFloat("moveSpeed", interpolationRatio);
                }
            }
            else if(animator.GetBool("isWalkingBack") && animator.GetBool("isRunning")) // run back
            {
                if (interpolationRatio > -2.0f)
                {
                    if (interpolationRatio < -1.99f)
                    {
                        return;
                    }

                    interpolationRatio -= Time.deltaTime * 2; // times 2 to make the transition faster
                    animator.SetFloat("moveSpeed", interpolationRatio);
                }
            }
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            interpolationRatio = 0.0f; // reset interpolation ratio
            animator.SetFloat("moveSpeed", interpolationRatio); // reset movement speed
        }
    }
}
