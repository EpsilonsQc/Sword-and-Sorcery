using UnityEngine;

namespace RPG_Project.Player.States
{
    public class Idle : StateMachineBehaviour
    {
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        animator.SetInteger("idleStateSelector", Random.Range(0, 3)); // select a random idle animation
        }
    }
}
