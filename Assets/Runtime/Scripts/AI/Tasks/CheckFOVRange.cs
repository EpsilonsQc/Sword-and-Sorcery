using UnityEngine;
using RPG_Project.AI.BehaviourTree;
using RPG_Project.AI.Manager;

namespace RPG_Project.AI.Tasks
{
    public class CheckFOVRange : Node
    {
        private Transform _player;
        private Transform _AI;
        private DataManager _dataManager;

        public CheckFOVRange(Transform transform)
        {
            _player = GameObject.Find("Player").transform;
            _AI = transform; // Get the transform of the AI
            _dataManager = transform.GetComponent<DataManager>();
        }

        public override NodeState Evaluate()
        {
            if (Vector3.Distance(_AI.position, _player.position) <= _dataManager.FOVRange) // Check if target is in range
            {
                return NodeState.RUNNING;
            }
            else // Target is not in range
            {
                return NodeState.FAILURE;
            }
        }
    }
}
